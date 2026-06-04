using System;
using System.Windows.Forms;
using SharpDX.XInput;

namespace DataManager.Services.DataCollection.Services
{
    public class DriveInputService
    {
        public enum InputMode { Keyboard, Joystick, Gamepad }
        private InputMode _mode = InputMode.Keyboard;
        private bool _keyUp, _keyDown, _keyLeft, _keyRight;
        private Controller _gamepad;
        private System.Windows.Forms.Timer _timer;
        public float Angle { get; private set; }
        public float Throttle { get; private set; }
        public event Action<float, float> OnInputChanged;
        public void SetMode(InputMode mode)
        {
            _mode = mode;
            Angle = 0f;
            Throttle = 0f;
            _gamepad = mode == InputMode.Gamepad
                ? new Controller(UserIndex.One)
                : null;
        }
        public void Start()
        {
            _timer = new System.Windows.Forms.Timer { Interval = 50 };
            _timer.Tick += Update;
            _timer.Start();
        }
        public void Stop() => _timer?.Stop();
        // 키보드 이벤트 전달 (Form에서 호출)
        public void KeyDown(Keys key)
        {
            if (_mode != InputMode.Keyboard) return;
            switch (key)
            {
                case Keys.Up: _keyUp = true; break;
                case Keys.Down: _keyDown = true; break;
                case Keys.Left: _keyLeft = true; break;
                case Keys.Right: _keyRight = true; break;
                case Keys.Space: Angle = 0; Throttle = 0; break;
            }
        }
        public void KeyUp(Keys key)
        {
            if (_mode != InputMode.Keyboard) return;
            switch (key)
            {
                case Keys.Up: _keyUp = false; break;
                case Keys.Down: _keyDown = false; break;
                case Keys.Left: _keyLeft = false; break;
                case Keys.Right: _keyRight = false; break;
            }
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
            // Joystick은 SetJoystick()에서 직접 업데이트
            OnInputChanged?.Invoke(Angle, Throttle);
        }
        private void UpdateKeyboard()
        {
            float accel = 0.03f, decel = 0.02f, steerSpeed = 0.08f;
            Throttle = _keyUp ? Math.Min(Throttle + accel, 0.7f)
                     : _keyDown ? Math.Max(Throttle - accel, -0.3f)
                     : Throttle > 0 ? Math.Max(Throttle - decel, 0f)
                                   : Math.Min(Throttle + decel, 0f);
            Angle = _keyLeft ? Math.Max(Angle - steerSpeed, -1f)
                  : _keyRight ? Math.Min(Angle + steerSpeed, 1f)
                  : Angle > 0 ? Math.Max(Angle - steerSpeed * 0.5f, 0f)
                              : Math.Min(Angle + steerSpeed * 0.5f, 0f);
        }
        private void UpdateGamepad()
        {
            if (_gamepad == null || !_gamepad.IsConnected) return;
            var gp = _gamepad.GetState().Gamepad;
            float a = gp.LeftThumbX / 32767f;
            float t = (gp.RightTrigger - gp.LeftTrigger) / 255f;
            Angle = Math.Abs(a) < 0.1f ? 0f : a;
            Throttle = Math.Abs(t) < 0.05f ? 0f : t;
        }
    }
}
