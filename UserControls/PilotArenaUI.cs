using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using DataManager.Forms;

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
        private DateTime _lastGraphRefresh = DateTime.MinValue;

        public PilotArenaUI()
        {
            InitializeComponent();

            lblEmptyModels = new Label
            {
                Text = "모델을 추가해주세요",
                AutoSize = false,
                Dock = DockStyle.Fill,
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
                // 기준 속도 1.0 -> 67ms (원래 1배속 대비 15배 빠름)
                int interval = (int)(66.67 / speed);
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
            if (playbackTimer.Enabled)
            {
                playbackTimer.Stop();
                SetPlayButtonState(false);
            }
            else
            {
                if (frames.Count > 0)
                {
                    UpdateTimerInterval();
                    playbackTimer.Start();
                    SetPlayButtonState(true);
                }
            }
        }

        private void SetPlayButtonState(bool isPlaying)
        {
            if (isPlaying)
            {
                btnPlay.Text = "■ 중지";
                btnPlay.BackColor = Color.FromArgb(210, 70, 70);
            }
            else
            {
                btnPlay.Text = "▶ 재생";
                btnPlay.BackColor = Color.FromArgb(72, 175, 120);
            }
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
                SetPlayButtonState(false);
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
                ReportLog("오류", $"catalog_0.catalog 파일을 찾을 수 없습니다. 경로: {folderPath}");
            }
            if (!Directory.Exists(imagesPath))
            {
                ReportLog("오류", $"images 폴더를 찾을 수 없습니다. 경로: {folderPath}");
            }

            if (File.Exists(catalogPath))
            {
                ReportLog("알림", $"Tub 카탈로그 로드 시도 중: {catalogPath}");
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
                                    ReportLog("디버그",
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
                ReportLog("알림", $"총 {frames.Count} 프레임의 Tub 데이터가 성공적으로 로드되었습니다.");
                trkProgress.Minimum = 0;
                trkProgress.Maximum = frames.Count - 1;
                trkProgress.Value = 0;
            }
            else
            {
                ReportLog("알림", "유효한 Tub 프레임 데이터가 없습니다.");
                trkProgress.Minimum = 0;
                trkProgress.Maximum = 0;
                trkProgress.Value = 0;
            }

            ShowCurrentFrame();

            // Tub 로드 시점에 이미 모델이 준비된 모듈이 있으면 전체 스캔 시작
            foreach (var module in flpModule.Controls.OfType<ModelTestModule>())
            {
                if (module.IsModelReady)
                    TriggerFullScanForModule(module);
            }
        }

        private void TrkTransform_ValueChanged(object? sender, EventArgs e)
        {
            lblBright.Text = $"밝기 : {trkBright.Value}";
            lblBlur.Text = $"흐림 : {trkBlur.Value}";

            // 변환 파라미터가 바뀌면 기존 예측 캐시를 무효화해 모델이 새 이미지를 다시 추론하게 함
            foreach (Control control in flpModule.Controls)
            {
                if (control is ModelTestModule module)
                    module.ClearPredictions();
            }

            ShowCurrentFrame();
        }

        // 파일 저장 대신 가공된 Bitmap 자체를 메모리에서 반환하는 메서드
        // 파일로 저장하지 않고, 가공된 비트맵 객체 자체를 메모리 상에서 반환하는 메서드
        // [변경] 하드디스크에 임시 파일을 쓰지 않고, 가공된 원본 비트맵 객체 자체를 리턴하도록 수정
        private Bitmap? GetTransformedBitmap(string originalImagePath)
        {
            if (!File.Exists(originalImagePath)) return null;

            int bright = trkBright.Value;
            int blur = trkBlur.Value;

            // 둘 다 원본 상태(0)라면 null을 반환하여 원본 처리를 유도합니다.
            if (bright == 0 && blur == 0) return null;

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
                            rgbValues[i] = (byte)Math.Clamp(rgbValues[i] + bright, 0, 255);
                            rgbValues[i + 1] = (byte)Math.Clamp(rgbValues[i + 1] + bright, 0, 255);
                            rgbValues[i + 2] = (byte)Math.Clamp(rgbValues[i + 2] + bright, 0, 255);
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

                    // [주요 수정] 하드디스크 저장(Save) 코드 전면 제거! 순수 인메모리 비트맵 오브젝트 반환
                    return transformed;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"이미지 실시간 가공 변환 에러: {ex.Message}");
                return null;
            }
        }

        private void ShowCurrentFrame()
        {
            if (frames.Count == 0) return;
            if (currentFrameIndex < 0) currentFrameIndex = 0;
            if (currentFrameIndex >= frames.Count) currentFrameIndex = frames.Count - 1;

            var currentFrame = frames[currentFrameIndex];
            string rawImagePath = Path.Combine(tubFolderPath, "images", currentFrame.ImageFileName);

            // [수정] 중심 프레임의 실시간 메모리 필터 이미지 추출
            using (Bitmap? finalMemoryBitmap = GetTransformedBitmap(rawImagePath))
            {
                foreach (Control control in flpModule.Controls)
                {
                    if (control is ModelTestModule module)
                    {
                        // ±5 윈도우 데이터 컨텍스트 동기화
                        for (int offset = -5; offset <= 5; offset++)
                        {
                            int idx = currentFrameIndex + offset;
                            if (idx < 0 || idx >= frames.Count) continue;

                            var f = frames[idx];
                            string wRaw = Path.Combine(tubFolderPath, "images", f.ImageFileName);

                            // 각 서브 프레임도 하드디스크 저장 없이 메모리 비트맵으로 생성해서 주입
                            using (Bitmap? windowMemoryBitmap = GetTransformedBitmap(wRaw))
                            {
                                module.SetFrameContext(idx, wRaw, windowMemoryBitmap, f.Angle, f.Throttle);
                            }
                        }

                        // 모듈 화면 출력 갱신 (메모리 비트맵 직접 전달)
                        module.UpdateFrame(rawImagePath, finalMemoryBitmap, currentFrame.Angle, currentFrame.Throttle, currentFrameIndex);
                    }
                }
            }

            foreach (var module in flpModule.Controls.OfType<ModelTestModule>())
                module.UpdateOwnGraphFrame(currentFrameIndex);
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

                flpModule.SuspendLayout();
                for (int i = 0; i < count; i++)
                {
                    // 마지막 모듈은 나머지 픽셀을 모두 차지해 1px 공백 방지
                    int x     = totalW / count * i;
                    int width = (i == count - 1) ? totalW - x : totalW / count;

                    modules[i].Margin   = new Padding(0);
                    modules[i].Location = new Point(x, 0);
                    modules[i].Size     = new Size(width, height);
                    modules[i].PerformLayout();
                    modules[i].UpdateLayout();
                }
                flpModule.ResumeLayout(false);
            }
            else
            {
                lblEmptyModels.Visible = true;
            }
        }

        // SizeChanged 이벤트 → 창 리사이즈 시 호출
        private void FlpModule_SizeChanged(object? sender, EventArgs e) => RefreshModuleLayout();

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }

        private List<(int Index, double Angle, double Throttle)> GetUserFrames() =>
            frames.Select((f, i) => (Index: i, Angle: f.Angle, Throttle: f.Throttle)).ToList();

        // 예측이 추가될 때마다 호출 — 500ms 스로틀링으로 과도한 갱신 방지
        private void Module_PredictionUpdated(object? sender, EventArgs e)
        {
            var now = DateTime.Now;
            if ((now - _lastGraphRefresh).TotalMilliseconds < 500) return;
            _lastGraphRefresh = now;
            foreach (var module in flpModule.Controls.OfType<ModelTestModule>())
                module.RefreshOwnGraph();
        }

        private void TriggerFullScanForModule(ModelTestModule module)
        {
            if (frames.Count == 0 || string.IsNullOrEmpty(tubFolderPath)) return;
            for (int i = 0; i < frames.Count; i++)
            {
                var f = frames[i];
                string rawPath = Path.Combine(tubFolderPath, "images", f.ImageFileName);
                module.SetFrameContext(i, rawPath, f.Angle, f.Throttle);
            }
            _ = module.RunAllFramePredictionsAsync();
        }

        private void btnModelAdd_Click(object sender, EventArgs e)
        {
            var modules = flpModule.Controls.OfType<ModelTestModule>().ToList();
            if (modules.Count >= 3) return; // 최대 3개까지 허용

            var module = new ModelTestModule();
            module.CloseRequested += Module_CloseRequested;
            module.OnLogReported += (time, level, msg) => OnLogReported?.Invoke(time, level, msg);
            module.ModelReady += (s, e) => TriggerFullScanForModule(module);
            module.PredictionUpdated += Module_PredictionUpdated;
            module.UserFramesProvider = GetUserFrames;

            flpModule.Controls.Add(module);

            // BeginInvoke로 AutoScaleMode.Font 스케일링이 완전히 끝난 뒤 레이아웃 확정
            BeginInvoke(new Action(() =>
            {
                RefreshModuleLayout();
                if (frames.Count > 0)
                    ShowCurrentFrame();
            }));
        }

        private void Module_CloseRequested(object? sender, EventArgs e)
        {
            if (sender is ModelTestModule module)
            {
                module.CloseRequested -= Module_CloseRequested;
                module.PredictionUpdated -= Module_PredictionUpdated;
                flpModule.Controls.Remove(module);
                module.Dispose();
                RefreshModuleLayout();
            }
        }

        private void btnLoadTub_Click(object sender, EventArgs e)
        {
            string root = AppPaths.EditedData;
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);


            using (var browser = new CustomFolderBrowser(root, "Tub 데이터 폴더 선택"))
            {
                browser.AllowFileSelection = true;

                if (browser.ShowDialog(this) == DialogResult.OK)
                    LoadTubFolder(browser.SelectedPath);
            }

        }
    }
}