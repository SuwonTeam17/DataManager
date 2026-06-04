using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DataManager.UserControls
{
    public class GaugeBar : Control
    {
        private double _value = 0.0;

        public double Minimum { get; set; } = -1.0;
        public double Maximum { get; set; } = 1.0;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Value
        {
            get => _value;
            set
            {
                _value = Math.Clamp(value, Minimum, Maximum);
                Invalidate();
            }
        }

        public GaugeBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Size = new Size(138, 15);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            var rect = ClientRectangle;

            using (var bgBrush = new SolidBrush(Color.FromArgb(220, 220, 220)))
                g.FillRectangle(bgBrush, rect);

            int centerX = rect.Width / 2;
            double absMax = Math.Max(Math.Abs(Minimum), Math.Abs(Maximum));
            int barHalfWidth = absMax > 0 ? (int)(_value / absMax * (rect.Width / 2.0)) : 0;

            if (barHalfWidth != 0)
            {
                Rectangle barRect;
                Color barColor;

                if (barHalfWidth > 0)
                {
                    barRect = new Rectangle(centerX, 2, barHalfWidth, rect.Height - 4);
                    barColor = Color.SteelBlue;
                }
                else
                {
                    barRect = new Rectangle(centerX + barHalfWidth, 2, -barHalfWidth, rect.Height - 4);
                    barColor = Color.IndianRed;
                }

                using (var barBrush = new SolidBrush(barColor))
                    g.FillRectangle(barBrush, barRect);
            }

            using (var centerPen = new Pen(Color.DimGray, 2))
                g.DrawLine(centerPen, centerX, 0, centerX, rect.Height);

            using (var borderPen = new Pen(Color.DarkGray, 1))
                g.DrawRectangle(borderPen, 0, 0, rect.Width - 1, rect.Height - 1);
        }
    }
}
