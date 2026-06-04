using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataManager.Services.DataCollection.Controls
{
    public class DirectionBar : Control
    {
        private float _value = 0f; // -1 ~ 1
        private string _label = "";
        private bool _isVertical = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public float Value
        {
            get => _value;
            set { _value = Math.Clamp(value, -1f, 1f); Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string Label
        {
            get => _label;
            set { _label = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool IsVertical
        {
            get => _isVertical;
            set { _isVertical = value; Invalidate(); }
        }
        public DirectionBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            int w = Width, h = Height;
            if (!_isVertical)
            {
                // ─── 가로 바 (조향용) ───
                int barH = h - 24;
                int cx = w / 2;
                int barY = 4;
                // 배경
                g.FillRectangle(new SolidBrush(Color.FromArgb(40, 40, 40)),
                    0, barY, w, barH);
                // 중앙선
                g.DrawLine(new Pen(Color.Gray, 1),
                    cx, barY, cx, barY + barH);
                // 값 바
                if (_value != 0)
                {
                    int barW = (int)(Math.Abs(_value) * cx);
                    int barX = _value < 0 ? cx - barW : cx;
                    var color = _value < 0 ? Color.DodgerBlue : Color.OrangeRed;
                    g.FillRectangle(new SolidBrush(color),
                        barX, barY + 2, barW, barH - 4);
                }
                // 라벨
                g.DrawString($"{_label}: {_value:F2}",
                    new Font("Arial", 8), Brushes.White,
                    new RectangleF(0, barY + barH + 2, w, 16),
                    new StringFormat { Alignment = StringAlignment.Center });
            }
            else
            {
                // ─── 세로 바 (스로틀용) ───
                int barW = w - 24;
                int cy = h / 2;
                int barX = 4;
                // 배경
                g.FillRectangle(new SolidBrush(Color.FromArgb(40, 40, 40)),
                    barX, 0, barW, h);
                // 중앙선
                g.DrawLine(new Pen(Color.Gray, 1),
                    barX, cy, barX + barW, cy);
                // 값 바 (위가 전진 = 양수)
                if (_value != 0)
                {
                    int barH = (int)(Math.Abs(_value) * cy);
                    int barY = _value > 0 ? cy - barH : cy;
                    var color = _value > 0 ? Color.LimeGreen : Color.OrangeRed;
                    g.FillRectangle(new SolidBrush(color),
                        barX + 2, barY, barW - 4, barH);
                }
                // 라벨
                g.DrawString($"{_label}\n{_value:F2}",
                    new Font("Arial", 8), Brushes.White,
                    new RectangleF(barX + barW + 2, 0, 22, h),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
            }
        }
    }
}
