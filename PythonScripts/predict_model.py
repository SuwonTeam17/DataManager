import os
# TensorFlow 로그 억제
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

# Python 레벨 TF 경고도 억제
tf.get_logger().setLevel(logging.ERROR)
logging.getLogger("tensorflow").setLevel(logging.ERROR)

# ────────────────────────────────────────────────────────────────────────────
# [핵심 패치]
# TF 2.16+ (Keras 3) 환경에서 구버전(Keras 2) SavedModel 로드 시 발생하는
# '_UserObject' object has no attribute 'add_slot' 에러를 우회합니다.
#
# 원인:
#   - SavedModel에 포함된 옵티마이저(Adam 등)는 Keras 2의 OptimizerV2로 저장됨
#   - Keras 3에서는 해당 클래스가 등록 해제되어 _UserObject(AutoTrackable 서브클래스)로 대체됨
#   - 체크포인트 복원 시 TF가 optimizer_object.add_slot(var, slot_name)을 호출하는데
#     _UserObject에는 이 메서드가 없어 AttributeError 발생
#
# 해결:
#   - AutoTrackable에 더미 add_slot 메서드 주입 → _UserObject가 이를 상속
#   - 슬롯 변수는 모듈 레벨 dict에 저장하여 TF 추적 시스템에 노출되지 않게 함
#   - 옵티마이저 슬롯(모멘텀 등)은 추론에 불필요하므로 복원 실패해도 무관
# ────────────────────────────────────────────────────────────────────────────
_dummy_slots = {}  # {(optimizer_id, var_id, slot_name): tf.Variable}

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
        else:
            print("[patch] add_slot 이미 존재 - 패치 불필요", file=sys.stderr)
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
    """
    DonkeyCar 모델 출력 파싱.
    지원 형태:
      - list/tuple of arrays: [angle, throttle]
      - dict: {"n_outputs0": ..., "n_outputs1": ...} 또는 임의 키
      - ndarray shape (1, 2) 또는 (1, 1)
    """
    if isinstance(preds, dict):
        # 키를 이름 순 정렬하여 n_outputs0 → angle, n_outputs1 → throttle 순서 보장
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
        # tf.Tensor 등
        try:
            arr = np.array(preds).flatten()
            return (float(arr[0]), float(arr[1])) if len(arr) >= 2 else (float(arr[0]), 0.0)
        except Exception:
            return 0.0, 0.0


def load_and_predict_savedmodel(model_path, img_arr):
    """
    SavedModel 로드 및 추론.
    방법 1: AutoTrackable 패치 + tf.saved_model.load() → 가장 신뢰성 높음
    방법 2: TFSMLayer (Keras 3용)
    """
    last_error = None

    # ── 방법 1: AutoTrackable 패치 후 tf.saved_model.load() ────────────────
    # add_slot 에러를 근본적으로 우회. 모델 가중치는 정상 복원됨.
    # 옵티마이저 슬롯(Adam m/v)만 더미로 처리되며 추론에 영향 없음.
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

        result = infer(**{input_keys[0]: tf.constant(img_arr, dtype=tf.float32)})
        print("[1/2] 성공.", file=sys.stderr)

        # result는 dict(output_name → tf.Tensor)
        numpy_result = {k: v.numpy() for k, v in result.items()}
        return parse_predictions(numpy_result)

    except Exception as e:
        last_error = e
        print(f"[1/2] 실패: {e}", file=sys.stderr)

    # ── 방법 2: TFSMLayer ──────────────────────────────────────────────────
    try:
        print("[2/2] TFSMLayer 시도...", file=sys.stderr)
        layer = tf.keras.layers.TFSMLayer(model_path, call_endpoint="serving_default")
        input_tensor = tf.convert_to_tensor(img_arr, dtype=tf.float32)
        preds = layer(input_tensor)
        print("[2/2] 성공.", file=sys.stderr)

        if isinstance(preds, dict):
            numpy_result = {k: v.numpy() for k, v in preds.items()}
        elif isinstance(preds, (list, tuple)):
            numpy_result = [v.numpy() for v in preds]
        else:
            numpy_result = preds.numpy()
        return parse_predictions(numpy_result)

    except Exception as e:
        last_error = e
        print(f"[2/2] 실패: {e}", file=sys.stderr)

    raise RuntimeError(
        f"SavedModel 로드에 모든 방법이 실패했습니다.\n"
        f"모델 경로: {model_path}\n"
        f"마지막 에러: {last_error}"
    )


def load_and_predict(model_path, model_type, image_path):
    img_arr = preprocess_image(image_path)

    if model_type.lower() == "savedmodel":
        print("SavedModel 형식으로 로드 중...", file=sys.stderr)
        return load_and_predict_savedmodel(model_path, img_arr)

    elif model_type.lower() == "h5":
        actual_model_path = model_path
        if os.path.isdir(model_path):
            h5_files = glob.glob(os.path.join(model_path, "*.h5"))
            if not h5_files:
                raise FileNotFoundError("지정된 폴더에 .h5 파일이 없습니다.")
            actual_model_path = h5_files[0]

        model = tf.keras.models.load_model(actual_model_path, compile=False)
        preds = model.predict(img_arr, verbose=0)
        return parse_predictions(preds)

    else:
        raise ValueError(f"지원하지 않는 모델 형식: {model_type}")


def main():
    try:
        parser = argparse.ArgumentParser()
        parser.add_argument("--model", type=str, required=True)
        parser.add_argument("--type", type=str, required=True)
        parser.add_argument("--image", type=str, required=True)
        args = parser.parse_args()

        angle, throttle = load_and_predict(args.model, args.type, args.image)

        print(json.dumps({"angle": angle, "throttle": throttle}))
        sys.stdout.flush()

    except Exception as e:
        print(f"Error: {str(e)}", file=sys.stderr)
        sys.exit(1)


if __name__ == "__main__":
    main()
