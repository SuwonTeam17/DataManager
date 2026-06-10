using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.Json;
using System.Drawing.Imaging;


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

            // ── 추가 필드 ──────────────────────────
            public string SessionId { get; set; }
            public long TimestampMs { get; set; }
            public string Mode { get; set; }

            // ── [신규 추가] 필터 상태 기억용 ──
            public bool IsInverted { get; set; }  // 색상 반전 여부
            public bool IsGrayscale { get; set; } // 흑백 적용 여부

        }

        public event Action<string, string, string> OnLogReported;

        private List<DrivingFrame> drivingData = new List<DrivingFrame>();
        private int currentFrameIndex = 0;
        private bool isPlaying = false;
        private bool isRangePlaying = false; // [추가] 구간 재생 중인지 여부 변수
        private System.Windows.Forms.Timer playTimer;
        // [기존 코드 제거] private Tuple<int, int> selectedRange = new Tuple<int, int>(0, 0);

        // [신규 변수 등록]
        public class RangeSegment
        {
            public int Start { get; set; }
            public int End { get; set; }
            public RangeSegment(int start, int end) { Start = start; End = end; }
        }

        private List<RangeSegment> selectedRanges = new List<RangeSegment>();
        private int activeRangeIndex = -1; // 현재 편집/재생 중인 구간 (-1이면 없음)
        // 임시 필터 적용 이미지 저장소 (key: drivingData 인덱스, value: 가공된 Bitmap)
        private Dictionary<int, Bitmap> filteredFrameMap = new Dictionary<int, Bitmap>();

        // 필터로 인해 "숨김 처리"할 프레임 인덱스 집합 (실제 삭제 X, 뷰에서만 스킵)
        private HashSet<int> filteredHideSet = new HashSet<int>();

        // 기존 코드 근처에 아래 2줄을 추가합니다.
        private HashSet<int> filteredInvertSet = new HashSet<int>();
        private HashSet<int> filteredGrayscaleSet = new HashSet<int>();


        private readonly string baseEditedPath = AppPaths.EditedData;
        private string targetSavePath = string.Empty;

        // ── 타임라인 드래그 상태 변수 추가 ──
        private bool isDraggingTimeline = false;


        // ── 기존 frameStep을 double로 변경하고 누적 변수 추가 ──────────────────
        private double frameStep = 1.0;       // 1배속: 1.0, 2배속: 2.0, 0.5배속: 0.5
        private double accumulatedFrame = 0.0; // 소수점 프레임 누적 계산용 변수

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
            trkSetBright.Minimum = -100;
            trkSetBright.Maximum = 100;
            trkSetBright.Value = 0;

            trkSetBlur.Minimum = 0;
            trkSetBlur.Maximum = 10;
            trkSetBlur.Value = 0;

            UpdateFilterCheckboxTexts();

            trkSetBright.ValueChanged += (s, e) => UpdateFilterCheckboxTexts();
            trkSetBlur.ValueChanged += (s, e) => UpdateFilterCheckboxTexts();


            playTimer = new System.Windows.Forms.Timer();
            playTimer.Tick += PlayTimer_Tick;

            // 배속을 직관적인 정수 형태로 선택할 수 있게 콤보박스 아이템 설정
            comboBox1.Items.Clear();
            // 5.00, 10.00, 20.00 배속 항목 추가
            comboBox1.Items.AddRange(new object[] { "0.50", "1.00", "2.00", "3.00", "4.00", "5.00", "10.00", "20.00" });
            comboBox1.SelectedIndex = 1; // 기본값 1.00배속 위치

            // 배속과 상관없이 타이머 주기는 60ms로 고정! (디스크 부하 방지)
            playTimer.Interval = 60;

            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            trkProgress.Scroll += TrkProgress_Scroll;

            // 타임라인 패널 깜빡임 방지 (더블 버퍼링 켜기)
            if (pnlTimeStamp != null)
            {
                System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control)
                    .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                aProp.SetValue(pnlTimeStamp, true, null);
            }

            // ── 타임라인 이벤트 등록 ──
            if (pnlTimeStamp != null)
            {
                pnlTimeStamp.Paint += PnlTimeStamp_Paint;
                pnlTimeStamp.MouseDown += PnlTimeStamp_MouseDown;
                pnlTimeStamp.MouseMove += PnlTimeStamp_MouseMove;
                pnlTimeStamp.MouseUp += PnlTimeStamp_MouseUp;

                // ⭐ [추가] 화면 확장/축소 시 타임라인을 다시 그리도록 이벤트 연결
                pnlTimeStamp.Resize += PnlTimeStamp_Resize;
            }
        }

        private void btnFileLoad_Click(object sender, EventArgs e)
        {
            string root = AppPaths.MycarData;
            if (!Directory.Exists(root))
            {
                MessageBox.Show($"mycar/data 폴더를 찾을 수 없습니다.\n경로: {root}", "알림");
                return;
            }
            using (var browser = new CustomFolderBrowser(root, "주행 데이터 폴더 선택"))
            {
                browser.AllowFileSelection = true;

                if (browser.ShowDialog(this) == DialogResult.OK)
                    LoadDonkeyCarData(browser.SelectedPath);
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

                ReportLog("오류", "해당 폴더에 .catalog 파일이 존재하지 않습니다.");
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

                            // 기존 _angle, _throttle 파싱 아래에 추가
                            string _mode = "user";
                            if (_root.TryGetProperty("user/mode", out JsonElement _modeProp))
                                _mode = _modeProp.GetString();

                            string _sessionId = string.Empty;
                            if (_root.TryGetProperty("_session_id", out JsonElement _sessionProp))
                                _sessionId = _sessionProp.GetString();

                            long _timestampMs = 0;
                            if (_root.TryGetProperty("_timestamp_ms", out JsonElement _tsProp))
                                _timestampMs = _tsProp.GetInt64();



                            string _fullImagePath = Path.Combine(_imagesFolderPath, _imageName);

                            drivingData.Add(new DrivingFrame
                            {
                                Index = _globalIndex++,
                                ImagePath = _fullImagePath,
                                Angle = _angle,
                                Throttle = _throttle,
                                OriginalAngle = _angle,
                                OriginalThrottle = _throttle,
                                SessionId = _sessionId,     // 추가
                                Mode = _mode,               // 추가
                                TimestampMs = _timestampMs  // 추가
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

            pnlTimeStamp?.Invalidate();

            ReportLog("알림", $"데이터 로드 완료 (총 {drivingData.Count} 프레임)");
        }





        private void btnDelFolder_Click(object sender, EventArgs e)
        {
            string root = AppPaths.EditedData;
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            using (var browser = new CustomFolderBrowser(root, "삭제할 폴더 선택 (EditedData 내부 폴더)"))
            {
                browser.AllowFileSelection = true;

                if (browser.ShowDialog(this) != DialogResult.OK) return;

                string _targetDelPath = browser.SelectedPath;

                // 맨 뒤에 인수로 'ReportLog' 메서드 자체를 전달합니다!
                // 이제 CustomFolderBrowser 내부에서 알아서 ReportLog를 실시간으로 작동시킵니다.
                bool isDeleted = CustomFolderBrowser.SafeDeleteDirectoryImmediate(
                    _targetDelPath,
                    root,
                    "mycar/data/EditedData",
                    ReportLog
                );

                // 삭제 성공 시 남은 UI 상태 처리만 깔끔하게 수행
                if (isDeleted)
                {
                    if (targetSavePath == _targetDelPath)
                    {
                        targetSavePath = string.Empty;
                        lblSaveRoute.Text = "선택된 저장 경로 없음";
                    }
                }
            }
        }


        private void btnSaveRoute_Click(object sender, EventArgs e)
        {
            string root = AppPaths.EditedData;
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            using (var browser = new CustomFolderBrowser(root, "저장 경로 선택 (EditedData 내 하위 폴더)"))
            {
                browser.AllowFileSelection = true;

                if (browser.ShowDialog(this) == DialogResult.OK)
                {
                    string chosen = browser.SelectedPath;
                    if (chosen.Equals(root, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("EditedData 안에 있는 하위 폴더를 선택해야 합니다.", "알림");
                        return;
                    }
                    targetSavePath = chosen;
                    lblSaveRoute.Text = $"[저장 경로] {Path.GetFileName(targetSavePath)}";
                    ReportLog("알림", $"데이터 저장 경로 지정됨: {targetSavePath}");
                }
            }
        }


        private void btnSaveData_Click(object sender, EventArgs e)
        {
            // 1. 저장할 데이터가 존재하는지 검사
            if (drivingData.Count == 0)
            {
                MessageBox.Show("저장할 주행 데이터가 존재하지 않습니다. 먼저 데이터를 로드하세요.", "알림");
                return;
            }

            // 2. 저장 경로가 지정되지 않았거나 폴더가 존재하지 않는 경우 새 폴더 생성창 호출
            if (string.IsNullOrEmpty(targetSavePath) || !Directory.Exists(targetSavePath))
            {
                // 상위 저장 경로인 baseEditedPath가 없으면 미리 생성
                if (!Directory.Exists(baseEditedPath))
                {
                    Directory.CreateDirectory(baseEditedPath);
                }

                using (Form _inputForm = new Form())
                {
                    _inputForm.Width = 400;
                    _inputForm.Height = 150;
                    _inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    _inputForm.Text = "새 폴더 생성 및 저장 경로 지정";
                    _inputForm.StartPosition = FormStartPosition.CenterParent;
                    _inputForm.MaximizeBox = false;
                    _inputForm.MinimizeBox = false;

                    Label _lblText = new Label() { Left = 20, Top = 20, Width = 350, Text = "경로가 지정되지 않았습니다. 새 폴더 이름을 입력하세요:" };
                    TextBox _txtInput = new TextBox() { Left = 20, Top = 45, Width = 340, Text = "" };
                    Button _btnOk = new Button() { Text = "확인", Left = 180, Width = 80, Top = 80, DialogResult = DialogResult.OK };
                    Button _btnCancel = new Button() { Text = "취소", Left = 280, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

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
                            MessageBox.Show("폴더 이름을 입력해야 합니다. 저장이 취소됩니다.", "알림");
                            return;
                        }

                        // 폴더명에 사용할 수 없는 특수문자 치환
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
                                lblSaveRoute.Text = $"[저장 경로] {_folderName}";

                                ReportLog("알림", $"새 폴더 생성 및 지정 완료: {_folderName}");
                                MessageBox.Show($"[{_folderName}] 폴더가 생성되었습니다.\n이어서 데이터 저장을 진행합니다.", "성공");
                            }
                            else
                            {
                                // 이미 존재하는 폴더인 경우 활용 여부 확인
                                DialogResult _useExisting = MessageBox.Show("이미 존재하는 폴더 이름입니다. 이 폴더를 저장 경로로 사용하여 저장을 진행하시겠습니까?", "알림", MessageBoxButtons.YesNo);
                                if (_useExisting == DialogResult.Yes)
                                {
                                    targetSavePath = _finalNewFolderPath;
                                    lblSaveRoute.Text = $"[저장 경로] {_folderName}";
                                }
                                else
                                {
                                    ReportLog("알림", "폴더 이름 중복으로 데이터 저장이 취소되었습니다.");
                                    return;
                                }
                            }
                        }
                        catch (Exception _ex)
                        {
                            ReportLog("오류", $"폴더 생성 실패: {_ex.Message}");
                            MessageBox.Show($"폴더 생성 중 오류가 발생하여 저장이 취소됩니다.\n{_ex.Message}", "오류");
                            return;
                        }
                    }
                    else
                    {
                        ReportLog("알림", "경로가 지정되지 않아 데이터 저장이 취소되었습니다.");
                        return;
                    }
                }
            }

            // 3. 실제 데이터 저장 프로세스 실행 (기존 원본 로직 완벽 반영)
            try
            {
                string _saveImagesDir = Path.Combine(targetSavePath, "images");
                string _catalogPath = Path.Combine(targetSavePath, "catalog_0.catalog");
                string _catalogManifestPath = Path.Combine(targetSavePath, "catalog_0.catalog_manifest");
                string _manifestJsonPath = Path.Combine(targetSavePath, "manifest.json");

                bool _isContinue = chkSaveContinue.Checked;
                int _startIndex = 0;
                List<string> _catalogLines = new List<string>();

                // 이어쓰기(Continue) 모드 처리
                if (_isContinue)
                {
                    if (Directory.Exists(_saveImagesDir))
                    {
                        int _existingCount = Directory.GetFiles(_saveImagesDir).Length;
                        _startIndex = _existingCount;
                    }
                    else
                    {
                        Directory.CreateDirectory(_saveImagesDir);
                    }

                    if (File.Exists(_catalogPath))
                    {
                        _catalogLines.AddRange(File.ReadAllLines(_catalogPath));
                    }
                }
                else
                {
                    // 새로 저장 모드인 경우 기존 폴더 비우고 새로 생성
                    if (Directory.Exists(_saveImagesDir))
                        Directory.Delete(_saveImagesDir, true);
                    Directory.CreateDirectory(_saveImagesDir);
                }

                int _savedCount = 0;
                List<int> _lineLengths = new List<int>();

                // 이어쓰기일 때 기존 라인들의 바이트 길이 계산 보존
                if (_isContinue && _catalogLines.Count > 0)
                {
                    foreach (var line in _catalogLines)
                    {
                        _lineLengths.Add(System.Text.Encoding.UTF8.GetByteCount(line) + 1); // +1은 줄바꿈(\n) 반영
                    }
                }

                // 프레임 순회 및 저장
                for (int _i = 0; _i < drivingData.Count; _i++)
                {
                    if (filteredHideSet.Contains(_i))
                        continue;

                    var _frame = drivingData[_i];
                    int _newIndex = _startIndex + _savedCount;
                    string _ext = Path.GetExtension(_frame.ImagePath);
                    string _newFileName = $"{_newIndex}_cam_image_array_{_ext}";
                    string _destPath = Path.Combine(_saveImagesDir, _newFileName);

                    // 편집(필터링 등)된 이미지가 존재하면 변경된 비트맵 저장, 없으면 원본 복사
                    if (filteredFrameMap.TryGetValue(_i, out Bitmap _filteredBmp))
                    {
                        ImageFormat _format;
                        switch (_ext.ToLower())
                        {
                            case ".png": _format = ImageFormat.Png; break;
                            case ".bmp": _format = ImageFormat.Bmp; break;
                            default: _format = ImageFormat.Jpeg; break;
                        }
                        _filteredBmp.Save(_destPath, _format);
                    }
                    else
                    {
                        if (!File.Exists(_frame.ImagePath)) continue;
                        File.Copy(_frame.ImagePath, _destPath, true);
                    }

                    // JSON 라인 생성 (OrderedDictionary 순서 보장)
                    var _ordered = new System.Collections.Specialized.OrderedDictionary
            {
                { "_index",          _newIndex },
                { "_session_id",     _frame.SessionId ?? string.Empty },
                { "_timestamp_ms",   _frame.TimestampMs },
                { "cam/image_array", _newFileName },
                { "user/angle",      _frame.Angle },
                { "user/mode",       _frame.Mode ?? "user" },
                { "user/throttle",   _frame.Throttle }
            };

                    var _sb = new System.Text.StringBuilder();
                    _sb.Append("{");
                    bool _first = true;
                    foreach (System.Collections.DictionaryEntry _kv in _ordered)
                    {
                        if (!_first) _sb.Append(", ");
                        _first = false;
                        string _key = JsonSerializer.Serialize(_kv.Key.ToString());
                        string _val;
                        switch (_kv.Value)
                        {
                            case int v: _val = v.ToString(); break;
                            case long v: _val = v.ToString(); break;
                            case double v: _val = JsonSerializer.Serialize(v); break;
                            default: _val = JsonSerializer.Serialize(_kv.Value?.ToString() ?? ""); break;
                        }
                        _sb.Append($"{_key}: {_val}");
                    }
                    _sb.Append("}");

                    string _jsonLine = _sb.ToString();
                    _catalogLines.Add(_jsonLine);

                    int _lineByteCount = System.Text.Encoding.UTF8.GetByteCount(_jsonLine) + 1;
                    _lineLengths.Add(_lineByteCount);

                    _savedCount++;
                }

                // 파일 쓰기 작업
                File.WriteAllLines(_catalogPath, _catalogLines);

                double _unixTimestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                // catalog_manifest 파일 생성
                var _mb = new System.Text.StringBuilder();
                _mb.Append("{");
                _mb.Append($"\"created_at\": {_unixTimestamp}, ");
                _mb.Append("\"line_lengths\": [");
                _mb.Append(string.Join(", ", _lineLengths));
                _mb.Append("], ");
                _mb.Append("\"path\": \"catalog_0.catalog\", ");
                _mb.Append("\"start_index\": 0");
                _mb.Append("}");
                File.WriteAllText(_catalogManifestPath, _mb.ToString());

                // manifest.json 파일 생성
                var _manifestLines = new List<string>
        {
            "[\"cam/image_array\", \"user/angle\", \"user/throttle\", \"user/mode\"]",
            "[\"image_array\", \"float\", \"float\", \"str\"]",
            "{}",
            $"{{\"created_at\": {_unixTimestamp}}}",
            $"{{\"paths\": [\"catalog_0.catalog\"], \"current_index\": {_startIndex + _savedCount}, \"max_len\": 1000, \"deleted_indexes\": []}}"
        };
                File.WriteAllLines(_manifestJsonPath, _manifestLines);

                string _modeText = _isContinue ? "이어쓰기" : "새로 저장";
                ReportLog("정보", $"데이터 저장 완료 [{_modeText}] (총 {_savedCount} 프레임, {_startIndex}번부터 시작)");
                MessageBox.Show($"저장 완료! [{_modeText}]\n{_startIndex}번 ~ {_startIndex + _savedCount - 1}번 프레임이 안전하게 저장되었습니다.", "저장 완료");
            }
            catch (Exception _ex)
            {
                ReportLog("오류", $"데이터 저장 실패: {_ex.Message}");
                MessageBox.Show($"저장 중 오류가 발생했습니다:\n{_ex.Message}", "오류");
            }
        }

        private void DisplayFrame(int index, int direction = 0)
        {
            if (drivingData == null || drivingData.Count == 0) return;

            index = FindNearestVisibleIndex(index, direction);
            if (index < 0) return;

            currentFrameIndex = index;
            var _frame = drivingData[index];

            double _anglePercentage = (_frame.Angle + 1.0) / 2.0 * 100.0;
            prgAngle.Value = Math.Max(0, Math.Min(100, (int)_anglePercentage));

            double _throttlePercentage = (_frame.Throttle + 1.0) / 2.0 * 100.0;
            prgThrottle.Value = Math.Max(0, Math.Min(100, (int)_throttlePercentage));

            lblAngleDetail.Text = _frame.Angle.ToString("F2");
            lblThrottleDetail.Text = _frame.Throttle.ToString("F2");

            // 현재 보이는 프레임 기준으로 레이블 갱신
            UpdateAllImageNumRangeLabel(index);

            trkProgress.Value = index;

            // ── 타임라인 갱신 추가 ──
            pnlTimeStamp?.Invalidate();


            if (filteredFrameMap.TryGetValue(index, out Bitmap _filtered))
            {
                if (picImage.Image != null)
                {
                    picImage.Image.Dispose();
                    picImage.Image = null;
                }

                picImage.Image = new Bitmap(_filtered);
            }
            else if (!string.IsNullOrEmpty(_frame.ImagePath) && File.Exists(_frame.ImagePath))
            {
                if (picImage.Image != null)
                {
                    picImage.Image.Dispose();
                    picImage.Image = null;
                }

                using (var _img = Image.FromFile(_frame.ImagePath))
                {
                    picImage.Image = new Bitmap(_img);
                }
            }
        }

        /// <summary>
        /// 주어진 index에서 가장 가까운 숨김되지 않은 프레임 인덱스를 반환.
        /// 앞뒤로 탐색하며, 전부 숨김이면 -1 반환.
        /// </summary>
        /// <summary>
        /// direction: +1 = 앞으로, -1 = 뒤로, 0 = 양방향(가장 가까운 것)
        /// </summary>
        private int FindNearestVisibleIndex(int index, int direction = 0)
        {
            if (drivingData.Count == 0) return -1;

            // 클램프
            index = Math.Max(0, Math.Min(drivingData.Count - 1, index));

            if (!filteredHideSet.Contains(index)) return index;

            if (direction == 1)
            {
                for (int _i = index + 1; _i < drivingData.Count; _i++)
                    if (!filteredHideSet.Contains(_i)) return _i;
                // 앞으로 더 없으면 뒤로 탐색
                for (int _i = index - 1; _i >= 0; _i--)
                    if (!filteredHideSet.Contains(_i)) return _i;
            }
            else if (direction == -1)
            {
                for (int _i = index - 1; _i >= 0; _i--)
                    if (!filteredHideSet.Contains(_i)) return _i;
                // 뒤로 더 없으면 앞으로 탐색
                for (int _i = index + 1; _i < drivingData.Count; _i++)
                    if (!filteredHideSet.Contains(_i)) return _i;
            }
            else // direction == 0, 양방향
            {
                int _fw = index + 1, _bw = index - 1;
                while (_fw < drivingData.Count || _bw >= 0)
                {
                    if (_fw < drivingData.Count && !filteredHideSet.Contains(_fw)) return _fw;
                    if (_bw >= 0 && !filteredHideSet.Contains(_bw)) return _bw;
                    _fw++; _bw--;
                }
            }
            return -1; // 전부 숨김
        }

        // 재생 타이머: 항상 앞방향
        // 재생 타이머: 항상 앞방향
        // 재생 타이머: 항상 앞방향
        // 재생 타이머 이벤트
        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            if (drivingData == null || drivingData.Count == 0)
            {
                StopPlayback();
                return;
            }

            // 1. 현재 타이머 틱에서 이동해야 할 배속(정수+소수점)을 누적합산합니다.
            accumulatedFrame += frameStep;

            // 2. 누적된 값에서 정수 부분(실제 이동할 프레임 수)만 추출합니다.
            int stepsToMove = (int)Math.Floor(accumulatedFrame);

            // 3. 만약 0.5배속이라서 아직 정수 값이 1 이상 쌓이지 않았다면, 다음 틱을 기다립니다.
            if (stepsToMove < 1)
            {
                return;
            }

            // 4. 소수점 아래 남은 자릿수만 남기고 정수 부분은 차감합니다.
            accumulatedFrame -= stepsToMove;

            // 5. 계산된 정수 step만큼 현재 인덱스에서 전진합니다.
            int nextTargetIndex = currentFrameIndex + stepsToMove;
            int _next = FindNearestVisibleIndex(nextTargetIndex, direction: 1);

            // [구간 재생 모드]
            if (isRangePlaying)
            {
                // [수정] 현재 활성화된 다중 구간 인덱스가 안전한 범위 내에 있는지 검사
                if (selectedRanges != null && activeRangeIndex >= 0 && activeRangeIndex < selectedRanges.Count)
                {
                    var currentRange = selectedRanges[activeRangeIndex];

                    // 현재 활성화된 구간의 End 범위 안인지 체크
                    if (_next > currentFrameIndex && _next <= currentRange.End && _next < drivingData.Count)
                    {
                        DisplayFrame(_next, direction: 1);
                    }
                    else
                    {
                        // 현재 구간의 Start 지점으로 되돌아감
                        currentFrameIndex = currentRange.Start;
                        accumulatedFrame = 0.0; // 구간 반복 시 누적치 초기화

                        int startFrame = FindNearestVisibleIndex(currentFrameIndex, direction: 1);
                        if (startFrame >= currentRange.Start && startFrame <= currentRange.End)
                        {
                            DisplayFrame(startFrame, direction: 1);
                        }
                        ReportLog("알림", $"구간 [{activeRangeIndex + 1}]의 끝에 도달하여 처음부터 다시 반복 재생합니다.");
                    }
                }
                else
                {
                    // 예외 방어: 활성화된 구간 정보가 유효하지 않다면 구간 재생을 중단합니다.
                    StopPlayback();
                    ReportLog("경고", "활성화된 재생 구간 정보가 없어 재생을 중단합니다.");
                }
            }
            // [일반 재생 모드]
            else
            {
                if (_next > currentFrameIndex && _next < drivingData.Count)
                {
                    DisplayFrame(_next, direction: 1);
                }
                else
                {
                    StopPlayback();
                }
            }
        }

        private void StopPlayback()
        {
            playTimer.Stop();
            isPlaying = false;
            isRangePlaying = false; // 반복 재생 상태도 안전하게 해제

            // 일반 재생 버튼 디자인 원상복구 (초록색)
            if (btnPlay != null)
            {
                btnPlay.FlatStyle = FlatStyle.Flat;
                btnPlay.FlatAppearance.BorderSize = 0;
                btnPlay.Text = "▶ 재생";
                btnPlay.ForeColor = Color.White;
                btnPlay.BackColor = Color.FromArgb(72, 175, 120);
            }

            // 구간 재생 버튼 디자인 원상복구 (보라색)
            if (btnRangePlay != null)
            {
                btnRangePlay.FlatStyle = FlatStyle.Flat;
                btnRangePlay.FlatAppearance.BorderSize = 0;
                btnRangePlay.Text = "🔁 구간 재생";
                btnRangePlay.ForeColor = Color.White;
                btnRangePlay.BackColor = Color.FromArgb(142, 68, 173);
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (drivingData == null || drivingData.Count == 0) return;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.FlatAppearance.BorderSize = 0;

            // 1. 만약 '구간 재생 중'이었다면 -> 일반 재생으로 모드 전환
            if (isRangePlaying)
            {
                isRangePlaying = false;
                isPlaying = true;
                btnPlay.Text = "■ 정지";
                btnPlay.ForeColor = Color.White;
                btnPlay.BackColor = Color.FromArgb(210, 70, 70);

                if (btnRangePlay != null)
                {
                    btnRangePlay.Text = "🔁 구간 재생";
                    btnRangePlay.BackColor = Color.FromArgb(142, 68, 173);
                }

                ReportLog("알림", "구간 재생을 취소하고 이어서 일반 재생을 시작합니다.");
                return;
            }

            // 2. 일반 재생 중일 때 버튼을 누르면 -> 정지 처리
            if (isPlaying || playTimer.Enabled)
            {
                StopPlayback();
            }
            // 3. 완전히 멈춰있는 상태에서 버튼을 누르면 -> 일반 재생 시작
            else
            {
                playTimer.Stop(); // 타이머 안전 초기화

                // ⭐ [추가된 로직] 마지막 프레임까지 도달한 상태에서 재생을 누르면 처음부터 재생
                // FindNearestVisibleIndex를 이용해 마지막 유효 프레임 위치를 확인합니다.
                int lastVisibleIdx = FindNearestVisibleIndex(drivingData.Count - 1, direction: -1);
                if (currentFrameIndex >= lastVisibleIdx)
                {
                    currentFrameIndex = 0; // 인덱스를 처음으로 초기화
                                           // 첫 번째 유효한 프레임을 찾아 화면을 갱신합니다.
                    int firstVisibleIdx = FindNearestVisibleIndex(0, direction: 1);
                    if (firstVisibleIdx >= 0)
                    {
                        DisplayFrame(firstVisibleIdx, direction: 1);
                    }
                    ReportLog("알림", "마지막 프레임에 도달하여 처음부터 다시 재생합니다.");
                }

                // 0.5배속 지원을 위한 소수점 누적 버퍼 초기화
                accumulatedFrame = 0.0;

                isPlaying = true;
                playTimer.Start();

                btnPlay.Text = "■ 정지";
                btnPlay.ForeColor = Color.White;
                btnPlay.BackColor = Color.FromArgb(210, 70, 70); // 빨간색

                ReportLog("알림", "데이터 재생을 시작합니다.");
            }
        }

        private void btnRangePlay_Click(object sender, EventArgs e)
        {
            if (drivingData == null || drivingData.Count == 0) return;

            // [수정] 다중 구간 유효성 검사 (구간 목록이 비어있거나 활성화된 구간 인덱스가 잘못된 경우)
            if (selectedRanges == null || selectedRanges.Count == 0 || activeRangeIndex < 0 || activeRangeIndex >= selectedRanges.Count)
            {
                ReportLog("경고", "활성화된(선택된) 재생 구간이 없습니다. 타임라인의 구간을 클릭하거나 새로운 구간을 설정하세요.");
                return;
            }

            var currentRange = selectedRanges[activeRangeIndex];

            btnRangePlay.FlatStyle = FlatStyle.Flat;
            btnRangePlay.FlatAppearance.BorderSize = 0;

            // 1. 만약 '일반 재생 중'이었다면 -> 일반 재생을 끄고 즉시 구간 재생 모드로 가로챕니다.
            if (isPlaying)
            {
                isPlaying = false;
                isRangePlaying = true;

                // 현재 프레임 위치가 활성화된 구간 범위를 벗어나 있다면 구간의 시작점으로 강제 점프
                if (currentFrameIndex < currentRange.Start || currentFrameIndex >= currentRange.End)
                {
                    currentFrameIndex = currentRange.Start;
                    DisplayFrame(currentFrameIndex, direction: 1);
                }

                // 구간 재생 버튼을 '■ 구간 정지 (빨간색)' 상태로 변경
                btnRangePlay.Text = "■ 구간 정지";
                btnRangePlay.ForeColor = Color.White;
                btnRangePlay.BackColor = Color.FromArgb(210, 70, 70);

                // 일반 재생 버튼은 대기 상태(초록색)로 돌려놓음
                if (btnPlay != null)
                {
                    btnPlay.Text = "▶ 재생";
                    btnPlay.BackColor = Color.FromArgb(72, 175, 120);
                }

                ReportLog("알림", $"일반 재생을 중단하고 즉시 지정된 구간 [{activeRangeIndex + 1}] 재생으로 전환합니다.");
                return; // 전환 완료 후 메서드 종료
            }

            // 2. 이미 구간 재생 중일 때 버튼을 누르면 -> 정지 처리
            if (isRangePlaying || (playTimer.Enabled && isRangePlaying))
            {
                StopPlayback();
                return;
            }

            // 3. 완전히 멈춰있는 상태에서 버튼을 누르면 -> 구간 재생 시작
            playTimer.Stop();
            isRangePlaying = true;

            if (currentFrameIndex < currentRange.Start || currentFrameIndex >= currentRange.End)
            {
                currentFrameIndex = currentRange.Start;
                DisplayFrame(currentFrameIndex, direction: 1);
            }

            playTimer.Start();

            btnRangePlay.Text = "■ 구간 정지";
            btnRangePlay.ForeColor = Color.White;
            btnRangePlay.BackColor = Color.FromArgb(210, 70, 70);

            if (btnPlay != null)
            {
                btnPlay.Text = "▶ 재생";
                btnPlay.BackColor = Color.FromArgb(72, 175, 120);
            }

            ReportLog("알림", $"[{activeRangeIndex + 1}번 구간] {currentRange.Start}번부터 {currentRange.End}번까지 구간 재생을 시작합니다.");
        }



        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && double.TryParse(comboBox1.SelectedItem.ToString(), out double _speed))
            {
                // 이제 반올림하지 않고 소수점 배속을 그대로 저장합니다.
                frameStep = _speed;
                accumulatedFrame = 0.0; // 배속이 바뀔 때 누적치 초기화
            }
        }

        // 이동 버튼: 방향 전달
        private void TrkProgress_Scroll(object sender, EventArgs e) => DisplayFrame(trkProgress.Value, 0);
        private void btnFrameLeft_Click(object sender, EventArgs e) => DisplayFrame(currentFrameIndex - 1, -1);
        private void btnFrameRight_Click(object sender, EventArgs e) => DisplayFrame(currentFrameIndex + 1, 1);
        private void btn5FrameLeft_Click(object sender, EventArgs e) => DisplayFrame(currentFrameIndex - 5, -1);
        private void btn5FrameRight_Click(object sender, EventArgs e) => DisplayFrame(currentFrameIndex + 5, 1);

        private void btnLeftRange_Click(object sender, EventArgs e)
        {
            // [방어 코드] 주행 데이터가 로드되지 않았다면 무시
            if (drivingData == null || drivingData.Count == 0) return;

            // 활성화된 구간이 없다면 새로 생성하여 추가합니다.
            if (activeRangeIndex == -1 || selectedRanges.Count == 0)
            {
                selectedRanges.Add(new RangeSegment(currentFrameIndex, currentFrameIndex));
                activeRangeIndex = selectedRanges.Count - 1;
                ReportLog("알림", $"새로운 구간[{activeRangeIndex + 1}]의 시작 범위를 지정했습니다.");
            }
            else
            {
                // 이미 활성화된 구간이 존재하는 경우 데이터 수정
                var currentRange = selectedRanges[activeRangeIndex];
                if (currentFrameIndex > currentRange.End)
                {
                    currentRange.End = currentFrameIndex;
                }
                currentRange.Start = currentFrameIndex;
                ReportLog("알림", $"구간[{activeRangeIndex + 1}]의 시작 범위 수정: {drivingData[currentFrameIndex].Index}번 프레임");
            }

            UpdateRangeLabel();
        }

        private void btnRightRange_Click(object sender, EventArgs e)
        {
            // [방어 코드] 주행 데이터가 로드되지 않았다면 무시
            if (drivingData == null || drivingData.Count == 0) return;

            // 만약 구간이 아예 생성되지 않은 상태에서 오른쪽 버튼을 눌렀다면 새로운 구간을 시작점=끝점으로 생성합니다.
            if (activeRangeIndex == -1 || selectedRanges.Count == 0)
            {
                selectedRanges.Add(new RangeSegment(currentFrameIndex, currentFrameIndex));
                activeRangeIndex = selectedRanges.Count - 1;
                ReportLog("알림", $"새로운 구간[{activeRangeIndex + 1}]의 종료 범위를 지정했습니다.");
            }
            else
            {
                // 이미 활성화된 구간이 존재하는 경우 데이터 수정
                var currentRange = selectedRanges[activeRangeIndex];
                if (currentFrameIndex < currentRange.Start)
                {
                    currentRange.Start = currentFrameIndex;
                }
                currentRange.End = currentFrameIndex;
                ReportLog("알림", $"구간[{activeRangeIndex + 1}]의 종료 범위 수정: {drivingData[currentFrameIndex].Index}번 프레임");
            }

            UpdateRangeLabel();
        }



        private void btnAllRange_Click(object sender, EventArgs e)
        {
            if (drivingData == null || drivingData.Count == 0) return;

            selectedRanges.Clear(); // 기존 다중 구간 초기화
            selectedRanges.Add(new RangeSegment(0, drivingData.Count - 1));
            activeRangeIndex = 0;

            UpdateRangeLabel();
            ReportLog("알림", "기존 구간 목록을 비우고 전체 범위가 선택되었습니다.");
        }

        private void UpdateRangeLabel()
        {
            // [방어 코드] 주행 데이터가 없으면 실행 안 함
            if (drivingData == null || drivingData.Count == 0) return;

            // 만약 등록된 구간이 하나도 없거나 포커스가 유효하지 않은 경우
            if (selectedRanges == null || selectedRanges.Count == 0 || activeRangeIndex == -1)
            {
                lblSelectedRange.Text = "선택된 범위 없음";
                pnlTimeStamp?.Invalidate();
                return;
            }

            // 현재 활성화된 구간을 기준으로 예외 처리 및 표시 작업 수행
            var currentRange = selectedRanges[activeRangeIndex];
            int _start = currentRange.Start;
            int _end = currentRange.End;

            // 혹시 모를 인덱스 초과 및 역전 현상 최종 방어
            if (_start < 0) _start = 0;
            if (_end >= drivingData.Count) _end = drivingData.Count - 1;
            if (_start > _end) _start = _end;

            // 수정한 값 객체에 실시간 업데이트 반영
            currentRange.Start = _start;
            currentRange.End = _end;

            int _startDisplayIdx = drivingData[_start].Index;
            int _endDisplayIdx = drivingData[_end].Index;

            // 라벨 텍스트 표현 가공 (예시: 구간 [1 / 3] 선택됨 (100, 450))
            lblSelectedRange.Text = $"구간 [{activeRangeIndex + 1}/{selectedRanges.Count}] ({_startDisplayIdx}, {_endDisplayIdx})";

            // ── 타임라인 갱신 추가 ──
            pnlTimeStamp?.Invalidate();
        }

        private void btnApplyFillter_Click(object sender, EventArgs e)
        {
            if (drivingData == null || drivingData.Count == 0) return;

            // [수정] 활성화된 특정 구간이 없거나 인덱스가 올바르지 않으면 경고 후 리턴
            if (activeRangeIndex == -1 || selectedRanges == null || activeRangeIndex >= selectedRanges.Count)
            {
                MessageBox.Show("필터를 적용할 구간을 먼저 선택하거나 생성하세요.", "알림");
                return;
            }

            // 1단계: 현재 '선택된 단 하나의 구간'에 대해서만 이전 임시 결과 초기화 수행
            var activeRange = selectedRanges[activeRangeIndex];
            int _start = activeRange.Start;
            int _end = activeRange.End;

            for (int _i = _start; _i <= _end; _i++)
            {
                if (_i < 0 || _i >= drivingData.Count) continue; // 배열 인덱스 초과 방지
                if (filteredFrameMap.TryGetValue(_i, out Bitmap _old))
                {
                    _old.Dispose();
                    filteredFrameMap.Remove(_i);
                }
                filteredHideSet.Remove(_i);
                filteredInvertSet.Remove(_i);
                filteredGrayscaleSet.Remove(_i);
            }

            bool _anyImageFilter = chkInverseColor.Checked || chkApplyBlackWhite.Checked || chkSetBright.Checked || chkSetBlur.Checked;
            int totalHiddenCount = 0;
            int totalFilteredCount = 0;

            // 2단계: 현재 '선택된 단 하나의 구간'만 순회하며 필터 및 변환 작업 적용
            for (int _i = _start; _i <= _end; _i++)
            {
                if (_i < 0 || _i >= drivingData.Count) continue; // 배열 인덱스 초과 방지
                var _frame = drivingData[_i];

                // ── 이미지 변환 필터 상태(선) 먼저 기록 ──
                if (chkApplyBlackWhite.Checked)
                {
                    filteredGrayscaleSet.Add(_i);
                }
                if (chkInverseColor.Checked)
                {
                    filteredInvertSet.Add(_i);
                }

                // ── 삭제(숨김) 필터 ──
                bool _shouldHide = false;
                if (chkDelThrottle.Checked && _frame.Throttle >= -(double)numLeftThrottle.Value && _frame.Throttle <= (double)numRightThrottle.Value)
                {
                    filteredHideSet.Add(_i);
                    _shouldHide = true;
                }
                if (chkDelAngle.Checked && _frame.Angle >= -(double)numLeftAngle.Value && _frame.Angle <= (double)numRightAngle.Value)
                {
                    filteredHideSet.Add(_i);
                    _shouldHide = true;
                }
                if (chkRemoveImage.Checked)
                {
                    filteredHideSet.Add(_i);
                    _shouldHide = true;
                }

                if (_shouldHide)
                {
                    totalHiddenCount++;
                }
                else if (_anyImageFilter)
                {
                    // 실제 비트맵 변환 로직 (기존 원본 로직 유지)
                    try
                    {
                        if (File.Exists(_frame.ImagePath))
                        {
                            Bitmap _currentBmp;
                            using (var _tempImg = Image.FromFile(_frame.ImagePath))
                            {
                                _currentBmp = new Bitmap(_tempImg);
                            }

                            if (chkApplyBlackWhite.Checked) _currentBmp = ApplyGrayscale(_currentBmp);
                            if (chkInverseColor.Checked) _currentBmp = ApplyInvert(_currentBmp);
                            if (chkSetBright.Checked && trkSetBright.Value != 0) _currentBmp = ApplyBrightness(_currentBmp, trkSetBright.Value);
                            if (chkSetBlur.Checked && trkSetBlur.Value > 0) _currentBmp = ApplyBlur(_currentBmp, trkSetBlur.Value);

                            filteredFrameMap[_i] = _currentBmp;
                            totalFilteredCount++;
                        }
                    }
                    catch (Exception _ex)
                    {
                        ReportLog("오류", $"프레임 [{_i}] 필터 변환 실패: {_ex.Message}");
                    }
                }
            }

            // UI 및 차트 업데이트
            UpdateChart();
            DisplayFrame(currentFrameIndex);
            pnlTimeStamp?.Invalidate();

            ReportLog("정보", $"선택 구간 필터 적용 완료: {totalFilteredCount}개 프레임 변환, {totalHiddenCount}개 프레임 숨김");
        }

        private void btnCancelFillter_Click(object sender, EventArgs e)
        {
            if (drivingData == null || drivingData.Count == 0) return;

            // [수정] 활성화된 특정 구간이 없으면 리턴
            if (activeRangeIndex == -1 || selectedRanges == null || activeRangeIndex >= selectedRanges.Count)
            {
                return;
            }

            // 현재 활성화된 단 하나의 구간만 가져옴
            var activeRange = selectedRanges[activeRangeIndex];
            int _start = activeRange.Start;
            int _end = activeRange.End;

            // 현재 구간 범위 내의 데이터만 필터 해제
            for (int _i = _start; _i <= _end; _i++)
            {
                if (_i < 0 || _i >= drivingData.Count) continue;

                if (filteredFrameMap.TryGetValue(_i, out Bitmap _old))
                {
                    _old.Dispose();
                    filteredFrameMap.Remove(_i);
                }
                filteredHideSet.Remove(_i);
                filteredInvertSet.Remove(_i);
                filteredGrayscaleSet.Remove(_i);
            }

            // UI 및 차트 업데이트
            UpdateChart();
            DisplayFrame(currentFrameIndex);
            pnlTimeStamp?.Invalidate();

            ReportLog("알림", $"선택된 구간 [{activeRangeIndex + 1}]의 임시 필터 결과가 취소되었습니다.");
        }
        private void btnInitFillterSet_Click(object sender, EventArgs e)
        {
            chkDelThrottle.Checked = false;
            chkDelAngle.Checked = false;
            chkRemoveImage.Checked = false;
            chkInverseColor.Checked = false;
            chkApplyBlackWhite.Checked = false;
            chkSetBright.Checked = false;
            chkSetBlur.Checked = false;

            numLeftThrottle.Value = 0;
            numRightThrottle.Value = 0;

            numLeftAngle.Value = 0;
            numRightAngle.Value = 0;

            trkSetBright.Value = 0;
            trkSetBlur.Value = 0;
            ReportLog("알림", "필터 설정이 초기화되었습니다.");
        }

        private void ApplyImageFiltersOnView()
        {
            if (picImage.Image == null) return;
            if (chkApplyBlackWhite.Checked) { }
            if (chkInverseColor.Checked) { }
        }

        private void UpdateChart()
        {
            chtData.Series.Clear();


            Series _angleSeries = new Series("각도") { ChartType = SeriesChartType.Line, BorderWidth = 2 };


            Series _throttleSeries = new Series("속도") { ChartType = SeriesChartType.Line, BorderWidth = 2 };
            _throttleSeries.Color = Color.FromArgb(72, 175, 120); // 노란색에서 초록색으로 변경

            for (int _i = 0; _i < drivingData.Count; _i++)
            {
                if (filteredHideSet.Contains(_i))
                    continue;
                var _frame = drivingData[_i];
                _angleSeries.Points.AddXY(_frame.Index, _frame.Angle);
                _throttleSeries.Points.AddXY(_frame.Index, _frame.Throttle);
            }

            chtData.Series.Add(_angleSeries);
            chtData.Series.Add(_throttleSeries);

            chtData.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chtData.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
        }

        /// <summary>
        /// 흑백(그레이스케일) — ColorMatrix 방식, GetPixel 대비 ~30배 빠름
        /// </summary>
        private Bitmap ApplyGrayscale(Bitmap src)
        {
            Bitmap _result = new Bitmap(src.Width, src.Height);

            ColorMatrix _matrix = new ColorMatrix(new float[][]
            {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0,      0,      0,      1, 0 },
                new float[] { 0,      0,      0,      0, 1 }
            });

            using (ImageAttributes _attr = new ImageAttributes())
            using (Graphics _g = Graphics.FromImage(_result))
            {
                _attr.SetColorMatrix(_matrix);
                _g.DrawImage(src,
                    new Rectangle(0, 0, src.Width, src.Height),
                    0, 0, src.Width, src.Height,
                    GraphicsUnit.Pixel, _attr);
            }

            src.Dispose();
            return _result;
        }

        /// <summary>
        /// 색상 반전 — ColorMatrix 방식
        /// </summary>
        private Bitmap ApplyInvert(Bitmap src)
        {
            Bitmap _result = new Bitmap(src.Width, src.Height);

            ColorMatrix _matrix = new ColorMatrix(new float[][]
            {
                new float[] { -1,  0,  0, 0, 0 },
                new float[] {  0, -1,  0, 0, 0 },
                new float[] {  0,  0, -1, 0, 0 },
                new float[] {  0,  0,  0, 1, 0 },
                new float[] {  1,  1,  1, 0, 1 }  // +1 오프셋으로 255-R/G/B 효과
            });

            using (ImageAttributes _attr = new ImageAttributes())
            using (Graphics _g = Graphics.FromImage(_result))
            {
                _attr.SetColorMatrix(_matrix);
                _g.DrawImage(src,
                    new Rectangle(0, 0, src.Width, src.Height),
                    0, 0, src.Width, src.Height,
                    GraphicsUnit.Pixel, _attr);
            }

            src.Dispose();
            return _result;
        }

        /// <summary>
        /// 밝기 조정 — ColorMatrix 방식 (trkSetBright 범위: -100 ~ 100)
        /// </summary>
        private Bitmap ApplyBrightness(Bitmap src, int delta)
        {
            Bitmap _result = new Bitmap(src.Width, src.Height);

            // ColorMatrix의 오프셋은 0~1 범위이므로 /255f 로 정규화
            float _offset = delta / 255f;

            ColorMatrix _matrix = new ColorMatrix(new float[][]
            {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { 0, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { _offset, _offset, _offset, 0, 1 }
            });

            using (ImageAttributes _attr = new ImageAttributes())
            using (Graphics _g = Graphics.FromImage(_result))
            {
                _attr.SetColorMatrix(_matrix);
                _g.DrawImage(src,
                    new Rectangle(0, 0, src.Width, src.Height),
                    0, 0, src.Width, src.Height,
                    GraphicsUnit.Pixel, _attr);
            }

            src.Dispose();
            return _result;
        }

        /// <summary>
        /// 박스 블러 — LockBits 방식, GetPixel 대비 ~50배 빠름 (trkSetBlur 범위: 1 ~ 10)
        /// </summary>
        private unsafe Bitmap ApplyBlur(Bitmap src, int radius)
        {
            if (radius <= 0) return src;

            Bitmap _result = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            int _w = src.Width, _h = src.Height;

            BitmapData _srcData = src.LockBits(
                new Rectangle(0, 0, _w, _h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            BitmapData _dstData = _result.LockBits(
                new Rectangle(0, 0, _w, _h),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            byte* _srcPtr = (byte*)_srcData.Scan0;
            byte* _dstPtr = (byte*)_dstData.Scan0;
            int _stride = _srcData.Stride;

            for (int _y = 0; _y < _h; _y++)
            {
                for (int _x = 0; _x < _w; _x++)
                {
                    int _r = 0, _g = 0, _b = 0, _count = 0;

                    for (int _dy = -radius; _dy <= radius; _dy++)
                    {
                        int _ny = Math.Max(0, Math.Min(_h - 1, _y + _dy));
                        for (int _dx = -radius; _dx <= radius; _dx++)
                        {
                            int _nx = Math.Max(0, Math.Min(_w - 1, _x + _dx));
                            byte* _px = _srcPtr + _ny * _stride + _nx * 4;
                            _b += _px[0]; _g += _px[1]; _r += _px[2];
                            _count++;
                        }
                    }

                    byte* _out = _dstPtr + _y * _stride + _x * 4;
                    _out[0] = (byte)(_b / _count);
                    _out[1] = (byte)(_g / _count);
                    _out[2] = (byte)(_r / _count);
                    _out[3] = 255;
                }
            }

            src.UnlockBits(_srcData);
            _result.UnlockBits(_dstData);
            src.Dispose();
            return _result;
        }

        /// <summary>
        /// 숨김 제외한 첫/현재/마지막 프레임의 원본 Index를 표시
        /// </summary>
        private void UpdateAllImageNumRangeLabel(int currentListIndex)
        {
            // 보이는 프레임만 추출
            var _visible = Enumerable.Range(0, drivingData.Count)
                                        .Where(i => !filteredHideSet.Contains(i))
                                        .ToList();

            if (_visible.Count == 0)
            {
                lblAllImageNumRange.Text = "(없음)";
                return;
            }

            int _firstIdx = drivingData[_visible.First()].Index;
            int _lastIdx = drivingData[_visible.Last()].Index;
            int _currentIdx = drivingData[currentListIndex].Index;

            lblAllImageNumRange.Text = $"({_firstIdx}, {_currentIdx}, {_lastIdx})  [표시: {_visible.Count}]";
        }

        // =====================================================================
        // 타임라인 (pnlTimeStamp) 전용 기능 구현부
        // =====================================================================

        private void PnlTimeStamp_Paint(object sender, PaintEventArgs e)
        {
            if (drivingData == null || drivingData.Count <= 1) return;

            Graphics g = e.Graphics;
            int width = pnlTimeStamp.Width;
            int height = pnlTimeStamp.Height;
            int totalFrames = drivingData.Count;

            // ── 기존 로직: 타임라인 배경에 썸네일 이미지 그리기 ──
            int thumbnailCount = 8;
            int thumbWidth = width / thumbnailCount;

            for (int i = 0; i < thumbnailCount; i++)
            {
                int targetFrameIdx = (int)((double)i / thumbnailCount * (totalFrames - 1));
                if (targetFrameIdx >= 0 && targetFrameIdx < totalFrames)
                {
                    string imgPath = drivingData[targetFrameIdx].ImagePath;
                    if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
                    {
                        try
                        {
                            using (Image thumbImg = Image.FromFile(imgPath))
                            {
                                Rectangle rect = new Rectangle(i * thumbWidth, 0, thumbWidth, height);
                                g.DrawImage(thumbImg, rect);
                            }
                        }
                        catch { /* 이미지 로드 실패 시 넘어감 */ }
                    }
                }
            }

            // ── [수정] 다중 구간 범위(Ranges) 그리기 ──
            // 기존 selectedRange 변수 하나 대신 리스트(selectedRanges) 전체를 돕니다.
            for (int i = 0; i < selectedRanges.Count; i++)
            {
                var range = selectedRanges[i];
                int startX = (int)((double)range.Start / (totalFrames - 1) * width);
                int endX = (int)((double)range.End / (totalFrames - 1) * width);

                if (startX <= endX && endX > 0)
                {
                    // 현재 조작 중인 활성화된 구간(activeRangeIndex)은 조금 더 진하게, 
                    // 나머지 구간들은 연하게 구분해서 그려줍니다. (기존 연한 파란색 계열 유지)
                    int alpha = (i == activeRangeIndex) ? 140 : 60;
                    Color rangeColor = Color.FromArgb(alpha, 100, 149, 237); // CornflowerBlue 기반 투명색

                    using (SolidBrush rangeBrush = new SolidBrush(rangeColor))
                    {
                        g.FillRectangle(rangeBrush, startX, 0, Math.Max(1, endX - startX), height);
                    }

                    // (선택 사항) 구간 위에 번호(1, 2, 3...)를 작게 텍스트로 표시
                    using (Font numFont = new Font("Arial", 9, FontStyle.Bold))
                    {
                        g.DrawString((i + 1).ToString(), numFont, Brushes.White, startX + 3, 5);
                    }
                }
            }

            // ── 필터 상태 표시 (회색을 더 진한 DimGray로 변경) ──
            using (Pen purplePen = new Pen(Color.FromArgb(255, 0, 128), 1))
            using (Pen grayPen = new Pen(Color.DarkOrange, 1))
            {
                for (int i = 0; i < totalFrames; i++)
                {
                    int x = (int)((double)i / (totalFrames - 1) * width);

                    // 1. 색상 반전 -> 최상단 보라색 선 (Y: 0 ~ 1)
                    if (filteredInvertSet.Contains(i))
                    {
                        g.DrawLine(purplePen, x, 0, x, 2);
                    }

                    // 2. 흑백 적용 -> 그 아래 진한 회색 선 (Y: 2 ~ 3)
                    if (filteredGrayscaleSet.Contains(i))
                    {
                        g.DrawLine(grayPen, x, 3, x, 4);
                    }
                }
            }

            // ── 기존 로직 2. 필터로 인해 숨김 처리된 프레임 표시 (빨간색 하단 마커) ──
            if (filteredHideSet.Count > 0)
            {
                using (Pen hiddenPen = new Pen(Color.FromArgb(180, Color.Red), 1))
                {
                    foreach (int hiddenIndex in filteredHideSet)
                    {
                        int x = (int)((double)hiddenIndex / (totalFrames - 1) * width);
                        g.DrawLine(hiddenPen, x, height - 10, x, height);
                    }
                }
            }

            // ── 기존 로직 3. 현재 재생 위치(Playhead) 표시 (파란색 선) ──
            int currentX = (int)((double)currentFrameIndex / (totalFrames - 1) * width);
            using (Pen playheadPen = new Pen(Color.Blue, 3))
            {
                g.DrawLine(playheadPen, currentX, 0, currentX, height);
            }
        }


        // ── [오류 해결] 마우스 다운(클릭) 이벤트 메서드 ──
        private void PnlTimeStamp_MouseDown(object sender, MouseEventArgs e)
        {
            if (drivingData == null || drivingData.Count <= 1) return;

            if (e.Button == MouseButtons.Left)
            {
                // 1. 드래그 시작 플래그 켜기 (핵심 누락되었던 부분)
                isDraggingTimeline = true;

                int width = pnlTimeStamp.Width;
                int totalFrames = drivingData.Count;

                // 클릭한 X 좌표를 프레임 인덱스로 역산
                double ratio = (double)e.X / width;
                int clickedFrame = (int)(ratio * (totalFrames - 1));
                clickedFrame = Math.Max(0, Math.Min(clickedFrame, totalFrames - 1));

                // 2. 클릭한 프레임이 이미 지정된 다중 구간 중 어디에 포함되어 있는지 검사
                bool found = false;
                if (selectedRanges != null && selectedRanges.Count > 0)
                {
                    for (int i = 0; i < selectedRanges.Count; i++)
                    {
                        if (clickedFrame >= selectedRanges[i].Start && clickedFrame <= selectedRanges[i].End)
                        {
                            activeRangeIndex = i; // 해당 구간을 활성화
                            found = true;
                            ReportLog("알림", $"구간 {i + 1}이 선택되었습니다.");
                            break;
                        }
                    }
                }

                // 구간 내부 클릭이 아니라면 현재 재생 위치를 클릭한 곳으로 변경
                currentFrameIndex = clickedFrame;

                // 화면 갱신
                UpdateRangeLabel();
                DisplayFrame(currentFrameIndex, direction: 0);
                pnlTimeStamp.Invalidate();
            }
        }

        // ── [오류 해결] 마우스 무브(드래그) 이벤트 메서드 ──
        private void PnlTimeStamp_MouseMove(object sender, MouseEventArgs e)
        {
            // 마우스를 꾹 누른 채 움직이고 있을 때만 작동
            if (isDraggingTimeline && drivingData != null && drivingData.Count > 1)
            {
                int width = pnlTimeStamp.Width;
                int totalFrames = drivingData.Count;

                // 마우스의 현재 X 좌표를 프레임 인덱스로 실시간 계산
                double ratio = (double)e.X / width;
                int targetFrame = (int)(ratio * (totalFrames - 1));
                targetFrame = Math.Max(0, Math.Min(targetFrame, totalFrames - 1));

                // 프레임 위치가 실제로 바뀔 때만 화면을 새로 그려 성능 최적화
                if (currentFrameIndex != targetFrame)
                {
                    currentFrameIndex = targetFrame;

                    // 이미지 및 화면 갱신 (지연 없도록 0 방향 전달)
                    DisplayFrame(currentFrameIndex, direction: 0);
                    pnlTimeStamp.Invalidate();
                }
            }
        }

        // ── [오류 해결] 마우스 업(클릭 해제) 이벤트 메서드 ──
        private void PnlTimeStamp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 마우스를 떼면 드래그 상태 해제
                isDraggingTimeline = false;
            }
        }

        // ── 타임라인 좌표를 프레임 인덱스로 변환하고 이미지를 갱신하는 메서드 ──
        private void MoveTimelineToPosition(int mouseX)
        {
            if (drivingData == null || drivingData.Count == 0) return;

            // 마우스 포인터가 패널을 벗어나도 예외가 발생하지 않도록 보정
            mouseX = Math.Max(0, Math.Min(mouseX, pnlTimeStamp.Width));

            // 클릭한 X좌표 비율을 계산하여 타겟 프레임 인덱스 도출
            double percentage = (double)mouseX / pnlTimeStamp.Width;
            int targetIndex = (int)Math.Round(percentage * (drivingData.Count - 1));

            // 가장 가까운 유효 인덱스를 먼저 찾습니다 (기존 코드 활용)
            int nearestIndex = FindNearestVisibleIndex(targetIndex, 0);

            // 마우스가 움직였어도 프레임 번호가 '실제로' 바뀌었을 때만 이미지를 로드합니다.
            if (nearestIndex >= 0 && nearestIndex != currentFrameIndex)
            {
                DisplayFrame(nearestIndex);

                // PictureBox에게 이미지를 즉시 그리라고 강제 명령 (버벅임 방지)
                picImage?.Refresh();
            }
        }

        private void PnlTimeStamp_Resize(object sender, EventArgs e)
        {
            // 패널의 크기가 가로/세로로 확장되면 전체 영역을 무효화하여 Paint 내용을 새로 갱신합니다.
            pnlTimeStamp?.Invalidate();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 수식키(Ctrl, Shift, Alt)를 제외한 순수 키 코드 추출
            Keys keyCode = keyData & Keys.KeyCode;
            Keys modifiers = keyData & Keys.Modifiers;

            // 1. [신규] Ctrl + Shift 조합키 처리
            if (modifiers == (Keys.Control | Keys.Shift))
            {
                if (keyCode == Keys.S) // Ctrl + Shift + S : 저장 경로 지정
                {
                    btnSaveRoute_Click(this, EventArgs.Empty);
                    return true;
                }
            }

            if (keyData == Keys.Delete)
            {
                // 이미 만들어둔 버튼 클릭 메서드를 수동으로 호출
                btnRangeDel_Click(this, EventArgs.Empty);
                return true;
            }

            if (modifiers == Keys.Shift)
            {
                switch (keyCode)
                {
                    case Keys.Left: // Shift + 왼쪽 방향키
                        btn5FrameLeft_Click(this, EventArgs.Empty);
                        return true;

                    case Keys.Right: // Shift + 오른쪽 방향키
                        btn5FrameRight_Click(this, EventArgs.Empty);
                        return true;
                }
            }

            // 2. [신규] Ctrl 조합키 처리
            if (modifiers == Keys.Control)
            {
                switch (keyCode)
                {
                    case Keys.O: // Ctrl + O : 주행데이터 가져오기
                        btnFileLoad_Click(this, EventArgs.Empty);
                        return true;

                    case Keys.S: // Ctrl + S : 데이터 저장
                        btnSaveData_Click(this, EventArgs.Empty);
                        return true;

                    case Keys.Delete: // Ctrl + Delete : 폴더 삭제 (안전장치)
                        btnDelFolder_Click(this, EventArgs.Empty);
                        return true;
                }
            }

            // 3. 일반 단일 키 입력 처리 (기존 기능 유지)
            if (modifiers == Keys.None)
            {
                switch (keyCode)
                {
                    
                    // 데이터 편집 제어 (U: 적용, I: 취소, O: 초기화)
                    case Keys.U:
                        btnApplyFillter_Click(this, EventArgs.Empty);
                        return true;
                    case Keys.I:
                        btnCancelFillter_Click(this, EventArgs.Empty);
                        return true;
                    case Keys.O:
                        btnInitFillterSet_Click(this, EventArgs.Empty);
                        return true;

                    // 방향키 위/아래 -> 재생 속도 조절
                    case Keys.Up: // 위쪽 방향키 : 재생 속도 빠르게
                        if (comboBox1 != null && comboBox1.SelectedIndex < comboBox1.Items.Count - 1)
                        {
                            comboBox1.SelectedIndex++;
                        }
                        return true;

                    case Keys.Down: // 아래쪽 방향키 : 재생 속도 느리게
                        if (comboBox1 != null && comboBox1.SelectedIndex > 0)
                        {
                            comboBox1.SelectedIndex--;
                        }
                        return true;

                    // 1프레임씩 이동 (방향키)
                    case Keys.Left:
                        btnFrameLeft_Click(this, EventArgs.Empty);
                        return true;
                    case Keys.Right:
                        btnFrameRight_Click(this, EventArgs.Empty);
                        return true;

                    // 재생 / 정지 (스페이스바)
                    case Keys.Space:
                        btnPlay_Click(this, EventArgs.Empty);
                        return true;

                    // 구간 재생 (탭 키)
                    case Keys.Tab:
                        btnRangePlay_Click(this, EventArgs.Empty);
                        return true;

                    // 전체 선택 (P 키)
                    case Keys.P:
                        btnAllRange_Click(this, EventArgs.Empty);
                        return true;
                }

                // [ : 왼쪽 구간 설정 (새 구간 생성 또는 기존 구간 수정)
                if (keyData == Keys.OemOpenBrackets) // '[' 키
                {
                    if (drivingData == null || drivingData.Count == 0) return true;

                    // 활성화된 구간이 없거나 새로 만들어야 하는 타이밍이라면 추가
                    if (activeRangeIndex == -1 || selectedRanges.Count == 0)
                    {
                        selectedRanges.Add(new RangeSegment(currentFrameIndex, currentFrameIndex));
                        activeRangeIndex = selectedRanges.Count - 1;
                        ReportLog("알림", $"새로운 구간({activeRangeIndex + 1})의 시작점을 설정했습니다.");
                    }
                    else
                    {
                        selectedRanges[activeRangeIndex].Start = currentFrameIndex;
                        ReportLog("알림", $"구간({activeRangeIndex + 1})의 시작점을 수정했습니다.");
                    }

                    // [추가] 시작점 조정에 따른 자동 구간 병합 수행
                    MergeOverlappingRanges();
                    UpdateRangeLabel();
                    pnlTimeStamp?.Invalidate();
                    return true;
                }

                // ] : 오른쪽 구간 설정
                if (keyData == Keys.OemCloseBrackets) // ']' 키
                {
                    if (drivingData == null || drivingData.Count == 0 || activeRangeIndex == -1) return true;

                    var currentRange = selectedRanges[activeRangeIndex];
                    if (currentFrameIndex >= currentRange.Start)
                    {
                        currentRange.End = currentFrameIndex;
                        ReportLog("알림", $"구간({activeRangeIndex + 1})의 종료점을 설정했습니다.");
                    }
                    else
                    {
                        ReportLog("경고", "종료점은 시작점보다 뒤에 있어야 합니다.");
                    }

                    // [추가] 종료점 조정에 따른 자동 구간 병합 수행
                    MergeOverlappingRanges();
                    UpdateRangeLabel();
                    pnlTimeStamp?.Invalidate();
                    return true;
                }

                // N : 새로운 구간 추가 모드로 전환
                if (keyData == Keys.N)
                {
                    activeRangeIndex = -1; // 인덱스를 초기화하여 다음 '['를 누를 때 새 구간이 생성되도록 유도
                    ReportLog("알림", "새 구간 생성 대기 상태입니다. '[' 키를 눌러 시작점을 지정하세요.");
                    return true;
                }

                // Delete : 현재 활성화된 구간 삭제 (기존 기능 유지)
                if (keyData == Keys.Delete)
                {
                    if (activeRangeIndex >= 0 && activeRangeIndex < selectedRanges.Count)
                    {
                        selectedRanges.RemoveAt(activeRangeIndex);
                        activeRangeIndex = selectedRanges.Count - 1; // 마지막 구간으로 포커스 이동
                        ReportLog("알림", "선택된 구간을 삭제했습니다.");
                        pnlTimeStamp?.Invalidate();
                    }
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ShowShortcutGuide()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("⚡ [재생 및 프레임 조작]");
            sb.AppendLine("• Space\t\t: 재생 / 정지");
            sb.AppendLine("• ← / →\t\t: 1프레임 이동");
            sb.AppendLine("• Shift + ← / →\t: 5프레임 이동");
            sb.AppendLine("• ↑ / ↓\t\t: 재생 속도 빠르게 / 느리게 (0.5x ~ 4.0x)");
            sb.AppendLine();

            sb.AppendLine("🎬 [구간 선택 및 재생]");
            sb.AppendLine("• [\t\t: 활성 구간의 시작점(왼쪽) 설정");
            sb.AppendLine("• ]\t\t: 활성 구간의 끝점(오른쪽) 설정");
            sb.AppendLine("• N\t\t: 새로운 구간 추가 준비 (인덱스 초기화)"); // ★ 추가됨
            sb.AppendLine("• Delete\t\t: 마지막으로 조작한 활성 구간 삭제");
            sb.AppendLine("• P\t\t: 전체 구간을 하나의 영역으로 선택");
            sb.AppendLine("• Tab\t\t: 현재 선택된(활성) 구간만 반복 재생");
            sb.AppendLine();

            sb.AppendLine("✏️ [데이터 값 편집]");
            sb.AppendLine("• U\t\t: 변경 사항 적용 (Apply)");
            sb.AppendLine("• I\t\t: 편집 내용 취소 (Cancel)");
            sb.AppendLine("• O\t\t: 원본 데이터로 초기화 (Reset)");
            sb.AppendLine();

            sb.AppendLine("💾 [파일 및 폴더 관리]");
            sb.AppendLine("• Ctrl + O\t: 주행 데이터 가져오기");
            sb.AppendLine("• Ctrl + S\t: 데이터 파일 저장");
            sb.AppendLine("• Ctrl + Shift + S\t: 저장 경로 지정");
            sb.AppendLine("• Ctrl + Delete\t: 현재 폴더 삭제 (안전장치)");

            // 메세지 박스 출력 (정보 아이콘 형태)
            MessageBox.Show(sb.ToString(), "프로그램 단축키 안내 가이드", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void btnEx_Click(object sender, EventArgs e)
        {
            // 이전 답변에서 만든 단축키 가이드 메서드 호출
            ShowShortcutGuide();
        }

        private void MergeOverlappingRanges()
        {
            if (selectedRanges == null || selectedRanges.Count <= 1) return;

            // 1. 시작점(Start) 기준으로 오름차순 정렬
            var sorted = selectedRanges.OrderBy(r => r.Start).ToList();
            var merged = new List<RangeSegment>();

            var current = sorted[0];
            merged.Add(current);

            for (int i = 1; i < sorted.Count; i++)
            {
                var next = sorted[i];

                // 현재 구간의 끝점(End)이 다음 구간의 시작점(Start) 이상이면 서로 겹쳐있는 상태
                if (current.End >= next.Start)
                {
                    // 더 먼 끝점 좌표를 선택하여 확장 병합
                    current.End = Math.Max(current.End, next.End);
                }
                else
                {
                    // 겹치지 않으면 분리된 독립 구간으로 보고 리스트에 추가
                    current = next;
                    merged.Add(current);
                }
            }

            // 2. 병합된 최신 결과를 원본 리스트에 대입
            selectedRanges = merged;

            // 3. 현재 포커싱된 activeRangeIndex가 배열 인덱스 에러가 안 나도록 상한선 재설정
            if (activeRangeIndex >= selectedRanges.Count)
            {
                activeRangeIndex = selectedRanges.Count - 1;
            }
        }

        private void btnRangeDel_Click(object sender, EventArgs e)
        {
            // 데이터가 없으면 무시
            if (drivingData == null || drivingData.Count == 0) return;

            // 현재 활성화(선택)된 구간이 유효한지 체크
            if (activeRangeIndex >= 0 && activeRangeIndex < selectedRanges.Count)
            {
                int lastActiveIndex = activeRangeIndex;

                // 활성 구간 삭제
                selectedRanges.RemoveAt(activeRangeIndex);

                // 삭제 후 포커스(선택 인덱스) 재조정
                if (selectedRanges.Count == 0)
                {
                    activeRangeIndex = -1; // 구간이 하나도 없으면 포커스 해제
                }
                else
                {
                    // 마지막 구간으로 포커스 이동 (인덱스 초과 방지)
                    activeRangeIndex = selectedRanges.Count - 1;
                }

                // 라벨 및 타임라인 그래픽 실시간 갱신
                UpdateRangeLabel();
                pnlTimeStamp?.Invalidate();

                ReportLog("알림", $"구간 [{lastActiveIndex + 1}]을 삭제했습니다.");
            }
            else
            {
                ReportLog("경고", "삭제할 활성 구간이 선택되어 있지 않습니다. 타임라인을 클릭하여 구간을 선택해 주세요.");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            string _root = AppPaths.EditedData;
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }

            // 새 폴더명을 입력받기 위한 커스텀 입력창 생성
            using (Form _inputForm = new Form())
            {
                _inputForm.Width = 400;
                _inputForm.Height = 150;
                _inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                _inputForm.Text = "새 폴더 생성";
                _inputForm.StartPosition = FormStartPosition.CenterParent;
                _inputForm.MaximizeBox = false;
                _inputForm.MinimizeBox = false;

                Label _lblText = new Label() { Left = 20, Top = 20, Width = 350, Text = "생성할 새 폴더 이름을 입력하세요:" };
                TextBox _txtInput = new TextBox() { Left = 20, Top = 45, Width = 340, Text = "" };
                Button _btnOk = new Button() { Text = "확인", Left = 180, Width = 80, Top = 80, DialogResult = DialogResult.OK };
                Button _btnCancel = new Button() { Text = "취소", Left = 280, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

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

                    // 폴더명에 사용할 수 없는 특수문자 금지 및 치환
                    foreach (char _c in Path.GetInvalidFileNameChars())
                    {
                        _folderName = _folderName.Replace(_c, '_');
                    }

                    string _finalNewFolderPath = Path.Combine(_root, _folderName);
                    try
                    {
                        if (!Directory.Exists(_finalNewFolderPath))
                        {
                            Directory.CreateDirectory(_finalNewFolderPath);

                            // 폴더 생성 후, 해당 폴더를 현재 저장 경로(targetSavePath)로 자동 지정
                            targetSavePath = _finalNewFolderPath;
                            if (lblSaveRoute != null)
                            {
                                lblSaveRoute.Text = $"[저장 경로] {_folderName}";
                            }

                            ReportLog("알림", $"새 폴더 생성 완료: {_folderName}");
                            MessageBox.Show($"[{_folderName}] 폴더가 성공적으로 생성되고 저장 경로로 지정되었습니다.", "성공");
                        }
                        else
                        {
                            ReportLog("경고", "이미 존재하는 폴더 이름입니다.");
                            MessageBox.Show("이미 존재하는 폴더 이름입니다. 다른 이름을 사용해주세요.", "알림");
                        }
                    }
                    catch (Exception _ex)
                    {
                        ReportLog("오류", $"폴더 생성 실패: {_ex.Message}");
                        MessageBox.Show($"폴더 생성 중 오류가 발생했습니다.\n{_ex.Message}", "오류");
                    }
                }
            }
        }


        /// <summary>
        /// 밝기 및 흐림 체크박스의 텍스트에 현재 트랙바 수치를 실시간 반영합니다.
        /// </summary>
        private void UpdateFilterCheckboxTexts()
        {
            if (chkSetBright != null && trkSetBright != null)
            {
                
                chkSetBright.Text = $"밝기 : {trkSetBright.Value}";
            }

            if (chkSetBlur != null && trkSetBlur != null)
            {
                
                chkSetBlur.Text = $"흐림 : {trkSetBlur.Value}";
            }
        }

    }
}