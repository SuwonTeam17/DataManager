using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace DataManager.UserControls
{
    public partial class PilotArenaUI : UserControl
    {
        private class FrameData
        {
            public string ImageFileName { get; set; } = string.Empty;
            public double Angle { get; set; }
            public double Throttle { get; set; }
        }

        public event Action<string, string, string> OnLogReported;

        private List<FrameData> frames = new List<FrameData>();
        private string tubFolderPath = string.Empty;
        private int currentFrameIndex = 0;
        private System.Windows.Forms.Timer playbackTimer;

        private Label lblEmptyModels;

        public PilotArenaUI()
        {
            InitializeComponent();

            lblEmptyModels = new Label
            {
                Text = "모델을 추가해주세요",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("맑은 고딕", 14, FontStyle.Bold),
                ForeColor = Color.Gray,
                Visible = true
            };
            flpModule.Controls.Add(lblEmptyModels);

            flpModule.SizeChanged += FlpModule_SizeChanged;

            btnFrameLeft.Click += (s, e) => MoveFrame(-1);
            btnFrameRight.Click += (s, e) => MoveFrame(1);
            btn5FrameLeft.Click += (s, e) => MoveFrame(-5);
            btn5FrameRight.Click += (s, e) => MoveFrame(5);

            trkProgress.ValueChanged += TrkProgress_ValueChanged;
            btnPlay.Click += BtnPlay_Click;
            btnStop.Click += BtnStop_Click;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            comboBox1.TextChanged += ComboBox1_SelectedIndexChanged;

            trkBright.Minimum = -100;
            trkBright.Maximum = 100;
            trkBright.Value = 0;
            trkBlur.Minimum = 0;
            trkBlur.Maximum = 10;
            trkBlur.Value = 0;
            trkBright.ValueChanged += TrkTransform_ValueChanged;
            trkBlur.ValueChanged += TrkTransform_ValueChanged;

            playbackTimer = new System.Windows.Forms.Timer();
            playbackTimer.Tick += PlaybackTimer_Tick;
            UpdateTimerInterval();
        }

        private void UpdateTimerInterval()
        {
            if (double.TryParse(comboBox1.Text, out double speed) && speed > 0)
            {
                // 기준 속도 1.0 -> 1000ms
                int interval = (int)(1000.0 / speed);
                if (interval < 1) interval = 1;
                playbackTimer.Interval = interval;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateTimerInterval();
        }

        private void BtnPlay_Click(object? sender, EventArgs e)
        {
            if (frames.Count > 0)
            {
                UpdateTimerInterval();
                playbackTimer.Start();
            }
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            playbackTimer.Stop();
        }

        private void PlaybackTimer_Tick(object? sender, EventArgs e)
        {
            if (frames.Count == 0) return;
            
            if (currentFrameIndex < frames.Count - 1)
            {
                currentFrameIndex++;
                UpdateTrackBarQuietly();
                ShowCurrentFrame();
            }
            else
            {
                playbackTimer.Stop();
            }
        }

        private void TrkProgress_ValueChanged(object? sender, EventArgs e)
        {
            if (frames.Count == 0) return;
            if (currentFrameIndex != trkProgress.Value)
            {
                currentFrameIndex = trkProgress.Value;
                ShowCurrentFrame();
            }
        }

        private void UpdateTrackBarQuietly()
        {
            if (trkProgress.Maximum >= currentFrameIndex && trkProgress.Minimum <= currentFrameIndex && trkProgress.Value != currentFrameIndex)
            {
                trkProgress.Value = currentFrameIndex;
            }
        }

        private void MoveFrame(int offset)
        {
            if (frames.Count == 0) return;

            int newIndex = currentFrameIndex + offset;
            if (newIndex < 0) newIndex = 0;
            if (newIndex >= frames.Count) newIndex = frames.Count - 1;

            if (currentFrameIndex != newIndex)
            {
                currentFrameIndex = newIndex;
                UpdateTrackBarQuietly();
                ShowCurrentFrame();
            }
        }

        public void LoadTubFolder(string folderPath)
        {
            tubFolderPath = folderPath;
            frames.Clear();
            currentFrameIndex = 0;

            string catalogPath = Path.Combine(folderPath, "catalog_0.catalog");
            string imagesPath = Path.Combine(folderPath, "images");

            if (!File.Exists(catalogPath))
            {
                ReportLog("ERROR", $"catalog_0.catalog 파일을 찾을 수 없습니다. 경로: {folderPath}");
            }
            if (!Directory.Exists(imagesPath))
            {
                ReportLog("ERROR", $"images 폴더를 찾을 수 없습니다. 경로: {folderPath}");
            }

            if (File.Exists(catalogPath))
            {
                ReportLog("INFO", $"Tub 카탈로그 로드 시도 중: {catalogPath}");
                var lines = File.ReadAllLines(catalogPath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        using (JsonDocument doc = JsonDocument.Parse(line))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("cam/image_array", out JsonElement imgElem) &&
                                root.TryGetProperty("user/angle", out JsonElement angleElem) &&
                                root.TryGetProperty("user/throttle", out JsonElement throttleElem))
                            {
                                double rawAngle = angleElem.GetDouble();
                                double rawThrottle = throttleElem.GetDouble();
                                string imgFile = imgElem.GetString() ?? "";

                                // 처음 5개 프레임은 catalog 원본 값 로깅
                                if (frames.Count < 5)
                                {
                                    ReportLog("DEBUG",
                                        $"[Catalog raw] frame={frames.Count} | img={imgFile} | user/angle={rawAngle:F4} | user/throttle={rawThrottle:F4}");
                                }

                                frames.Add(new FrameData
                                {
                                    ImageFileName = imgFile,
                                    Angle = rawAngle,
                                    Throttle = rawThrottle
                                });
                            }
                        }
                    }
                    catch
                    {
                        // 구조가 다르거나 파싱 에러인 라인은 무시
                    }
                }
            }

            if (frames.Count > 0)
            {
                ReportLog("INFO", $"총 {frames.Count} 프레임의 Tub 데이터가 성공적으로 로드되었습니다.");
                trkProgress.Minimum = 0;
                trkProgress.Maximum = frames.Count - 1;
                trkProgress.Value = 0;
            }
            else
            {
                ReportLog("WARN", "유효한 Tub 프레임 데이터가 없습니다.");
                trkProgress.Minimum = 0;
                trkProgress.Maximum = 0;
                trkProgress.Value = 0;
            }

            ShowCurrentFrame();
        }

        private void TrkTransform_ValueChanged(object? sender, EventArgs e)
        {
            lblBright.Text = $"밝기 : {trkBright.Value}";
            lblBlur.Text = $"흐림 : {trkBlur.Value}";
            ShowCurrentFrame();
        }

        private string CreateTransformedImage(string originalImagePath)
        {
            if (!File.Exists(originalImagePath)) return originalImagePath;

            int bright = trkBright.Value;
            int blur = trkBlur.Value;

            if (bright == 0 && blur == 0) return originalImagePath;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            // 프로젝트 루트 또는 BaseDirectory 기준 EditedData 폴더 (여기서는 BaseDirectory와 부모 경로 고려)
            string editedFolder = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\EditedData"));
            if (!Directory.Exists(editedFolder))
            {
                // 부모 경로에 없으면 실행 폴더 바로 밑에 생성
                try { Directory.CreateDirectory(editedFolder); }
                catch { editedFolder = Path.Combine(baseDir, "EditedData"); Directory.CreateDirectory(editedFolder); }
            }

            string fileNameOnly = Path.GetFileNameWithoutExtension(originalImagePath);
            string ext = Path.GetExtension(originalImagePath);
            string newFileName = $"{fileNameOnly}_bright_{bright}_blur_{blur}{ext}";
            string newPath = Path.Combine(editedFolder, newFileName);

            // 캐시 처리
            if (File.Exists(newPath)) return newPath;

            try
            {
                using (Bitmap original = new Bitmap(originalImagePath))
                {
                    Bitmap transformed = original.Clone(new Rectangle(0, 0, original.Width, original.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    int width = transformed.Width;
                    int height = transformed.Height;

                    var rect = new Rectangle(0, 0, width, height);
                    var data = transformed.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, transformed.PixelFormat);

                    int stride = data.Stride;
                    int bytes = Math.Abs(stride) * height;
                    byte[] rgbValues = new byte[bytes];

                    System.Runtime.InteropServices.Marshal.Copy(data.Scan0, rgbValues, 0, bytes);

                    // 밝기 처리
                    if (bright != 0)
                    {
                        for (int i = 0; i < rgbValues.Length; i += 4)
                        {
                            rgbValues[i] = (byte)Math.Clamp(rgbValues[i] + bright, 0, 255);         // B
                            rgbValues[i + 1] = (byte)Math.Clamp(rgbValues[i + 1] + bright, 0, 255); // G
                            rgbValues[i + 2] = (byte)Math.Clamp(rgbValues[i + 2] + bright, 0, 255); // R
                        }
                    }

                    // Box Blur
                    if (blur > 0)
                    {
                        byte[] blurredValues = new byte[bytes];
                        Array.Copy(rgbValues, blurredValues, bytes);
                        int d = blur;

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                int rSum = 0, gSum = 0, bSum = 0, count = 0;

                                for (int dy = -d; dy <= d; dy++)
                                {
                                    int ny = y + dy;
                                    if (ny < 0 || ny >= height) continue;

                                    int nyOffset = ny * stride;
                                    for (int dx = -d; dx <= d; dx++)
                                    {
                                        int nx = x + dx;
                                        if (nx < 0 || nx >= width) continue;

                                        int pixelOffset = nyOffset + (nx * 4);
                                        bSum += rgbValues[pixelOffset];
                                        gSum += rgbValues[pixelOffset + 1];
                                        rSum += rgbValues[pixelOffset + 2];
                                        count++;
                                    }
                                }

                                int outOffset = y * stride + (x * 4);
                                blurredValues[outOffset] = (byte)(bSum / count);
                                blurredValues[outOffset + 1] = (byte)(gSum / count);
                                blurredValues[outOffset + 2] = (byte)(rSum / count);
                                blurredValues[outOffset + 3] = rgbValues[outOffset + 3];
                            }
                        }
                        rgbValues = blurredValues;
                    }

                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, data.Scan0, bytes);
                    transformed.UnlockBits(data);

                    transformed.Save(newPath);
                    transformed.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"이미지 변환 에러: {ex.Message}");
                return originalImagePath;
            }

            return newPath;
        }

        private void ShowCurrentFrame()
        {
            if (frames.Count == 0) return;

            if (currentFrameIndex < 0) currentFrameIndex = 0;
            if (currentFrameIndex >= frames.Count) currentFrameIndex = frames.Count - 1;

            var currentFrame = frames[currentFrameIndex];
            string rawImagePath = Path.Combine(tubFolderPath, "images", currentFrame.ImageFileName);

            System.Diagnostics.Debug.WriteLine(
                $"[ShowFrame] idx={currentFrameIndex} | img={currentFrame.ImageFileName} | user/angle={currentFrame.Angle:F4} | user/throttle={currentFrame.Throttle:F4}");

            string finalImagePath = CreateTransformedImage(rawImagePath);
            if (finalImagePath != rawImagePath)
                System.Diagnostics.Debug.WriteLine($"[ShowFrame] 이미지 변환 적용됨 → {finalImagePath}");

            foreach (Control control in flpModule.Controls)
            {
                if (control is ModelTestModule module)
                {
                    // ±5 윈도우 범위의 이미지 경로 + user 데이터를 미리 등록
                    // → ModelTestModule이 오른쪽 프레임도 예측할 수 있게 됨
                    for (int offset = -5; offset <= 5; offset++)
                    {
                        int idx = currentFrameIndex + offset;
                        if (idx < 0 || idx >= frames.Count) continue;
                        var f = frames[idx];
                        string wRaw  = Path.Combine(tubFolderPath, "images", f.ImageFileName);
                        string wPath = CreateTransformedImage(wRaw);
                        module.SetFrameContext(idx, wPath, f.Angle, f.Throttle);
                    }

                    module.UpdateFrame(finalImagePath, currentFrame.Angle, currentFrame.Throttle, currentFrameIndex);
                }
            }
        }

        private void RefreshModuleLayout()
        {
            var modules = flpModule.Controls.OfType<ModelTestModule>().ToList();
            int count = modules.Count;

            if (count > 0)
            {
                lblEmptyModels.Visible = false;
                int totalW = flpModule.ClientSize.Width;
                int height = flpModule.ClientSize.Height;

                for (int i = 0; i < count; i++)
                {
                    // 마지막 모듈은 나머지 픽셀을 모두 차지해 1px 공백 방지
                    int x     = totalW / count * i;
                    int width = (i == count - 1) ? totalW - x : totalW / count;

                    modules[i].Margin   = new Padding(0);
                    modules[i].Location = new Point(x, 0);   // 위치도 직접 지정
                    modules[i].Size     = new Size(width, height);
                    modules[i].PerformLayout();
                    modules[i].UpdateLayout();
                }
            }
            else
            {
                lblEmptyModels.Visible  = true;
                lblEmptyModels.Location = Point.Empty;
                lblEmptyModels.Margin   = new Padding(0);
                lblEmptyModels.Size     = flpModule.ClientSize;
            }
        }

        // SizeChanged 이벤트 → 창 리사이즈 시 호출
        private void FlpModule_SizeChanged(object? sender, EventArgs e) => RefreshModuleLayout();

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }

        private void btnModelAdd_Click(object sender, EventArgs e)
        {
            var modules = flpModule.Controls.OfType<ModelTestModule>().ToList();
            if (modules.Count >= 3) return; // 최대 3개까지 허용

            var module = new ModelTestModule();
            module.CloseRequested += Module_CloseRequested;
            module.OnLogReported += (time, level, msg) => OnLogReported?.Invoke(time, level, msg);

            flpModule.Controls.Add(module);
            RefreshModuleLayout();

            if (frames.Count > 0)
            {
                ShowCurrentFrame();
            }
        }

        private void Module_CloseRequested(object? sender, EventArgs e)
        {
            if (sender is ModelTestModule module)
            {
                module.CloseRequested -= Module_CloseRequested;
                flpModule.Controls.Remove(module);
                module.Dispose();
                RefreshModuleLayout();
            }
        }

        private void btnLoadTub_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog())
                {
                    dialog.IsFolderPicker = true;
                    dialog.Title = "Tub 데이터 폴더를 선택하세요";

                    if (dialog.ShowDialog() == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
                    {
                        LoadTubFolder(dialog.FileName);
                    }
                }
            }
            catch
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Tub 데이터 폴더를 선택하세요";
                    fbd.UseDescriptionForTitle = true;

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        LoadTubFolder(fbd.SelectedPath);
                    }
                }
            }
        }
    }
}