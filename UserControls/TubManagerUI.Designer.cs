namespace DataManager.UserControls
{
    partial class TubManagerUI
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            pnlChart = new Panel();
            chtData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            btnFileLoad = new Button();
            btnNewFolder = new Button();
            btnSaveRoute = new Button();
            btnSaveData = new Button();
            btnDelFolder = new Button();
            lblSaveRoute = new Label();
            pnlFile = new Panel();
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
            pnlChart.BorderStyle = BorderStyle.FixedSingle;
            pnlChart.Controls.Add(chtData);
            pnlChart.Dock = DockStyle.Bottom;
            pnlChart.Location = new Point(0, 572);
            pnlChart.Margin = new Padding(4, 4, 4, 4);
            pnlChart.Name = "pnlChart";
            pnlChart.Size = new Size(964, 175);
            pnlChart.TabIndex = 3;
            // 
            // chtData
            // 
            chtData.BackColor = SystemColors.Control;
            chartArea2.Name = "ChartArea1";
            chtData.ChartAreas.Add(chartArea2);
            chtData.Dock = DockStyle.Fill;
            legend2.Name = "Legend1";
            chtData.Legends.Add(legend2);
            chtData.Location = new Point(0, 0);
            chtData.Margin = new Padding(4, 4, 4, 4);
            chtData.Name = "chtData";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "각도";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Legend = "Legend1";
            series4.Name = "속도";
            chtData.Series.Add(series3);
            chtData.Series.Add(series4);
            chtData.Size = new Size(962, 173);
            chtData.TabIndex = 0;
            chtData.Text = "chart1";
            // 
            // btnFileLoad
            // 
            btnFileLoad.Location = new Point(4, 4);
            btnFileLoad.Margin = new Padding(4, 4, 4, 4);
            btnFileLoad.Name = "btnFileLoad";
            btnFileLoad.Size = new Size(136, 64);
            btnFileLoad.TabIndex = 0;
            btnFileLoad.Text = "파일 가져오기";
            btnFileLoad.UseVisualStyleBackColor = true;
            btnFileLoad.Click += btnFileLoad_Click;
            // 
            // btnNewFolder
            // 
            btnNewFolder.Location = new Point(148, 4);
            btnNewFolder.Margin = new Padding(4, 4, 4, 4);
            btnNewFolder.Name = "btnNewFolder";
            btnNewFolder.Size = new Size(136, 64);
            btnNewFolder.TabIndex = 1;
            btnNewFolder.Text = "새 폴더 생성";
            btnNewFolder.UseVisualStyleBackColor = true;
            btnNewFolder.Click += btnNewFolder_Click;
            // 
            // btnSaveRoute
            // 
            btnSaveRoute.Location = new Point(436, 4);
            btnSaveRoute.Margin = new Padding(4, 4, 4, 4);
            btnSaveRoute.Name = "btnSaveRoute";
            btnSaveRoute.Size = new Size(136, 64);
            btnSaveRoute.TabIndex = 3;
            btnSaveRoute.Text = "저장 경로 지정";
            btnSaveRoute.UseVisualStyleBackColor = true;
            btnSaveRoute.Click += btnSaveRoute_Click;
            // 
            // btnSaveData
            // 
            btnSaveData.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveData.Location = new Point(822, 4);
            btnSaveData.Margin = new Padding(4, 4, 4, 4);
            btnSaveData.Name = "btnSaveData";
            btnSaveData.Size = new Size(136, 64);
            btnSaveData.TabIndex = 5;
            btnSaveData.Text = "데이터 저장";
            btnSaveData.UseVisualStyleBackColor = true;
            btnSaveData.Click += btnSaveData_Click;
            // 
            // btnDelFolder
            // 
            btnDelFolder.Location = new Point(292, 4);
            btnDelFolder.Margin = new Padding(4, 4, 4, 4);
            btnDelFolder.Name = "btnDelFolder";
            btnDelFolder.Size = new Size(136, 64);
            btnDelFolder.TabIndex = 2;
            btnDelFolder.Text = "폴더 삭제";
            btnDelFolder.UseVisualStyleBackColor = true;
            btnDelFolder.Click += btnDelFolder_Click;
            // 
            // lblSaveRoute
            // 
            lblSaveRoute.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSaveRoute.Location = new Point(580, 4);
            lblSaveRoute.Margin = new Padding(4, 0, 4, 0);
            lblSaveRoute.Name = "lblSaveRoute";
            lblSaveRoute.Size = new Size(234, 64);
            lblSaveRoute.TabIndex = 4;
            lblSaveRoute.Text = "(현재 경로)";
            // 
            // pnlFile
            // 
            pnlFile.BorderStyle = BorderStyle.FixedSingle;
            pnlFile.Controls.Add(btnFileLoad);
            pnlFile.Controls.Add(btnDelFolder);
            pnlFile.Controls.Add(lblSaveRoute);
            pnlFile.Controls.Add(btnSaveRoute);
            pnlFile.Controls.Add(btnNewFolder);
            pnlFile.Controls.Add(btnSaveData);
            pnlFile.Dock = DockStyle.Top;
            pnlFile.Location = new Point(0, 0);
            pnlFile.Margin = new Padding(4, 4, 4, 4);
            pnlFile.Name = "pnlFile";
            pnlFile.Size = new Size(964, 73);
            pnlFile.TabIndex = 1;
            // 
            // pnlSetting
            // 
            pnlSetting.Controls.Add(pnlSet);
            pnlSetting.Controls.Add(pnlFillter);
            pnlSetting.Dock = DockStyle.Bottom;
            pnlSetting.Location = new Point(0, 452);
            pnlSetting.Margin = new Padding(4, 4, 4, 4);
            pnlSetting.Name = "pnlSetting";
            pnlSetting.Size = new Size(964, 120);
            pnlSetting.TabIndex = 4;
            // 
            // pnlSet
            // 
            pnlSet.BorderStyle = BorderStyle.FixedSingle;
            pnlSet.Controls.Add(btnInitFillterSet);
            pnlSet.Controls.Add(btnCancelFillter);
            pnlSet.Controls.Add(btnApplyFillter);
            pnlSet.Controls.Add(lblSelectedRange);
            pnlSet.Controls.Add(btnAllRange);
            pnlSet.Controls.Add(btnRightRange);
            pnlSet.Controls.Add(btnLeftRange);
            pnlSet.Dock = DockStyle.Fill;
            pnlSet.Location = new Point(0, 0);
            pnlSet.Margin = new Padding(4, 4, 4, 4);
            pnlSet.Name = "pnlSet";
            pnlSet.Size = new Size(964, 54);
            pnlSet.TabIndex = 6;
            // 
            // btnInitFillterSet
            // 
            btnInitFillterSet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnInitFillterSet.Location = new Point(788, 7);
            btnInitFillterSet.Margin = new Padding(4, 4, 4, 4);
            btnInitFillterSet.Name = "btnInitFillterSet";
            btnInitFillterSet.Size = new Size(174, 38);
            btnInitFillterSet.TabIndex = 8;
            btnInitFillterSet.Text = "변경 사항 설정 초기화";
            btnInitFillterSet.UseVisualStyleBackColor = true;
            btnInitFillterSet.Click += btnInitFillterSet_Click;
            // 
            // btnCancelFillter
            // 
            btnCancelFillter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelFillter.Location = new Point(665, 7);
            btnCancelFillter.Margin = new Padding(4, 4, 4, 4);
            btnCancelFillter.Name = "btnCancelFillter";
            btnCancelFillter.Size = new Size(122, 38);
            btnCancelFillter.TabIndex = 7;
            btnCancelFillter.Text = "변경 사항 취소";
            btnCancelFillter.UseVisualStyleBackColor = true;
            btnCancelFillter.Click += btnCancelFillter_Click;
            // 
            // btnApplyFillter
            // 
            btnApplyFillter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnApplyFillter.Location = new Point(541, 7);
            btnApplyFillter.Margin = new Padding(4, 4, 4, 4);
            btnApplyFillter.Name = "btnApplyFillter";
            btnApplyFillter.Size = new Size(122, 38);
            btnApplyFillter.TabIndex = 6;
            btnApplyFillter.Text = "변경 사항 적용";
            btnApplyFillter.UseVisualStyleBackColor = true;
            btnApplyFillter.Click += btnApplyFillter_Click;
            // 
            // lblSelectedRange
            // 
            lblSelectedRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblSelectedRange.Location = new Point(354, 7);
            lblSelectedRange.Margin = new Padding(4, 0, 4, 0);
            lblSelectedRange.Name = "lblSelectedRange";
            lblSelectedRange.Size = new Size(206, 38);
            lblSelectedRange.TabIndex = 2;
            lblSelectedRange.Text = "선택된 범위 (0, 0)";
            lblSelectedRange.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAllRange
            // 
            btnAllRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnAllRange.Location = new Point(265, 7);
            btnAllRange.Margin = new Padding(4, 4, 4, 4);
            btnAllRange.Name = "btnAllRange";
            btnAllRange.Size = new Size(86, 38);
            btnAllRange.TabIndex = 5;
            btnAllRange.Text = "전체 선택";
            btnAllRange.UseVisualStyleBackColor = true;
            btnAllRange.Click += btnAllRange_Click;
            // 
            // btnRightRange
            // 
            btnRightRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnRightRange.Location = new Point(126, 7);
            btnRightRange.Margin = new Padding(4, 4, 4, 4);
            btnRightRange.Name = "btnRightRange";
            btnRightRange.Size = new Size(138, 38);
            btnRightRange.TabIndex = 1;
            btnRightRange.Text = "오른쪽 범위 선택";
            btnRightRange.UseVisualStyleBackColor = true;
            btnRightRange.Click += btnRightRange_Click;
            // 
            // btnLeftRange
            // 
            btnLeftRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnLeftRange.Location = new Point(3, 7);
            btnLeftRange.Margin = new Padding(4, 4, 4, 4);
            btnLeftRange.Name = "btnLeftRange";
            btnLeftRange.Size = new Size(122, 38);
            btnLeftRange.TabIndex = 0;
            btnLeftRange.Text = "왼쪽 범위 선택";
            btnLeftRange.UseVisualStyleBackColor = true;
            btnLeftRange.Click += btnLeftRange_Click;
            // 
            // pnlFillter
            // 
            pnlFillter.BackColor = Color.White;
            pnlFillter.BorderStyle = BorderStyle.FixedSingle;
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
            pnlFillter.Location = new Point(0, 54);
            pnlFillter.Margin = new Padding(4, 4, 4, 4);
            pnlFillter.Name = "pnlFillter";
            pnlFillter.Size = new Size(964, 66);
            pnlFillter.TabIndex = 4;
            // 
            // chkSetBlur
            // 
            chkSetBlur.BackColor = SystemColors.Control;
            chkSetBlur.Location = new Point(552, 35);
            chkSetBlur.Margin = new Padding(4, 4, 4, 4);
            chkSetBlur.Name = "chkSetBlur";
            chkSetBlur.Size = new Size(100, 28);
            chkSetBlur.TabIndex = 11;
            chkSetBlur.Text = "흐림 설정";
            chkSetBlur.UseVisualStyleBackColor = false;
            // 
            // trkSetBlur
            // 
            trkSetBlur.AutoSize = false;
            trkSetBlur.BackColor = SystemColors.Control;
            trkSetBlur.LargeChange = 1;
            trkSetBlur.Location = new Point(654, 35);
            trkSetBlur.Margin = new Padding(4, 4, 4, 4);
            trkSetBlur.Name = "trkSetBlur";
            trkSetBlur.Size = new Size(213, 28);
            trkSetBlur.TabIndex = 10;
            trkSetBlur.TickStyle = TickStyle.None;
            // 
            // chkSetBright
            // 
            chkSetBright.BackColor = SystemColors.Control;
            chkSetBright.Location = new Point(229, 35);
            chkSetBright.Margin = new Padding(4, 4, 4, 4);
            chkSetBright.Name = "chkSetBright";
            chkSetBright.Size = new Size(100, 28);
            chkSetBright.TabIndex = 9;
            chkSetBright.Text = "밝기 설정";
            chkSetBright.UseVisualStyleBackColor = false;
            // 
            // trkSetBright
            // 
            trkSetBright.AutoSize = false;
            trkSetBright.BackColor = SystemColors.Control;
            trkSetBright.LargeChange = 1;
            trkSetBright.Location = new Point(332, 35);
            trkSetBright.Margin = new Padding(4, 4, 4, 4);
            trkSetBright.Name = "trkSetBright";
            trkSetBright.Size = new Size(213, 28);
            trkSetBright.TabIndex = 8;
            trkSetBright.TickStyle = TickStyle.None;
            // 
            // chkApplyBlackWhite
            // 
            chkApplyBlackWhite.BackColor = SystemColors.Control;
            chkApplyBlackWhite.Location = new Point(621, 4);
            chkApplyBlackWhite.Margin = new Padding(4, 4, 4, 4);
            chkApplyBlackWhite.Name = "chkApplyBlackWhite";
            chkApplyBlackWhite.Size = new Size(100, 28);
            chkApplyBlackWhite.TabIndex = 7;
            chkApplyBlackWhite.Text = "흑백 적용";
            chkApplyBlackWhite.UseVisualStyleBackColor = false;
            // 
            // chkInverseColor
            // 
            chkInverseColor.BackColor = SystemColors.Control;
            chkInverseColor.Location = new Point(513, 4);
            chkInverseColor.Margin = new Padding(4, 4, 4, 4);
            chkInverseColor.Name = "chkInverseColor";
            chkInverseColor.Size = new Size(100, 28);
            chkInverseColor.TabIndex = 6;
            chkInverseColor.Text = "색상 반전";
            chkInverseColor.UseVisualStyleBackColor = false;
            // 
            // chkDelAngle
            // 
            chkDelAngle.BackColor = SystemColors.Control;
            chkDelAngle.Location = new Point(370, 4);
            chkDelAngle.Margin = new Padding(4, 4, 4, 4);
            chkDelAngle.Name = "chkDelAngle";
            chkDelAngle.Size = new Size(135, 28);
            chkDelAngle.TabIndex = 5;
            chkDelAngle.Text = "각도 값 0 제거";
            chkDelAngle.UseVisualStyleBackColor = false;
            // 
            // chkDelThrottle
            // 
            chkDelThrottle.BackColor = SystemColors.Control;
            chkDelThrottle.Location = new Point(229, 4);
            chkDelThrottle.Margin = new Padding(4, 4, 4, 4);
            chkDelThrottle.Name = "chkDelThrottle";
            chkDelThrottle.Size = new Size(135, 28);
            chkDelThrottle.TabIndex = 4;
            chkDelThrottle.Text = "속도 값 0 제거";
            chkDelThrottle.UseVisualStyleBackColor = false;
            // 
            // lblFillter
            // 
            lblFillter.BackColor = SystemColors.Control;
            lblFillter.Location = new Point(5, 4);
            lblFillter.Margin = new Padding(4, 0, 4, 0);
            lblFillter.Name = "lblFillter";
            lblFillter.Size = new Size(212, 59);
            lblFillter.TabIndex = 3;
            lblFillter.Text = "선택 범위 내 변경 사항 설정";
            lblFillter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlSub
            // 
            pnlSub.Controls.Add(pnlControl);
            pnlSub.Controls.Add(pnlData);
            pnlSub.Dock = DockStyle.Right;
            pnlSub.Location = new Point(681, 73);
            pnlSub.Margin = new Padding(4, 4, 4, 4);
            pnlSub.Name = "pnlSub";
            pnlSub.Size = new Size(283, 379);
            pnlSub.TabIndex = 6;
            // 
            // pnlControl
            // 
            pnlControl.BorderStyle = BorderStyle.FixedSingle;
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
            pnlControl.Location = new Point(0, 85);
            pnlControl.Margin = new Padding(4, 4, 4, 4);
            pnlControl.Name = "pnlControl";
            pnlControl.Size = new Size(283, 294);
            pnlControl.TabIndex = 3;
            // 
            // lblSpeed
            // 
            lblSpeed.Location = new Point(4, 77);
            lblSpeed.Margin = new Padding(4, 0, 4, 0);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(131, 31);
            lblSpeed.TabIndex = 9;
            lblSpeed.Text = "Speed";
            lblSpeed.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "0.25", "0.50", "1.00", "1.50", "2.00", "3.00", "4.00" });
            comboBox1.Location = new Point(143, 77);
            comboBox1.Margin = new Padding(4, 4, 4, 4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(133, 28);
            comboBox1.TabIndex = 8;
            comboBox1.Text = "1.00";
            // 
            // lblAllImageNumRange
            // 
            lblAllImageNumRange.Font = new Font("맑은 고딕", 9F);
            lblAllImageNumRange.Location = new Point(8, 12);
            lblAllImageNumRange.Margin = new Padding(4, 0, 4, 0);
            lblAllImageNumRange.Name = "lblAllImageNumRange";
            lblAllImageNumRange.Size = new Size(269, 57);
            lblAllImageNumRange.TabIndex = 7;
            lblAllImageNumRange.Text = "(이미지 첫 번호, 현재 이미지 번호, 이미지 끝 번호 표시)";
            lblAllImageNumRange.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(143, 233);
            btnStop.Margin = new Padding(4, 4, 4, 4);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(136, 51);
            btnStop.TabIndex = 6;
            btnStop.Text = "중지";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(4, 233);
            btnPlay.Margin = new Padding(4, 4, 4, 4);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(131, 51);
            btnPlay.TabIndex = 5;
            btnPlay.Text = "재생";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btn5FrameRight
            // 
            btn5FrameRight.Location = new Point(143, 175);
            btn5FrameRight.Margin = new Padding(4, 4, 4, 4);
            btn5FrameRight.Name = "btn5FrameRight";
            btn5FrameRight.Size = new Size(136, 51);
            btn5FrameRight.TabIndex = 4;
            btn5FrameRight.Text = ">>>";
            btn5FrameRight.UseVisualStyleBackColor = true;
            btn5FrameRight.Click += btn5FrameRight_Click;
            // 
            // btn5FrameLeft
            // 
            btn5FrameLeft.Location = new Point(4, 175);
            btn5FrameLeft.Margin = new Padding(4, 4, 4, 4);
            btn5FrameLeft.Name = "btn5FrameLeft";
            btn5FrameLeft.Size = new Size(131, 51);
            btn5FrameLeft.TabIndex = 3;
            btn5FrameLeft.Text = "<<<";
            btn5FrameLeft.UseVisualStyleBackColor = true;
            btn5FrameLeft.Click += btn5FrameLeft_Click;
            // 
            // btnFrameRight
            // 
            btnFrameRight.Location = new Point(143, 116);
            btnFrameRight.Margin = new Padding(4, 4, 4, 4);
            btnFrameRight.Name = "btnFrameRight";
            btnFrameRight.Size = new Size(136, 51);
            btnFrameRight.TabIndex = 2;
            btnFrameRight.Text = ">";
            btnFrameRight.UseVisualStyleBackColor = true;
            btnFrameRight.Click += btnFrameRight_Click;
            // 
            // btnFrameLeft
            // 
            btnFrameLeft.Location = new Point(4, 116);
            btnFrameLeft.Margin = new Padding(4, 4, 4, 4);
            btnFrameLeft.Name = "btnFrameLeft";
            btnFrameLeft.Size = new Size(131, 51);
            btnFrameLeft.TabIndex = 1;
            btnFrameLeft.Text = "<";
            btnFrameLeft.UseVisualStyleBackColor = true;
            btnFrameLeft.Click += btnFrameLeft_Click;
            // 
            // pnlData
            // 
            pnlData.BorderStyle = BorderStyle.FixedSingle;
            pnlData.Controls.Add(lblThrottleDetail);
            pnlData.Controls.Add(lblAngleDetail);
            pnlData.Controls.Add(lblThrottle);
            pnlData.Controls.Add(lblAngle);
            pnlData.Controls.Add(prgAngle);
            pnlData.Controls.Add(prgThrottle);
            pnlData.Dock = DockStyle.Top;
            pnlData.Location = new Point(0, 0);
            pnlData.Margin = new Padding(4, 4, 4, 4);
            pnlData.Name = "pnlData";
            pnlData.Size = new Size(283, 85);
            pnlData.TabIndex = 2;
            // 
            // lblThrottleDetail
            // 
            lblThrottleDetail.ImageAlign = ContentAlignment.MiddleRight;
            lblThrottleDetail.Location = new Point(73, 52);
            lblThrottleDetail.Margin = new Padding(4, 0, 4, 0);
            lblThrottleDetail.Name = "lblThrottleDetail";
            lblThrottleDetail.Size = new Size(59, 20);
            lblThrottleDetail.TabIndex = 5;
            lblThrottleDetail.Text = "수치";
            lblThrottleDetail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAngleDetail
            // 
            lblAngleDetail.ImageAlign = ContentAlignment.MiddleRight;
            lblAngleDetail.Location = new Point(73, 13);
            lblAngleDetail.Margin = new Padding(4, 0, 4, 0);
            lblAngleDetail.Name = "lblAngleDetail";
            lblAngleDetail.Size = new Size(59, 20);
            lblAngleDetail.TabIndex = 4;
            lblAngleDetail.Text = "수치";
            lblAngleDetail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblThrottle
            // 
            lblThrottle.Location = new Point(8, 47);
            lblThrottle.Margin = new Padding(4, 0, 4, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(58, 31);
            lblThrottle.TabIndex = 3;
            lblThrottle.Text = "속도";
            lblThrottle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAngle
            // 
            lblAngle.Location = new Point(8, 8);
            lblAngle.Margin = new Padding(4, 0, 4, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(58, 31);
            lblAngle.TabIndex = 2;
            lblAngle.Text = "각도";
            lblAngle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // prgAngle
            // 
            prgAngle.Location = new Point(140, 8);
            prgAngle.Margin = new Padding(4, 4, 4, 4);
            prgAngle.Name = "prgAngle";
            prgAngle.Size = new Size(139, 31);
            prgAngle.TabIndex = 0;
            // 
            // prgThrottle
            // 
            prgThrottle.Location = new Point(140, 47);
            prgThrottle.Margin = new Padding(4, 4, 4, 4);
            prgThrottle.Name = "prgThrottle";
            prgThrottle.Size = new Size(139, 31);
            prgThrottle.TabIndex = 1;
            // 
            // pnlImage
            // 
            pnlImage.BorderStyle = BorderStyle.FixedSingle;
            pnlImage.Controls.Add(picImage);
            pnlImage.Controls.Add(trkProgress);
            pnlImage.Dock = DockStyle.Fill;
            pnlImage.Location = new Point(0, 73);
            pnlImage.Margin = new Padding(4, 4, 4, 4);
            pnlImage.Name = "pnlImage";
            pnlImage.Size = new Size(681, 379);
            pnlImage.TabIndex = 7;
            // 
            // picImage
            // 
            picImage.BackColor = Color.Black;
            picImage.Dock = DockStyle.Fill;
            picImage.Location = new Point(0, 0);
            picImage.Margin = new Padding(4, 4, 4, 4);
            picImage.Name = "picImage";
            picImage.Size = new Size(679, 349);
            picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            picImage.TabIndex = 2;
            picImage.TabStop = false;
            // 
            // trkProgress
            // 
            trkProgress.AutoSize = false;
            trkProgress.Dock = DockStyle.Bottom;
            trkProgress.LargeChange = 1;
            trkProgress.Location = new Point(0, 349);
            trkProgress.Margin = new Padding(4, 4, 4, 4);
            trkProgress.Name = "trkProgress";
            trkProgress.Size = new Size(679, 28);
            trkProgress.TabIndex = 1;
            trkProgress.TickStyle = TickStyle.None;
            // 
            // TubManagerUI
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlImage);
            Controls.Add(pnlSub);
            Controls.Add(pnlSetting);
            Controls.Add(pnlFile);
            Controls.Add(pnlChart);
            Margin = new Padding(4, 4, 4, 4);
            Name = "TubManagerUI";
            Size = new Size(964, 747);
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
    }
}
