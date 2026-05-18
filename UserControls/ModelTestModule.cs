using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

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

            // Chart 1 (Throttle)
            var seriesThrottleUser = new Series("user/throttle");
            seriesThrottleUser.ChartType = SeriesChartType.Line;
            chart1.Series.Add(seriesThrottleUser);

            var seriesThrottlePilot = new Series("pilot/throttle");
            seriesThrottlePilot.ChartType = SeriesChartType.Line;
            chart1.Series.Add(seriesThrottlePilot);

            // Chart 2 (Angle)
            var seriesAngleUser = new Series("user/angle");
            seriesAngleUser.ChartType = SeriesChartType.Line;
            chart2.Series.Add(seriesAngleUser);

            var seriesAnglePilot = new Series("pilot/angle");
            seriesAnglePilot.ChartType = SeriesChartType.Line;
            chart2.Series.Add(seriesAnglePilot);
        }

        private void btnDelModel_Click(object sender, EventArgs e)
        {
            var parent = this.Parent;
            parent?.Controls.Remove(this);
            this.Dispose();
        }

        private void btnLoadModel_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "모델 폴더를 선택하세요.";
                fbd.UseDescriptionForTitle = true;
                
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = fbd.SelectedPath;
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
                        MessageBox.Show("지원하지 않는 모델 폴더입니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        isModelLoaded = false;
                        selectedModelType = string.Empty;
                        selectedModelFolderPath = string.Empty;
                        selectedModelFilePath = string.Empty;
                        lblModelRoute.Text = "(Route)";
                    }
                }
            }
        }

        public void UpdateFrame(string imagePath, double actualAngle, double actualThrottle, double? predictedAngle = null, double? predictedThrottle = null)
        {
            currentActualAngle = actualAngle;
            currentActualThrottle = actualThrottle;

            if (File.Exists(imagePath))
            {
                var oldImage = picImage.Image;
                try
                {
                    // Image.FromFile로 로드 후 새 Bitmap으로 복사하여 기존 이미지 파일의 잠금을 해제
                    using (var img = Image.FromFile(imagePath))
                    {
                        picImage.Image = new Bitmap(img);
                    }
                    oldImage?.Dispose();
                }
                catch
                {
                    // 로드 중 파일 접근 점유 등의 예외 무시
                }
            }

            lblAngle.Text = $"Angle: {actualAngle:F3}";
            lblThrottle.Text = $"Throttle: {actualThrottle:F3}";

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
        }

        public void UpdatePrediction(double predictedAngle, double predictedThrottle)
        {
            double angleError = Math.Abs(currentActualAngle - predictedAngle);
            double throttleError = Math.Abs(currentActualThrottle - predictedThrottle);

            lblAngle.Text = $"user/angle: {currentActualAngle:F3} | pilot/angle: {predictedAngle:F3} | error: {angleError:F3}";
            lblThrottle.Text = $"user/throttle: {currentActualThrottle:F3} | pilot/throttle: {predictedThrottle:F3} | error: {throttleError:F3}";

            // Chart2: Angle update
            chart2.Series["user/angle"].Points.AddY(currentActualAngle);
            chart2.Series["pilot/angle"].Points.AddY(predictedAngle);

            if (chart2.Series["user/angle"].Points.Count > 100)
            {
                chart2.Series["user/angle"].Points.RemoveAt(0);
                chart2.Series["pilot/angle"].Points.RemoveAt(0);
            }

            // Chart1: Throttle update
            chart1.Series["user/throttle"].Points.AddY(currentActualThrottle);
            chart1.Series["pilot/throttle"].Points.AddY(predictedThrottle);

            if (chart1.Series["user/throttle"].Points.Count > 100)
            {
                chart1.Series["user/throttle"].Points.RemoveAt(0);
                chart1.Series["pilot/throttle"].Points.RemoveAt(0);
            }
        }
    }
}