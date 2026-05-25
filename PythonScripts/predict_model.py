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


# ─────────────────────────────────────────────────────────────────────────────
# 모델 로드
# ─────────────────────────────────────────────────────────────────────────────

def load_model_savedmodel(model_path):
    last_error = None

    # 방법 1: AutoTrackable 패치 + tf.saved_model.load()
    try:
        print("[1/2] tf.saved_model.load() 시도...", file=sys.stderr)
        _apply_autotrackable_patch()
        loaded  = tf.saved_model.load(model_path)
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
        print("[1/2] 성공.", file=sys.stderr)

        def predict(img_arr):
            result = infer(**{input_key: tf.constant(img_arr, dtype=tf.float32)})
            return parse_outputs({k: v.numpy() for k, v in result.items()})

        return predict

    except Exception as e:
        last_error = e
        print(f"[1/2] 실패: {e}", file=sys.stderr)

    # 방법 2: TFSMLayer
    try:
        print("[2/2] TFSMLayer 시도...", file=sys.stderr)
        layer = tf.keras.layers.TFSMLayer(model_path, call_endpoint="serving_default")
        print("[2/2] 성공.", file=sys.stderr)

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
        print(f"[2/2] 실패: {e}", file=sys.stderr)

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

    def predict(img_arr):
        return parse_outputs(model.predict(img_arr, verbose=0))

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
