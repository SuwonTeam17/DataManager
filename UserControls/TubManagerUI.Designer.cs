namespace DataManager.UserControls
{
    partial class TubManagerUI
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            pnlChart = new Panel();
            chtData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            btnFileLoad = new Button();
            btnNewFolder = new Button();
            btnSaveRoute = new Button();
            btnSaveData = new Button();
            btnDelFolder = new Button();
            lblSaveRoute = new Label();
            pnlFile = new Panel();
            chkSaveContinue = new CheckBox();
            pnlSetting = new Panel();
            pnlSet = new Panel();
            btnInitFillterSet = new Button();
            btnCancelFillter = new Button();
            btnApplyFillter = new Button();
            lblSelectedRange = new Label();
            btnAllRange = new Button();
            btnRightRange = new Button();
            btnLeftRange = new Button();
            pnlFillter = new Panel();
            chkRemoveImage = new CheckBox();
            chkSetBlur = new CheckBox();
            trkSetBlur = new TrackBar();
            chkSetBright = new CheckBox();
            trkSetBright = new TrackBar();
            chkApplyBlackWhite = new CheckBox();
            chkInverseColor = new CheckBox();
            chkDelAngle = new CheckBox();
            chkDelThrottle = new CheckBox();
            lblFillter = new Label();
            pnlSub = new Panel();
            pnlControl = new Panel();
            lblSpeed = new Label();
            comboBox1 = new ComboBox();
            lblAllImageNumRange = new Label();
            btnStop = new Button();
            btnPlay = new Button();
            btn5FrameRight = new Button();
            btn5FrameLeft = new Button();
            btnFrameRight = new Button();
            btnFrameLeft = new Button();
            pnlData = new Panel();
            lblThrottleDetail = new Label();
            lblAngleDetail = new Label();
            lblThrottle = new Label();
            lblAngle = new Label();
            prgAngle = new ProgressBar();
            prgThrottle = new ProgressBar();
            pnlImage = new Panel();
            picImage = new PictureBox();
            trkProgress = new TrackBar();
            pnlTimeStamp = new Panel();
            pnlChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chtData).BeginInit();
            pnlFile.SuspendLayout();
            pnlSetting.SuspendLayout();
            pnlSet.SuspendLayout();
            pnlFillter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkSetBlur).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkSetBright).BeginInit();
            pnlSub.SuspendLayout();
            pnlControl.SuspendLayout();
            pnlData.SuspendLayout();
            pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkProgress).BeginInit();
            SuspendLayout();
            // 
            // pnlChart
            // 
            pnlChart.BackColor = Color.FromArgb(250, 251, 253);
            pnlChart.Controls.Add(chtData);
            pnlChart.Dock = DockStyle.Bottom;
            pnlChart.Location = new Point(0, 616);
            pnlChart.Name = "pnlChart";
            pnlChart.Size = new Size(950, 150);
            pnlChart.TabIndex = 3;
            // 
            // chtData
            // 
            chtData.BackColor = Color.FromArgb(250, 251, 253);
            chartArea1.BackColor = Color.FromArgb(250, 251, 253);
            chartArea1.Name = "ChartArea1";
            chtData.ChartAreas.Add(chartArea1);
            chtData.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chtData.Legends.Add(legend1);
            chtData.Location = new Point(0, 0);
            chtData.Name = "chtData";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = Color.FromArgb(67, 130, 220);
            series1.Legend = "Legend1";
            series1.Name = "각도";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Color = Color.FromArgb(72, 175, 120);
            series2.Legend = "Legend1";
            series2.Name = "속도";
            chtData.Series.Add(series1);
            chtData.Series.Add(series2);
            chtData.Size = new Size(950, 150);
            chtData.TabIndex = 0;
            chtData.Text = "chart1";
            // 
            // btnFileLoad
            // 
            btnFileLoad.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnFileLoad.BackColor = Color.FromArgb(67, 130, 220);
            btnFileLoad.Cursor = Cursors.Hand;
            btnFileLoad.FlatAppearance.BorderSize = 0;
            btnFileLoad.FlatStyle = FlatStyle.Flat;
            btnFileLoad.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFileLoad.ForeColor = Color.White;
            btnFileLoad.Location = new Point(4, 5);
            btnFileLoad.Name = "btnFileLoad";
            btnFileLoad.Size = new Size(143, 30);
            btnFileLoad.TabIndex = 0;
            btnFileLoad.Text = "주행 데이터 가져오기";
            btnFileLoad.UseVisualStyleBackColor = false;
            btnFileLoad.Click += btnFileLoad_Click;
            // 
            // btnNewFolder
            // 
            btnNewFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnNewFolder.BackColor = Color.FromArgb(72, 175, 120);
            btnNewFolder.Cursor = Cursors.Hand;
            btnNewFolder.FlatAppearance.BorderSize = 0;
            btnNewFolder.FlatStyle = FlatStyle.Flat;
            btnNewFolder.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnNewFolder.ForeColor = Color.White;
            btnNewFolder.Location = new Point(153, 5);
            btnNewFolder.Name = "btnNewFolder";
            btnNewFolder.Size = new Size(100, 30);
            btnNewFolder.TabIndex = 1;
            btnNewFolder.Text = "새 폴더 생성";
            btnNewFolder.UseVisualStyleBackColor = false;
            btnNewFolder.Click += btnNewFolder_Click;
            // 
            // btnSaveRoute
            // 
            btnSaveRoute.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnSaveRoute.BackColor = Color.FromArgb(210, 140, 40);
            btnSaveRoute.Cursor = Cursors.Hand;
            btnSaveRoute.FlatAppearance.BorderSize = 0;
            btnSaveRoute.FlatStyle = FlatStyle.Flat;
            btnSaveRoute.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnSaveRoute.ForeColor = Color.White;
            btnSaveRoute.Location = new Point(345, 5);
            btnSaveRoute.Name = "btnSaveRoute";
            btnSaveRoute.Size = new Size(110, 30);
            btnSaveRoute.TabIndex = 3;
            btnSaveRoute.Text = "저장 경로 지정";
            btnSaveRoute.UseVisualStyleBackColor = false;
            btnSaveRoute.Click += btnSaveRoute_Click;
            // 
            // btnSaveData
            // 
            btnSaveData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveData.BackColor = Color.FromArgb(67, 130, 220);
            btnSaveData.Cursor = Cursors.Hand;
            btnSaveData.FlatAppearance.BorderSize = 0;
            btnSaveData.FlatStyle = FlatStyle.Flat;
            btnSaveData.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnSaveData.ForeColor = Color.White;
            btnSaveData.Location = new Point(820, 5);
            btnSaveData.Name = "btnSaveData";
            btnSaveData.Size = new Size(124, 30);
            btnSaveData.TabIndex = 5;
            btnSaveData.Text = "데이터 저장";
            btnSaveData.UseVisualStyleBackColor = false;
            btnSaveData.Click += btnSaveData_Click;
            // 
            // btnDelFolder
            // 
            btnDelFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnDelFolder.BackColor = Color.FromArgb(210, 70, 70);
            btnDelFolder.Cursor = Cursors.Hand;
            btnDelFolder.FlatAppearance.BorderSize = 0;
            btnDelFolder.FlatStyle = FlatStyle.Flat;
            btnDelFolder.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnDelFolder.ForeColor = Color.White;
            btnDelFolder.Location = new Point(259, 5);
            btnDelFolder.Name = "btnDelFolder";
            btnDelFolder.Size = new Size(80, 30);
            btnDelFolder.TabIndex = 2;
            btnDelFolder.Text = "폴더 삭제";
            btnDelFolder.UseVisualStyleBackColor = false;
            btnDelFolder.Click += btnDelFolder_Click;
            // 
            // lblSaveRoute
            // 
            lblSaveRoute.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblSaveRoute.Font = new Font("맑은 고딕", 9F);
            lblSaveRoute.ForeColor = Color.FromArgb(120, 130, 150);
            lblSaveRoute.Location = new Point(569, 5);
            lblSaveRoute.Name = "lblSaveRoute";
            lblSaveRoute.Size = new Size(220, 30);
            lblSaveRoute.TabIndex = 4;
            lblSaveRoute.Text = "(현재 경로)";
            // 
            // pnlFile
            // 
            pnlFile.BackColor = Color.FromArgb(250, 251, 253);
            pnlFile.Controls.Add(btnSaveData);
            pnlFile.Controls.Add(chkSaveContinue);
            pnlFile.Controls.Add(btnFileLoad);
            pnlFile.Controls.Add(btnDelFolder);
            pnlFile.Controls.Add(lblSaveRoute);
            pnlFile.Controls.Add(btnSaveRoute);
            pnlFile.Controls.Add(btnNewFolder);
            pnlFile.Dock = DockStyle.Top;
            pnlFile.Location = new Point(0, 0);
            pnlFile.Name = "pnlFile";
            pnlFile.Size = new Size(950, 40);
            pnlFile.TabIndex = 1;
            // 
            // chkSaveContinue
            // 
            chkSaveContinue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            chkSaveContinue.Font = new Font("맑은 고딕", 9.5F);
            chkSaveContinue.ForeColor = Color.FromArgb(60, 72, 92);
            chkSaveContinue.Location = new Point(463, 9);
            chkSaveContinue.Margin = new Padding(0);
            chkSaveContinue.Name = "chkSaveContinue";
            chkSaveContinue.Size = new Size(100, 22);
            chkSaveContinue.TabIndex = 6;
            chkSaveContinue.Text = "이어서 저장";
            chkSaveContinue.UseVisualStyleBackColor = true;
            // 
            // pnlSetting
            // 
            pnlSetting.BackColor = Color.FromArgb(250, 251, 253);
            pnlSetting.Controls.Add(pnlSet);
            pnlSetting.Controls.Add(pnlFillter);
            pnlSetting.Dock = DockStyle.Bottom;
            pnlSetting.Location = new Point(0, 516);
            pnlSetting.Name = "pnlSetting";
            pnlSetting.Size = new Size(950, 100);
            pnlSetting.TabIndex = 4;
            // 
            // pnlSet
            // 
            pnlSet.BackColor = Color.FromArgb(250, 251, 253);
            pnlSet.Controls.Add(btnInitFillterSet);
            pnlSet.Controls.Add(btnCancelFillter);
            pnlSet.Controls.Add(btnApplyFillter);
            pnlSet.Controls.Add(lblSelectedRange);
            pnlSet.Controls.Add(btnAllRange);
            pnlSet.Controls.Add(btnRightRange);
            pnlSet.Controls.Add(btnLeftRange);
            pnlSet.Dock = DockStyle.Fill;
            pnlSet.Location = new Point(0, 0);
            pnlSet.Name = "pnlSet";
            pnlSet.Size = new Size(950, 44);
            pnlSet.TabIndex = 6;
            // 
            // btnInitFillterSet
            // 
            btnInitFillterSet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnInitFillterSet.BackColor = Color.FromArgb(210, 70, 70);
            btnInitFillterSet.Cursor = Cursors.Hand;
            btnInitFillterSet.FlatAppearance.BorderSize = 0;
            btnInitFillterSet.FlatStyle = FlatStyle.Flat;
            btnInitFillterSet.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnInitFillterSet.ForeColor = Color.White;
            btnInitFillterSet.Location = new Point(754, 7);
            btnInitFillterSet.Name = "btnInitFillterSet";
            btnInitFillterSet.Size = new Size(160, 27);
            btnInitFillterSet.TabIndex = 8;
            btnInitFillterSet.Text = "변경 사항 설정 초기화";
            btnInitFillterSet.UseVisualStyleBackColor = false;
            btnInitFillterSet.Click += btnInitFillterSet_Click;
            // 
            // btnCancelFillter
            // 
            btnCancelFillter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelFillter.BackColor = Color.FromArgb(210, 140, 40);
            btnCancelFillter.Cursor = Cursors.Hand;
            btnCancelFillter.FlatAppearance.BorderSize = 0;
            btnCancelFillter.FlatStyle = FlatStyle.Flat;
            btnCancelFillter.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnCancelFillter.ForeColor = Color.White;
            btnCancelFillter.Location = new Point(638, 7);
            btnCancelFillter.Name = "btnCancelFillter";
            btnCancelFillter.Size = new Size(110, 27);
            btnCancelFillter.TabIndex = 7;
            btnCancelFillter.Text = "변경 사항 취소";
            btnCancelFillter.UseVisualStyleBackColor = false;
            btnCancelFillter.Click += btnCancelFillter_Click;
            // 
            // btnApplyFillter
            // 
            btnApplyFillter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnApplyFillter.BackColor = Color.FromArgb(72, 175, 120);
            btnApplyFillter.Cursor = Cursors.Hand;
            btnApplyFillter.FlatAppearance.BorderSize = 0;
            btnApplyFillter.FlatStyle = FlatStyle.Flat;
            btnApplyFillter.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnApplyFillter.ForeColor = Color.White;
            btnApplyFillter.Location = new Point(522, 7);
            btnApplyFillter.Name = "btnApplyFillter";
            btnApplyFillter.Size = new Size(110, 27);
            btnApplyFillter.TabIndex = 6;
            btnApplyFillter.Text = "변경 사항 적용";
            btnApplyFillter.UseVisualStyleBackColor = false;
            btnApplyFillter.Click += btnApplyFillter_Click;
            // 
            // lblSelectedRange
            // 
            lblSelectedRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblSelectedRange.Font = new Font("맑은 고딕", 9.5F);
            lblSelectedRange.ForeColor = Color.FromArgb(60, 72, 92);
            lblSelectedRange.Location = new Point(336, 7);
            lblSelectedRange.Name = "lblSelectedRange";
            lblSelectedRange.Size = new Size(180, 27);
            lblSelectedRange.TabIndex = 2;
            lblSelectedRange.Text = "선택된 범위 (0, 0)";
            lblSelectedRange.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAllRange
            // 
            btnAllRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnAllRange.BackColor = Color.FromArgb(80, 140, 190);
            btnAllRange.Cursor = Cursors.Hand;
            btnAllRange.FlatAppearance.BorderSize = 0;
            btnAllRange.FlatStyle = FlatStyle.Flat;
            btnAllRange.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnAllRange.ForeColor = Color.White;
            btnAllRange.Location = new Point(250, 7);
            btnAllRange.Name = "btnAllRange";
            btnAllRange.Size = new Size(80, 27);
            btnAllRange.TabIndex = 5;
            btnAllRange.Text = "전체 선택";
            btnAllRange.UseVisualStyleBackColor = false;
            btnAllRange.Click += btnAllRange_Click;
            // 
            // btnRightRange
            // 
            btnRightRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnRightRange.BackColor = Color.FromArgb(100, 150, 210);
            btnRightRange.Cursor = Cursors.Hand;
            btnRightRange.FlatAppearance.BorderSize = 0;
            btnRightRange.FlatStyle = FlatStyle.Flat;
            btnRightRange.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnRightRange.ForeColor = Color.White;
            btnRightRange.Location = new Point(120, 7);
            btnRightRange.Name = "btnRightRange";
            btnRightRange.Size = new Size(124, 27);
            btnRightRange.TabIndex = 1;
            btnRightRange.Text = "오른쪽 범위 선택";
            btnRightRange.UseVisualStyleBackColor = false;
            btnRightRange.Click += btnRightRange_Click;
            // 
            // btnLeftRange
            // 
            btnLeftRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnLeftRange.BackColor = Color.FromArgb(100, 150, 210);
            btnLeftRange.Cursor = Cursors.Hand;
            btnLeftRange.FlatAppearance.BorderSize = 0;
            btnLeftRange.FlatStyle = FlatStyle.Flat;
            btnLeftRange.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnLeftRange.ForeColor = Color.White;
            btnLeftRange.Location = new Point(4, 7);
            btnLeftRange.Name = "btnLeftRange";
            btnLeftRange.Size = new Size(110, 27);
            btnLeftRange.TabIndex = 0;
            btnLeftRange.Text = "왼쪽 범위 선택";
            btnLeftRange.UseVisualStyleBackColor = false;
            btnLeftRange.Click += btnLeftRange_Click;
            // 
            // pnlFillter
            // 
            pnlFillter.BackColor = Color.White;
            pnlFillter.Controls.Add(chkRemoveImage);
            pnlFillter.Controls.Add(chkSetBlur);
            pnlFillter.Controls.Add(trkSetBlur);
            pnlFillter.Controls.Add(chkSetBright);
            pnlFillter.Controls.Add(trkSetBright);
            pnlFillter.Controls.Add(chkApplyBlackWhite);
            pnlFillter.Controls.Add(chkInverseColor);
            pnlFillter.Controls.Add(chkDelAngle);
            pnlFillter.Controls.Add(chkDelThrottle);
            pnlFillter.Controls.Add(lblFillter);
            pnlFillter.Dock = DockStyle.Bottom;
            pnlFillter.Location = new Point(0, 44);
            pnlFillter.Name = "pnlFillter";
            pnlFillter.Size = new Size(950, 56);
            pnlFillter.TabIndex = 4;
            // 
            // chkRemoveImage
            // 
            chkRemoveImage.Location = new Point(634, 4);
            chkRemoveImage.Margin = new Padding(2, 2, 2, 2);
            chkRemoveImage.Name = "chkRemoveImage";
            chkRemoveImage.Size = new Size(90, 22);
            chkRemoveImage.TabIndex = 12;
            chkRemoveImage.Text = "이미지 제거";
            chkRemoveImage.UseVisualStyleBackColor = true;
            // 
            // chkSetBlur
            // 
            chkSetBlur.BackColor = Color.White;
            chkSetBlur.Font = new Font("맑은 고딕", 9.5F);
            chkSetBlur.ForeColor = Color.FromArgb(60, 72, 92);
            chkSetBlur.Location = new Point(502, 28);
            chkSetBlur.Name = "chkSetBlur";
            chkSetBlur.Size = new Size(90, 22);
            chkSetBlur.TabIndex = 11;
            chkSetBlur.Text = "흐림 설정";
            chkSetBlur.UseVisualStyleBackColor = false;
            // 
            // trkSetBlur
            // 
            trkSetBlur.AutoSize = false;
            trkSetBlur.BackColor = Color.White;
            trkSetBlur.LargeChange = 1;
            trkSetBlur.Location = new Point(596, 28);
            trkSetBlur.Name = "trkSetBlur";
            trkSetBlur.Size = new Size(200, 22);
            trkSetBlur.TabIndex = 10;
            trkSetBlur.TickStyle = TickStyle.None;
            // 
            // chkSetBright
            // 
            chkSetBright.BackColor = Color.White;
            chkSetBright.Font = new Font("맑은 고딕", 9.5F);
            chkSetBright.ForeColor = Color.FromArgb(60, 72, 92);
            chkSetBright.Location = new Point(200, 28);
            chkSetBright.Name = "chkSetBright";
            chkSetBright.Size = new Size(90, 22);
            chkSetBright.TabIndex = 9;
            chkSetBright.Text = "밝기 설정";
            chkSetBright.UseVisualStyleBackColor = false;
            // 
            // trkSetBright
            // 
            trkSetBright.AutoSize = false;
            trkSetBright.BackColor = Color.White;
            trkSetBright.LargeChange = 1;
            trkSetBright.Location = new Point(294, 28);
            trkSetBright.Name = "trkSetBright";
            trkSetBright.Size = new Size(200, 22);
            trkSetBright.TabIndex = 8;
            trkSetBright.TickStyle = TickStyle.None;
            // 
            // chkApplyBlackWhite
            // 
            chkApplyBlackWhite.BackColor = Color.White;
            chkApplyBlackWhite.Font = new Font("맑은 고딕", 9.5F);
            chkApplyBlackWhite.ForeColor = Color.FromArgb(60, 72, 92);
            chkApplyBlackWhite.Location = new Point(540, 3);
            chkApplyBlackWhite.Name = "chkApplyBlackWhite";
            chkApplyBlackWhite.Size = new Size(90, 22);
            chkApplyBlackWhite.TabIndex = 7;
            chkApplyBlackWhite.Text = "흑백 적용";
            chkApplyBlackWhite.UseVisualStyleBackColor = false;
            // 
            // chkInverseColor
            // 
            chkInverseColor.BackColor = Color.White;
            chkInverseColor.Font = new Font("맑은 고딕", 9.5F);
            chkInverseColor.ForeColor = Color.FromArgb(60, 72, 92);
            chkInverseColor.Location = new Point(444, 3);
            chkInverseColor.Name = "chkInverseColor";
            chkInverseColor.Size = new Size(90, 22);
            chkInverseColor.TabIndex = 6;
            chkInverseColor.Text = "색상 반전";
            chkInverseColor.UseVisualStyleBackColor = false;
            // 
            // chkDelAngle
            // 
            chkDelAngle.BackColor = Color.White;
            chkDelAngle.Font = new Font("맑은 고딕", 9.5F);
            chkDelAngle.ForeColor = Color.FromArgb(60, 72, 92);
            chkDelAngle.Location = new Point(322, 3);
            chkDelAngle.Name = "chkDelAngle";
            chkDelAngle.Size = new Size(116, 22);
            chkDelAngle.TabIndex = 5;
            chkDelAngle.Text = "각도 값 0 제거";
            chkDelAngle.UseVisualStyleBackColor = false;
            // 
            // chkDelThrottle
            // 
            chkDelThrottle.BackColor = Color.White;
            chkDelThrottle.Font = new Font("맑은 고딕", 9.5F);
            chkDelThrottle.ForeColor = Color.FromArgb(60, 72, 92);
            chkDelThrottle.Location = new Point(200, 3);
            chkDelThrottle.Name = "chkDelThrottle";
            chkDelThrottle.Size = new Size(116, 22);
            chkDelThrottle.TabIndex = 4;
            chkDelThrottle.Text = "속도 값 0 제거";
            chkDelThrottle.UseVisualStyleBackColor = false;
            // 
            // lblFillter
            // 
            lblFillter.BackColor = Color.White;
            lblFillter.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblFillter.ForeColor = Color.FromArgb(67, 130, 220);
            lblFillter.Location = new Point(4, 3);
            lblFillter.Name = "lblFillter";
            lblFillter.Size = new Size(190, 50);
            lblFillter.TabIndex = 3;
            lblFillter.Text = "선택 범위 내 변경 사항 설정";
            lblFillter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlSub
            // 
            pnlSub.BackColor = Color.FromArgb(245, 247, 250);
            pnlSub.Controls.Add(pnlControl);
            pnlSub.Controls.Add(pnlData);
            pnlSub.Dock = DockStyle.Right;
            pnlSub.Location = new Point(670, 40);
            pnlSub.Name = "pnlSub";
            pnlSub.Size = new Size(280, 476);
            pnlSub.TabIndex = 6;
            // 
            // pnlControl
            // 
            pnlControl.BackColor = Color.FromArgb(250, 251, 253);
            pnlControl.Controls.Add(lblSpeed);
            pnlControl.Controls.Add(comboBox1);
            pnlControl.Controls.Add(lblAllImageNumRange);
            pnlControl.Controls.Add(btnStop);
            pnlControl.Controls.Add(btnPlay);
            pnlControl.Controls.Add(btn5FrameRight);
            pnlControl.Controls.Add(btn5FrameLeft);
            pnlControl.Controls.Add(btnFrameRight);
            pnlControl.Controls.Add(btnFrameLeft);
            pnlControl.Dock = DockStyle.Fill;
            pnlControl.Location = new Point(0, 76);
            pnlControl.Name = "pnlControl";
            pnlControl.Size = new Size(280, 400);
            pnlControl.TabIndex = 3;
            // 
            // lblSpeed
            // 
            lblSpeed.Font = new Font("맑은 고딕", 9.5F);
            lblSpeed.ForeColor = Color.FromArgb(60, 72, 92);
            lblSpeed.Location = new Point(4, 68);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(120, 26);
            lblSpeed.TabIndex = 9;
            lblSpeed.Text = "배속";
            lblSpeed.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.White;
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox1.Font = new Font("맑은 고딕", 9.5F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "0.25", "0.50", "1.00", "1.50", "2.00", "3.00", "4.00" });
            comboBox1.Location = new Point(130, 68);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(130, 25);
            comboBox1.TabIndex = 8;
            comboBox1.Text = "1.00";
            // 
            // lblAllImageNumRange
            // 
            lblAllImageNumRange.Font = new Font("맑은 고딕", 9.5F);
            lblAllImageNumRange.ForeColor = Color.FromArgb(60, 72, 92);
            lblAllImageNumRange.Location = new Point(6, 10);
            lblAllImageNumRange.Name = "lblAllImageNumRange";
            lblAllImageNumRange.Size = new Size(268, 50);
            lblAllImageNumRange.TabIndex = 7;
            lblAllImageNumRange.Text = "(이미지 첫 번호, 현재 이미지 번호, 이미지 끝 번호 표시)";
            lblAllImageNumRange.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(210, 70, 70);
            btnStop.Cursor = Cursors.Hand;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(140, 210);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(130, 46);
            btnStop.TabIndex = 6;
            btnStop.Text = "■ 중지";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.FromArgb(72, 175, 120);
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnPlay.ForeColor = Color.White;
            btnPlay.Location = new Point(4, 210);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(130, 46);
            btnPlay.TabIndex = 5;
            btnPlay.Text = "▶ 재생";
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Click += btnPlay_Click;
            // 
            // btn5FrameRight
            // 
            btn5FrameRight.BackColor = Color.FromArgb(80, 130, 195);
            btn5FrameRight.Cursor = Cursors.Hand;
            btn5FrameRight.FlatAppearance.BorderSize = 0;
            btn5FrameRight.FlatStyle = FlatStyle.Flat;
            btn5FrameRight.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btn5FrameRight.ForeColor = Color.White;
            btn5FrameRight.Location = new Point(140, 158);
            btn5FrameRight.Name = "btn5FrameRight";
            btn5FrameRight.Size = new Size(130, 46);
            btn5FrameRight.TabIndex = 4;
            btn5FrameRight.Text = ">>>";
            btn5FrameRight.UseVisualStyleBackColor = false;
            btn5FrameRight.Click += btn5FrameRight_Click;
            // 
            // btn5FrameLeft
            // 
            btn5FrameLeft.BackColor = Color.FromArgb(80, 130, 195);
            btn5FrameLeft.Cursor = Cursors.Hand;
            btn5FrameLeft.FlatAppearance.BorderSize = 0;
            btn5FrameLeft.FlatStyle = FlatStyle.Flat;
            btn5FrameLeft.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btn5FrameLeft.ForeColor = Color.White;
            btn5FrameLeft.Location = new Point(4, 158);
            btn5FrameLeft.Name = "btn5FrameLeft";
            btn5FrameLeft.Size = new Size(130, 46);
            btn5FrameLeft.TabIndex = 3;
            btn5FrameLeft.Text = "<<<";
            btn5FrameLeft.UseVisualStyleBackColor = false;
            btn5FrameLeft.Click += btn5FrameLeft_Click;
            // 
            // btnFrameRight
            // 
            btnFrameRight.BackColor = Color.FromArgb(100, 150, 210);
            btnFrameRight.Cursor = Cursors.Hand;
            btnFrameRight.FlatAppearance.BorderSize = 0;
            btnFrameRight.FlatStyle = FlatStyle.Flat;
            btnFrameRight.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFrameRight.ForeColor = Color.White;
            btnFrameRight.Location = new Point(140, 106);
            btnFrameRight.Name = "btnFrameRight";
            btnFrameRight.Size = new Size(130, 46);
            btnFrameRight.TabIndex = 2;
            btnFrameRight.Text = ">";
            btnFrameRight.UseVisualStyleBackColor = false;
            btnFrameRight.Click += btnFrameRight_Click;
            // 
            // btnFrameLeft
            // 
            btnFrameLeft.BackColor = Color.FromArgb(100, 150, 210);
            btnFrameLeft.Cursor = Cursors.Hand;
            btnFrameLeft.FlatAppearance.BorderSize = 0;
            btnFrameLeft.FlatStyle = FlatStyle.Flat;
            btnFrameLeft.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFrameLeft.ForeColor = Color.White;
            btnFrameLeft.Location = new Point(4, 106);
            btnFrameLeft.Name = "btnFrameLeft";
            btnFrameLeft.Size = new Size(130, 46);
            btnFrameLeft.TabIndex = 1;
            btnFrameLeft.Text = "<";
            btnFrameLeft.UseVisualStyleBackColor = false;
            btnFrameLeft.Click += btnFrameLeft_Click;
            // 
            // pnlData
            // 
            pnlData.BackColor = Color.FromArgb(250, 251, 253);
            pnlData.Controls.Add(lblThrottleDetail);
            pnlData.Controls.Add(lblAngleDetail);
            pnlData.Controls.Add(lblThrottle);
            pnlData.Controls.Add(lblAngle);
            pnlData.Controls.Add(prgAngle);
            pnlData.Controls.Add(prgThrottle);
            pnlData.Dock = DockStyle.Top;
            pnlData.Location = new Point(0, 0);
            pnlData.Name = "pnlData";
            pnlData.Size = new Size(280, 76);
            pnlData.TabIndex = 2;
            // 
            // lblThrottleDetail
            // 
            lblThrottleDetail.Font = new Font("맑은 고딕", 9F);
            lblThrottleDetail.ForeColor = Color.FromArgb(60, 72, 92);
            lblThrottleDetail.ImageAlign = ContentAlignment.MiddleRight;
            lblThrottleDetail.Location = new Point(60, 46);
            lblThrottleDetail.Name = "lblThrottleDetail";
            lblThrottleDetail.Size = new Size(52, 17);
            lblThrottleDetail.TabIndex = 5;
            lblThrottleDetail.Text = "수치";
            lblThrottleDetail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAngleDetail
            // 
            lblAngleDetail.Font = new Font("맑은 고딕", 9F);
            lblAngleDetail.ForeColor = Color.FromArgb(60, 72, 92);
            lblAngleDetail.ImageAlign = ContentAlignment.MiddleRight;
            lblAngleDetail.Location = new Point(60, 12);
            lblAngleDetail.Name = "lblAngleDetail";
            lblAngleDetail.Size = new Size(52, 17);
            lblAngleDetail.TabIndex = 4;
            lblAngleDetail.Text = "수치";
            lblAngleDetail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblThrottle
            // 
            lblThrottle.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblThrottle.ForeColor = Color.FromArgb(72, 175, 120);
            lblThrottle.Location = new Point(6, 42);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(50, 24);
            lblThrottle.TabIndex = 3;
            lblThrottle.Text = "속도";
            lblThrottle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAngle
            // 
            lblAngle.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblAngle.ForeColor = Color.FromArgb(67, 130, 220);
            lblAngle.Location = new Point(6, 8);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(50, 24);
            lblAngle.TabIndex = 2;
            lblAngle.Text = "각도";
            lblAngle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // prgAngle
            // 
            prgAngle.ForeColor = Color.FromArgb(67, 130, 220);
            prgAngle.Location = new Point(118, 8);
            prgAngle.Name = "prgAngle";
            prgAngle.Size = new Size(148, 26);
            prgAngle.TabIndex = 0;
            // 
            // prgThrottle
            // 
            prgThrottle.ForeColor = Color.FromArgb(72, 175, 120);
            prgThrottle.Location = new Point(118, 42);
            prgThrottle.Name = "prgThrottle";
            prgThrottle.Size = new Size(148, 26);
            prgThrottle.TabIndex = 1;
            // 
            // pnlImage
            // 
            pnlImage.BackColor = Color.FromArgb(20, 20, 30);
            pnlImage.Controls.Add(picImage);
            pnlImage.Controls.Add(trkProgress);
            pnlImage.Controls.Add(pnlTimeStamp);
            pnlImage.Dock = DockStyle.Fill;
            pnlImage.Location = new Point(0, 40);
            pnlImage.Name = "pnlImage";
            pnlImage.Size = new Size(670, 476);
            pnlImage.TabIndex = 7;
            // 
            // picImage
            // 
            picImage.BackColor = Color.FromArgb(20, 20, 30);
            picImage.Dock = DockStyle.Fill;
            picImage.Location = new Point(0, 0);
            picImage.Name = "picImage";
            picImage.Size = new Size(670, 412);
            picImage.SizeMode = PictureBoxSizeMode.Zoom;
            picImage.TabIndex = 2;
            picImage.TabStop = false;
            // 
            // trkProgress
            // 
            trkProgress.AutoSize = false;
            trkProgress.BackColor = Color.FromArgb(228, 232, 240);
            trkProgress.Dock = DockStyle.Bottom;
            trkProgress.LargeChange = 1;
            trkProgress.Location = new Point(0, 412);
            trkProgress.Name = "trkProgress";
            trkProgress.Size = new Size(670, 26);
            trkProgress.TabIndex = 1;
            trkProgress.TickStyle = TickStyle.None;
            // 
            // pnlTimeStamp
            // 
            pnlTimeStamp.BackColor = Color.FromArgb(250, 251, 253);
            pnlTimeStamp.Dock = DockStyle.Bottom;
            pnlTimeStamp.Location = new Point(0, 438);
            pnlTimeStamp.Margin = new Padding(2, 2, 2, 2);
            pnlTimeStamp.Name = "pnlTimeStamp";
            pnlTimeStamp.Size = new Size(670, 38);
            pnlTimeStamp.TabIndex = 12;
            // 
            // TubManagerUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlImage);
            Controls.Add(pnlSub);
            Controls.Add(pnlSetting);
            Controls.Add(pnlFile);
            Controls.Add(pnlChart);
            Name = "TubManagerUI";
            Size = new Size(950, 766);
            pnlChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chtData).EndInit();
            pnlFile.ResumeLayout(false);
            pnlSetting.ResumeLayout(false);
            pnlSet.ResumeLayout(false);
            pnlFillter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trkSetBlur).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkSetBright).EndInit();
            pnlSub.ResumeLayout(false);
            pnlControl.ResumeLayout(false);
            pnlData.ResumeLayout(false);
            pnlImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkProgress).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtData;
        private Button btnFileLoad;
        private Button btnNewFolder;
        private Button btnSaveRoute;
        private Button btnSaveData;
        private Button btnDelFolder;
        private Label lblSaveRoute;
        private Panel pnlFile;
        private Panel pnlSetting;
        private Panel pnlData;
        private Panel pnlControl;
        private Label lblThrottle;
        private Label lblAngle;
        private Button btnStop;
        private Button btnPlay;
        private Button btn5FrameRight;
        private Button btn5FrameLeft;
        private Button btnFrameRight;
        private Button btnFrameLeft;
        private Panel pnlSub;
        private Panel pnlImage;
        private TrackBar trkProgress;
        private PictureBox picImage;
        private ProgressBar prgAngle;
        private ProgressBar prgThrottle;
        private Button btnRightRange;
        private Button btnLeftRange;
        private Label lblSelectedRange;
        private Label lblFillter;
        private Panel pnlFillter;
        private CheckBox chkDelThrottle;
        private CheckBox chkApplyBlackWhite;
        private CheckBox chkInverseColor;
        private CheckBox chkDelAngle;
        private Button btnAllRange;
        private TrackBar trkSetBright;
        private Panel pnlSet;
        private CheckBox chkSetBright;
        private CheckBox chkSetBlur;
        private TrackBar trkSetBlur;
        private Button btnApplyFillter;
        private Button btnCancelFillter;
        private Button btnInitFillterSet;
        private Label lblAllImageNumRange;
        private Label lblSpeed;
        private ComboBox comboBox1;
        private Label lblThrottleDetail;
        private Label lblAngleDetail;
        private Panel pnlTimeStamp;
        private CheckBox chkSaveContinue;
        private CheckBox chkRemoveImage;
    }
}