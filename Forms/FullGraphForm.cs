using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataManager.Forms
{
    public class FullGraphForm : Form
    {
        private Chart chartAngle;
        private Chart chartThrottle;
        private Chart chartError;
        private double _currentFrameX = 0;

        private static readonly Color[] PilotColors =
        {
            Color.OrangeRed,
            Color.MediumSeaGreen,
            Color.MediumPurple,
        };

        private static readonly Color[] AngleErrorColors   = { Color.Coral,          Color.OrangeRed,    Color.DarkSalmon };
        private static readonly Color[] ThrottleErrorColors = { Color.MediumSeaGreen, Color.CornflowerBlue, Color.MediumPurple };

        public FullGraphForm(
            List<(int Index, double Angle, double Throttle)> userFrames,
            List<(string ModelName, Dictionary<int, double> Angles, Dictionary<int, double> Throttles)> pilotData)
        {
            Text = "전체 구간 그래프";
            Size = new Size(1100, 720);
            MinimumSize = new Size(700, 500);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            BuildLayout();
            PopulateCharts(userFrames, pilotData);
        }

        private void BuildLayout()
        {
            chartAngle    = CreateChart("조향각 (Angle)");
            chartThrottle = CreateChart("가속값 (Throttle)");
            chartError    = CreateErrorChart();

            AttachFrameIndicator(chartAngle);
            AttachFrameIndicator(chartThrottle);
            AttachFrameIndicator(chartError);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(6),
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40f));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40f));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            layout.Controls.Add(chartAngle,    0, 0);
            layout.Controls.Add(chartThrottle, 0, 1);
            layout.Controls.Add(chartError,    0, 2);

            Controls.Add(layout);
        }

        // PostPaint로 직접 그려서 항상 2px 고정 너비 유지
        private void AttachFrameIndicator(Chart chart)
        {
            chart.PostPaint += (s, e) =>
            {
                if (IsDisposed) return;
                try
                {
                    var area = chart.ChartAreas[0];
                    float xPx = (float)area.AxisX.ValueToPixelPosition(_currentFrameX);

                    float h = chart.Height;
                    float top    = h * (area.Position.Y + area.InnerPlotPosition.Y    * area.Position.Height / 100f) / 100f;
                    float bottom = top + h * area.InnerPlotPosition.Height * area.Position.Height / 10000f;

                    using var pen = new Pen(Color.FromArgb(210, Color.Crimson), 2f);
                    e.ChartGraphics.Graphics.DrawLine(pen, xPx, top, xPx, bottom);
                }
                catch { }
            };
        }

        public void UpdateCurrentFrame(int frameIndex)
        {
            if (IsDisposed) return;
            _currentFrameX = frameIndex;
            chartAngle.Invalidate();
            chartThrottle.Invalidate();
            chartError.Invalidate();
        }

        private static Chart CreateErrorChart()
        {
            var chart = new Chart { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 4) };
            chart.AntiAliasing = AntiAliasingStyles.None;
            chart.IsSoftShadows = false;

            var area = new ChartArea("Main");
            area.AxisX.Title = "Frame";
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MinorGrid.Enabled = false;
            area.AxisY.Title = "오차";
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 1.2;
            area.AxisY.Interval = 0.3;
            area.AxisY.IsStartedFromZero = true;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("Main") { Docking = Docking.Bottom });
            return chart;
        }

        private static Chart CreateChart(string title)
        {
            var chart = new Chart { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 4) };
            chart.AntiAliasing = AntiAliasingStyles.None;
            chart.IsSoftShadows = false;

            var area = new ChartArea("Main");
            area.AxisX.Title = "Frame";
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MinorGrid.Enabled = false;
            area.AxisY.Title = title;
            area.AxisY.Minimum = -1.2;
            area.AxisY.Maximum = 1.2;
            area.AxisY.Interval = 0.4;
            area.AxisY.IsStartedFromZero = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            // 0 기준선
            area.AxisY.StripLines.Add(new StripLine
            {
                IntervalOffset = 0,
                StripWidth = 0.01,
                BackColor = Color.FromArgb(80, Color.DimGray),
                Interval = 0
            });

            chart.ChartAreas.Add(area);

            var legend = new Legend("Main") { Docking = Docking.Bottom };
            chart.Legends.Add(legend);

            return chart;
        }

        public void RefreshData(
            List<(int Index, double Angle, double Throttle)> userFrames,
            List<(string ModelName, Dictionary<int, double> Angles, Dictionary<int, double> Throttles)> pilotData)
        {
            chartAngle.Series.Clear();
            chartThrottle.Series.Clear();
            chartError.Series.Clear();
            PopulateCharts(userFrames, pilotData);
        }

        private void PopulateCharts(
            List<(int Index, double Angle, double Throttle)> userFrames,
            List<(string ModelName, Dictionary<int, double> Angles, Dictionary<int, double> Throttles)> pilotData)
        {
            if (userFrames.Count == 0) return;

            int minIdx = userFrames[0].Index;
            int maxIdx = userFrames[userFrames.Count - 1].Index;

            SetAxisRange(chartAngle,    minIdx, maxIdx);
            SetAxisRange(chartThrottle, minIdx, maxIdx);
            SetAxisRange(chartError,    minIdx, maxIdx);

            // 사용자 데이터
            var userAngleSeries    = MakeSeries("사용자 조향각", Color.SteelBlue, 2, "Main");
            var userThrottleSeries = MakeSeries("사용자 가속값", Color.SteelBlue, 2, "Main");

            foreach (var f in userFrames)
            {
                userAngleSeries.Points.AddXY(f.Index, f.Angle);
                userThrottleSeries.Points.AddXY(f.Index, f.Throttle);
            }

            chartAngle.Series.Add(userAngleSeries);
            chartThrottle.Series.Add(userThrottleSeries);

            // 사용자 데이터 조회용 딕셔너리 (오차 계산에 사용)
            var userAngleMap    = userFrames.ToDictionary(f => f.Index, f => f.Angle);
            var userThrottleMap = userFrames.ToDictionary(f => f.Index, f => f.Throttle);

            // 파일럿(모델) 데이터 + 오차 차트
            for (int i = 0; i < pilotData.Count; i++)
            {
                var (name, angles, throttles) = pilotData[i];
                Color color = PilotColors[i % PilotColors.Length];
                string label = string.IsNullOrEmpty(name) ? $"모델 {i + 1}" : name;

                var aSeries = MakeSeries($"{label} 조향각", color, 2, "Main");
                var tSeries = MakeSeries($"{label} 가속값", color, 2, "Main");

                var aErrSeries = MakeSeries($"{label} 조향각 오차", AngleErrorColors[i % AngleErrorColors.Length],     2, "Main");
                var tErrSeries = MakeSeries($"{label} 가속값 오차", ThrottleErrorColors[i % ThrottleErrorColors.Length], 2, "Main");

                for (int idx = minIdx; idx <= maxIdx; idx++)
                {
                    if (angles.TryGetValue(idx, out double a))
                    {
                        aSeries.Points.AddXY(idx, a);
                        if (userAngleMap.TryGetValue(idx, out double ua))
                            aErrSeries.Points.AddXY(idx, Math.Abs(ua - a));
                    }
                    if (throttles.TryGetValue(idx, out double t))
                    {
                        tSeries.Points.AddXY(idx, t);
                        if (userThrottleMap.TryGetValue(idx, out double ut))
                            tErrSeries.Points.AddXY(idx, Math.Abs(ut - t));
                    }
                }

                chartAngle.Series.Add(aSeries);
                chartThrottle.Series.Add(tSeries);
                chartError.Series.Add(aErrSeries);
                chartError.Series.Add(tErrSeries);
            }
        }

        private static void SetAxisRange(Chart chart, int min, int max)
        {
            var area = chart.ChartAreas[0];
            area.AxisX.Minimum = min;
            area.AxisX.Maximum = max;

            int range = max - min;
            area.AxisX.Interval = Math.Max(1, range / 20);
        }

        private static Series MakeSeries(string name, Color color, int borderWidth, string chartArea)
        {
            return new Series(name)
            {
                ChartType = SeriesChartType.Line,
                Color = color,
                BorderWidth = borderWidth,
                ChartArea = chartArea,
                Legend = "Main",
                MarkerStyle = MarkerStyle.None,
            };
        }
    }
}
