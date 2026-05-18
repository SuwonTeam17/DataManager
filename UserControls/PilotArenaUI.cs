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

        private List<FrameData> frames = new List<FrameData>();
        private string tubFolderPath = string.Empty;
        private int currentFrameIndex = 0;
        private System.Windows.Forms.Timer playbackTimer;

        public PilotArenaUI()
        {
            InitializeComponent();
            flpModule.WrapContents = false;
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
            
            if (File.Exists(catalogPath))
            {
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
                                frames.Add(new FrameData
                                {
                                    ImageFileName = imgElem.GetString() ?? "",
                                    Angle = angleElem.GetDouble(),
                                    Throttle = throttleElem.GetDouble()
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
                trkProgress.Minimum = 0;
                trkProgress.Maximum = frames.Count - 1;
                trkProgress.Value = 0;
            }
            else
            {
                trkProgress.Minimum = 0;
                trkProgress.Maximum = 0;
                trkProgress.Value = 0;
            }

            ShowCurrentFrame();
        }

        private void ShowCurrentFrame()
        {
            if (frames.Count == 0) return;

            if (currentFrameIndex < 0) currentFrameIndex = 0;
            if (currentFrameIndex >= frames.Count) currentFrameIndex = frames.Count - 1;

            var currentFrame = frames[currentFrameIndex];
            string imagePath = Path.Combine(tubFolderPath, "images", currentFrame.ImageFileName);

            foreach (Control control in flpModule.Controls)
            {
                if (control is ModelTestModule module)
                {
                    module.UpdateFrame(imagePath, currentFrame.Angle, currentFrame.Throttle);
                }
            }
        }

        private void FlpModule_SizeChanged(object? sender, EventArgs e)
        {
            int count = flpModule.Controls.Count;
            if (count > 0)
            {
                int width = flpModule.ClientSize.Width / count;
                int height = flpModule.ClientSize.Height;
                
                // 마진 제거를 위해 Margin 을 0으로 설정
                foreach (Control control in flpModule.Controls)
                {
                    control.Margin = new Padding(0);
                    control.Size = new Size(width, height);
                }
            }
        }

        private void btnModelAdd_Click(object sender, EventArgs e)
        {
            if (flpModule.Controls.Count >= 2) return;

            var module = new ModelTestModule();
            flpModule.Controls.Add(module);
            FlpModule_SizeChanged(null, EventArgs.Empty);

            if (frames.Count > 0)
            {
                ShowCurrentFrame();
            }
        }

        private void btnLoadTub_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Tub 데이터 폴더를 선택하세요. (catalog_0.catalog와 images 폴더 포함)";
                fbd.UseDescriptionForTitle = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    LoadTubFolder(fbd.SelectedPath);
                }
            }
        }
    }
}