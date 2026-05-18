using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.Json;

namespace DataManager.UserControls
{
    public partial class TubManagerUI : UserControl
    {

        public class DrivingFrame
        {
            public int Index { get; set; }
            public string ImagePath { get; set; }
            public double Angle { get; set; }
            public double Throttle { get; set; }
            public double OriginalAngle { get; set; }
            public double OriginalThrottle { get; set; }
        }

        private List<DrivingFrame> _drivingData = new List<DrivingFrame>();
        private int _currentFrameIndex = 0;
        private bool _isPlaying = false;
        private System.Windows.Forms.Timer _playTimer;
        private Tuple<int, int> _selectedRange = new Tuple<int, int>(0, 0);


        public TubManagerUI()
        {
            InitializeComponent();
            InitializeCustomLogic(); // 커스텀 초기화 로직 호출
        }

        private void InitializeCustomLogic()
        {
            //  재생 타이머 생성 및 이벤트 연결
            _playTimer = new System.Windows.Forms.Timer();
            _playTimer.Tick += PlayTimer_Tick;

            //  배속 설정 초기화 
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "0.50", "1.00", "1.50", "2.00" });


            comboBox1.SelectedIndex = 1;


            int baseInterval = 60;
            _playTimer.Interval = (int)(baseInterval / 1.00);

            // 배속이 바뀌었을 때 타이머 속도도 같이 바뀌도록 이벤트 연결
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            // 슬라이더 변경 이벤트 연결
            trkProgress.Scroll += TrkProgress_Scroll;
        }


        //  파일 및 폴더 관리 

        private void btnFileLoad_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    lblSaveRoute.Text = fbd.SelectedPath;
                    LoadDonkeyCarData(fbd.SelectedPath);
                }
            }
        }

        private void LoadDonkeyCarData(string path)
        {
            _drivingData.Clear();

            if (!Directory.Exists(path)) return;

            //  images 폴더 경로와 catalog 파일들이 있는지 확인
            string imagesFolderPath = Path.Combine(path, "images");
            string[] catalogFiles = Directory.GetFiles(path, "catalog_*.catalog");

            if (catalogFiles.Length == 0)
            {
                MessageBox.Show("해당 폴더에 catalog_*.catalog 파일이 없습니다.", "알림");
                return;
            }

            //  catalog_0, catalog_1 순서대로 파일 정렬
            var sortedCatalogFiles = catalogFiles.OrderBy(f =>
            {
                string filename = Path.GetFileNameWithoutExtension(f); // "catalog_0.catalog" -> "catalog_0"
                string numStr = filename.Replace("catalog_", "");
                int.TryParse(numStr, out int num);
                return num;
            }).ToList();

            int globalIndex = 1;

            // 각 catalog 파일을 순차적으로 읽기
            foreach (var catalogPath in sortedCatalogFiles)
            {
                try
                {
                    // 파일의 모든 줄을 읽어옵니다 (한 줄이 데이터 1개 프레임)
                    string[] lines = File.ReadAllLines(catalogPath);

                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // 한 줄(JSON) 파싱
                        using (JsonDocument doc = JsonDocument.Parse(line))
                        {
                            JsonElement root = doc.RootElement;

                            // 키값 추출 (Donkey Car 표준 포맷)
                            string imageName = root.GetProperty("cam/image_array").GetString();
                            double angle = root.GetProperty("user/angle").GetDouble();
                            double throttle = root.GetProperty("user/throttle").GetDouble();


                            string fullImagePath = Path.Combine(imagesFolderPath, imageName);

                            // 리스트에 데이터 추가
                            _drivingData.Add(new DrivingFrame
                            {
                                Index = globalIndex++,
                                ImagePath = fullImagePath,
                                Angle = angle,
                                Throttle = throttle,
                                OriginalAngle = angle,
                                OriginalThrottle = throttle
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Catalog 파일 파싱 실패: {catalogPath}, 에러: {ex.Message}");
                }
            }

            if (_drivingData.Count == 0)
            {
                MessageBox.Show("유효한 자율주행 데이터를 읽어오지 못했습니다.", "알림");
                return;
            }


            //  UI 및 차트 새로고침

            trkProgress.Minimum = 0;
            trkProgress.Maximum = _drivingData.Count - 1;
            trkProgress.Value = 0;

            // 하단 그래프에 실제 주행 내역 전체 시각화
            UpdateChart();

            // 첫 번째 프레임 이미지 및 데이터 로드
            DisplayFrame(0);
        }

        private void btnNewFolder_Click(object sender, EventArgs e) { }
        private void btnDelFolder_Click(object sender, EventArgs e) { }
        private void btnSaveRoute_Click(object sender, EventArgs e) { }
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            MessageBox.Show("데이터가 성공적으로 저장되었습니다.", "알림");
        }


        //  이미지 표시 및 재생 제어

        private void DisplayFrame(int index)
        {
            if (_drivingData == null || _drivingData.Count == 0 || index < 0 || index >= _drivingData.Count) return;

            _currentFrameIndex = index;
            var frame = _drivingData[index];

            //  각도(Angle): 범위가 -1.0(최좌측) ~ 1.0(최우측) 일 때 -> 0 ~ 100으로 변환

            double anglePercentage = (frame.Angle + 1.0) / 2.0 * 100.0;


            int finalAngleVal = Math.Max(0, Math.Min(100, (int)anglePercentage));
            prgAngle.Value = finalAngleVal;


            // 속도(Throttle): 범위가 0.0(정지) ~ 1.0(최고속도) 일 때 -> 0 ~ 100으로 변환

            double throttlePercentage = (frame.Throttle + 1.0) / 2.0 * 100.0;

            int finalThrottleVal = Math.Max(0, Math.Min(100, (int)throttlePercentage));
            prgThrottle.Value = finalThrottleVal;




            lblAllImageNumRange.Text = $"({_drivingData[0].Index}, {frame.Index}, {_drivingData[_drivingData.Count - 1].Index})";
            trkProgress.Value = index;

            if (!string.IsNullOrEmpty(frame.ImagePath) && File.Exists(frame.ImagePath))
            {
                picImage.Image = Image.FromFile(frame.ImagePath);
                ApplyImageFiltersOnView();
            }
        }

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            if (_currentFrameIndex < _drivingData.Count - 1)
                DisplayFrame(_currentFrameIndex + 1);
            else
                btnStop_Click(null, null);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_drivingData.Count == 0) return;
            _isPlaying = true;
            _playTimer.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _isPlaying = false;
            _playTimer.Stop();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && double.TryParse(comboBox1.SelectedItem.ToString(), out double speed))
            {

                int baseInterval = 60;
                _playTimer.Interval = (int)(baseInterval / speed);
            }
        }

        private void TrkProgress_Scroll(object sender, EventArgs e) => DisplayFrame(trkProgress.Value);
        private void btnFrameLeft_Click(object sender, EventArgs e) => DisplayFrame(_currentFrameIndex - 1);
        private void btnFrameRight_Click(object sender, EventArgs e) => DisplayFrame(_currentFrameIndex + 1);
        private void btn5FrameLeft_Click(object sender, EventArgs e) => DisplayFrame(_currentFrameIndex - 5);
        private void btn5FrameRight_Click(object sender, EventArgs e) => DisplayFrame(_currentFrameIndex + 5);


        //  범위 선택 및 데이터 변형 필터 기능 (Fillter)

        private void btnLeftRange_Click(object sender, EventArgs e)
        {
            _selectedRange = new Tuple<int, int>(_currentFrameIndex, _selectedRange.Item2);
            UpdateRangeLabel();
        }

        private void btnRightRange_Click(object sender, EventArgs e)
        {
            if (_currentFrameIndex >= _selectedRange.Item1)
            {
                _selectedRange = new Tuple<int, int>(_selectedRange.Item1, _currentFrameIndex);
                UpdateRangeLabel();
            }
        }

        private void btnAllRange_Click(object sender, EventArgs e)
        {
            if (_drivingData.Count > 0)
            {
                _selectedRange = new Tuple<int, int>(0, _drivingData.Count - 1);
                UpdateRangeLabel();
            }
        }

        private void UpdateRangeLabel()
        {
            lblSelectedRange.Text = $"선택된 범위 ({_drivingData[_selectedRange.Item1].Index}, {_drivingData[_selectedRange.Item2].Index})";
        }

        private void btnApplyFillter_Click(object sender, EventArgs e)
        {
            int start = _selectedRange.Item1;
            int end = _selectedRange.Item2;

            for (int i = start; i <= end; i++)
            {
                if (chkDelThrottle.Checked && _drivingData[i].Throttle == 0) { }
                if (chkDelAngle.Checked && _drivingData[i].Angle == 0) { }
            }

            UpdateChart();
            DisplayFrame(_currentFrameIndex);
            MessageBox.Show("선택 범위 내 데이터 필터가 적용되었습니다.", "알림");
        }

        private void btnCancelFillter_Click(object sender, EventArgs e) { }
        private void btnInitFillterSet_Click(object sender, EventArgs e)
        {
            chkDelThrottle.Checked = false;
            chkDelAngle.Checked = false;
            chkInverseColor.Checked = false;
            btnApplyBlackWhite.Checked = false;
            chkSetBright.Checked = false;
            chkSetBlur.Checked = false;
            trkSetBright.Value = trkSetBright.Minimum;
            trkSetBlur.Value = trkSetBlur.Minimum;
        }

        private void ApplyImageFiltersOnView()
        {
            if (picImage.Image == null) return;
            if (btnApplyBlackWhite.Checked) { }
            if (chkInverseColor.Checked) { }
        }


        //  차트 업데이트 로직

        private void UpdateChart()
        {
            chtData.Series.Clear();

            Series angleSeries = new Series("각도") { ChartType = SeriesChartType.Line, BorderWidth = 2 };
            Series throttleSeries = new Series("속도") { ChartType = SeriesChartType.Line, BorderWidth = 2 };

            foreach (var frame in _drivingData)
            {
                angleSeries.Points.AddXY(frame.Index, frame.Angle);
                throttleSeries.Points.AddXY(frame.Index, frame.Throttle);
            }

            chtData.Series.Add(angleSeries);
            chtData.Series.Add(throttleSeries);

            chtData.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chtData.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
        }
    }
}

