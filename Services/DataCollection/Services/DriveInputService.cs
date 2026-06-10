using System;
using System.Linq;
using System.Windows.Forms;
using Windows.Gaming.Input; // Windows WinRT API

namespace DataManager.Services.DataCollection.Services
{
    public class DriveInputService : IMessageFilter
    {
        public enum InputMode { Keyboard, Joystick, Gamepad }
        private InputMode? _mode = null;
        private Gamepad _gamepad;
        private System.Windows.Forms.Timer _timer;
        public float Angle { get; private set; }
        public float Throttle { get; private set; }
        public event Action<float, float> OnInputChanged;

        private const float Step = 0.05f;
        private const float ThrottleMax = 1.0f;
        private const float ThrottleMin = -1.0f;
        private const float AngleMax = 1.0f;
        private const float AngleMin = -1.0f;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYUP = 0x0105;

        private bool _isUpPressed, _isDownPressed, _isLeftPressed, _isRightPressed;

        public DriveInputService()
        {
            // 앱 전역 메시지 필터 등록 — 포커스 위치와 무관하게 KeyDown/KeyUp 수신
            Application.AddMessageFilter(this);

            Gamepad.GamepadAdded += (s, e) => { if (_mode == InputMode.Gamepad) _gamepad = e; };
            Gamepad.GamepadRemoved += (s, e) => { if (_gamepad == e) _gamepad = null; };
        }

        // ── IMessageFilter ────────────────────────────────────────────────────
        public bool PreFilterMessage(ref Message m)
        {
            if (_mode != InputMode.Keyboard) return false;

            if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
            {
                Keys key = (Keys)(int)m.WParam & Keys.KeyCode;
                HandleKeyDown(key);
                return false; // 소비하지 않고 전파 유지
            }

            if (m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP)
            {
                Keys key = (Keys)(int)m.WParam & Keys.KeyCode;
                KeyUp(key);
                return false;
            }

            return false;
        }
        // ─────────────────────────────────────────────────────────────────────

        public void SetMode(InputMode mode)
        {
            _mode = mode;
            Angle = 0f;
            Throttle = 0f;
            _gamepad = (mode == InputMode.Gamepad) ? Gamepad.Gamepads.FirstOrDefault() : null;
        }

        public void Start()
        {
            _timer = new System.Windows.Forms.Timer { Interval = 50 };
            _timer.Tick += Update;
            _timer.Start();
        }

        public void Stop()
        {
            _timer?.Stop();
            Application.RemoveMessageFilter(this);
        }

        // KeyDown — OS 키리피트를 그대로 이용해 누르는 동안 0.05씩 증감 (원래 방식)
        private void HandleKeyDown(Keys key)
        {
            if (key == Keys.Up || key == Keys.W) _isUpPressed = true;
            if (key == Keys.Down || key == Keys.S) _isDownPressed = true;
            if (key == Keys.Left || key == Keys.A) _isLeftPressed = true;
            if (key == Keys.Right || key == Keys.D) _isRightPressed = true;
        }

        // KeyUp — 키를 떼면 해당 축 즉시 0 복귀
        public void KeyUp(Keys key)
        {
            if (key == Keys.Up || key == Keys.W) _isUpPressed = false;
            if (key == Keys.Down || key == Keys.S) _isDownPressed = false;
            if (key == Keys.Left || key == Keys.A) _isLeftPressed = false;
            if (key == Keys.Right || key == Keys.D) _isRightPressed = false;
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
            if (_mode == InputMode.Keyboard) UpdateKeyboard();

            else if (_mode == InputMode.Gamepad) UpdateGamepad();

            // 주기적으로 외부 이벤트 호출 (연결 keepalive 유지)
            OnInputChanged?.Invoke(Angle, Throttle);
        }

        private void UpdateKeyboard()
        {
            // 서서히 증가/감소 (Smoothing)
            if (_isUpPressed) Throttle = Math.Min(Throttle + 0.1f, 1.0f);
            else if (_isDownPressed) Throttle = Math.Max(Throttle - 0.1f, 1.0f * -1);
            else Throttle = 0f; // 키를 떼면 바로 0 (기존 방식 유지)

            if (_isLeftPressed) Angle = Math.Max(Angle - 0.1f, -1.0f);
            else if (_isRightPressed) Angle = Math.Min(Angle + 0.1f, 1.0f);
            else Angle = 0f;
        }

        private void UpdateGamepad()
        {
            if (_gamepad == null && Gamepad.Gamepads.Count > 0)
                _gamepad = Gamepad.Gamepads[0];

            if (_gamepad == null) return;

            GamepadReading reading = _gamepad.GetCurrentReading();

            float a = (float)reading.LeftThumbstickX;
            float t = (float)reading.LeftThumbstickY;

            Angle = Math.Abs(a) < 0.1f ? 0f : a;
            Throttle = Math.Abs(t) < 0.1f ? 0f : t;
        }
    }
}