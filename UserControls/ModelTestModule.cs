using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.Json;

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
        private int chartFrameIndex = 0;

        public ModelTestModule()
        {
            InitializeComponent();

            picImage.SizeMode = PictureBoxSizeMode.Zoom;

            if (!cboModelType.Items.Contains("linear"))
            {
                cboModelType.Items.Add("linear");
            }
            cboModelType.SelectedItem = "linear";

            // 라벨 크기를 내용에 맞게 자동 조절하고 초기 텍스트 설정
            lblAngle.AutoSize = true;
            lblThrottle.AutoSize = true;
            lblAngle.Text = "Angle: -";
            lblThrottle.Text = "Throttle: -";

            SetupCharts();
        }

        private void SetupCharts()
        {
            chart1.Series.Clear();
            chart2.Series.Clear();

            if (chart1.ChartAreas.Count > 0)
            {
                chart1.ChartAreas[0].AxisX.Title = "Frame";
            }

            if (chart2.ChartAreas.Count > 0)
            {
                chart2.ChartAreas[0].AxisX.Title = "Frame";
            }

            // Chart 1 (Angle)
            var seriesAngleUser = new Series("user/angle");
            seriesAngleUser.ChartType = SeriesChartType.Line;
            seriesAngleUser.MarkerStyle = MarkerStyle.Circle;
            chart1.Series.Add(seriesAngleUser);

            var seriesAnglePilot = new Series("pilot/angle");
            seriesAnglePilot.ChartType = SeriesChartType.Line;
            seriesAnglePilot.MarkerStyle = MarkerStyle.Circle;
            chart1.Series.Add(seriesAnglePilot);

            // Chart 2 (Throttle)
            var seriesThrottleUser = new Series("user/throttle");
            seriesThrottleUser.ChartType = SeriesChartType.Line;
            seriesThrottleUser.MarkerStyle = MarkerStyle.Circle;
            chart2.Series.Add(seriesThrottleUser);

            var seriesThrottlePilot = new Series("pilot/throttle");
            seriesThrottlePilot.ChartType = SeriesChartType.Line;
            seriesThrottlePilot.MarkerStyle = MarkerStyle.Circle;
            chart2.Series.Add(seriesThrottlePilot);
        }

        public event EventHandler CloseRequested;

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
                ReportLog("ERROR", "지원하지 않는 모델 폴더입니다.");
                isModelLoaded = false;
                selectedModelType = string.Empty;
                selectedModelFolderPath = string.Empty;
                selectedModelFilePath = string.Empty;
                lblModelRoute.Text = "(Route)";
            }

            if (isModelLoaded && !string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath))
            {
                _ = RunPredictionAsync();
            }
        }

        public void UpdateFrame(string imagePath, double actualAngle, double actualThrottle, double? predictedAngle = null, double? predictedThrottle = null)
        {
            currentActualAngle = actualAngle;
            currentActualThrottle = actualThrottle;
            currentImagePath = imagePath;

            if (File.Exists(imagePath))
            {
                var oldImage = picImage.Image;
                try
                {
                    // 디스크 I/O 최적화: 파일을 한 번에 메모리로 읽고 스트림으로 변환
                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var img = Image.FromStream(ms))
                        {
                            picImage.Image = new Bitmap(img);
                        }
                    }
                    oldImage?.Dispose();
                }
                catch
                {
                    // 로드 중 파일 접근 점유 등의 예외 무시
                }
            }

            if (selectedModelType == "savedmodel")
            {
                lblAngle.Text = "savedmodel prediction not supported";
                lblThrottle.Text = "savedmodel prediction not supported";
            }
            else
            {
                lblAngle.Text = $"Angle: {actualAngle:F3}";
                lblThrottle.Text = $"Throttle: {actualThrottle:F3}";
            }

            // Angle (-1.0 ~ 1.0) -> (0 ~ 100)
            int angleVal = (int)Math.Round((actualAngle + 1.0) * 50.0);

            // Throttle (0.0 ~ 1.0) -> (0 ~ 100)
            int throttleVal = (int)Math.Round(actualThrottle * 100.0);

            // progressBar1 (Angle), progressBar2 (Throttle)
            progressBar1.Value = Math.Clamp(angleVal, 0, 100);
            progressBar2.Value = Math.Clamp(throttleVal, 0, 100);

            // 추후 AI 예측값을 표시하기 위한 뼈대 구조
            if (predictedAngle.HasValue && predictedThrottle.HasValue)
            {
                UpdatePrediction(predictedAngle.Value, predictedThrottle.Value);
            }
            else if (isModelLoaded && !string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath) && !isPredicting)
            {
                _ = RunPredictionAsync();
            }
        }

        public void UpdatePrediction(double predictedAngle, double predictedThrottle)
        {
            if (selectedModelType == "savedmodel") return;

            double angleError = Math.Abs(currentActualAngle - predictedAngle);
            double throttleError = Math.Abs(currentActualThrottle - predictedThrottle);

            lblAngle.Text = $"user/angle: {currentActualAngle:F3} | pilot/angle: {predictedAngle:F3} | error: {angleError:F3}";
            lblThrottle.Text = $"user/throttle: {currentActualThrottle:F3} | pilot/throttle: {predictedThrottle:F3} | error: {throttleError:F3}";

            // Chart1: Angle update
            chart1.Series["user/angle"].Points.AddXY(chartFrameIndex, currentActualAngle);
            chart1.Series["pilot/angle"].Points.AddXY(chartFrameIndex, predictedAngle);

            if (chart1.Series["user/angle"].Points.Count > 100)
            {
                chart1.Series["user/angle"].Points.RemoveAt(0);
                chart1.Series["pilot/angle"].Points.RemoveAt(0);
            }

            // Chart2: Throttle update
            chart2.Series["user/throttle"].Points.AddXY(chartFrameIndex, currentActualThrottle);
            chart2.Series["pilot/throttle"].Points.AddXY(chartFrameIndex, predictedThrottle);

            if (chart2.Series["user/throttle"].Points.Count > 100)
            {
                chart2.Series["user/throttle"].Points.RemoveAt(0);
                chart2.Series["pilot/throttle"].Points.RemoveAt(0);
            }

            chartFrameIndex++;
        }

        private async Task RunPredictionAsync()
        {
            if (!isModelLoaded || string.IsNullOrEmpty(selectedModelFilePath) || string.IsNullOrEmpty(selectedModelType) || string.IsNullOrEmpty(currentImagePath) || !File.Exists(currentImagePath))
                return;

            if (selectedModelType == "savedmodel")
                return;

            if (isPredicting) return;
            isPredicting = true;

            try
            {
                string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                // DataManager 폴더 바깥에 있는 .venv를 참조하도록 수정합니다.
                string pythonExePath = Path.GetFullPath(Path.Combine(projectRoot, "..", ".venv", "Scripts", "python.exe"));

                if (!File.Exists(pythonExePath))
                {
                    ReportLog("ERROR", $"Python 실행 파일을 찾을 수 없습니다: {pythonExePath}");
                    Debug.WriteLine($"[RunPredictionAsync] Error: Python executable not found at {pythonExePath}");
                    return;
                }

                string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PythonScripts", "predict_model.py");

                Debug.WriteLine("[RunPredictionAsync] Executing prediction...");
                Debug.WriteLine($"[RunPredictionAsync] Using python executable: {pythonExePath}");
                Debug.WriteLine($"[RunPredictionAsync] selectedModelPath: {selectedModelFilePath}");
                Debug.WriteLine($"[RunPredictionAsync] selectedModelType: {selectedModelType}");
                Debug.WriteLine($"[RunPredictionAsync] currentImagePath: {currentImagePath}");
                Debug.WriteLine($"[RunPredictionAsync] scriptPath: {scriptPath}");

                var psi = new ProcessStartInfo
                {
                    FileName = pythonExePath,
                    Arguments = $"\"{scriptPath}\" --model \"{selectedModelFilePath}\" --type \"{selectedModelType}\" --image \"{currentImagePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = psi })
                {
                    process.Start();
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    Debug.WriteLine($"[RunPredictionAsync] Python stdout: {output}");
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        ReportLog("WARN", $"Python stderr: {error}");
                        Debug.WriteLine($"[RunPredictionAsync] Python stderr: {error}");
                    }

                    if (process.ExitCode != 0)
                    {
                        ReportLog("ERROR", $"Python 실행이 종료 코드 {process.ExitCode}로 실패했습니다.");
                        Debug.WriteLine($"[RunPredictionAsync] Python execution failed with exit code: {process.ExitCode}");
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        try
                        {
                            var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            // 마지막으로 출력된 JSON 형식 데이터를 찾음
                            string jsonLine = System.Linq.Enumerable.LastOrDefault(lines, l => l.Trim().StartsWith("{"));
                            if (jsonLine != null)
                            {
                                using (JsonDocument doc = JsonDocument.Parse(jsonLine))
                                {
                                    var root = doc.RootElement;
                                    if (root.TryGetProperty("angle", out JsonElement angleElem) &&
                                        root.TryGetProperty("throttle", out JsonElement throttleElem))
                                    {
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            UpdatePrediction(angleElem.GetDouble(), throttleElem.GetDouble());
                                        });
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[RunPredictionAsync] JSON Parse Exception: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportLog("ERROR", $"Python 예측 중 예외 발생: {ex.Message}");
                Debug.WriteLine($"[RunPredictionAsync] Exception: {ex.Message}");
            }
            finally
            {
                isPredicting = false;
            }
        }

        private void ModelTestModule_Load(object sender, EventArgs e)
        {

        }

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }
    }
}