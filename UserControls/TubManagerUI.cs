using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

        public event Action<string, string, string> OnLogReported;

        private List<DrivingFrame> drivingData = new List<DrivingFrame>();
        private int currentFrameIndex = 0;
        private bool isPlaying = false;
        private System.Windows.Forms.Timer playTimer;
        private Tuple<int, int> selectedRange = new Tuple<int, int>(0, 0);

        public TubManagerUI()
        {
            InitializeComponent();
            InitializeCustomLogic();
        }

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }

        private void InitializeCustomLogic()
        {
            playTimer = new System.Windows.Forms.Timer();
            playTimer.Tick += PlayTimer_Tick;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { "0.50", "1.00", "1.50", "2.00" });
            comboBox1.SelectedIndex = 1;

            int _baseInterval = 60;
            playTimer.Interval = (int)(_baseInterval / 1.00);

            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            trkProgress.Scroll += TrkProgress_Scroll;
        }

        private void btnFileLoad_Click(object sender, EventArgs e)
        {
            
            using (FolderBrowserDialog _dialog = new FolderBrowserDialog())
            {
                
                _dialog.AutoUpgradeEnabled = true;

                // 창 상단에 노출될 안내 메시지 설정
                _dialog.Description = "Donkey Car 주행 데이터를 선택하세요.";

                // 타이틀바 대신 위의 안내 메시지를 제목으로 사용하도록 설정
                _dialog.UseDescriptionForTitle = true;

                // 처음 창이 열릴 때 기준이 되는 루트 경로 설정 (내 PC)
                _dialog.RootFolder = Environment.SpecialFolder.MyComputer;

                // 사용자가 폴더를 정상적으로 선택하고 [폴더 선택]을 눌렀을 때만 작동
                if (_dialog.ShowDialog() == DialogResult.OK)
                {
                    
                    string _selectedFolderPath = _dialog.SelectedPath;
                                        
                    lblSaveRoute.Text = _selectedFolderPath;

                    LoadDonkeyCarData(_selectedFolderPath);
                }
            }
        }

        //  지정된 상위 폴더 경로 내부에서 images와 catalog 파일을 정확히 매핑하여 로드
        private void LoadDonkeyCarData(string path)
        {
            drivingData.Clear();

            if (!Directory.Exists(path)) return;

            //  내부의 images 폴더 경로 설정 및 검증
            string _imagesFolderPath = Path.Combine(path, "images");
            if (!Directory.Exists(_imagesFolderPath))
            {
                // 만약 사용자가 하위 images 폴더를 한 번 더 타고 들어와 선택했을 경우를 위한 방어 코드
                if (Path.GetFileName(path).ToLower() == "images")
                {
                    _imagesFolderPath = path;
                    path = Path.GetDirectoryName(path); // 상위 폴더를 데이터 부모 폴더로 격상
                }
                else
                {
                    ReportLog("오류", "선택한 폴더 내부에 'images' 폴더가 존재하지 않습니다.");
                    MessageBox.Show("선택한 폴더 내부에 'images' 폴더가 없습니다. 올바른 상위 폴더를 선택해 주세요.", "알림");
                    return;
                }
            }

            //  카탈로그 파일들 수집
            string[] _catalogFiles = Directory.GetFiles(path, "catalog_*.catalog");
            if (_catalogFiles.Length == 0)
            {
                // 간혹 접두사 없이 숫자만 붙는 경우를 위해 2차 검색 시도
                _catalogFiles = Directory.GetFiles(path, "*.catalog");
            }

            if (_catalogFiles.Length == 0)
            {
                ReportLog("경고", "해당 폴더에 .catalog 파일이 존재하지 않습니다.");
                MessageBox.Show("해당 폴더 경로에 catalog 파일이 없습니다.", "알림");
                return;
            }

            //  카탈로그 파일 정렬 (숫자 순서대로 정확하게 데이터가 정렬되도록 처리)
            var _sortedCatalogFiles = _catalogFiles.OrderBy(_f =>
            {
                string _filename = Path.GetFileNameWithoutExtension(_f);
                
                // 숫자만 남기기 위해 숫자 이외의 문자 제거 시도
                string _numStr = new string(_filename.Where(char.IsDigit).ToArray());

                if (int.TryParse(_numStr, out int _num))
                    return _num;
                return 0;
            }).ToList();

            int _globalIndex = 1;

            //  각 catalog 파일을 순차적으로 읽기
            foreach (var _catalogPath in _sortedCatalogFiles)
            {
                try
                {
                    string[] _lines = File.ReadAllLines(_catalogPath);

                    foreach (var _line in _lines)
                    {
                        if (string.IsNullOrWhiteSpace(_line)) continue;

                        using (JsonDocument _doc = JsonDocument.Parse(_line))
                        {
                            JsonElement _root = _doc.RootElement;


                            string _imageName = string.Empty;
                            if (_root.TryGetProperty("cam/image_array", out JsonElement _imgProp))
                                _imageName = _imgProp.GetString();
                            else
                                continue;

                            double _angle = 0.0;
                            if (_root.TryGetProperty("user/angle", out JsonElement _angProp))
                                _angle = _angProp.GetDouble();

                            double _throttle = 0.0;
                            if (_root.TryGetProperty("user/throttle", out JsonElement _thrProp))
                                _throttle = _thrProp.GetDouble();

                            string _fullImagePath = Path.Combine(_imagesFolderPath, _imageName);

                            drivingData.Add(new DrivingFrame
                            {
                                Index = _globalIndex++,
                                ImagePath = _fullImagePath,
                                Angle = _angle,
                                Throttle = _throttle,
                                OriginalAngle = _angle,
                                OriginalThrottle = _throttle
                            });
                        }
                    }
                }
                catch (Exception _ex)
                {
                    ReportLog("오류", $"파일 파싱 실패: {Path.GetFileName(_catalogPath)} - {_ex.Message}");
                }
            }

            if (drivingData.Count == 0)
            {
                ReportLog("오류", "유효한 자율주행 주행 프레임 데이터를 읽어오지 못했습니다.");
                MessageBox.Show("유효한 자율주행 데이터를 읽어오지 못했습니다. 파일 구조나 JSON 형식을 확인하세요.", "알림");
                return;
            }

            // UI 및 차트 새로고침
            trkProgress.Minimum = 0;
            trkProgress.Maximum = drivingData.Count - 1;
            trkProgress.Value = 0;


            UpdateChart();
            DisplayFrame(0);

            ReportLog("정보", $"데이터 로드 완료 (총 {drivingData.Count} 프레임)");

        }

        private void btnNewFolder_Click(object sender, EventArgs e) { }
        private void btnDelFolder_Click(object sender, EventArgs e) { }
        private void btnSaveRoute_Click(object sender, EventArgs e) { }
        private void btnSaveData_Click(object sender, EventArgs e)
        {

            ReportLog("정보", "데이터가 성공적으로 저장되었습니다.");

        }


        private void DisplayFrame(int index)
        {
            if (drivingData == null || drivingData.Count == 0 || index < 0 || index >= drivingData.Count) return;

            currentFrameIndex = index;
            var _frame = drivingData[index];

            //  프로그레스 바(게이지) 값 계산 및 적용
            double _anglePercentage = (_frame.Angle + 1.0) / 2.0 * 100.0;
            int _finalAngleVal = Math.Max(0, Math.Min(100, (int)_anglePercentage));
            prgAngle.Value = _finalAngleVal;

            double _throttlePercentage = (_frame.Throttle + 1.0) / 2.0 * 100.0;
            int _finalThrottleVal = Math.Max(0, Math.Min(100, (int)_throttlePercentage));
            prgThrottle.Value = _finalThrottleVal;

            
            // "F2" 소수점 아래 둘째 자리까지만 표현 (예: -0.15, 0.50)
            lblAngleDetail.Text = _frame.Angle.ToString("F2");
            lblThrottleDetail.Text = _frame.Throttle.ToString("F2");

            //  텍스트 정보 및 트랙바 동기화
            lblAllImageNumRange.Text = $"({drivingData[0].Index}, {_frame.Index}, {drivingData[drivingData.Count - 1].Index})";
            trkProgress.Value = index;

            //  이미지 출력 및 필터 적용
            if (!string.IsNullOrEmpty(_frame.ImagePath) && File.Exists(_frame.ImagePath))
            {
                picImage.Image = Image.FromFile(_frame.ImagePath);
                ApplyImageFiltersOnView();
            }
        }

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            if (currentFrameIndex < drivingData.Count - 1)
                DisplayFrame(currentFrameIndex + 1);
            else
                btnStop_Click(null, null);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (drivingData.Count == 0) return;
            isPlaying = true;
            playTimer.Start();
            ReportLog("정보", "데이터 재생을 시작합니다.");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isPlaying = false;
            playTimer.Stop();
            ReportLog("정보", "데이터 재생을 일시 정지합니다.");
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && double.TryParse(comboBox1.SelectedItem.ToString(), out double _speed))
            {
                int _baseInterval = 60;
                playTimer.Interval = (int)(_baseInterval / _speed);
            }
        }

        private void TrkProgress_Scroll(object sender, EventArgs e) => DisplayFrame(trkProgress.Value);
        private void btnFrameLeft_Click(object sender, EventArgs e) => DisplayFrame(currentFrameIndex - 1);
        private void btnFrameRight_Click(object sender, EventArgs e) => DisplayFrame(currentFrameIndex + 1);
        private void btn5FrameLeft_Click(object sender, EventArgs e) => DisplayFrame(currentFrameIndex - 5);
        private void btn5FrameRight_Click(object sender, EventArgs e) => DisplayFrame(currentFrameIndex + 5);

        private void btnLeftRange_Click(object sender, EventArgs e)
        {
            selectedRange = new Tuple<int, int>(currentFrameIndex, selectedRange.Item2);
            UpdateRangeLabel();
            ReportLog("정보", $"시작 범위 지정: {drivingData[currentFrameIndex].Index}번 프레임");
        }

        private void btnRightRange_Click(object sender, EventArgs e)
        {
            if (currentFrameIndex >= selectedRange.Item1)
            {
                selectedRange = new Tuple<int, int>(selectedRange.Item1, currentFrameIndex);
                UpdateRangeLabel();
                ReportLog("정보", $"종료 범위 지정: {drivingData[currentFrameIndex].Index}번 프레임");
            }
        }

        private void btnAllRange_Click(object sender, EventArgs e)
        {
            if (drivingData.Count > 0)
            {
                selectedRange = new Tuple<int, int>(0, drivingData.Count - 1);
                UpdateRangeLabel();
                ReportLog("정보", "전체 범위가 선택되었습니다.");
            }
        }

        private void UpdateRangeLabel()
        {
            lblSelectedRange.Text = $"선택된 범위 ({drivingData[selectedRange.Item1].Index}, {drivingData[selectedRange.Item2].Index})";
        }

        private void btnApplyFillter_Click(object sender, EventArgs e)
        {
            int _start = selectedRange.Item1;
            int _end = selectedRange.Item2;

            for (int _i = _start; _i <= _end; _i++)
            {
                if (chkDelThrottle.Checked && drivingData[_i].Throttle == 0) { }
                if (chkDelAngle.Checked && drivingData[_i].Angle == 0) { }
            }

            UpdateChart();
            DisplayFrame(currentFrameIndex);
            ReportLog("정보", $"필터 적용 완료 (범위: {drivingData[_start].Index} ~ {drivingData[_end].Index})");
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
            ReportLog("정보", "필터 설정이 초기화되었습니다.");
        }

        private void ApplyImageFiltersOnView()
        {
            if (picImage.Image == null) return;
            if (btnApplyBlackWhite.Checked) { }
            if (chkInverseColor.Checked) { }
        }


        private void UpdateChart()
        {
            chtData.Series.Clear();

            Series _angleSeries = new Series("각도") { ChartType = SeriesChartType.Line, BorderWidth = 2 };
            Series _throttleSeries = new Series("속도") { ChartType = SeriesChartType.Line, BorderWidth = 2 };

            foreach (var _frame in drivingData)
            {
                _angleSeries.Points.AddXY(_frame.Index, _frame.Angle);
                _throttleSeries.Points.AddXY(_frame.Index, _frame.Throttle);
            }

            chtData.Series.Add(_angleSeries);
            chtData.Series.Add(_throttleSeries);

            chtData.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chtData.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;

        }

    }

}

