using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;
using DataManager.Forms;

namespace DataManager.UserControls
{
    public partial class ModelTestModule : UserControl
    {
        public event Action<string, string, string> OnLogReported;

        private string selectedModelFolderPath = string.Empty;
        private string selectedModelFilePath = string.Empty;
        private string selectedModelType = string.Empty;
        private bool isModelLoaded = false;

        private double currentActualAngle = 0.0;
        private double currentActualThrottle = 0.0;
        private string currentImagePath = string.Empty;
        private bool isPredicting = false;
        private int currentFrameIndex = 0;

        private Process? _pythonProcess;
        private StreamWriter? _pythonStdin;
        private StreamReader? _pythonStdout;
        private bool _pythonReady = false;

        private Dictionary<int, double> frameAngleUser = new();
        private Dictionary<int, double> frameAnglePilot = new();
        private Dictionary<int, double> frameThrottleUser = new();
        private Dictionary<int, double> frameThrottlePilot = new();
        private Dictionary<int, string> frameImagePaths = new();
        // [추가] 실시간 메모리 필터 이미지 보관소 (키: 프레임 인덱스, 값: 가공된 Bitmap)
        private Dictionary<int, Bitmap> frameMemoryBitmaps = new();

        // 가장 최근에 요청된 중심 프레임 — 사용자가 다른 프레임으로 이동하면 창 예측을 중단하는 데 사용
        private volatile int _latestWindowCenter = -1;

        private Series? seriesAngleUser;
        private Series? seriesAnglePilot;
        private Series? seriesThrottleUser;
        private Series? seriesThrottlePilot;

        private string lastLoadedImagePath = string.Empty;
        private MemoryStream? currentImageStream;
        private Label lblModelType = null!;

        public ModelTestModule()
        {
            InitializeComponent();

            gaugeBar2.Minimum = -100.0;
            gaugeBar2.Maximum = 100.0;

            picImage.SizeMode = PictureBoxSizeMode.Zoom;
            picImage.Paint += PicImage_Paint;

            // 콤보박스 자리에 meta.txt에서 읽은 모델 유형 레이블 배치
            cboModelType.Visible = false;
            lblModelType = new Label
            {
                Location = new Point(cboModelType.Location.X, cboModelType.Location.Y),
                Size = new Size(72, 26),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "-",
                Padding = new Padding(3, 0, 0, 0),
            };
            pnlSetting.Controls.Add(lblModelType);

            lblAngle.Font = new Font("Consolas", 11f, FontStyle.Bold);
            lblThrottle.Font = new Font("Consolas", 11f, FontStyle.Bold);
            lblAngle.Text = "조향각\n-";
            lblThrottle.Text = "가속값\n-";
            pnlData.Resize += PnlData_Resize;
            pnlSetting.Resize += PnlSetting_Resize;
            Disposed += (s, e) => StopPythonProcess();

            SetupCharts();
        }

        private void SetupCharts()
        {
            chart1.Series.Clear();
            chart2.Series.Clear();

            chart1.AntiAliasing = AntiAliasingStyles.None;
            chart2.AntiAliasing = AntiAliasingStyles.None;
            chart1.IsSoftShadows = false;
            chart2.IsSoftShadows = false;

            foreach (var chart in new[] { chart1, chart2 })
            {
                if (chart.ChartAreas.Count == 0) continue;
                var area = chart.ChartAreas[0];
                area.AxisX.Title = "Frame";
                area.AxisX.Minimum = -5;
                area.AxisX.Maximum = 5;
                area.AxisX.Interval = 1;
                area.AxisX.MajorGrid.LineColor = Color.LightGray;
                area.AxisY.IsStartedFromZero = false;

                // 현재 프레임(0) 강조 세로선
                var strip = new StripLine
                {
                    IntervalOffset = 0,
                    StripWidth = 0.05,
                    BackColor = Color.FromArgb(60, Color.Red),
                    Interval = 0
                };
                area.AxisX.StripLines.Add(strip);
            }

            // 조향각 차트: 실제 각도(°) 단위 (-45° ~ +45°, 여유 포함)
            {
                var area = chart1.ChartAreas[0];
                area.AxisY.Minimum = -54.0;
                area.AxisY.Maximum = 54.0;
                area.AxisY.Interval = 18.0;
            }

            // 가속값 차트: 퍼센트 값 (-100 ~ +100)
            {
                var area = chart2.ChartAreas[0];
                area.AxisY.Minimum = -120.0;
                area.AxisY.Maximum = 120.0;
                area.AxisY.Interval = 50.0;
            }

            string c1Area = chart1.ChartAreas[0].Name;
            string c2Area = chart2.ChartAreas[0].Name;

            seriesAngleUser = new Series("사용자 조향각") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Circle, MarkerSize = 5, BorderWidth = 3, ChartArea = c1Area, Color = Color.FromArgb(34, 177, 76) };
            seriesAnglePilot = new Series("자율 조향각") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Circle, MarkerSize = 5, BorderWidth = 3, ChartArea = c1Area, Color = Color.DodgerBlue };
            chart1.Series.Add(seriesAngleUser);
            chart1.Series.Add(seriesAnglePilot);

            seriesThrottleUser = new Series("사용자 가속값") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Circle, MarkerSize = 5, BorderWidth = 3, ChartArea = c2Area, Color = Color.FromArgb(34, 177, 76) };
            seriesThrottlePilot = new Series("자율 가속값") { ChartType = SeriesChartType.Line, MarkerStyle = MarkerStyle.Circle, MarkerSize = 5, BorderWidth = 3, ChartArea = c2Area, Color = Color.DodgerBlue };
            chart2.Series.Add(seriesThrottleUser);
            chart2.Series.Add(seriesThrottlePilot);
        }

        private void UpdateErrorLabels()
        {
            if (frameAnglePilot.TryGetValue(currentFrameIndex, out double pa) &&
                frameAngleUser.TryGetValue(currentFrameIndex, out double ua))
                lblAngleError.Text = $"조향각 오차\n{Math.Abs(ua - pa) * 45.0:F2}°";
            else
                lblAngleError.Text = "조향각 오차\n-";

            if (frameThrottlePilot.TryGetValue(currentFrameIndex, out double pt) &&
                frameThrottleUser.TryGetValue(currentFrameIndex, out double ut))
                lblThrottleError.Text = $"가속값 오차\n{Math.Abs(ut - pt) * 100.0:F1}";
            else
                lblThrottleError.Text = "가속값 오차\n-";
        }


        public event EventHandler CloseRequested;

        private FullGraphForm? _fullGraphForm;

        // PilotArenaUI가 주입하는 콜백 — 현재 사용자 프레임 목록을 반환
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<List<(int Index, double Angle, double Throttle)>>? UserFramesProvider { get; set; }

        private void btnFullGraph_Click(object sender, EventArgs e)
        {
            var userFrames = UserFramesProvider?.Invoke();
            if (userFrames == null || userFrames.Count == 0)
            {
                MessageBox.Show("먼저 Tub 데이터를 로드하세요.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_fullGraphForm != null && !_fullGraphForm.IsDisposed)
            {
                RefreshOwnGraph(userFrames);
                _fullGraphForm.BringToFront();
                _fullGraphForm.Focus();
                return;
            }

            var (angles, throttles) = GetAllPredictions();
            var pilotData = new List<(string ModelName, Dictionary<int, double> Angles, Dictionary<int, double> Throttles)>
            {
                (ModelName, angles, throttles)
            };

            _fullGraphForm = new FullGraphForm(userFrames, pilotData);
            _fullGraphForm.FormClosed += (s, _) => _fullGraphForm = null;
            _fullGraphForm.Show(this.FindForm());
        }

        public void RefreshOwnGraph()
        {
            if (_fullGraphForm == null || _fullGraphForm.IsDisposed) return;
            var userFrames = UserFramesProvider?.Invoke();
            if (userFrames == null) return;
            RefreshOwnGraph(userFrames);
        }

        private void RefreshOwnGraph(List<(int Index, double Angle, double Throttle)> userFrames)
        {
            if (_fullGraphForm == null || _fullGraphForm.IsDisposed) return;
            var (angles, throttles) = GetAllPredictions();
            var pilotData = new List<(string ModelName, Dictionary<int, double> Angles, Dictionary<int, double> Throttles)>
            {
                (ModelName, angles, throttles)
            };
            _fullGraphForm.RefreshData(userFrames, pilotData);
        }

        public void UpdateOwnGraphFrame(int frameIndex)
        {
            if (_fullGraphForm == null || _fullGraphForm.IsDisposed) return;
            _fullGraphForm.UpdateCurrentFrame(frameIndex);
        }

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }

        private void btnDelModel_Click(object sender, EventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnLoadModel_Click(object sender, EventArgs e)
        {
            string root = AppPaths.MycarModels;
            if (!Directory.Exists(root))
            {
                MessageBox.Show($"mycar/models 폴더를 찾을 수 없습니다.\n경로: {root}", "알림");
                return;
            }
            using (var browser = new CustomFolderBrowser(root, "모델 폴더 선택"))
            {
                browser.AllowFileSelection = true;

                if (browser.ShowDialog(this) == DialogResult.OK)
                    ProcessSelectedModelFolder(browser.SelectedPath);
            }
        }

        private void ProcessSelectedModelFolder(string folderPath)
        {
            string folderName = Path.GetFileName(folderPath);

            bool hasSavedModel = File.Exists(Path.Combine(folderPath, "saved_model.pb"));
            string[] h5Files = Directory.GetFiles(folderPath, "*.h5");

            if (hasSavedModel)
            {
                selectedModelType = "savedmodel";
                selectedModelFolderPath = folderPath;
                selectedModelFilePath = folderPath;
                isModelLoaded = true;

                lblModelRoute.Text = $"Model: {folderName} / Type: {selectedModelType}";
            }
            else if (h5Files.Length > 0)
            {
                selectedModelType = "h5";
                selectedModelFolderPath = folderPath;
                selectedModelFilePath = h5Files[0];
                isModelLoaded = true;

                lblModelRoute.Text = $"Model: {folderName} / Type: {selectedModelType}";
            }
            else
            {
                ReportLog("오류", "지원하지 않는 모델 폴더입니다.");
                isModelLoaded = false;
                selectedModelType = string.Empty;
                selectedModelFolderPath = string.Empty;
                selectedModelFilePath = string.Empty;
                lblModelRoute.Text = "(Route)";
            }

            if (isModelLoaded)
            {
                lblModelRoute.Text += " (로드 중...)";
                lblModelType.Text = ReadModelTypeFromMeta(folderPath);
                string memo = ReadMemoFromMeta(folderPath);
                if (!string.IsNullOrEmpty(memo))
                    ReportLog("메모", memo);
                ClearPredictions();
                _ = StartPythonProcessAsync();
            }
            else
            {
                lblModelType.Text = "-";
            }
        }

        private static string? FindMetaPath(string folderPath)
        {
            string metaPath = Path.Combine(folderPath, "meta.txt");
            if (File.Exists(metaPath)) return metaPath;
            string? parentDir = Path.GetDirectoryName(folderPath);
            if (parentDir != null)
            {
                string parentMeta = Path.Combine(parentDir, "meta.txt");
                if (File.Exists(parentMeta)) return parentMeta;
            }
            return null;
        }

        private static string ReadModelTypeFromMeta(string folderPath)
        {
            try
            {
                string? metaPath = FindMetaPath(folderPath);
                if (metaPath == null) return "-";

                string content = File.ReadAllText(metaPath);
                var match = Regex.Match(content, @"\(([^)]+)\)");
                return match.Success ? match.Groups[1].Value : "-";
            }
            catch
            {
                return "-";
            }
        }

        private static string ReadMemoFromMeta(string folderPath)
        {
            try
            {
                string? metaPath = FindMetaPath(folderPath);
                if (metaPath == null) return string.Empty;

                string[] lines = File.ReadAllLines(metaPath);
                if (lines.Length < 4) return string.Empty;
                return lines[3].Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public event EventHandler? ModelReady;
        public event EventHandler? PredictionUpdated;

        public bool IsModelReady => _pythonReady;

        public string ModelName => string.IsNullOrEmpty(selectedModelFolderPath)
            ? string.Empty
            : System.IO.Path.GetFileName(selectedModelFolderPath);

        public (Dictionary<int, double> Angles, Dictionary<int, double> Throttles) GetAllPredictions()
            => (new Dictionary<int, double>(frameAnglePilot), new Dictionary<int, double>(frameThrottlePilot));

        public async Task RunAllFramePredictionsAsync()
        {
            if (!_pythonReady || _pythonStdin == null || _pythonStdout == null) return;
            if (isPredicting) return;
            isPredicting = true;

            var indices = frameImagePaths.Keys
                .OrderBy(idx => Math.Abs(idx - currentFrameIndex))
                .ToList();

            try
            {
                foreach (var idx in indices)
                {
                    if (!_pythonReady) break;
                    if (frameAnglePilot.ContainsKey(idx)) continue;
                    if (!frameImagePaths.TryGetValue(idx, out string? imgPath)) continue;
                    if (!File.Exists(imgPath)) continue;

                    // [핵심 보완] 루프 도중 '삭제/종료'로 인해 null이 되는 것을 방지하기 위해 지역 변수에 복사
                    var stdin = _pythonStdin;
                    var stdout = _pythonStdout;
                    if (stdin == null || stdout == null) break;

                    // 이제 전역 필드가 아닌, 안전하게 확보된 지역 변수(stdin)를 사용합니다.
                    await stdin.WriteLineAsync(imgPath);
                    await stdin.FlushAsync();

                    string? response = await stdout.ReadLineAsync();
                    if (string.IsNullOrEmpty(response)) { _pythonReady = false; break; }

                    using var doc = JsonDocument.Parse(response);
                    var root = doc.RootElement;
                    if (!root.TryGetProperty("angle", out var aElem) ||
                        !root.TryGetProperty("throttle", out var tElem)) continue;

                    double pa = aElem.GetDouble();
                    double pt = tElem.GetDouble();
                    int capturedIdx = idx;

                    this.Invoke((MethodInvoker)delegate
                    {
                        frameAnglePilot[capturedIdx] = pa;
                        frameThrottlePilot[capturedIdx] = pt;

                        if (capturedIdx == currentFrameIndex)
                        {
                            lblAngle.Text = $"조향각\n사용자:{FormatAngle(currentActualAngle)} 자율:{FormatAngle(pa)}";
                            lblThrottle.Text = $"가속값\n사용자:{FormatThrottle(currentActualThrottle)} 자율:{FormatThrottle(pt)}";
                            gaugeBar1.Value = Math.Clamp(pa, -1.0, 1.0);
                            gaugeBar2.Value = pt * 100.0;
                            picImage.Invalidate();
                            UpdateErrorLabels();
                        }
                        RedrawCharts();
                        PredictionUpdated?.Invoke(this, EventArgs.Empty);
                    });
                }
            }
            catch (Exception ex)
            {
                _pythonReady = false;
                Debug.WriteLine($"[RunAllFramePredictionsAsync] {ex.Message}");
            }
            finally
            {
                isPredicting = false;
            }
        }

        /// <summary>밝기/흐림 등 변환 파라미터가 바뀔 때 호출 — 이전 파일럿 예측 캐시를 지웁니다.</summary>
        public void ClearPredictions()
        {
            frameAnglePilot.Clear();
            frameThrottlePilot.Clear();

            // [추가] 필터 초기화 시 보관 중인 가공 비트맵 해제 및 클리어
            foreach (var bmp in frameMemoryBitmaps.Values)
            {
                bmp?.Dispose();
            }
            frameMemoryBitmaps.Clear();
        }

        /// <summary>이미지 경로, 가공 비트맵 및 user 값을 함께 저장.</summary>
        // 1. [기존 호출부 호환용] Bitmap이 넘어오지 않는 기존 코드들을 위한 오버로딩 메서드 추가
        public void SetFrameContext(int frameIndex, string imagePath, double angle, double throttle)
        {
            // 내부적으로 새로 만든 메서드를 호출하되, Bitmap 자리에 null을 전달합니다.
            SetFrameContext(frameIndex, imagePath, null, angle, throttle);
        }

        // 2. [실시간 가공용] 새로 수정하신 매개변수 5개짜리 메서드
        public void SetFrameContext(int frameIndex, string imagePath, Bitmap? memoryBitmap, double angle, double throttle)
        {
            frameImagePaths[frameIndex] = imagePath;
            frameAngleUser[frameIndex] = angle;
            frameThrottleUser[frameIndex] = throttle;

            // 기존에 보관 중이던 비트맵이 있다면 메모리 누수 방지를 위해 해제
            if (frameMemoryBitmaps.TryGetValue(frameIndex, out var oldBmp))
            {
                oldBmp?.Dispose();
            }

            // 새 가공 비트맵 보관
            if (memoryBitmap != null)
            {
                frameMemoryBitmaps[frameIndex] = new Bitmap(memoryBitmap);
            }
            else
            {
                frameMemoryBitmaps.Remove(frameIndex);
            }
        }

        public void UpdateFrame(string imagePath, Bitmap? memoryBitmap, double actualAngle, double actualThrottle, int frameIndex = 0, double? predictedAngle = null, double? predictedThrottle = null)
        {
            currentActualAngle = actualAngle;
            currentActualThrottle = actualThrottle;
            currentImagePath = imagePath; // 기본값은 원본 경로
            currentFrameIndex = frameIndex;

            frameAngleUser[frameIndex] = actualAngle;
            frameThrottleUser[frameIndex] = actualThrottle;
            frameImagePaths[frameIndex] = imagePath;
            _latestWindowCenter = frameIndex;

            var oldImage = picImage.Image;
            var oldStream = currentImageStream;

            try
            {
                // 실시간 가공된 비트맵이 들어온 경우
                if (memoryBitmap != null)
                {
                    picImage.Image = new Bitmap(memoryBitmap);
                    oldImage?.Dispose();
                    oldStream?.Dispose();
                    currentImageStream = null;

                    // [보완 핵심] 파이썬 AI도 흐림/밝기가 적용된 이미지를 보게 만듭니다.
                    // EditedData 대신 윈도우 임시 폴더를 사용해 컴퓨터에 찌꺼기 파일이 남지 않습니다.
                    string tempDir = Path.Combine(Path.GetTempPath(), "PilotArenaCache");
                    if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

                    string tempImagePath = Path.Combine(tempDir, $"temp_frame_{frameIndex}.jpg");

                    // 메모리 비트맵을 임시 파일로 딱 한 장만 저장 (기존 파일에 덮어쓰기 되므로 용량 차지 X)
                    memoryBitmap.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    // 파이썬 프로세스가 읽어갈 경로를 임시 파일 경로로 교체!
                    currentImagePath = tempImagePath;
                    frameImagePaths[frameIndex] = tempImagePath;
                    lastLoadedImagePath = "memory_transformed_" + frameIndex;
                }
                else if (imagePath != lastLoadedImagePath)
                {
                    var newStream = new MemoryStream(File.ReadAllBytes(imagePath));
                    picImage.Image = new Bitmap(newStream);
                    currentImageStream = newStream;
                    lastLoadedImagePath = imagePath;
                    oldImage?.Dispose();
                    oldStream?.Dispose();
                }
            }
            catch { }

            lblAngle.Text = $"조향각\n{FormatAngle(actualAngle)}";
            lblThrottle.Text = $"가속값\n{FormatThrottle(actualThrottle)}";

            if (frameAnglePilot.TryGetValue(frameIndex, out double gaugeA))
                gaugeBar1.Value = Math.Clamp(gaugeA, -1.0, 1.0);
            else
                gaugeBar1.Value = 0.0;

            if (frameThrottlePilot.TryGetValue(frameIndex, out double gaugeT))
                gaugeBar2.Value = gaugeT * 100.0;
            else
                gaugeBar2.Value = 0.0;

            // 하단 그래프 다시 그리기
            RedrawCharts();
            UpdateErrorLabels();

            // AI 예측 실행 및 그래프 반영
            if (predictedAngle.HasValue && predictedThrottle.HasValue)
            {
                UpdatePrediction(predictedAngle.Value, predictedThrottle.Value);
            }
            else if (isModelLoaded && !isPredicting)
            {
                _ = RunPredictionWindowAsync(frameIndex);
            }
        }

        public void UpdatePrediction(double predictedAngle, double predictedThrottle)
        {
            Debug.WriteLine($"[UpdatePrediction] frame={currentFrameIndex} | predictedAngle={predictedAngle:F4} | predictedThrottle={predictedThrottle:F4}");
            Debug.WriteLine($"[UpdatePrediction] currentActualAngle={currentActualAngle:F4} | currentActualThrottle={currentActualThrottle:F4}");
            Debug.WriteLine($"[UpdatePrediction] diff angle={predictedAngle - currentActualAngle:F4} | diff throttle={predictedThrottle - currentActualThrottle:F4}");

            lblAngle.Text = $"조향각\n사용자:{FormatAngle(currentActualAngle)} 자율:{FormatAngle(predictedAngle)}";
            lblThrottle.Text = $"가속값\n사용자:{FormatThrottle(currentActualThrottle)} 자율:{FormatThrottle(predictedThrottle)}";

            frameAnglePilot[currentFrameIndex] = predictedAngle;
            frameThrottlePilot[currentFrameIndex] = predictedThrottle;

            gaugeBar1.Value = Math.Clamp(predictedAngle, -1.0, 1.0);
            gaugeBar2.Value = predictedThrottle * 100.0;

            picImage.Invalidate();
            RedrawCharts();
            UpdateErrorLabels();
        }

        private void RedrawCharts()
        {
            chart1.SuspendLayout();
            chart2.SuspendLayout();

            seriesAngleUser!.Points.Clear();
            seriesAnglePilot!.Points.Clear();
            seriesThrottleUser!.Points.Clear();
            seriesThrottlePilot!.Points.Clear();

            for (int offset = -5; offset <= 5; offset++)
            {
                int idx = currentFrameIndex + offset;
                if (frameAngleUser.TryGetValue(idx, out double ua))
                    seriesAngleUser.Points.AddXY(offset, ua * 45.0);
                if (frameAnglePilot.TryGetValue(idx, out double pa))
                    seriesAnglePilot.Points.AddXY(offset, pa * 45.0);
                if (frameThrottleUser.TryGetValue(idx, out double ut))
                    seriesThrottleUser.Points.AddXY(offset, ut * 100.0);
                if (frameThrottlePilot.TryGetValue(idx, out double pt))
                    seriesThrottlePilot.Points.AddXY(offset, pt * 100.0);
            }

            chart1.ResumeLayout();
            chart2.ResumeLayout();
            chart1.Refresh();
            chart2.Refresh();
        }

        private async Task StartPythonProcessAsync()
        {
            StopPythonProcess();

            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
            string pythonExePath = Path.GetFullPath(Path.Combine(projectRoot, "..", "env", "Scripts", "python.exe"));

            if (!File.Exists(pythonExePath))
            {
                this.Invoke((MethodInvoker)(() =>
                    ReportLog("오류", $"Python 실행 파일을 찾을 수 없습니다: {pythonExePath}")));
                return;
            }

            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PythonScripts", "predict_model.py");

            var psi = new ProcessStartInfo
            {
                FileName = pythonExePath,
                Arguments = $"\"{scriptPath}\" --model \"{selectedModelFilePath}\" --type \"{selectedModelType}\"",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _pythonProcess = new Process { StartInfo = psi };
            _pythonProcess.Start();
            _pythonStdin = _pythonProcess.StandardInput;
            _pythonStdout = _pythonProcess.StandardOutput;

            // stderr 백그라운드 소비 (블록 방지)
            var proc = _pythonProcess;
            _ = Task.Run(async () =>
            {
                try
                {
                    while (!proc.HasExited)
                    {
                        var line = await proc.StandardError.ReadLineAsync();
                        if (line == null) break;
                        Debug.WriteLine($"[Python stderr] {line}");
                    }
                }
                catch { }
            });

            // READY 신호 대기
            try
            {
                string? signal = await _pythonStdout.ReadLineAsync();
                if (signal == "READY")
                {
                    _pythonReady = true;
                    this.Invoke((MethodInvoker)(() =>
                    {
                        string cur = lblModelRoute.Text.Replace(" (로드 중...)", "");
                        lblModelRoute.Text = cur + " ✓";
                        ReportLog("알림", "Python 모델 로드 완료. 추론 준비됨.");
                        ModelReady?.Invoke(this, EventArgs.Empty);
                    }));
                }
                else
                {
                    this.Invoke((MethodInvoker)(() =>
                        ReportLog("오류", $"Python 준비 실패: {signal}")));
                    StopPythonProcess();
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)(() =>
                    ReportLog("오류", $"Python 프로세스 시작 오류: {ex.Message}")));
                StopPythonProcess();
            }
        }

        private void StopPythonProcess()
        {
            _pythonReady = false;
            try { _pythonStdin?.WriteLine("EXIT"); } catch { }
            try { _pythonStdin?.Close(); } catch { }
            _pythonStdin = null;
            _pythonStdout = null;
            try
            {
                if (_pythonProcess?.HasExited == false)
                {
                    _pythonProcess.WaitForExit(2000);
                    if (!_pythonProcess.HasExited)
                        _pythonProcess.Kill();
                }
            }
            catch { }
            try { _pythonProcess?.Dispose(); } catch { }
            _pythonProcess = null;
        }

        /// <summary>
        /// 현재 프레임을 중심으로 ±5 윈도우를 순차 예측합니다.
        /// 예측 순서: 0, +1, -1, +2, -2, ... (현재 프레임 우선)
        /// 사용자가 다른 프레임으로 이동하면 현재 진행 중인 예측 완료 후 중단하고
        /// 새로운 중심으로 다시 시작합니다.
        /// </summary>
        private async Task RunPredictionWindowAsync(int centerFrame)
        {
            if (!_pythonReady || _pythonStdin == null || _pythonStdout == null) return;
            if (isPredicting) return;
            isPredicting = true;

            // 현재 프레임부터, 그 다음 +1/-1, +2/-2 순으로 예측
            int[] offsets = { 0, 1, -1, 2, -2, 3, -3, 4, -4, 5, -5 };

            try
            {
                foreach (int offset in offsets)
                {
                    // 사용자가 다른 프레임으로 이동했으면 중단
                    if (_latestWindowCenter != centerFrame) break;

                    int idx = centerFrame + offset;

                    // 이미 예측된 프레임은 건너뜀
                    if (frameAnglePilot.ContainsKey(idx)) continue;

                    // 이미지 경로가 없으면 건너뜀
                    if (!frameImagePaths.TryGetValue(idx, out string? imgPath)) continue;
                    if (!File.Exists(imgPath)) continue;

                    var stdin = _pythonStdin;
                    var stdout = _pythonStdout;
                    if (stdin == null || stdout == null) break;

                    await stdin.WriteLineAsync(imgPath);
                    await stdin.FlushAsync();

                    string? response = await stdout.ReadLineAsync();
                    if (string.IsNullOrEmpty(response))
                    {
                        _pythonReady = false;
                        break;
                    }

                    using var doc = JsonDocument.Parse(response);
                    var root = doc.RootElement;
                    if (!root.TryGetProperty("angle", out JsonElement aElem) ||
                        !root.TryGetProperty("throttle", out JsonElement tElem)) continue;

                    double pa = aElem.GetDouble();
                    double pt = tElem.GetDouble();
                    int capturedIdx = idx;

                    this.Invoke((MethodInvoker)delegate
                    {
                        frameAnglePilot[capturedIdx] = pa;
                        frameThrottlePilot[capturedIdx] = pt;

                        // 현재 보여주는 프레임이 center인 경우에만 레이블/오버레이 갱신
                        if (capturedIdx == centerFrame && _latestWindowCenter == centerFrame)
                        {
                            lblAngle.Text = $"조향각\n사용자:{FormatAngle(currentActualAngle)} 자율:{FormatAngle(pa)}";
                            lblThrottle.Text = $"가속값\n사용자:{FormatThrottle(currentActualThrottle)} 자율:{FormatThrottle(pt)}";
                            gaugeBar1.Value = Math.Clamp(pa, -1.0, 1.0);
                            gaugeBar2.Value = pt * 100.0;
                            picImage.Invalidate();
                            UpdateErrorLabels();
                        }

                        RedrawCharts();
                        PredictionUpdated?.Invoke(this, EventArgs.Empty);
                    });

                    Debug.WriteLine($"[Window] center={centerFrame} offset={offset:+0;-0;0} frame={capturedIdx} angle={pa:F4} throttle={pt:F4}");
                }
            }
            catch (Exception ex)
            {
                _pythonReady = false;
                Debug.WriteLine($"[RunPredictionWindowAsync] {ex.Message}");
            }
            finally
            {
                isPredicting = false;

                // 예측 도중 사용자가 이동했다면 새 중심으로 재시작
                int latest = _latestWindowCenter;
                if (latest != centerFrame && latest >= 0 && _pythonReady)
                {
                    this.BeginInvoke(new Action(() => _ = RunPredictionWindowAsync(latest)));
                }
            }
        }

        public void UpdateLayout()
        {
            PnlSetting_Resize(this, EventArgs.Empty);
            PnlData_Resize(this, EventArgs.Empty);
        }

        private void PnlSetting_Resize(object? sender, EventArgs e)
        {
            const int m = 4;
            const int lblTypeW = 72; // "linear" 등 모델 유형 고정 너비
            int w = pnlSetting.ClientSize.Width;
            if (w <= 0) return;

            const int btnY = 5, btnH = 26;

            // 창 닫기: 항상 오른쪽 끝
            btnDelModel.SetBounds(w - m - btnDelModel.Width, btnY, btnDelModel.Width, btnH);
            // 전체 그래프: 창 닫기 바로 왼쪽
            btnFullGraph.SetBounds(btnDelModel.Left - m - btnFullGraph.Width, btnY, btnFullGraph.Width, btnH);
            // 모델 타입 레이블: 고정 너비 — 모델 가져오기 오른쪽에 고정
            int lblLeft = m + btnLoadModel.Width + m;
            lblModelType?.SetBounds(lblLeft, btnY, lblTypeW, btnH);
        }

        private void PnlData_Resize(object? sender, EventArgs e)
        {
            const int margin = 3;
            const int gap = 3;
            const int labelW = 65;
            int barH = Math.Max(1, pnlData.ClientSize.Height - margin * 2);
            int half = pnlData.ClientSize.Width / 2;
            int gaugeW = Math.Max(10, half - margin - labelW - gap);

            lblAngle.SetBounds(margin, margin, labelW, barH);
            gaugeBar1.SetBounds(margin + labelW + gap, margin, gaugeW, barH);
            lblThrottle.SetBounds(half + margin, margin, labelW, barH);
            gaugeBar2.SetBounds(half + margin + labelW + gap, margin, gaugeW, barH);
        }

        // FlpModule_SizeChanged가 Controls.Add 이후에 호출되므로
        // BeginInvoke로 레이아웃 확정 후 실행해야 두 번째 모듈도 동일하게 배치됨
        private void ModelTestModule_Load(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                PnlSetting_Resize(this, EventArgs.Empty);
                PnlData_Resize(this, EventArgs.Empty);
            }));
        }

        private static string FormatVal(double v) => v < 0 ? $"{v:F3}" : $" {v:F3}";
        private static string FormatAngle(double v) { double deg = v * 45.0; return deg < 0 ? $"{deg:F1}°" : $" {deg:F1}°"; }
        private static string FormatThrottle(double v) { double pct = v * 100.0; return pct < 0 ? $"{pct:F1}" : $" {pct:F1}"; }

        private void picImage_Click(object sender, EventArgs e) { }

        private void PicImage_Paint(object? sender, PaintEventArgs e)
        {
            if (picImage.Width <= 0 || picImage.Height <= 0) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            DrawSteeringLine(e.Graphics, picImage.Width, picImage.Height, currentActualAngle, currentActualThrottle, Color.FromArgb(220, 0, 210, 0));
            if (frameAnglePilot.TryGetValue(currentFrameIndex, out double pa))
            {
                double pt = frameThrottlePilot.TryGetValue(currentFrameIndex, out double ptVal) ? ptVal : 0.0;
                DrawSteeringLine(e.Graphics, picImage.Width, picImage.Height, pa, pt, Color.FromArgb(220, 30, 144, 255));
            }
        }

        private static void DrawSteeringLine(Graphics g, int w, int h, double angle, double throttle, Color color)
        {
            float startX = w / 2f;
            float startY = h * 0.9f;
            float length = h * (0.225f + 0.45f * (float)Math.Abs(throttle));

            double radians = angle * 45.0 * Math.PI / 180.0;
            float endX = startX + (float)(Math.Sin(radians) * length);
            float endY = startY - (float)(Math.Cos(radians) * length);

            double lineAngle = Math.Atan2(endY - startY, endX - startX);
            float arrowLen = 27f;
            double spread = 25.0 * Math.PI / 180.0;

            using var outline = new Pen(Color.FromArgb(color.A, Color.Black), 13f) { EndCap = System.Drawing.Drawing2D.LineCap.Round, StartCap = System.Drawing.Drawing2D.LineCap.Round };
            g.DrawLine(outline, startX, startY, endX, endY);
            g.DrawLine(outline, endX, endY,
                endX - (float)(Math.Cos(lineAngle + spread) * arrowLen),
                endY - (float)(Math.Sin(lineAngle + spread) * arrowLen));
            g.DrawLine(outline, endX, endY,
                endX - (float)(Math.Cos(lineAngle - spread) * arrowLen),
                endY - (float)(Math.Sin(lineAngle - spread) * arrowLen));

            using var pen = new Pen(color, 9f) { EndCap = System.Drawing.Drawing2D.LineCap.Round, StartCap = System.Drawing.Drawing2D.LineCap.Round };
            g.DrawLine(pen, startX, startY, endX, endY);

            // 화살촉
            g.DrawLine(pen, endX, endY,
                endX - (float)(Math.Cos(lineAngle + spread) * arrowLen),
                endY - (float)(Math.Sin(lineAngle + spread) * arrowLen));
            g.DrawLine(pen, endX, endY,
                endX - (float)(Math.Cos(lineAngle - spread) * arrowLen),
                endY - (float)(Math.Sin(lineAngle - spread) * arrowLen));
        }

        private void lblAngleError_Click(object sender, EventArgs e)
        {

        }

        private void lblThrottleError_Click(object sender, EventArgs e)
        {

        }
    }
}
