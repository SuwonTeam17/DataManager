namespace DataManager.UserControls
{
    partial class ModelTestModule
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            cboModelType = new ComboBox();
            btnLoadModel = new Button();
            pnlSetting = new Panel();
            btnDelModel = new Button();
            lblModelRoute = new Label();
            pnlChart = new Panel();
            chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            pnlImage = new Panel();
            picImage = new PictureBox();
            pnlData = new Panel();
            gaugeBar2 = new GaugeBar();
            gaugeBar1 = new GaugeBar();
            lblThrottle = new Label();
            lblAngle = new Label();
            pnlSetting.SuspendLayout();
            pnlChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chart2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picImage).BeginInit();
            pnlData.SuspendLayout();
            SuspendLayout();
            //
            // cboModelType
            //
            cboModelType.FormattingEnabled = true;
            cboModelType.Items.AddRange(new object[] { "linear" });
            cboModelType.Location = new Point(101, 4);
            cboModelType.Name = "cboModelType";
            cboModelType.Size = new Size(112, 23);
            cboModelType.TabIndex = 5;
            cboModelType.Text = "linear";
            //
            // btnLoadModel
            //
            btnLoadModel.Location = new Point(3, 3);
            btnLoadModel.Name = "btnLoadModel";
            btnLoadModel.Size = new Size(92, 24);
            btnLoadModel.TabIndex = 4;
            btnLoadModel.Text = "모델 가져오기";
            btnLoadModel.UseVisualStyleBackColor = true;
            btnLoadModel.Click += btnLoadModel_Click;
            //
            // pnlSetting
            //
            pnlSetting.BorderStyle = BorderStyle.FixedSingle;
            pnlSetting.Controls.Add(btnDelModel);
            pnlSetting.Controls.Add(lblModelRoute);
            pnlSetting.Controls.Add(btnLoadModel);
            pnlSetting.Controls.Add(cboModelType);
            pnlSetting.Dock = DockStyle.Top;
            pnlSetting.Location = new Point(0, 0);
            pnlSetting.Name = "pnlSetting";
            pnlSetting.Size = new Size(375, 52);
            pnlSetting.TabIndex = 6;
            //
            // btnDelModel
            //
            btnDelModel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelModel.Location = new Point(311, 4);
            btnDelModel.Name = "btnDelModel";
            btnDelModel.Size = new Size(61, 24);
            btnDelModel.TabIndex = 7;
            btnDelModel.Text = "창 닫기";
            btnDelModel.UseVisualStyleBackColor = true;
            btnDelModel.Click += btnDelModel_Click;
            //
            // lblModelRoute
            //
            lblModelRoute.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblModelRoute.Location = new Point(3, 30);
            lblModelRoute.Margin = new Padding(3);
            lblModelRoute.Name = "lblModelRoute";
            lblModelRoute.Size = new Size(369, 19);
            lblModelRoute.TabIndex = 6;
            lblModelRoute.Text = "(Route)";
            lblModelRoute.TextAlign = ContentAlignment.MiddleLeft;
            //
            // pnlChart
            //
            pnlChart.BorderStyle = BorderStyle.FixedSingle;
            pnlChart.Controls.Add(chart2);
            pnlChart.Controls.Add(chart1);
            pnlChart.Dock = DockStyle.Bottom;
            pnlChart.Location = new Point(0, 289);
            pnlChart.Name = "pnlChart";
            pnlChart.Size = new Size(375, 248);
            pnlChart.TabIndex = 7;
            //
            // chart2
            //
            chartArea1.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea1);
            chart2.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chart2.Legends.Add(legend1);
            chart2.Location = new Point(0, 123);
            chart2.Margin = new Padding(0);
            chart2.Name = "chart2";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "각도 오차";
            chart2.Series.Add(series1);
            chart2.Size = new Size(373, 123);
            chart2.TabIndex = 1;
            chart2.Text = "chart2";
            //
            // chart1
            //
            chartArea2.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea2);
            chart1.Dock = DockStyle.Top;
            legend2.Name = "Legend1";
            chart1.Legends.Add(legend2);
            chart1.Location = new Point(0, 0);
            chart1.Margin = new Padding(0);
            chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "속도 오차";
            chart1.Series.Add(series2);
            chart1.Size = new Size(373, 123);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            //
            // pnlImage
            //
            pnlImage.Controls.Add(picImage);
            pnlImage.Controls.Add(pnlData);
            pnlImage.Dock = DockStyle.Fill;
            pnlImage.Location = new Point(0, 52);
            pnlImage.Name = "pnlImage";
            pnlImage.Size = new Size(375, 237);
            pnlImage.TabIndex = 8;
            //
            // picImage
            //
            picImage.BackColor = Color.Black;
            picImage.Dock = DockStyle.Fill;
            picImage.Location = new Point(0, 0);
            picImage.Name = "picImage";
            picImage.Size = new Size(375, 212);
            picImage.TabIndex = 1;
            picImage.TabStop = false;
            //
            // pnlData
            //
            pnlData.BorderStyle = BorderStyle.FixedSingle;
            pnlData.Controls.Add(gaugeBar2);
            pnlData.Controls.Add(gaugeBar1);
            pnlData.Controls.Add(lblThrottle);
            pnlData.Controls.Add(lblAngle);
            pnlData.Dock = DockStyle.Bottom;
            pnlData.Location = new Point(0, 212);
            pnlData.Name = "pnlData";
            pnlData.Size = new Size(375, 50);
            pnlData.TabIndex = 0;
            //
            // gaugeBar2
            //
            gaugeBar2.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            gaugeBar2.Location = new Point(225, 4);
            gaugeBar2.Name = "gaugeBar2";
            gaugeBar2.Size = new Size(138, 15);
            gaugeBar2.TabIndex = 2;
            //
            // gaugeBar1
            //
            gaugeBar1.Location = new Point(42, 4);
            gaugeBar1.Name = "gaugeBar1";
            gaugeBar1.Size = new Size(138, 15);
            gaugeBar1.TabIndex = 0;
            //
            // lblThrottle
            //
            lblThrottle.Location = new Point(188, 4);
            lblThrottle.Margin = new Padding(5, 3, 3, 3);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(31, 15);
            lblThrottle.TabIndex = 1;
            lblThrottle.Text = "속도";
            //
            // lblAngle
            //
            lblAngle.Location = new Point(5, 4);
            lblAngle.Margin = new Padding(5, 3, 3, 3);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(31, 15);
            lblAngle.TabIndex = 0;
            lblAngle.Text = "각도";
            //
            // ModelTestModule
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlImage);
            Controls.Add(pnlChart);
            Controls.Add(pnlSetting);
            Margin = new Padding(0);
            Name = "ModelTestModule";
            Size = new Size(375, 537);
            Load += ModelTestModule_Load;
            pnlSetting.ResumeLayout(false);
            pnlChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chart2).EndInit();
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            pnlImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picImage).EndInit();
            pnlData.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cboModelType;
        private Button btnLoadModel;
        private Panel pnlSetting;
        private Label lblModelRoute;
        private Button btnDelModel;
        private Panel pnlChart;
        private Panel pnlImage;
        private PictureBox picImage;
        private Panel pnlData;
        private Label lblAngle;
        private Label lblThrottle;
        private GaugeBar gaugeBar1;
        private GaugeBar gaugeBar2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}
