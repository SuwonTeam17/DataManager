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

namespace DataManager.UserControls
{
    public partial class ModelTestModule : UserControl
    {
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

        // 가장 최근에 요청된 중심 프레임 — 사용자가 다른 프레임으로 이동하면 창 예측을 중단하는 데 사용
        private volatile int _latestWindowCenter = -1;

        private Series? seriesAngleUser;
        private Series? seriesAnglePilot;
        private Series? seriesThrottleUser;
        private Series? seriesThrottlePilot;

        private string lastLoadedImagePath = string.Empty;
        private MemoryStream? currentImageStream;
        private Label lblModelType = null!;
        private Label lblAngleError = null!;
        private Label lblThrottleError = null!;

        public ModelTestModule()
        {
            InitializeComponent();

            picImage.SizeMode = PictureBoxSizeMode.Zoom;
            picImage.Paint += PicImage_Paint;

            // 콤보박스 자리에 meta.txt에서 읽은 모델 유형 레이블 배치
            cboModelType.Visible = false;
            lblModelType = new Label
            {
                Location = cboModelType.Location,
                Size = cboModelType.Size,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "-",
                Padding = new Padding(3, 0, 0, 0),
            };
            pnlSetting.Controls.Add(lblModelType);

            lblAngle.Font = new Font("Consolas", 9f, FontStyle.Bold);
            lblThrottle.Font = new Font("Consolas", 9f, FontStyle.Bold);
            lblAngle.Text = "조향각\n-";
            lblThrottle.Text = "가속값\n-";
            pnlData.Resize += PnlData_Resize;
            Disposed += (s, e) => StopPythonProcess();

            SetupCharts();
            SetupErrorLabels();
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
                area.AxisY.Minimum = -1.2;
                area.AxisY.Maximum = 1.2;
                area.AxisY.Interval = 0.5;
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
                lblAngleError.Text = $"조향각 오차\n{Math.Abs(ua - pa):F4}";
            else
                lblAngleError.Text = "조향각 오차\n-";

            if (frameThrottlePilot.TryGetValue(currentFrameIndex, out double pt) &&
                frameThrottleUser.TryGetValue(currentFrameIndex, out double ut))
                lblThrottleError.Text = $"가속값 오차\n{Math.Abs(ut - pt):F4}";
            else
                lblThrottleError.Text = "가속값 오차\n-";
        }

        private void SetupErrorLabels()
        {
            lblAngleError = MakeErrorLabel("조향각 오차\n-", Color.FromArgb(34, 177, 76));
            lblThrottleError = MakeErrorLabel("가속값 오차\n-", Color.DodgerBlue);
            Controls.Add(lblAngleError);
            Controls.Add(lblThrottleError);
            lblAngleError.BringToFront();
            lblThrottleError.BringToFront();
            Resize += (s, e) => PositionErrorLabels();
        }

        private static Label MakeErrorLabel(string text, Color foreColor) => new Label
        {
            AutoSize = false,
            Size = new Size(90, 34),
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Consolas", 8f, FontStyle.Bold),
            ForeColor = foreColor,
            BackColor = Color.FromArgb(210, 245, 247, 250),
            Text = text,
            BorderStyle = BorderStyle.FixedSingle,
        };

        private void PositionErrorLabels()
        {
            const int chartTotalH = 248;
            const int chart1H    = 132;
            const int chart2H    = chartTotalH - chart1H;
            const int lblW = 90, lblH = 34;

            int chartTop = Height - chartTotalH;
            int x = Width - lblW - 4;

            lblAngleError.SetBounds(x, chartTop + (chart1H - lblH) / 2, lblW, lblH);
            lblThrottleError.SetBounds(x, chartTop + chart1H + (chart2H - lblH) / 2, lblW, lblH);
        }

        public event EventHandler CloseRequested;
        public event Action<string, string, string> OnLogReported;

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
            try
            {
                using (var dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog())
                {
                    dialog.IsFolderPicker = true;
                    dialog.Title = "모델 폴더를 선택하세요";

                    if (dialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
                    {
                        ProcessSelectedModelFolder(dialog.FileName);
                    }
                }
            }
            catch
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "모델 폴더를 선택하세요.";
                    fbd.UseDescriptionForTitle = true;

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        ProcessSelectedModelFolder(fbd.SelectedPath);
                    }
                }
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
                _ = StartPythonProcessAsync();
            }
            else
            {
                lblModelType.Text = "-";
            }
        }

        private static string ReadModelTypeFromMeta(string folderPath)
        {
            try
            {
                string metaPath = Path.Combine(folderPath, "meta.txt");
                if (!File.Exists(metaPath)) return "-";

                string content = File.ReadAllText(metaPath);
                var match = Regex.Match(content, @"\(([^)]+)\)");
                return match.Success ? match.Groups[1].Value : "-";
            }
            catch
            {
                return "-";
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

                    await _pythonStdin.WriteLineAsync(imgPath);
                    await _pythonStdin.FlushAsync();

                    string? response = await _pythonStdout.ReadLineAsync();
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
                            lblAngle.Text = $"조향각\n사용자:{FormatVal(currentActualAngle)} 자율:{FormatVal(pa)}";
                            lblThrottle.Text = $"가속값\n사용자:{FormatVal(currentActualThrottle)} 자율:{FormatVal(pt)}";
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
        }

        /// <summary>이미지 경로와 user 값을 함께 저장. PilotArenaUI가 ±5 윈도우 전체에 호출.</summary>
        public void SetFrameContext(int frameIndex, string imagePath, double angle, double throttle)
        {
            frameImagePaths[frameIndex] = imagePath;
            frameAngleUser[frameIndex] = angle;
            frameThrottleUser[frameIndex] = throttle;
        }

        public void UpdateFrame(string imagePath, double actualAngle, double actualThrottle, int frameIndex = 0, double? predictedAngle = null, double? predictedThrottle = null)
        {
            currentActualAngle = actualAngle;
            currentActualThrottle = actualThrottle;
            currentImagePath = imagePath;
            currentFrameIndex = frameIndex;

            frameAngleUser[frameIndex] = actualAngle;
            frameThrottleUser[frameIndex] = actualThrottle;
            frameImagePaths[frameIndex] = imagePath;
            _latestWindowCenter = frameIndex;

            if (imagePath != lastLoadedImagePath)
            {
                var oldImage = picImage.Image;
                var oldStream = currentImageStream;
                try
                {
                    var newStream = new MemoryStream(File.ReadAllBytes(imagePath));
                    picImage.Image = new Bitmap(newStream);
                    currentImageStream = newStream;
                    lastLoadedImagePath = imagePath;
                    oldImage?.Dispose();
                    oldStream?.Dispose();
                }
                catch { }
            }

            lblAngle.Text = $"조향각\n{FormatVal(actualAngle)}";
            lblThrottle.Text = $"가속값\n{FormatVal(actualThrottle)}";

            gaugeBar1.Value = Math.Clamp(actualAngle, -1.0, 1.0);
            gaugeBar2.Value = Math.Clamp(actualThrottle, -1.0, 1.0);

            RedrawCharts();
            UpdateErrorLabels();

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

            lblAngle.Text = $"조향각\n사용자:{FormatVal(currentActualAngle)} 자율:{FormatVal(predictedAngle)}";
            lblThrottle.Text = $"가속값\n사용자:{FormatVal(currentActualThrottle)} 자율:{FormatVal(predictedThrottle)}";

            frameAnglePilot[currentFrameIndex] = predictedAngle;
            frameThrottlePilot[currentFrameIndex] = predictedThrottle;

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
                    seriesAngleUser.Points.AddXY(offset, ua);
                if (frameAnglePilot.TryGetValue(idx, out double pa))
                    seriesAnglePilot.Points.AddXY(offset, pa);
                if (frameThrottleUser.TryGetValue(idx, out double ut))
                    seriesThrottleUser.Points.AddXY(offset, ut);
                if (frameThrottlePilot.TryGetValue(idx, out double pt))
                    seriesThrottlePilot.Points.AddXY(offset, pt);
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
            string pythonExePath = Path.GetFullPath(Path.Combine(projectRoot, "..", ".venv", "Scripts", "python.exe"));

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
                        ReportLog("정보", "Python 모델 로드 완료. 추론 준비됨.");
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

                    await _pythonStdin.WriteLineAsync(imgPath);
                    await _pythonStdin.FlushAsync();

                    string? response = await _pythonStdout.ReadLineAsync();
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
                            lblAngle.Text = $"조향각\n사용자:{FormatVal(currentActualAngle)} 자율:{FormatVal(pa)}";
                            lblThrottle.Text = $"가속값\n사용자:{FormatVal(currentActualThrottle)} 자율:{FormatVal(pt)}";
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
            PnlData_Resize(this, EventArgs.Empty);
            PositionErrorLabels();
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
            BeginInvoke(new Action(() => PnlData_Resize(this, EventArgs.Empty)));
        }

        private static string FormatVal(double v) => v < 0 ? $"{v:F3}" : $" {v:F3}";

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
            float length = h * (0.15f + 0.30f * (float)Math.Abs(throttle));

            double radians = angle * 45.0 * Math.PI / 180.0;
            float endX = startX + (float)(Math.Sin(radians) * length);
            float endY = startY - (float)(Math.Cos(radians) * length);

            using var pen = new Pen(color, 4.5f) { EndCap = System.Drawing.Drawing2D.LineCap.Round, StartCap = System.Drawing.Drawing2D.LineCap.Round };
            g.DrawLine(pen, startX, startY, endX, endY);

            // 화살촉
            double lineAngle = Math.Atan2(endY - startY, endX - startX);
            float arrowLen = 18f;
            double spread = 25.0 * Math.PI / 180.0;
            g.DrawLine(pen, endX, endY,
                endX - (float)(Math.Cos(lineAngle + spread) * arrowLen),
                endY - (float)(Math.Sin(lineAngle + spread) * arrowLen));
            g.DrawLine(pen, endX, endY,
                endX - (float)(Math.Cos(lineAngle - spread) * arrowLen),
                endY - (float)(Math.Sin(lineAngle - spread) * arrowLen));
        }
    }
}
