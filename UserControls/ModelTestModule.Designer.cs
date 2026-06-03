namespace DataManager.UserControls
{
    partial class ModelTestModule
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

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
            btnFullGraph = new Button();
            pnlSetting = new Panel();
            btnDelModel = new Button();
            lblModelRoute = new Label();
            pnlChart = new Panel();
            lblAngleError = new Label();
            lblThrottleError = new Label();
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
            cboModelType.BackColor = Color.White;
            cboModelType.FlatStyle = FlatStyle.Flat;
            cboModelType.Font = new Font("맑은 고딕", 9.5F);
            cboModelType.FormattingEnabled = true;
            cboModelType.Items.AddRange(new object[] { "linear" });
            cboModelType.Location = new Point(110, 5);
            cboModelType.Name = "cboModelType";
            cboModelType.Size = new Size(120, 25);
            cboModelType.TabIndex = 5;
            cboModelType.Text = "linear";
            // 
            // btnLoadModel
            // 
            btnLoadModel.BackColor = Color.FromArgb(67, 130, 220);
            btnLoadModel.Cursor = Cursors.Hand;
            btnLoadModel.FlatAppearance.BorderSize = 0;
            btnLoadModel.FlatStyle = FlatStyle.Flat;
            btnLoadModel.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnLoadModel.ForeColor = Color.White;
            btnLoadModel.Location = new Point(4, 5);
            btnLoadModel.Name = "btnLoadModel";
            btnLoadModel.Size = new Size(100, 26);
            btnLoadModel.TabIndex = 4;
            btnLoadModel.Text = "모델 가져오기";
            btnLoadModel.UseVisualStyleBackColor = false;
            btnLoadModel.Click += btnLoadModel_Click;
            // 
            // pnlSetting
            // 
            //
            // btnFullGraph
            //
            btnFullGraph.BackColor = Color.FromArgb(130, 100, 200);
            btnFullGraph.Cursor = Cursors.Hand;
            btnFullGraph.FlatAppearance.BorderSize = 0;
            btnFullGraph.FlatStyle = FlatStyle.Flat;
            btnFullGraph.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFullGraph.ForeColor = Color.White;
            btnFullGraph.Location = new Point(220, 5);
            btnFullGraph.Name = "btnFullGraph";
            btnFullGraph.Size = new Size(94, 26);
            btnFullGraph.TabIndex = 8;
            btnFullGraph.Text = "전체 그래프";
            btnFullGraph.UseVisualStyleBackColor = false;
            btnFullGraph.Click += btnFullGraph_Click;
            //
            // pnlSetting
            //
            pnlSetting.BackColor = Color.FromArgb(250, 251, 253);
            pnlSetting.Controls.Add(btnDelModel);
            pnlSetting.Controls.Add(btnFullGraph);
            pnlSetting.Controls.Add(lblModelRoute);
            pnlSetting.Controls.Add(btnLoadModel);
            pnlSetting.Controls.Add(cboModelType);
            pnlSetting.Dock = DockStyle.Top;
            pnlSetting.Location = new Point(0, 0);
            pnlSetting.Name = "pnlSetting";
            pnlSetting.Size = new Size(375, 54);
            pnlSetting.TabIndex = 6;
            // 
            // btnDelModel
            // 
            btnDelModel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelModel.BackColor = Color.FromArgb(210, 70, 70);
            btnDelModel.Cursor = Cursors.Hand;
            btnDelModel.FlatAppearance.BorderSize = 0;
            btnDelModel.FlatStyle = FlatStyle.Flat;
            btnDelModel.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnDelModel.ForeColor = Color.White;
            btnDelModel.Location = new Point(305, 5);
            btnDelModel.Name = "btnDelModel";
            btnDelModel.Size = new Size(66, 26);
            btnDelModel.TabIndex = 7;
            btnDelModel.Text = "창 닫기";
            btnDelModel.UseVisualStyleBackColor = false;
            btnDelModel.Click += btnDelModel_Click;
            // 
            // lblModelRoute
            // 
            lblModelRoute.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblModelRoute.Font = new Font("맑은 고딕", 9F);
            lblModelRoute.ForeColor = Color.FromArgb(120, 130, 150);
            lblModelRoute.Location = new Point(4, 34);
            lblModelRoute.Margin = new Padding(3);
            lblModelRoute.Name = "lblModelRoute";
            lblModelRoute.Size = new Size(369, 16);
            lblModelRoute.TabIndex = 6;
            lblModelRoute.Text = "(Route)";
            lblModelRoute.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlChart
            // 
            pnlChart.BackColor = Color.FromArgb(250, 251, 253);
            pnlChart.Controls.Add(lblAngleError);
            pnlChart.Controls.Add(lblThrottleError);
            pnlChart.Controls.Add(chart2);
            pnlChart.Controls.Add(chart1);
            pnlChart.Dock = DockStyle.Bottom;
            pnlChart.Location = new Point(0, 296);
            pnlChart.Name = "pnlChart";
            pnlChart.Size = new Size(375, 248);
            pnlChart.TabIndex = 7;
            // 
            // lblAngleError
            // 
            lblAngleError.Anchor = AnchorStyles.Right;
            lblAngleError.BackColor = Color.FromArgb(210, 245, 247, 250);
            lblAngleError.BorderStyle = BorderStyle.FixedSingle;
            lblAngleError.Font = new Font("Consolas", 10F, FontStyle.Bold);
            lblAngleError.ForeColor = Color.FromArgb(34, 177, 76);
            lblAngleError.Location = new Point(246, 44);
            lblAngleError.Name = "lblAngleError";
            lblAngleError.Size = new Size(90, 70);
            lblAngleError.TabIndex = 20;
            lblAngleError.Text = "조향각 오차\n-";
            lblAngleError.TextAlign = ContentAlignment.MiddleCenter;
            lblAngleError.Click += lblAngleError_Click;
            // 
            // lblThrottleError
            // 
            lblThrottleError.Anchor = AnchorStyles.Right;
            lblThrottleError.BackColor = Color.FromArgb(210, 245, 247, 250);
            lblThrottleError.BorderStyle = BorderStyle.FixedSingle;
            lblThrottleError.Font = new Font("Consolas", 10F, FontStyle.Bold);
            lblThrottleError.ForeColor = Color.DodgerBlue;
            lblThrottleError.Location = new Point(246, 170);
            lblThrottleError.Name = "lblThrottleError";
            lblThrottleError.Size = new Size(90, 70);
            lblThrottleError.TabIndex = 21;
            lblThrottleError.Text = "가속값 오차\n-";
            lblThrottleError.TextAlign = ContentAlignment.MiddleCenter;
            lblThrottleError.Click += lblThrottleError_Click;
            // 
            // chart2
            // 
            chart2.BackColor = Color.FromArgb(250, 251, 253);
            chartArea1.BackColor = Color.FromArgb(250, 251, 253);
            chartArea1.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea1);
            chart2.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chart2.Legends.Add(legend1);
            chart2.Location = new Point(0, 132);
            chart2.Margin = new Padding(0);
            chart2.Name = "chart2";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = Color.FromArgb(67, 130, 220);
            series1.Legend = "Legend1";
            series1.Name = "각도 오차";
            chart2.Series.Add(series1);
            chart2.Size = new Size(375, 125);
            chart2.TabIndex = 1;
            chart2.Text = "chart2";
            // 
            // chart1
            // 
            chart1.BackColor = Color.FromArgb(250, 251, 253);
            chartArea2.BackColor = Color.FromArgb(250, 251, 253);
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
            series2.Color = Color.FromArgb(72, 175, 120);
            series2.Legend = "Legend1";
            series2.Name = "속도 오차";
            chart1.Series.Add(series2);
            chart1.Size = new Size(375, 123);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            // 
            // pnlImage
            // 
            pnlImage.BackColor = Color.FromArgb(20, 20, 30);
            pnlImage.Controls.Add(picImage);
            pnlImage.Controls.Add(pnlData);
            pnlImage.Dock = DockStyle.Fill;
            pnlImage.Location = new Point(0, 54);
            pnlImage.Name = "pnlImage";
            pnlImage.Size = new Size(375, 242);
            pnlImage.TabIndex = 8;
            // 
            // picImage
            // 
            picImage.BackColor = Color.FromArgb(20, 20, 30);
            picImage.Dock = DockStyle.Fill;
            picImage.Location = new Point(0, 0);
            picImage.Name = "picImage";
            picImage.Size = new Size(375, 212);
            picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            picImage.TabIndex = 1;
            picImage.TabStop = false;
            // 
            // pnlData
            // 
            pnlData.BackColor = Color.FromArgb(250, 251, 253);
            pnlData.Controls.Add(gaugeBar2);
            pnlData.Controls.Add(gaugeBar1);
            pnlData.Controls.Add(lblThrottle);
            pnlData.Controls.Add(lblAngle);
            pnlData.Dock = DockStyle.Bottom;
            pnlData.Location = new Point(0, 212);
            pnlData.Name = "pnlData";
            pnlData.Size = new Size(375, 30);
            pnlData.TabIndex = 0;
            // 
            // gaugeBar2
            // 
            gaugeBar2.Location = new Point(227, 6);
            gaugeBar2.Name = "gaugeBar2";
            gaugeBar2.Size = new Size(138, 16);
            gaugeBar2.TabIndex = 2;
            // 
            // gaugeBar1
            // 
            gaugeBar1.Location = new Point(42, 6);
            gaugeBar1.Name = "gaugeBar1";
            gaugeBar1.Size = new Size(138, 16);
            gaugeBar1.TabIndex = 0;
            // 
            // lblThrottle
            // 
            lblThrottle.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblThrottle.ForeColor = Color.FromArgb(72, 175, 120);
            lblThrottle.Location = new Point(188, 6);
            lblThrottle.Margin = new Padding(5, 3, 3, 3);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(40, 18);
            lblThrottle.TabIndex = 1;
            lblThrottle.Text = "속도";
            // 
            // lblAngle
            // 
            lblAngle.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblAngle.ForeColor = Color.FromArgb(67, 130, 220);
            lblAngle.Location = new Point(5, 6);
            lblAngle.Margin = new Padding(5, 3, 3, 3);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(39, 18);
            lblAngle.TabIndex = 0;
            lblAngle.Text = "각도";
            // 
            // ModelTestModule
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlImage);
            Controls.Add(pnlChart);
            Controls.Add(pnlSetting);
            Margin = new Padding(0);
            Name = "ModelTestModule";
            Size = new Size(375, 544);
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
        private Button btnFullGraph;
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
        private Label lblAngleError;
        private Label lblThrottleError;
    }
}