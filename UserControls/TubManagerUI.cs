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
        }

        public event Action<string, string, string> OnLogReported;

        private List<DrivingFrame> drivingData = new List<DrivingFrame>();
        private int currentFrameIndex = 0;
        private bool isPlaying = false;
        private System.Windows.Forms.Timer playTimer;
        private Tuple<int, int> selectedRange = new Tuple<int, int>(0, 0);

        // 임시 필터 적용 이미지 저장소 (key: drivingData 인덱스, value: 가공된 Bitmap)
        private Dictionary<int, Bitmap> filteredFrameMap = new Dictionary<int, Bitmap>();

        // 필터로 인해 "숨김 처리"할 프레임 인덱스 집합 (실제 삭제 X, 뷰에서만 스킵)
        private HashSet<int> filteredHideSet = new HashSet<int>();


        private readonly string baseEditedPath = Path.Combine(
        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName,
        "EditedData"
        );
        private string targetSavePath = string.Empty;

        // ── 타임라인 드래그 상태 변수 추가 ──
        private bool isDraggingTimeline = false;


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
                    MessageBox.Show($"폴더 생성 중 오류 발생: {_ex.Message}", "오류");
                }
            }
        }


        private void btnDelFolder_Click(object sender, EventArgs e)
        {
            string root = AppPaths.EditedData;
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            using (var browser = new CustomFolderBrowser(root, "삭제할 폴더 선택 (EditedData 내부 폴더)"))
            {
                browser.AllowFileSelection = true;

                if (browser.ShowDialog(this) == DialogResult.OK)
                {
                    string _targetDelPath = browser.SelectedPath;

                    if (_targetDelPath.Equals(root, StringComparison.OrdinalIgnoreCase))
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
                            ReportLog("알림", $"편집 폴더 삭제 완료: {_folderName}");

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
                string _catalogPath = Path.Combine(targetSavePath, "catalog_0.catalog");
                string _catalogManifestPath = Path.Combine(targetSavePath, "catalog_0.catalog_manifest");
                string _manifestJsonPath = Path.Combine(targetSavePath, "manifest.json");

                // ── 이어쓰기 여부 판단 ────────────────────────────────────
                bool _isContinue = chkSaveContinue.Checked;
                int _startIndex = 0;
                List<string> _catalogLines = new List<string>();

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
                    if (Directory.Exists(_saveImagesDir))
                        Directory.Delete(_saveImagesDir, true);
                    Directory.CreateDirectory(_saveImagesDir);
                }

                int _savedCount = 0;

                // 각 라인의 바이트 길이를 저장할 리스트
                List<int> _lineLengths = new List<int>();

                // 이어쓰기 모드일 경우 기존 카탈로그 파일의 라인 바이트 수 계산
                if (_isContinue && _catalogLines.Count > 0)
                {
                    foreach (var line in _catalogLines)
                    {
                        _lineLengths.Add(System.Text.Encoding.UTF8.GetByteCount(line) + 1);
                    }
                }

                for (int _i = 0; _i < drivingData.Count; _i++)
                {
                    if (filteredHideSet.Contains(_i))
                        continue;
                    var _frame = drivingData[_i];

                    int _newIndex = _startIndex + _savedCount;
                    string _ext = Path.GetExtension(_frame.ImagePath);
                    string _newFileName = $"{_newIndex}_cam_image_array_{_ext}";
                    string _destPath = Path.Combine(_saveImagesDir, _newFileName);

                    // ── 이미지 저장 ───────────────────────────────────────
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

                    // ── catalog JSON 라인 조립 (콜론과 콤마 뒤 띄어쓰기 수동 추가) ─────────────────
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
                        if (!_first) _sb.Append(", "); // 콤마 뒤 공백 한 칸 추가
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
                        _sb.Append($"{_key}: {_val}"); // 콜론 뒤 공백 한 칸 추가
                    }
                    _sb.Append("}");

                    string _jsonLine = _sb.ToString();
                    _catalogLines.Add(_jsonLine);

                    // 현재 라인의 UTF-8 바이트 크기 계산 (+1은 줄바꿈 문자 \n 값 반영)
                    int _lineByteCount = System.Text.Encoding.UTF8.GetByteCount(_jsonLine) + 1;
                    _lineLengths.Add(_lineByteCount);

                    _savedCount++;
                }

                // ── 파일 최종 저장 ────────────────────────────────────────
                File.WriteAllLines(_catalogPath, _catalogLines);

                // 현재 Unix Timestamp 계산
                double _unixTimestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                // 1. [catalog_0.catalog_manifest] 파일 띄어쓰기 규칙에 맞춰 직접 조립
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


                // 2. [manifest.json] 원본 다중 행 포맷 및 띄어쓰기 완벽 반영하여 작성
                var _manifestLines = new List<string>
                {
                    "[\"cam/image_array\", \"user/angle\", \"user/throttle\", \"user/mode\"]",
                    "[\"image_array\", \"float\", \"float\", \"str\"]",
                    "{}",
                    $"{{\"created_at\": {_unixTimestamp}}}",
                    $"{{\"paths\": [\"catalog_0.catalog\"], \"current_index\": {_startIndex + _savedCount}, \"max_len\": 1000, \"deleted_indexes\": []}}"
                };

                // 줄바꿈(\n) 형태로 manifest.json 최종 저장
                File.WriteAllLines(_manifestJsonPath, _manifestLines);


                string _modeText = _isContinue ? "이어쓰기" : "새로 저장";
                ReportLog("정보", $"데이터 저장 완료 [{_modeText}] (총 {_savedCount} 프레임, {_startIndex}번부터 시작)");
                MessageBox.Show($"저장 완료! [{_modeText}]\n{_startIndex}번 ~ {_startIndex + _savedCount - 1}번 프레임 저장됨", "저장 완료");
            }
            catch (Exception _ex)
            {
                ReportLog("오류", $"데이터 저장 실패: {_ex.Message}");
                MessageBox.Show($"저장 중 오류 발생:\n{_ex.Message}", "오류");
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
        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            int _next = FindNearestVisibleIndex(currentFrameIndex + 1, direction: 1);
            if (_next > currentFrameIndex)
                DisplayFrame(_next, direction: 1);
            else
                btnStop_Click(null, null); // 더 이상 앞으로 갈 수 없음 = 끝
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (drivingData.Count == 0) return;
            isPlaying = true;
            playTimer.Start();
            ReportLog("알림", "데이터 재생을 시작합니다.");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isPlaying = false;
            playTimer.Stop();
            ReportLog("알림", "데이터 재생을 일시 정지합니다.");
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && double.TryParse(comboBox1.SelectedItem.ToString(), out double _speed))
            {
                int _baseInterval = 60;
                playTimer.Interval = (int)(_baseInterval / _speed);
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

            // 현재 프레임이 기존 종료 범위보다 뒤에 있다면, 종료 범위도 현재 프레임으로 맞춰서 오류를 방지합니다.
            if (currentFrameIndex > selectedRange.Item2)
            {
                selectedRange = new Tuple<int, int>(currentFrameIndex, currentFrameIndex);
            }
            else
            {
                selectedRange = new Tuple<int, int>(currentFrameIndex, selectedRange.Item2);
            }

            UpdateRangeLabel();
            ReportLog("알림", $"시작 범위 지정: {drivingData[currentFrameIndex].Index}번 프레임");
        }

        private void btnRightRange_Click(object sender, EventArgs e)
        {
            // [방어 코드] 주행 데이터가 로드되지 않았다면 무시
            if (drivingData == null || drivingData.Count == 0) return;

            // 현재 프레임이 기존 시작 범위보다 앞에 있다면, 시작 범위도 현재 프레임으로 맞춰서 오류를 방지합니다.
            if (currentFrameIndex < selectedRange.Item1)
            {
                selectedRange = new Tuple<int, int>(currentFrameIndex, currentFrameIndex);
            }
            else
            {
                selectedRange = new Tuple<int, int>(selectedRange.Item1, currentFrameIndex);
            }

            UpdateRangeLabel();
            ReportLog("알림", $"종료 범위 지정: {drivingData[currentFrameIndex].Index}번 프레임");
        }

        private void btnAllRange_Click(object sender, EventArgs e)
        {
            if (drivingData.Count > 0)
            {
                selectedRange = new Tuple<int, int>(0, drivingData.Count - 1);
                UpdateRangeLabel();
                ReportLog("알림", "전체 범위가 선택되었습니다.");
            }
        }

        private void UpdateRangeLabel()
        {
            // [방어 코드] 주행 데이터가 없으면 실행 안 함
            if (drivingData == null || drivingData.Count == 0) return;

            int _start = selectedRange.Item1;
            int _end = selectedRange.Item2;

            // 혹시 모를 인덱스 초과 및 역전 현상 최종 방어
            if (_start < 0) _start = 0;
            if (_end >= drivingData.Count) _end = drivingData.Count - 1;
            if (_start > _end) _start = _end;

            // 범위 내 보이는 프레임 수 계산 (이제 무조건 0 이상의 개수가 보장됩니다)
            int _visibleInRange = Enumerable.Range(_start, _end - _start + 1)
                                            .Count(i => !filteredHideSet.Contains(i));
            int _startDisplayIdx = drivingData[_start].Index;
            int _endDisplayIdx = drivingData[_end].Index;

            lblSelectedRange.Text = $"선택된 범위 ({_startDisplayIdx}, {_endDisplayIdx})";

            // ── 타임라인 갱신 추가 ──
            pnlTimeStamp?.Invalidate();
        }

        private void btnApplyFillter_Click(object sender, EventArgs e)
        {
            if (drivingData.Count == 0) return;

            int _start = selectedRange.Item1;
            int _end = selectedRange.Item2;

            // 이전 임시 결과 중 해당 범위만 초기화
            for (int _i = _start; _i <= _end; _i++)
            {
                if (filteredFrameMap.TryGetValue(_i, out Bitmap _old))
                {
                    _old.Dispose();
                    filteredFrameMap.Remove(_i);
                }
                filteredHideSet.Remove(_i);
            }

            bool _anyImageFilter = chkInverseColor.Checked
                                || chkApplyBlackWhite.Checked
                                || chkSetBright.Checked
                                || chkSetBlur.Checked;

            for (int _i = _start; _i <= _end; _i++)
            {
                var _frame = drivingData[_i];

                // ── 삭제(숨김) 필터 ──────────────────────────────
                if (chkDelThrottle.Checked && _frame.Throttle == 0.0)
                {
                    filteredHideSet.Add(_i);
                    continue;
                }
                if (chkDelAngle.Checked && _frame.Angle == 0.0)
                {
                    filteredHideSet.Add(_i);
                    continue;
                }
                if (chkRemoveImage.Checked)
                {
                    filteredHideSet.Add(_i);
                    continue;
                }

                // ── 이미지 변환 필터 ─────────────────────────────
                if (!_anyImageFilter) continue;
                if (string.IsNullOrEmpty(_frame.ImagePath) || !File.Exists(_frame.ImagePath)) continue;

                Bitmap _bmp;
                using (var _src = new Bitmap(_frame.ImagePath))
                    _bmp = new Bitmap(_src); // 원본 복사본

                if (chkApplyBlackWhite.Checked) _bmp = ApplyGrayscale(_bmp);
                if (chkInverseColor.Checked) _bmp = ApplyInvert(_bmp);
                if (chkSetBright.Checked) _bmp = ApplyBrightness(_bmp, trkSetBright.Value);
                if (chkSetBlur.Checked) _bmp = ApplyBlur(_bmp, trkSetBlur.Value);

                filteredFrameMap[_i] = _bmp;
            }

            UpdateChart();
            DisplayFrame(currentFrameIndex);

            // ── 타임라인 갱신 추가 ──
            pnlTimeStamp?.Invalidate();

            int _hiddenCount = filteredHideSet.Count(x => x >= _start && x <= _end);
            int _filteredCount = filteredFrameMap.Count(x => x.Key >= _start && x.Key <= _end);
            ReportLog("알림", $"필터 적용 완료 — 숨김: {_hiddenCount}개, 이미지 변환: {_filteredCount}개 " +
                              $"(범위: {drivingData[_start].Index} ~ {drivingData[_end].Index})");
        }

        private void btnCancelFillter_Click(object sender, EventArgs e)
        {
            // Bitmap 메모리 해제 후 딕셔너리 전체 삭제
            foreach (var _bmp in filteredFrameMap.Values)
                _bmp.Dispose();

            filteredFrameMap.Clear();
            filteredHideSet.Clear();

            UpdateChart();
            DisplayFrame(currentFrameIndex);

            // ── 타임라인 갱신 추가 ──
            pnlTimeStamp?.Invalidate();

            ReportLog("알림", "임시 필터 결과가 모두 취소되었습니다.");
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
            trkSetBright.Value = trkSetBright.Minimum;
            trkSetBlur.Value = trkSetBlur.Minimum;
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

            for (int _i = 0; _i < drivingData.Count; _i++)
            {
                if (filteredHideSet.Contains(_i)) continue;

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

            // ── [추가] 타임라인 배경에 썸네일 이미지 그리기 ──
            // 패널 크기에 맞춰 약 8개의 이미지 분할 표시 (크기에 따라 조절 가능)
            int thumbnailCount = 8;
            int thumbWidth = width / thumbnailCount;

            for (int i = 0; i < thumbnailCount; i++)
            {
                // 각 칸에 해당하는 프레임 인덱스 계산
                int targetFrameIdx = (int)((double)i / thumbnailCount * (totalFrames - 1));
                if (targetFrameIdx >= 0 && targetFrameIdx < totalFrames)
                {
                    string imgPath = drivingData[targetFrameIdx].ImagePath;
                    if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
                    {
                        try
                        {
                            // 디스크에서 이미지를 잠깐 가져와 지정된 칸에 맞게 그려줌
                            using (Image thumbImg = Image.FromFile(imgPath))
                            {
                                Rectangle rect = new Rectangle(i * thumbWidth, 0, thumbWidth, height);
                                // 이미지가 찌그러지지 않고 꽉 차게 그리기 위해 래핑
                                g.DrawImage(thumbImg, rect);
                            }
                        }
                        catch { /* 이미지 로드 실패 시 넘어감 */ }
                    }
                }
            }

            // ── 기존 로직 1. 선택된 범위(Range) 그리기 (연한 파란색 투명 배경) ──
            int startX = (int)((double)selectedRange.Item1 / (totalFrames - 1) * width);
            int endX = (int)((double)selectedRange.Item2 / (totalFrames - 1) * width);
            if (startX <= endX && endX > 0)
            {
                using (SolidBrush rangeBrush = new SolidBrush(Color.FromArgb(120, 173, 216, 230))) // 투명도 약간 낮춤
                {
                    g.FillRectangle(rangeBrush, startX, 0, Math.Max(1, endX - startX), height);
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

            // ── 기존 로직 3. 현재 재생 위치(Playhead) 표시 (빨간색이나 파란색 선) ──
            int currentX = (int)((double)currentFrameIndex / (totalFrames - 1) * width);
            using (Pen playheadPen = new Pen(Color.Blue, 3)) // 이미지 위에 잘 보이도록 빨간색 추천
            {
                g.DrawLine(playheadPen, currentX, 0, currentX, height);
            }



        }
        // ── [오류 해결] 마우스 다운(클릭) 이벤트 메서드 ──
        private void PnlTimeStamp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && drivingData != null && drivingData.Count > 0)
            {
                isDraggingTimeline = true;
                MoveTimelineToPosition(e.X);
            }
        }

        // ── [오류 해결] 마우스 무브(드래그) 이벤트 메서드 ──
        private void PnlTimeStamp_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingTimeline && drivingData != null && drivingData.Count > 0)
            {
                MoveTimelineToPosition(e.X);
            }
        }

        // ── [오류 해결] 마우스 업(클릭 해제) 이벤트 메서드 ──
        private void PnlTimeStamp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
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
    }
}