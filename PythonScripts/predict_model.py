import os
# TensorFlow의 내부 로그가 stdout을 오염시키는 것을 방지
os.environ["TF_CPP_MIN_LOG_LEVEL"] = "3"
os.environ["TF_ENABLE_ONEDNN_OPTS"] = "0"

import argparse
import sys
import json
import glob
import numpy as np
from PIL import Image
import tensorflow as tf

def preprocess_image(image_path):
    img = Image.open(image_path).convert('RGB')
    img = img.resize((160, 120))
    img_arr = np.array(img, dtype=np.float32)
    # Batch dimension 추가: (1, 120, 160, 3)
    img_arr = np.expand_dims(img_arr, axis=0)
    return img_arr

def load_and_predict(model_path, model_type, image_path):
    img_arr = preprocess_image(image_path)

    if model_type.lower() == 'savedmodel':
        print("Loading as TensorFlow SavedModel", file=sys.stderr)

        preds = None
        try:
            print("Attempting to load with TFSMLayer...", file=sys.stderr)
            layer = tf.keras.layers.TFSMLayer(model_path, call_endpoint="serving_default")
            input_tensor = tf.convert_to_tensor(img_arr, dtype=tf.float32)
            preds = layer(input_tensor)
        except Exception as e:
            print(f"TFSMLayer failed: {e}, falling back to tf.saved_model.load()", file=sys.stderr)
            loaded = tf.saved_model.load(model_path)
            if "serving_default" in loaded.signatures:
                infer = loaded.signatures["serving_default"]
            else:
                infer = loaded.signatures[list(loaded.signatures.keys())[0]]

            input_keys = list(infer.structured_input_signature[1].keys())
            input_key = input_keys[0]

            preds = infer(**{input_key: tf.constant(img_arr, dtype=tf.float32)})

        if isinstance(preds, dict):
            values = list(preds.values())
        else:
            values = preds if isinstance(preds, (list, tuple)) else [preds]

        if len(values) >= 2:
            angle = float(values[0].numpy().flatten()[0])
            throttle = float(values[1].numpy().flatten()[0])
        elif len(values) == 1:
            val = values[0].numpy()
            if len(val.shape) >= 2 and val.shape[1] >= 2:
                angle = float(val[0][0])
                throttle = float(val[0][1])
            else:
                angle = float(val.flatten()[0])
                throttle = 0.0
        else:
            angle = 0.0
            throttle = 0.0

    elif model_type.lower() == 'h5':
        actual_model_path = model_path
        if os.path.isdir(model_path):
            h5_files = glob.glob(os.path.join(model_path, "*.h5"))
            if not h5_files:
                raise FileNotFoundError("지정된 폴더에 .h5 파일이 없습니다.")
            actual_model_path = h5_files[0]

        model = tf.keras.models.load_model(actual_model_path, compile=False)
        preds = model.predict(img_arr, verbose=0)

        if isinstance(preds, (list, tuple)):
            angle = float(np.array(preds[0]).flatten()[0])
            throttle = float(np.array(preds[1]).flatten()[0])
        elif isinstance(preds, np.ndarray):
            flat_preds = preds.flatten()
            if len(flat_preds) >= 2:
                angle = float(flat_preds[0])
                throttle = float(flat_preds[1])
            else:
                angle = float(flat_preds[0])
                throttle = 0.0
        else:
            angle = 0.0
            throttle = 0.0
    else:
        raise ValueError(f"지원하지 않는 모델 형식입니다: {model_type}")

    return angle, throttle

def main():
    try:
        parser = argparse.ArgumentParser()
        parser.add_argument("--model", type=str, required=True)
        parser.add_argument("--type", type=str, required=True)
        parser.add_argument("--image", type=str, required=True)
        
        args = parser.parse_args()
        
        angle, throttle = load_and_predict(args.model, args.type, args.image)
        
        result = {
            "angle": angle,
            "throttle": throttle
        }
        
        print(json.dumps(result))
        sys.stdout.flush()

    except Exception as e:
        print(f"Error: {str(e)}", file=sys.stderr)
        sys.exit(1)

if __name__ == "__main__":
    main()
