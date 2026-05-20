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

        
        private readonly string baseEditedPath = Path.Combine(
        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName,
        "EditedData"
        );
        private string targetSavePath = string.Empty;


        public TubManagerUI()
        {
            InitializeComponent();
            InitializeCustomLogic();

            
            if (!Directory.Exists(baseEditedPath))
            {
                Directory.CreateDirectory(baseEditedPath);
            }
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
                _dialog.Description = "Donkey Car 주행 데이터를 선택하세요.";
            
                _dialog.UseDescriptionForTitle = true;
                _dialog.RootFolder = Environment.SpecialFolder.MyComputer;

                
                if (_dialog.ShowDialog() == DialogResult.OK)
                {
                
                    string _selectedFolderPath = _dialog.SelectedPath;
                    lblSaveRoute.Text = _selectedFolderPath;
                    LoadDonkeyCarData(_selectedFolderPath);
                
                }
            }
        }

        
        private void LoadDonkeyCarData(string path)
        {
        
            drivingData.Clear();
            
            if (!Directory.Exists(path)) return;

            
            string _imagesFolderPath = Path.Combine(path, "images");
            string[] _catalogFiles = Directory.GetFiles(path, "catalog_*.catalog");

            if (_catalogFiles.Length == 0)
            {
            
                _catalogFiles = Directory.GetFiles(path, "*.catalog");
            
            }

            if (_catalogFiles.Length == 0)
            {
            
                ReportLog("경고", "해당 폴더에 .catalog 파일이 존재하지 않습니다.");
                MessageBox.Show("해당 폴더 경로에 catalog 파일이 없습니다.", "알림");
                
                return;
            }

            var _sortedCatalogFiles = _catalogFiles.OrderBy(_f =>
            {
           
                string _filename = Path.GetFileNameWithoutExtension(_f);
                string _numStr = new string(_filename.Where(char.IsDigit).ToArray());
                
                if (int.TryParse(_numStr, out int _num)) return _num;
                
                return 0;
            }).ToList();

            int _globalIndex = 1;

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
                MessageBox.Show("유효한 자율주행 데이터를 읽어오지 못했습니다.", "알림");
                return;
            }

            trkProgress.Minimum = 0;
            trkProgress.Maximum = drivingData.Count - 1;
            trkProgress.Value = 0;

            UpdateChart();
            DisplayFrame(0);
            ReportLog("정보", $"데이터 로드 완료 (총 {drivingData.Count} 프레임)");
        }

        
        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            
            if (!Directory.Exists(baseEditedPath))
            {
                Directory.CreateDirectory(baseEditedPath);
            }

            
            Form _inputForm = new Form();
            _inputForm.Width = 400;
            _inputForm.Height = 150;
            _inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            _inputForm.Text = "새 폴더 생성";
            _inputForm.StartPosition = FormStartPosition.CenterParent;
            _inputForm.MaximizeBox = false;
            _inputForm.MinimizeBox = false;

            Label _lblText = new Label() { Left = 20, Top = 20, Width = 350, Text = "생성할 폴더 이름을 입력하세요 (EditedData 폴더 내부에 생성):" };

            
            TextBox _txtInput = new TextBox() { Left = 20, Top = 45, Width = 340, Text = "" };

            Button _btnOk = new Button() { Text = "확인", Left = 180, Width = 80, Top = 80, DialogResult = DialogResult.OK };
            Button _btnCancel = new Button() { Text = "취성", Left = 280, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

            _inputForm.Controls.Add(_lblText);
            _inputForm.Controls.Add(_txtInput);
            _inputForm.Controls.Add(_btnOk);
            _inputForm.Controls.Add(_btnCancel);
            _inputForm.AcceptButton = _btnOk; 
            _inputForm.CancelButton = _btnCancel;

            
            if (_inputForm.ShowDialog() == DialogResult.OK)
            {
                string _folderName = _txtInput.Text.Trim();

                
                if (string.IsNullOrEmpty(_folderName))
                {
                    MessageBox.Show("폴더 이름을 입력해야 합니다.", "알림");
                    return;
                }

                
                foreach (char _c in Path.GetInvalidFileNameChars())
                {
                    _folderName = _folderName.Replace(_c, '_');
                }

                
                string _finalNewFolderPath = Path.Combine(baseEditedPath, _folderName);

                try
                {
                    if (!Directory.Exists(_finalNewFolderPath))
                    {
                        
                        Directory.CreateDirectory(_finalNewFolderPath);

                        
                        targetSavePath = _finalNewFolderPath;
                        lblSaveRoute.Text = $"[저장지정] {_folderName}";

                        ReportLog("정보", $"새 폴더 생성 및 지정 완료: {_folderName}");
                        MessageBox.Show($"[{_folderName}] 폴더가 성공적으로 생성되고 저장 경로로 지정되었습니다.", "성공");
                    }
                    else
                    {
                        MessageBox.Show("이미 존재하는 폴더 이름입니다. 다른 이름을 사용해 주세요.", "알림");
                    }
                }
                catch (Exception _ex)
                {
                    ReportLog("오류", $"폴더 생성 실패: {_ex.Message}");
                    MessageBox.Show($"폴더 생성 중 오류 발생: {_ex.Message}", "에러");
                }
            }
        }

        
        private void btnDelFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog _dialog = new FolderBrowserDialog())
            {
                _dialog.AutoUpgradeEnabled = true;
                _dialog.Description = "삭제할 가공 폴더를 선택하세요 (EditedData 내부 폴더)";
                _dialog.SelectedPath = baseEditedPath;

                if (_dialog.ShowDialog() == DialogResult.OK)
                {
                    string _targetDelPath = _dialog.SelectedPath;

                    
                    if (!_targetDelPath.Contains(baseEditedPath) || _targetDelPath == baseEditedPath)
                    {
                        MessageBox.Show("EditedData 내부에 생성된 하위 가공 폴더만 삭제할 수 있습니다.", "보안 경고");
                        return;
                    }

                    string _folderName = Path.GetFileName(_targetDelPath);
                    DialogResult _confirm = MessageBox.Show($"[{_folderName}] 폴더와 내부 데이터 파일들을 정말 영구 삭제하시겠습니까?", "폴더 삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (_confirm == DialogResult.Yes)
                    {
                        try
                        {
                            Directory.Delete(_targetDelPath, true);
                            ReportLog("정보", $"편집 폴더 삭제 완료: {_folderName}");

                            if (targetSavePath == _targetDelPath)
                            {
                                targetSavePath = string.Empty;
                                lblSaveRoute.Text = "선택된 저장 경로 없음";
                            }
                            MessageBox.Show("폴더가 삭제되었습니다.", "성공");
                        }
                        catch (Exception _ex)
                        {
                            ReportLog("오류", $"폴더 삭제 실패: {_ex.Message}");
                        }
                    }
                }
            }
        }

        
        private void btnSaveRoute_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog _dialog = new FolderBrowserDialog())
            {
                _dialog.AutoUpgradeEnabled = true;
                _dialog.Description = "편집된 데이터를 저장할 하위 폴더를 지정해 주세요.";
                _dialog.SelectedPath = baseEditedPath;

                if (_dialog.ShowDialog() == DialogResult.OK)
                {
                    string _chosenPath = _dialog.SelectedPath;

                    if (!_chosenPath.Contains(baseEditedPath) || _chosenPath == baseEditedPath)
                    {
                        MessageBox.Show("반드시 EditedData 안에 존재하는 생성된 하위 폴더를 선택해야 합니다.", "알림");
                        return;
                    }

                    targetSavePath = _chosenPath;
                    lblSaveRoute.Text = $"[저장지정] {Path.GetFileName(targetSavePath)}";
                    ReportLog("정보", $"데이터 저장 경로 지정됨: {targetSavePath}");
                }
            }
        }

        
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (drivingData.Count == 0)
            {
                MessageBox.Show("저장할 주행 데이터가 존재하지 않습니다. 먼저 데이터를 로드하세요.", "알림");
                return;
            }

            if (string.IsNullOrEmpty(targetSavePath) || !Directory.Exists(targetSavePath))
            {
                MessageBox.Show("저장 경로가 지정되지 않았거나 올바르지 않습니다.\n'저장 경로 지정' 버튼을 먼저 눌러주세요.", "알림");
                return;
            }

            try
            {
                string _saveImagesDir = Path.Combine(targetSavePath, "images");
                if (!Directory.Exists(_saveImagesDir)) Directory.CreateDirectory(_saveImagesDir);

                string _catalogPath = Path.Combine(targetSavePath, "catalog_0.catalog");
                string _catalogManifestPath = Path.Combine(targetSavePath, "catalog_0.catalog_manifest");
                string _manifestJsonPath = Path.Combine(targetSavePath, "manifest.json");

                List<string> _catalogLines = new List<string>();

                foreach (var _frame in drivingData)
                {
                    string _fileNameOnly = Path.GetFileName(_frame.ImagePath);
                    string _destImagePath = Path.Combine(_saveImagesDir, _fileNameOnly);

                    if (File.Exists(_frame.ImagePath) && _frame.ImagePath != _destImagePath)
                    {
                        File.Copy(_frame.ImagePath, _destImagePath, true);
                    }

                    var _dataObj = new
                    {
                        @cam_image_array = _fileNameOnly,
                        @user_angle = _frame.Angle,
                        @user_throttle = _frame.Throttle
                    };

                    string _jsonLine = JsonSerializer.Serialize(_dataObj)
                        .Replace("cam_image_array", "cam/image_array")
                        .Replace("user_angle", "user/angle")
                        .Replace("user_throttle", "user/throttle");

                    _catalogLines.Add(_jsonLine);
                }

                File.WriteAllLines(_catalogPath, _catalogLines);
                File.WriteAllText(_catalogManifestPath, "{\"path\": \"catalog_0.catalog\", \"idx\": 0}");
                File.WriteAllText(_manifestJsonPath, "{\"format\": \"donkey_car\", \"version\": \"4.3.0\"}");

                ReportLog("정보", $"가공 완료 데이터 최종 저장 성공: {Path.GetFileName(targetSavePath)} (총 {drivingData.Count} 프레임)");
                MessageBox.Show("가공 및 편집된 주행 데이터 세트 저장이 완전히 끝났습니다!", "저장 완료");
            }
            catch (Exception _ex)
            {
                ReportLog("오류", $"데이터 저장 실패: {_ex.Message}");
                MessageBox.Show($"저장 중 오류 발생: {_ex.Message}", "에러");
            }
        }

        private void DisplayFrame(int index)
        {
            if (drivingData == null || drivingData.Count == 0 || index < 0 || index >= drivingData.Count) return;

            currentFrameIndex = index;
            var _frame = drivingData[index];

            double _anglePercentage = (_frame.Angle + 1.0) / 2.0 * 100.0;
            int _finalAngleVal = Math.Max(0, Math.Min(100, (int)_anglePercentage));
            prgAngle.Value = _finalAngleVal;

            double _throttlePercentage = (_frame.Throttle + 1.0) / 2.0 * 100.0;
            int _finalThrottleVal = Math.Max(0, Math.Min(100, (int)_throttlePercentage));
            prgThrottle.Value = _finalThrottleVal;

            lblAngleDetail.Text = _frame.Angle.ToString("F2");
            lblThrottleDetail.Text = _frame.Throttle.ToString("F2");

            lblAllImageNumRange.Text = $"({drivingData[0].Index}, {_frame.Index}, {drivingData[drivingData.Count - 1].Index})";
            trkProgress.Value = index;

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