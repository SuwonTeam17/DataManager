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

# ────────────────────────────────────────────────────────────────────────────
# [핵심 패치]
# TF 2.16+ (Keras 3) 환경에서 구버전(Keras 2) SavedModel 로드 시 발생하는
# '_UserObject' object has no attribute 'add_slot' 에러를 우회합니다.
# ────────────────────────────────────────────────────────────────────────────
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
                            tf.zeros(slot_shape, dtype=dtype),
                            trainable=False,
                            name=f"slot_{slot_name}",
                        )
                    except Exception:
                        _dummy_slots[key] = tf.Variable(0.0, trainable=False)
                return _dummy_slots.get(key)

            AutoTrackable.add_slot = _dummy_add_slot
            print("[patch] AutoTrackable.add_slot 주입 완료", file=sys.stderr)
        return True
    except Exception as e:
        print(f"[patch] 패치 실패: {e}", file=sys.stderr)
        return False


def preprocess_image(image_path):
    img = Image.open(image_path).convert("RGB")
    img = img.resize((160, 120))
    img_arr = np.array(img, dtype=np.float32)
    img_arr = np.expand_dims(img_arr, axis=0)  # (1, 120, 160, 3)
    return img_arr


def parse_predictions(preds):
    if isinstance(preds, dict):
        sorted_vals = [np.array(v).flatten() for v in
                       [preds[k] for k in sorted(preds.keys())]]
        if len(sorted_vals) >= 2:
            return float(sorted_vals[0][0]), float(sorted_vals[1][0])
        elif len(sorted_vals) == 1:
            v = sorted_vals[0]
            return (float(v[0]), float(v[1])) if len(v) >= 2 else (float(v[0]), 0.0)
        return 0.0, 0.0

    elif isinstance(preds, (list, tuple)):
        arr0 = np.array(preds[0]).flatten()
        if len(preds) >= 2:
            arr1 = np.array(preds[1]).flatten()
            return float(arr0[0]), float(arr1[0])
        return (float(arr0[0]), float(arr0[1])) if len(arr0) >= 2 else (float(arr0[0]), 0.0)

    elif isinstance(preds, np.ndarray):
        flat = preds.flatten()
        return (float(flat[0]), float(flat[1])) if len(flat) >= 2 else (float(flat[0]), 0.0)

    else:
        try:
            arr = np.array(preds).flatten()
            return (float(arr[0]), float(arr[1])) if len(arr) >= 2 else (float(arr[0]), 0.0)
        except Exception:
            return 0.0, 0.0


def load_model_savedmodel(model_path):
    """SavedModel을 로드하고 추론 함수를 반환합니다."""
    last_error = None

    # ── 방법 1: AutoTrackable 패치 후 tf.saved_model.load() ──────────────────
    try:
        print("[1/2] AutoTrackable 패치 후 tf.saved_model.load() 시도...", file=sys.stderr)
        _apply_autotrackable_patch()

        loaded = tf.saved_model.load(model_path)
        sig_keys = list(loaded.signatures.keys())
        if not sig_keys:
            raise RuntimeError("SavedModel에 serving signature가 없습니다.")

        sig_key = "serving_default" if "serving_default" in sig_keys else sig_keys[0]
        infer = loaded.signatures[sig_key]

        input_keys = list(infer.structured_input_signature[1].keys())
        if not input_keys:
            raise RuntimeError("signature 입력 키를 찾을 수 없습니다.")
        input_key = input_keys[0]

        print("[1/2] 성공.", file=sys.stderr)

        def predict_savedmodel(img_arr):
            result = infer(**{input_key: tf.constant(img_arr, dtype=tf.float32)})
            return parse_predictions({k: v.numpy() for k, v in result.items()})

        return predict_savedmodel

    except Exception as e:
        last_error = e
        print(f"[1/2] 실패: {e}", file=sys.stderr)

    # ── 방법 2: TFSMLayer ────────────────────────────────────────────────────
    try:
        print("[2/2] TFSMLayer 시도...", file=sys.stderr)
        layer = tf.keras.layers.TFSMLayer(model_path, call_endpoint="serving_default")
        print("[2/2] 성공.", file=sys.stderr)

        def predict_tfsm(img_arr):
            preds = layer(tf.convert_to_tensor(img_arr, dtype=tf.float32))
            if isinstance(preds, dict):
                numpy_result = {k: v.numpy() for k, v in preds.items()}
            elif isinstance(preds, (list, tuple)):
                numpy_result = [v.numpy() for v in preds]
            else:
                numpy_result = preds.numpy()
            return parse_predictions(numpy_result)

        return predict_tfsm

    except Exception as e:
        last_error = e
        print(f"[2/2] 실패: {e}", file=sys.stderr)

    raise RuntimeError(
        f"SavedModel 로드에 모든 방법이 실패했습니다.\n마지막 에러: {last_error}"
    )


def load_model_h5(model_path):
    """H5 모델을 로드하고 추론 함수를 반환합니다."""
    actual_path = model_path
    if os.path.isdir(model_path):
        h5_files = glob.glob(os.path.join(model_path, "*.h5"))
        if not h5_files:
            raise FileNotFoundError("지정된 폴더에 .h5 파일이 없습니다.")
        actual_path = h5_files[0]

    model = tf.keras.models.load_model(actual_path, compile=False)

    def predict_h5(img_arr):
        return parse_predictions(model.predict(img_arr, verbose=0))

    return predict_h5


def main():
    try:
        parser = argparse.ArgumentParser()
        parser.add_argument("--model", type=str, required=True)
        parser.add_argument("--type", type=str, required=True)
        args = parser.parse_args()

        print(f"모델 로드 중 ({args.type}): {args.model}", file=sys.stderr)

        if args.type.lower() == "savedmodel":
            predict = load_model_savedmodel(args.model)
        elif args.type.lower() == "h5":
            predict = load_model_h5(args.model)
        else:
            raise ValueError(f"지원하지 않는 모델 형식: {args.type}")

        # 모델 로드 완료 신호를 C#으로 전송
        print("READY", flush=True)

        # stdin에서 이미지 경로를 계속 받아 추론 수행
        for line in sys.stdin:
            image_path = line.strip()
            if not image_path:
                continue
            if image_path == "EXIT":
                break
            try:
                img_arr = preprocess_image(image_path)
                angle, throttle = predict(img_arr)
                print(json.dumps({"angle": angle, "throttle": throttle}), flush=True)
            except Exception as e:
                print(json.dumps({"error": str(e)}), flush=True)

    except Exception as e:
        print(f"FATAL: {str(e)}", file=sys.stderr)
        sys.exit(1)


if __name__ == "__main__":
    main()
