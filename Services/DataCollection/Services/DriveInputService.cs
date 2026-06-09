using System;
using System.Linq;
using System.Windows.Forms;
using Windows.Gaming.Input; // Windows WinRT API

namespace DataManager.Services.DataCollection.Services
{
    public class DriveInputService
    {
        public enum InputMode { Keyboard, Joystick, Gamepad }
        private InputMode? _mode = null;
        private Gamepad _gamepad; // Windows.Gaming.Input의 Gamepad 클래스
        private System.Windows.Forms.Timer _timer;
        public float Angle { get; private set; }
        public float Throttle { get; private set; }
        public event Action<float, float> OnInputChanged;

        private const float Step = 0.05f;
        private const float ThrottleMax = 1.0f;
        private const float ThrottleMin = -1.0f;
        private const float AngleMax = 1.0f;
        private const float AngleMin = -1.0f;

        public DriveInputService()
        {
            // 이벤트 구독: 패드가 새로 연결되거나 끊길 때 자동으로 갱신되도록 설정
            Gamepad.GamepadAdded += (s, e) => { if (_mode == InputMode.Gamepad) _gamepad = e; };
            Gamepad.GamepadRemoved += (s, e) => { if (_gamepad == e) _gamepad = null; };
        }

        public void SetMode(InputMode mode)
        {
            _mode = mode;
            Angle = 0f;
            Throttle = 0f;

            if (mode == InputMode.Gamepad)
            {
                // 현재 PC에 연결된 게임패드 중 첫 번째 장치를 가져옵니다 (엑박/플스 공통)
                _gamepad = Gamepad.Gamepads.FirstOrDefault();
            }
            else
            {
                _gamepad = null;
            }
        }

        public void Start()
        {
            _timer = new System.Windows.Forms.Timer { Interval = 50 };
            _timer.Tick += Update;
            _timer.Start();
        }

        public void Stop() => _timer?.Stop();

        // 키보드 입력 판별
        public bool HandleKeyboardKey(Keys key)
        {
            if (_mode != InputMode.Keyboard)
                return false;

            switch (key)
            {
                case Keys.Up:
                case Keys.W:
                case Keys.Down:
                case Keys.S:
                case Keys.Left:
                case Keys.A:
                case Keys.Right:
                case Keys.D:
                case Keys.Space:
                    KeyDown(key);
                    return true;

                default:
                    return false;
            }
        }

        // 키보드 — 키를 누를 때마다 0.05씩 증감 (KeyDown 1회 = 1스텝)
        public void KeyDown(Keys key)
        {
            if (_mode != InputMode.Keyboard) return;

            switch (key)
            {
                case Keys.Up:
                case Keys.W:
                    Throttle = (float)Math.Round(
                        Math.Min(Throttle + Step, ThrottleMax), 2);
                    break;

                case Keys.Down:
                case Keys.S:
                    Throttle = (float)Math.Round(
                        Math.Max(Throttle - Step, ThrottleMin), 2);
                    break;

                case Keys.Left:
                case Keys.A:
                    Angle = (float)Math.Round(
                        Math.Max(Angle - Step, AngleMin), 2);
                    break;

                case Keys.Right:
                case Keys.D:
                    Angle = (float)Math.Round(
                        Math.Min(Angle + Step, AngleMax), 2);
                    break;

                case Keys.Space:
                    Angle = 0f;
                    Throttle = 0f;
                    break;
            }

            OnInputChanged?.Invoke(Angle, Throttle);
        }

        // KeyUp은 아무 동작 없음 — 값이 유지되어야 하므로
        public void KeyUp(Keys key)
        {

        }

        // 조이스틱 값 전달 (VirtualJoystick에서 호출)
        public void SetJoystick(float angle, float throttle)
        {
            if (_mode != InputMode.Joystick) return;
            Angle = angle;
            Throttle = throttle;
        }

        private void Update(object sender, EventArgs e)
        {
            if (_mode == InputMode.Gamepad) UpdateGamepad();

            // 주기적으로 외부 이벤트 호출 (연결 keepalive 유지)
            OnInputChanged?.Invoke(Angle, Throttle);
        }

        private void UpdateGamepad()
        {
            // 프로그램 실행 중에 패드가 나중에 연결되었을 때를 대비한 방어 코드
            if (_gamepad == null && Gamepad.Gamepads.Count > 0)
            {
                _gamepad = Gamepad.Gamepads[0];
            }

            if (_gamepad == null) return;

            // Windows.Gaming.Input 방식으로 현재 패드 상태 읽기
            GamepadReading reading = _gamepad.GetCurrentReading();

            // 1. 조향각 (Angle): R 스틱(RightThumbstick)의 좌우(X축) 사용
            float a = (float)reading.LeftThumbstickX;

            // 2. 스로틀 (Throttle): L 스틱(LeftThumbstick)의 위아래(Y축) 사용
            // 위로 밀면(+), 아래로 당기면(-) 값이 나옵니다.
            float t = (float)reading.LeftThumbstickY;

            // 3. 데드존 처리 및 최종 값 대입
            // 스틱을 가만히 두어도 미세하게 값이 튀는 현상(쏠림)을 방지합니다.
            Angle = Math.Abs(a) < 0.1f ? 0f : a;
            Throttle = Math.Abs(t) < 0.1f ? 0f : t;

            // 디버깅용 로그: 값이 잘 들어오는지 비주얼 스튜디오 출력 창에서 확인 가능합니다.
            // System.Diagnostics.Debug.WriteLine($"[Gamepad] Angle(R_X): {Angle:F2}, Throttle(L_Y): {Throttle:F2}");
        }
    }
}