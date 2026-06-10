using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataManager.Services.DataCollection.Controls
{
    public class VirtualJoystick : Control
    {
        private PointF _thumbPos = new PointF(0, 0);
        private bool _isDragging = false;
        private int _radius;
        private int _thumbRadius;  // 고정값 제거 — OnPaint에서 동적 계산
        public event Action<float, float> OnJoystickMoved;
        public VirtualJoystick()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer, true);
            Size = new Size(200, 200);
            BackColor = Color.FromArgb(30, 30, 30);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(BackColor);
            int cx = Width / 2;
            int cy = Height / 2;
            // 컨트롤 크기에 비례해서 썸 크기 결정 (짧은 변의 약 15%)
            _thumbRadius = Math.Max(8, Math.Min(cx, cy) / 5);
            _radius = Math.Min(cx, cy) - _thumbRadius - 5;
            if (_radius <= 0) return;
            // 바깥 원 (배경)
            g.FillEllipse(
                new SolidBrush(Color.FromArgb(50, 50, 50)),
                cx - _radius - _thumbRadius,
                cy - _radius - _thumbRadius,
                (_radius + _thumbRadius) * 2,
                (_radius + _thumbRadius) * 2);
            // 바깥 원 테두리
            g.DrawEllipse(
                new Pen(Color.Gray, 2),
                cx - _radius - _thumbRadius,
                cy - _radius - _thumbRadius,
                (_radius + _thumbRadius) * 2,
                (_radius + _thumbRadius) * 2);
            // 십자선
            var crossPen = new Pen(Color.FromArgb(80, 80, 80), 1);
            int outerR = _radius + _thumbRadius;
            g.DrawLine(crossPen, cx - outerR, cy, cx + outerR, cy);
            g.DrawLine(crossPen, cx, cy - outerR, cx, cy + outerR);
            // 중앙 작은 원 (크기도 비례)
            int centerDot = Math.Max(3, _thumbRadius / 4);
            g.DrawEllipse(new Pen(Color.FromArgb(70, 70, 70), 1),
                cx - centerDot, cy - centerDot,
                centerDot * 2, centerDot * 2);
            // 썸 위치 계산
            float tx = cx + _thumbPos.X * _radius;
            float ty = cy + _thumbPos.Y * _radius;
            // 썸 그림자
            g.FillEllipse(
                new SolidBrush(Color.FromArgb(30, 0, 0, 0)),
                tx - _thumbRadius + 2,
                ty - _thumbRadius + 2,
                _thumbRadius * 2,
                _thumbRadius * 2);
            // 썸 원
            g.FillEllipse(
                new SolidBrush(Color.DodgerBlue),
                tx - _thumbRadius,
                ty - _thumbRadius,
                _thumbRadius * 2,
                _thumbRadius * 2);
            // 썸 하이라이트 (크기도 비례)
            int highlight = Math.Max(4, _thumbRadius / 3);
            g.FillEllipse(
                new SolidBrush(Color.FromArgb(80, 255, 255, 255)),
                tx - _thumbRadius + 4,
                ty - _thumbRadius + 4,
                highlight,
                highlight);
            // 값 텍스트 (폰트 크기도 비례)
            float fontSize = Math.Max(6f, _thumbRadius * 0.4f);
            g.DrawString(
                $"X:{_thumbPos.X:F2}  Y:{-_thumbPos.Y:F2}",
                new Font("Arial", fontSize),
                Brushes.Gray,
                new RectangleF(0, Height - fontSize * 2.5f, Width, fontSize * 2),
                new StringFormat { Alignment = StringAlignment.Center });
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate(); // 크기 바뀌면 즉시 다시 그리기
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isDragging = true;
            UpdateThumb(e.X, e.Y);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging) UpdateThumb(e.X, e.Y);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            _thumbPos = new PointF(0, 0);
            Invalidate();
            OnJoystickMoved?.Invoke(0f, 0f);
        }
        private void UpdateThumb(int mx, int my)
        {
            int cx = Width / 2;
            int cy = Height / 2;
            float dx = mx - cx;
            float dy = my - cy;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);
            if (dist > _radius && _radius > 0)
            {
                dx = dx / dist * _radius;
                dy = dy / dist * _radius;
            }
            _thumbPos = _radius > 0
                ? new PointF(dx / _radius, dy / _radius)
                : new PointF(0, 0);
            Invalidate();
            OnJoystickMoved?.Invoke(_thumbPos.X, -_thumbPos.Y);
        }
    }
}
