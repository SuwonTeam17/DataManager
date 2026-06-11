import os
os.environ["TF_CPP_MIN_LOG_LEVEL"] = "3"
os.environ["TF_ENABLE_ONEDNN_OPTS"] = "0"

import argparse
import sys
import json
import glob
import logging
import numpy as np
from PIL import Image
from concurrent.futures import ThreadPoolExecutor
import tensorflow as tf

tf.get_logger().setLevel(logging.ERROR)
logging.getLogger("tensorflow").setLevel(logging.ERROR)

# ─────────────────────────────────────────────────────────────────────────────
# 하드웨어 설정 (GPU 우선, 없으면 CPU 멀티코어 최대화)
# ─────────────────────────────────────────────────────────────────────────────

def configure_hardware():
    gpus = tf.config.experimental.list_physical_devices('GPU')
    if gpus:
        for gpu in gpus:
            try:
                tf.config.experimental.set_memory_growth(gpu, True)
            except Exception:
                pass
        print(f"[HW] GPU {len(gpus)}개 감지 — GPU 추론 활성화", file=sys.stderr)
        return True
    else:
        # CPU 연산 스레드를 0으로 설정하면 TF가 가용 코어를 모두 사용
        tf.config.threading.set_intra_op_parallelism_threads(0)
        tf.config.threading.set_inter_op_parallelism_threads(0)
        print("[HW] GPU 없음 — CPU 멀티코어 최적화 적용", file=sys.stderr)
        return False


# ─────────────────────────────────────────────────────────────────────────────
# TF 2.16+ (Keras 3) 환경에서 구버전 SavedModel 로드 시 add_slot 에러 우회
# ─────────────────────────────────────────────────────────────────────────────
_dummy_slots = {}

def _apply_autotrackable_patch():
    try:
        from tensorflow.python.trackable.autotrackable import AutoTrackable
        if not hasattr(AutoTrackable, "add_slot"):
            def _dummy_add_slot(self, var, slot_name, initializer="zeros", shape=None):
                key = (id(self), id(var), slot_name)
                if key not in _dummy_slots:
                    try:
                        slot_shape = shape if shape is not None else var.shape
                        dtype = getattr(var, "dtype", tf.float32)
                        _dummy_slots[key] = tf.Variable(
                            tf.zeros(slot_shape, dtype=dtype), trainable=False,
                            name=f"slot_{slot_name}")
                    except Exception:
                        _dummy_slots[key] = tf.Variable(0.0, trainable=False)
                return _dummy_slots.get(key)
            AutoTrackable.add_slot = _dummy_add_slot
            print("[patch] AutoTrackable.add_slot 주입 완료", file=sys.stderr)
        return True
    except Exception as e:
        print(f"[patch] 패치 실패: {e}", file=sys.stderr)
        return False


# ─────────────────────────────────────────────────────────────────────────────
# 이미지 전처리
# ─────────────────────────────────────────────────────────────────────────────

def preprocess_image(image_path):
    img = Image.open(image_path).convert("RGB")
    img = img.resize((160, 120))
    img_arr = np.array(img, dtype=np.float32) / 255.0
    return np.expand_dims(img_arr, axis=0)   # (1, 120, 160, 3)


def preprocess_batch(paths):
    """ThreadPoolExecutor로 이미지 병렬 전처리 후 배치 배열 반환."""
    with ThreadPoolExecutor(max_workers=min(8, len(paths))) as exe:
        arrays = list(exe.map(preprocess_image, paths))
    return np.concatenate(arrays, axis=0)   # (N, 120, 160, 3)


# ─────────────────────────────────────────────────────────────────────────────
# 출력 파싱 (단일 샘플)
# ─────────────────────────────────────────────────────────────────────────────

def parse_outputs(raw):
    if isinstance(raw, dict):
        keys = sorted(raw.keys())
        print(f"[MODEL_OUT] keys={keys}", file=sys.stderr)
        vals = [np.array(raw[k]).flatten() for k in keys]
        angle    = float(vals[0][0]) if len(vals) >= 1 else 0.0
        throttle = float(vals[1][0]) if len(vals) >= 2 else 0.0
    elif isinstance(raw, (list, tuple)):
        print(f"[MODEL_OUT] list len={len(raw)}", file=sys.stderr)
        angle    = float(np.array(raw[0]).flatten()[0]) if len(raw) >= 1 else 0.0
        throttle = float(np.array(raw[1]).flatten()[0]) if len(raw) >= 2 else 0.0
    elif isinstance(raw, np.ndarray):
        flat     = raw.flatten()
        print(f"[MODEL_OUT] ndarray shape={raw.shape}", file=sys.stderr)
        angle    = float(flat[0]) if len(flat) >= 1 else 0.0
        throttle = float(flat[1]) if len(flat) >= 2 else 0.0
    else:
        flat     = np.array(raw).flatten()
        angle    = float(flat[0]) if len(flat) >= 1 else 0.0
        throttle = float(flat[1]) if len(flat) >= 2 else 0.0

    print(f"[PRED] angle={angle:+.4f}  throttle={throttle:+.4f}", file=sys.stderr)
    return angle, throttle


def parse_batch_outputs(raw, n):
    """배치 출력 → list of (angle, throttle)."""
    results = []
    try:
        if isinstance(raw, dict):
            keys = sorted(raw.keys())
            ang_all = np.array(raw[keys[0]]).reshape(n, -1)[:, 0]
            thr_all = np.array(raw[keys[1]]).reshape(n, -1)[:, 0] if len(keys) >= 2 else np.zeros(n)
        elif isinstance(raw, (list, tuple)):
            ang_all = np.array(raw[0]).reshape(n, -1)[:, 0]
            thr_all = np.array(raw[1]).reshape(n, -1)[:, 0] if len(raw) >= 2 else np.zeros(n)
        elif isinstance(raw, np.ndarray):
            reshaped = raw.reshape(n, -1)
            ang_all  = reshaped[:, 0]
            thr_all  = reshaped[:, 1] if reshaped.shape[1] >= 2 else np.zeros(n)
        else:
            reshaped = np.array(raw).reshape(n, -1)
            ang_all  = reshaped[:, 0]
            thr_all  = reshaped[:, 1] if reshaped.shape[1] >= 2 else np.zeros(n)

        for i in range(n):
            results.append((float(ang_all[i]), float(thr_all[i])))
    except Exception as e:
        print(f"[BATCH_PARSE] 에러: {e}", file=sys.stderr)
        results = [(0.0, 0.0)] * n
    return results


# ─────────────────────────────────────────────────────────────────────────────
# 입력 shape 보조 함수
# ─────────────────────────────────────────────────────────────────────────────

def _get_input_info(shape):
    ndim = len(shape)
    if ndim == 5:
        t = shape[1]
        time_steps = int(t) if t is not None else 1
    else:
        time_steps = 1
    return ndim, time_steps


def _adapt_input(img_arr, ndim, time_steps):
    """(N, H, W, C) → 시계열 모델이면 (N, T, H, W, C) 로 변환."""
    if ndim == 5 and img_arr.ndim == 4:
        n = img_arr.shape[0]
        stacked = np.stack([img_arr] * time_steps, axis=1)   # (N, T, H, W, C)
        return stacked
    return img_arr


# ─────────────────────────────────────────────────────────────────────────────
# 모델 로드 — (predict_single, predict_batch) 쌍 반환
# ─────────────────────────────────────────────────────────────────────────────

def load_model_savedmodel(model_path):
    last_error = None

    # 방법 1: AutoTrackable 패치 + tf.saved_model.load()
    try:
        print("[1/3] tf.saved_model.load() 시도...", file=sys.stderr)
        _apply_autotrackable_patch()
        loaded   = tf.saved_model.load(model_path)
        sig_keys = list(loaded.signatures.keys())
        if not sig_keys:
            raise RuntimeError("serving signature가 없습니다.")

        sig_key    = "serving_default" if "serving_default" in sig_keys else sig_keys[0]
        infer      = loaded.signatures[sig_key]
        input_keys = list(infer.structured_input_signature[1].keys())
        out_keys   = list(infer.structured_outputs.keys())

        print(f"[MODEL] signature='{sig_key}'", file=sys.stderr)
        print(f"[MODEL] input  keys={input_keys}", file=sys.stderr)
        print(f"[MODEL] output keys={sorted(out_keys)}", file=sys.stderr)

        if not input_keys:
            raise RuntimeError("입력 키를 찾을 수 없습니다.")
        input_key = input_keys[0]

        ndim, time_steps = 4, 1
        try:
            spec = infer.structured_input_signature[1].get(input_key)
            if spec is not None:
                ndim, time_steps = _get_input_info(spec.shape.as_list())
        except Exception:
            pass
        print(f"[MODEL] 입력 차원={ndim}D, 시간스텝={time_steps}", file=sys.stderr)
        print("[1/3] 성공.", file=sys.stderr)

        def predict_single(img_arr, _key=input_key, _ndim=ndim, _ts=time_steps):
            adjusted = _adapt_input(img_arr, _ndim, _ts)
            result   = infer(**{_key: tf.constant(adjusted, dtype=tf.float32)})
            return parse_outputs({k: v.numpy() for k, v in result.items()})

        def predict_batch_fn(batch_arr, _key=input_key, _ndim=ndim, _ts=time_steps):
            adjusted = _adapt_input(batch_arr, _ndim, _ts)
            n = adjusted.shape[0] if _ndim == 4 else adjusted.shape[0]
            result   = infer(**{_key: tf.constant(adjusted, dtype=tf.float32)})
            return parse_batch_outputs({k: v.numpy() for k, v in result.items()}, n)

        return predict_single, predict_batch_fn

    except Exception as e:
        last_error = e
        print(f"[1/3] 실패: {e}", file=sys.stderr)

    # 방법 2: TFSMLayer
    try:
        print("[2/3] TFSMLayer 시도...", file=sys.stderr)
        layer = tf.keras.layers.TFSMLayer(model_path, call_endpoint="serving_default")
        print("[2/3] 성공.", file=sys.stderr)

        def predict_single_tfsm(img_arr):
            preds = layer(tf.convert_to_tensor(img_arr, dtype=tf.float32))
            if isinstance(preds, dict):
                return parse_outputs({k: v.numpy() for k, v in preds.items()})
            elif isinstance(preds, (list, tuple)):
                return parse_outputs([v.numpy() for v in preds])
            return parse_outputs(preds.numpy())

        def predict_batch_tfsm(batch_arr):
            n = batch_arr.shape[0]
            preds = layer(tf.convert_to_tensor(batch_arr, dtype=tf.float32))
            if isinstance(preds, dict):
                return parse_batch_outputs({k: v.numpy() for k, v in preds.items()}, n)
            elif isinstance(preds, (list, tuple)):
                return parse_batch_outputs([v.numpy() for v in preds], n)
            return parse_batch_outputs(preds.numpy(), n)

        return predict_single_tfsm, predict_batch_tfsm

    except Exception as e:
        last_error = e
        print(f"[2/3] 실패: {e}", file=sys.stderr)

    # 방법 3: Keras SavedModel 직접 로드
    try:
        print("[3/3] keras.models.load_model() 시도...", file=sys.stderr)
        model = tf.keras.models.load_model(model_path, compile=False)

        ndim, time_steps = 4, 1
        try:
            ndim, time_steps = _get_input_info(list(model.input_shape))
        except Exception:
            pass
        print(f"[MODEL] 입력 차원={ndim}D, 시간스텝={time_steps}", file=sys.stderr)
        print("[3/3] 성공.", file=sys.stderr)

        def predict_single_keras(img_arr, _ndim=ndim, _ts=time_steps):
            adjusted = _adapt_input(img_arr, _ndim, _ts)
            return parse_outputs(model.predict(adjusted, verbose=0))

        def predict_batch_keras(batch_arr, _ndim=ndim, _ts=time_steps):
            n = batch_arr.shape[0]
            adjusted = _adapt_input(batch_arr, _ndim, _ts)
            return parse_batch_outputs(model.predict(adjusted, verbose=0, batch_size=n), n)

        return predict_single_keras, predict_batch_keras

    except Exception as e:
        last_error = e
        print(f"[3/3] 실패: {e}", file=sys.stderr)

    raise RuntimeError(f"SavedModel 로드 실패. 마지막 에러: {last_error}")


def load_model_h5(model_path):
    actual_path = model_path
    if os.path.isdir(model_path):
        h5_files = glob.glob(os.path.join(model_path, "*.h5"))
        if not h5_files:
            raise FileNotFoundError("폴더에 .h5 파일이 없습니다.")
        actual_path = h5_files[0]

    model = tf.keras.models.load_model(actual_path, compile=False)
    print(f"[MODEL] H5 로드 완료: {actual_path}", file=sys.stderr)

    ndim, time_steps = 4, 1
    try:
        ndim, time_steps = _get_input_info(list(model.input_shape))
    except Exception:
        pass
    print(f"[MODEL] 입력 차원={ndim}D, 시간스텝={time_steps}", file=sys.stderr)

    def predict_single(img_arr, _ndim=ndim, _ts=time_steps):
        adjusted = _adapt_input(img_arr, _ndim, _ts)
        return parse_outputs(model.predict(adjusted, verbose=0))

    def predict_batch_fn(batch_arr, _ndim=ndim, _ts=time_steps):
        n = batch_arr.shape[0]
        adjusted = _adapt_input(batch_arr, _ndim, _ts)
        return parse_batch_outputs(model.predict(adjusted, verbose=0, batch_size=n), n)

    return predict_single, predict_batch_fn


# ─────────────────────────────────────────────────────────────────────────────
# 워밍업 — 첫 추론 JIT 컴파일 비용을 READY 이전에 소화
# ─────────────────────────────────────────────────────────────────────────────

def warmup(predict_single, ndim=4, time_steps=1):
    try:
        print("[WARMUP] 더미 추론으로 JIT 워밍업 시작...", file=sys.stderr)
        if ndim == 5:
            dummy = np.zeros((1, time_steps, 120, 160, 3), dtype=np.float32)
        else:
            dummy = np.zeros((1, 120, 160, 3), dtype=np.float32)
        predict_single(dummy)
        print("[WARMUP] 완료 — 이후 추론 즉시 응답", file=sys.stderr)
    except Exception as e:
        print(f"[WARMUP] 실패 (무시): {e}", file=sys.stderr)


# ─────────────────────────────────────────────────────────────────────────────
# main
# ─────────────────────────────────────────────────────────────────────────────

def main():
    try:
        parser = argparse.ArgumentParser()
        parser.add_argument("--model", type=str, required=True)
        parser.add_argument("--type",  type=str, required=True)
        args = parser.parse_args()

        # 하드웨어 설정 먼저
        configure_hardware()

        print(f"모델 로드 중 ({args.type}): {args.model}", file=sys.stderr)

        if args.type.lower() == "savedmodel":
            predict_single, predict_batch = load_model_savedmodel(args.model)
        elif args.type.lower() == "h5":
            predict_single, predict_batch = load_model_h5(args.model)
        else:
            raise ValueError(f"지원하지 않는 모델 형식: {args.type}")

        # JIT 워밍업 (첫 실제 추론을 빠르게 만들기 위해)
        warmup(predict_single)

        print("READY", flush=True)

        for line in sys.stdin:
            line = line.strip()
            if not line:
                continue

            # ── 배치 모드: "BATCH <count>" ──────────────────────────────────
            if line.startswith("BATCH "):
                try:
                    count = int(line[6:])
                except ValueError:
                    continue

                paths = []
                for _ in range(count):
                    p = sys.stdin.readline().strip()
                    if p:
                        paths.append(p)

                if not paths:
                    continue

                try:
                    # 이미지 병렬 전처리 + 배치 추론 (GPU/CPU 모두 이점 있음)
                    batch_arr = preprocess_batch(paths)
                    results   = predict_batch(batch_arr)
                    for angle, throttle in results:
                        print(json.dumps({"angle": angle, "throttle": throttle}), flush=True)
                except Exception as e:
                    print(f"[BATCH_ERROR] {e}", file=sys.stderr)
                    for _ in paths:
                        print(json.dumps({"error": str(e)}), flush=True)

            # ── 종료 ─────────────────────────────────────────────────────────
            elif line == "EXIT":
                break

            # ── 단일 이미지 모드 ─────────────────────────────────────────────
            else:
                image_path = line
                try:
                    img_arr = preprocess_image(image_path)
                    angle, throttle = predict_single(img_arr)
                    print(json.dumps({"angle": angle, "throttle": throttle}), flush=True)
                except Exception as e:
                    print(f"[ERROR] {e}", file=sys.stderr)
                    print(json.dumps({"error": str(e)}), flush=True)

    except Exception as e:
        print(f"FATAL: {str(e)}", file=sys.stderr)
        sys.exit(1)


if __name__ == "__main__":
    main()
