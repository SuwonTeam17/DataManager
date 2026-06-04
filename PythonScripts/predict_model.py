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
import tensorflow as tf

tf.get_logger().setLevel(logging.ERROR)
logging.getLogger("tensorflow").setLevel(logging.ERROR)

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
# 이미지 전처리: div255 고정
# ─────────────────────────────────────────────────────────────────────────────

def preprocess_image(image_path):
    """
    160×120 RGB 리사이즈 후 float32 / 255.0 정규화.
    반환: shape=(1, 120, 160, 3), dtype=float32, 범위 0~1
    """
    img = Image.open(image_path).convert("RGB")
    img = img.resize((160, 120))
    img_arr = np.array(img, dtype=np.float32) / 255.0
    return np.expand_dims(img_arr, axis=0)   # (1, 120, 160, 3)


# ─────────────────────────────────────────────────────────────────────────────
# 모델 출력 파싱: n_outputs0 = angle, n_outputs1 = throttle 고정
# ─────────────────────────────────────────────────────────────────────────────

def parse_outputs(raw):
    """
    raw 추론 결과 → (angle, throttle).
    dict 키는 알파벳 정렬 기준 0번째 = angle, 1번째 = throttle.
    """
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


def _get_input_info(shape):
    """
    shape 리스트에서 (ndim, time_steps)를 추출.
    5D 시계열 모델: shape = [None, T, H, W, C] → ndim=5, time_steps=T (없으면 1)
    4D 일반 모델:   shape = [None, H, W, C]    → ndim=4, time_steps=1
    """
    ndim = len(shape)
    if ndim == 5:
        t = shape[1]
        time_steps = int(t) if t is not None else 1
    else:
        time_steps = 1
    return ndim, time_steps


def _adapt_input(img_arr, ndim, time_steps):
    """
    (1, H, W, C) 입력을 모델이 요구하는 shape으로 맞춤.
    5D 시계열 모델: 같은 프레임을 time_steps만큼 복제 → (1, T, H, W, C)
    """
    if ndim == 5 and img_arr.ndim == 4:
        frame   = img_arr[0]                               # (H, W, C)
        stacked = np.stack([frame] * time_steps, axis=0)   # (T, H, W, C)
        return np.expand_dims(stacked, axis=0)             # (1, T, H, W, C)
    return img_arr


# ─────────────────────────────────────────────────────────────────────────────
# 모델 로드
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

        # 입력 shape에서 차원 수·시간 스텝 감지 (3D 시계열 모델 대응)
        ndim, time_steps = 4, 1
        try:
            spec = infer.structured_input_signature[1].get(input_key)
            if spec is not None:
                ndim, time_steps = _get_input_info(spec.shape.as_list())
        except Exception:
            pass
        print(f"[MODEL] 입력 차원={ndim}D, 시간스텝={time_steps}", file=sys.stderr)
        print("[1/3] 성공.", file=sys.stderr)

        def predict(img_arr, _key=input_key, _ndim=ndim, _ts=time_steps):
            adjusted = _adapt_input(img_arr, _ndim, _ts)
            result   = infer(**{_key: tf.constant(adjusted, dtype=tf.float32)})
            return parse_outputs({k: v.numpy() for k, v in result.items()})

        return predict

    except Exception as e:
        last_error = e
        print(f"[1/3] 실패: {e}", file=sys.stderr)

    # 방법 2: TFSMLayer
    try:
        print("[2/3] TFSMLayer 시도...", file=sys.stderr)
        layer = tf.keras.layers.TFSMLayer(model_path, call_endpoint="serving_default")
        print("[2/3] 성공.", file=sys.stderr)

        def predict_tfsm(img_arr):
            preds = layer(tf.convert_to_tensor(img_arr, dtype=tf.float32))
            if isinstance(preds, dict):
                return parse_outputs({k: v.numpy() for k, v in preds.items()})
            elif isinstance(preds, (list, tuple)):
                return parse_outputs([v.numpy() for v in preds])
            return parse_outputs(preds.numpy())

        return predict_tfsm

    except Exception as e:
        last_error = e
        print(f"[2/3] 실패: {e}", file=sys.stderr)

    # 방법 3: Keras SavedModel 직접 로드 (serving signature 없는 경우 대비)
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

        def predict_keras_sm(img_arr, _ndim=ndim, _ts=time_steps):
            adjusted = _adapt_input(img_arr, _ndim, _ts)
            return parse_outputs(model.predict(adjusted, verbose=0))

        return predict_keras_sm

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

    def predict(img_arr, _ndim=ndim, _ts=time_steps):
        adjusted = _adapt_input(img_arr, _ndim, _ts)
        return parse_outputs(model.predict(adjusted, verbose=0))

    return predict


# ─────────────────────────────────────────────────────────────────────────────
# main
# ─────────────────────────────────────────────────────────────────────────────

def main():
    try:
        parser = argparse.ArgumentParser()
        parser.add_argument("--model", type=str, required=True)
        parser.add_argument("--type",  type=str, required=True)
        args = parser.parse_args()

        print(f"모델 로드 중 ({args.type}): {args.model}", file=sys.stderr)
        print("전처리: float32 / 255.0  출력순서: n_outputs0=angle, n_outputs1=throttle", file=sys.stderr)

        if args.type.lower() == "savedmodel":
            predict = load_model_savedmodel(args.model)
        elif args.type.lower() == "h5":
            predict = load_model_h5(args.model)
        else:
            raise ValueError(f"지원하지 않는 모델 형식: {args.type}")

        print("READY", flush=True)

        for line in sys.stdin:
            image_path = line.strip()
            if not image_path:
                continue
            if image_path == "EXIT":
                break
            try:
                img_arr = preprocess_image(image_path)
                angle, throttle = predict(img_arr)
                # stdout: JSON 한 줄만
                print(json.dumps({"angle": angle, "throttle": throttle}), flush=True)
            except Exception as e:
                print(f"[ERROR] {e}", file=sys.stderr)
                print(json.dumps({"error": str(e)}), flush=True)

    except Exception as e:
        print(f"FATAL: {str(e)}", file=sys.stderr)
        sys.exit(1)


if __name__ == "__main__":
    main()
