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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            pnlChart.BorderStyle = BorderStyle.FixedSingle;
            pnlChart.Controls.Add(chtData);
            pnlChart.Dock = DockStyle.Bottom;
            pnlChart.Location = new Point(0, 428);
            pnlChart.Name = "pnlChart";
            pnlChart.Size = new Size(750, 132);
            pnlChart.TabIndex = 3;
            // 
            // chtData
            // 
            chtData.BackColor = SystemColors.Control;
            chartArea6.Name = "ChartArea1";
            chtData.ChartAreas.Add(chartArea6);
            chtData.Dock = DockStyle.Fill;
            legend6.Name = "Legend1";
            chtData.Legends.Add(legend6);
            chtData.Location = new Point(0, 0);
            chtData.Name = "chtData";
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.Legend = "Legend1";
            series11.Name = "각도";
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series12.Legend = "Legend1";
            series12.Name = "속도";
            chtData.Series.Add(series11);
            chtData.Series.Add(series12);
            chtData.Size = new Size(748, 130);
            chtData.TabIndex = 0;
            chtData.Text = "chart1";
            // 
            // btnFileLoad
            // 
            btnFileLoad.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnFileLoad.Location = new Point(3, 3);
            btnFileLoad.Name = "btnFileLoad";
            btnFileLoad.Size = new Size(106, 25);
            btnFileLoad.TabIndex = 0;
            btnFileLoad.Text = "파일 가져오기";
            btnFileLoad.UseVisualStyleBackColor = true;
            btnFileLoad.Click += btnFileLoad_Click;
            // 
            // btnNewFolder
            // 
            btnNewFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnNewFolder.Location = new Point(115, 3);
            btnNewFolder.Name = "btnNewFolder";
            btnNewFolder.Size = new Size(83, 25);
            btnNewFolder.TabIndex = 1;
            btnNewFolder.Text = "새 폴더 생성";
            btnNewFolder.UseVisualStyleBackColor = true;
            btnNewFolder.Click += btnNewFolder_Click;
            // 
            // btnSaveRoute
            // 
            btnSaveRoute.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnSaveRoute.Location = new Point(277, 3);
            btnSaveRoute.Name = "btnSaveRoute";
            btnSaveRoute.Size = new Size(95, 25);
            btnSaveRoute.TabIndex = 3;
            btnSaveRoute.Text = "저장 경로 지정";
            btnSaveRoute.UseVisualStyleBackColor = true;
            btnSaveRoute.Click += btnSaveRoute_Click;
            // 
            // btnSaveData
            // 
            btnSaveData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveData.Location = new Point(639, 3);
            btnSaveData.Name = "btnSaveData";
            btnSaveData.Size = new Size(106, 25);
            btnSaveData.TabIndex = 5;
            btnSaveData.Text = "데이터 저장";
            btnSaveData.UseVisualStyleBackColor = true;
            btnSaveData.Click += btnSaveData_Click;
            // 
            // btnDelFolder
            // 
            btnDelFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            btnDelFolder.Location = new Point(204, 3);
            btnDelFolder.Name = "btnDelFolder";
            btnDelFolder.Size = new Size(67, 25);
            btnDelFolder.TabIndex = 2;
            btnDelFolder.Text = "폴더 삭제";
            btnDelFolder.UseVisualStyleBackColor = true;
            btnDelFolder.Click += btnDelFolder_Click;
            // 
            // lblSaveRoute
            // 
            lblSaveRoute.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblSaveRoute.Location = new Point(470, 3);
            lblSaveRoute.Name = "lblSaveRoute";
            lblSaveRoute.Size = new Size(166, 25);
            lblSaveRoute.TabIndex = 4;
            lblSaveRoute.Text = "(현재 경로)";
            // 
            // pnlFile
            // 
            pnlFile.BorderStyle = BorderStyle.FixedSingle;
            pnlFile.Controls.Add(chkSaveContinue);
            pnlFile.Controls.Add(btnFileLoad);
            pnlFile.Controls.Add(btnDelFolder);
            pnlFile.Controls.Add(lblSaveRoute);
            pnlFile.Controls.Add(btnSaveRoute);
            pnlFile.Controls.Add(btnNewFolder);
            pnlFile.Controls.Add(btnSaveData);
            pnlFile.Dock = DockStyle.Top;
            pnlFile.Location = new Point(0, 0);
            pnlFile.Name = "pnlFile";
            pnlFile.Size = new Size(750, 32);
            pnlFile.TabIndex = 1;
            // 
            // chkSaveContinue
            // 
            chkSaveContinue.Location = new Point(377, 7);
            chkSaveContinue.Margin = new Padding(0);
            chkSaveContinue.Name = "chkSaveContinue";
            chkSaveContinue.Size = new Size(90, 19);
            chkSaveContinue.TabIndex = 6;
            chkSaveContinue.Text = "이어서 저장";
            chkSaveContinue.UseVisualStyleBackColor = true;
            // 
            // pnlSetting
            // 
            pnlSetting.Controls.Add(pnlSet);
            pnlSetting.Controls.Add(pnlFillter);
            pnlSetting.Dock = DockStyle.Bottom;
            pnlSetting.Location = new Point(0, 338);
            pnlSetting.Name = "pnlSetting";
            pnlSetting.Size = new Size(750, 90);
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
            pnlSet.Name = "pnlSet";
            pnlSet.Size = new Size(750, 40);
            pnlSet.TabIndex = 6;
            // 
            // btnInitFillterSet
            // 
            btnInitFillterSet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnInitFillterSet.Location = new Point(613, 5);
            btnInitFillterSet.Name = "btnInitFillterSet";
            btnInitFillterSet.Size = new Size(135, 27);
            btnInitFillterSet.TabIndex = 8;
            btnInitFillterSet.Text = "변경 사항 설정 초기화";
            btnInitFillterSet.UseVisualStyleBackColor = true;
            btnInitFillterSet.Click += btnInitFillterSet_Click;
            // 
            // btnCancelFillter
            // 
            btnCancelFillter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelFillter.Location = new Point(517, 5);
            btnCancelFillter.Name = "btnCancelFillter";
            btnCancelFillter.Size = new Size(95, 27);
            btnCancelFillter.TabIndex = 7;
            btnCancelFillter.Text = "변경 사항 취소";
            btnCancelFillter.UseVisualStyleBackColor = true;
            btnCancelFillter.Click += btnCancelFillter_Click;
            // 
            // btnApplyFillter
            // 
            btnApplyFillter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnApplyFillter.Location = new Point(421, 5);
            btnApplyFillter.Name = "btnApplyFillter";
            btnApplyFillter.Size = new Size(95, 27);
            btnApplyFillter.TabIndex = 6;
            btnApplyFillter.Text = "변경 사항 적용";
            btnApplyFillter.UseVisualStyleBackColor = true;
            btnApplyFillter.Click += btnApplyFillter_Click;
            // 
            // lblSelectedRange
            // 
            lblSelectedRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblSelectedRange.Location = new Point(275, 5);
            lblSelectedRange.Name = "lblSelectedRange";
            lblSelectedRange.Size = new Size(160, 27);
            lblSelectedRange.TabIndex = 2;
            lblSelectedRange.Text = "선택된 범위 (0, 0)";
            lblSelectedRange.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAllRange
            // 
            btnAllRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnAllRange.Location = new Point(206, 5);
            btnAllRange.Name = "btnAllRange";
            btnAllRange.Size = new Size(67, 27);
            btnAllRange.TabIndex = 5;
            btnAllRange.Text = "전체 선택";
            btnAllRange.UseVisualStyleBackColor = true;
            btnAllRange.Click += btnAllRange_Click;
            // 
            // btnRightRange
            // 
            btnRightRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnRightRange.Location = new Point(98, 5);
            btnRightRange.Name = "btnRightRange";
            btnRightRange.Size = new Size(107, 27);
            btnRightRange.TabIndex = 1;
            btnRightRange.Text = "오른쪽 범위 선택";
            btnRightRange.UseVisualStyleBackColor = true;
            btnRightRange.Click += btnRightRange_Click;
            // 
            // btnLeftRange
            // 
            btnLeftRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnLeftRange.Location = new Point(2, 5);
            btnLeftRange.Name = "btnLeftRange";
            btnLeftRange.Size = new Size(95, 27);
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
            pnlFillter.Location = new Point(0, 40);
            pnlFillter.Name = "pnlFillter";
            pnlFillter.Size = new Size(750, 50);
            pnlFillter.TabIndex = 4;
            // 
            // chkSetBlur
            // 
            chkSetBlur.BackColor = SystemColors.Control;
            chkSetBlur.Location = new Point(429, 26);
            chkSetBlur.Name = "chkSetBlur";
            chkSetBlur.Size = new Size(78, 21);
            chkSetBlur.TabIndex = 11;
            chkSetBlur.Text = "흐림 설정";
            chkSetBlur.UseVisualStyleBackColor = false;
            // 
            // trkSetBlur
            // 
            trkSetBlur.AutoSize = false;
            trkSetBlur.BackColor = SystemColors.Control;
            trkSetBlur.LargeChange = 1;
            trkSetBlur.Location = new Point(509, 26);
            trkSetBlur.Name = "trkSetBlur";
            trkSetBlur.Size = new Size(166, 21);
            trkSetBlur.TabIndex = 10;
            trkSetBlur.TickStyle = TickStyle.None;
            // 
            // chkSetBright
            // 
            chkSetBright.BackColor = SystemColors.Control;
            chkSetBright.Location = new Point(178, 26);
            chkSetBright.Name = "chkSetBright";
            chkSetBright.Size = new Size(78, 21);
            chkSetBright.TabIndex = 9;
            chkSetBright.Text = "밝기 설정";
            chkSetBright.UseVisualStyleBackColor = false;
            // 
            // trkSetBright
            // 
            trkSetBright.AutoSize = false;
            trkSetBright.BackColor = SystemColors.Control;
            trkSetBright.LargeChange = 1;
            trkSetBright.Location = new Point(258, 26);
            trkSetBright.Name = "trkSetBright";
            trkSetBright.Size = new Size(166, 21);
            trkSetBright.TabIndex = 8;
            trkSetBright.TickStyle = TickStyle.None;
            // 
            // chkApplyBlackWhite
            // 
            chkApplyBlackWhite.BackColor = SystemColors.Control;
            chkApplyBlackWhite.Location = new Point(483, 3);
            chkApplyBlackWhite.Name = "chkApplyBlackWhite";
            chkApplyBlackWhite.Size = new Size(78, 21);
            chkApplyBlackWhite.TabIndex = 7;
            chkApplyBlackWhite.Text = "흑백 적용";
            chkApplyBlackWhite.UseVisualStyleBackColor = false;
            // 
            // chkInverseColor
            // 
            chkInverseColor.BackColor = SystemColors.Control;
            chkInverseColor.Location = new Point(399, 3);
            chkInverseColor.Name = "chkInverseColor";
            chkInverseColor.Size = new Size(78, 21);
            chkInverseColor.TabIndex = 6;
            chkInverseColor.Text = "색상 반전";
            chkInverseColor.UseVisualStyleBackColor = false;
            // 
            // chkDelAngle
            // 
            chkDelAngle.BackColor = SystemColors.Control;
            chkDelAngle.Location = new Point(288, 3);
            chkDelAngle.Name = "chkDelAngle";
            chkDelAngle.Size = new Size(105, 21);
            chkDelAngle.TabIndex = 5;
            chkDelAngle.Text = "각도 값 0 제거";
            chkDelAngle.UseVisualStyleBackColor = false;
            // 
            // chkDelThrottle
            // 
            chkDelThrottle.BackColor = SystemColors.Control;
            chkDelThrottle.Location = new Point(178, 3);
            chkDelThrottle.Name = "chkDelThrottle";
            chkDelThrottle.Size = new Size(105, 21);
            chkDelThrottle.TabIndex = 4;
            chkDelThrottle.Text = "속도 값 0 제거";
            chkDelThrottle.UseVisualStyleBackColor = false;
            // 
            // lblFillter
            // 
            lblFillter.BackColor = SystemColors.Control;
            lblFillter.Location = new Point(4, 3);
            lblFillter.Name = "lblFillter";
            lblFillter.Size = new Size(165, 44);
            lblFillter.TabIndex = 3;
            lblFillter.Text = "선택 범위 내 변경 사항 설정";
            lblFillter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlSub
            // 
            pnlSub.Controls.Add(pnlControl);
            pnlSub.Controls.Add(pnlData);
            pnlSub.Dock = DockStyle.Right;
            pnlSub.Location = new Point(530, 32);
            pnlSub.Name = "pnlSub";
            pnlSub.Size = new Size(220, 306);
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
            pnlControl.Location = new Point(0, 64);
            pnlControl.Name = "pnlControl";
            pnlControl.Size = new Size(220, 242);
            pnlControl.TabIndex = 3;
            // 
            // lblSpeed
            // 
            lblSpeed.Location = new Point(3, 58);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(102, 23);
            lblSpeed.TabIndex = 9;
            lblSpeed.Text = "배속";
            lblSpeed.TextAlign = ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "0.25", "0.50", "1.00", "1.50", "2.00", "3.00", "4.00" });
            comboBox1.Location = new Point(111, 58);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(104, 23);
            comboBox1.TabIndex = 8;
            comboBox1.Text = "1.00";
            // 
            // lblAllImageNumRange
            // 
            lblAllImageNumRange.Font = new Font("맑은 고딕", 9F);
            lblAllImageNumRange.Location = new Point(6, 9);
            lblAllImageNumRange.Name = "lblAllImageNumRange";
            lblAllImageNumRange.Size = new Size(209, 43);
            lblAllImageNumRange.TabIndex = 7;
            lblAllImageNumRange.Text = "(이미지 첫 번호, 현재 이미지 번호, 이미지 끝 번호 표시)";
            lblAllImageNumRange.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(111, 175);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(106, 38);
            btnStop.TabIndex = 6;
            btnStop.Text = "중지";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(3, 175);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(102, 38);
            btnPlay.TabIndex = 5;
            btnPlay.Text = "재생";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // btn5FrameRight
            // 
            btn5FrameRight.Location = new Point(111, 131);
            btn5FrameRight.Name = "btn5FrameRight";
            btn5FrameRight.Size = new Size(106, 38);
            btn5FrameRight.TabIndex = 4;
            btn5FrameRight.Text = ">>>";
            btn5FrameRight.UseVisualStyleBackColor = true;
            btn5FrameRight.Click += btn5FrameRight_Click;
            // 
            // btn5FrameLeft
            // 
            btn5FrameLeft.Location = new Point(3, 131);
            btn5FrameLeft.Name = "btn5FrameLeft";
            btn5FrameLeft.Size = new Size(102, 38);
            btn5FrameLeft.TabIndex = 3;
            btn5FrameLeft.Text = "<<<";
            btn5FrameLeft.UseVisualStyleBackColor = true;
            btn5FrameLeft.Click += btn5FrameLeft_Click;
            // 
            // btnFrameRight
            // 
            btnFrameRight.Location = new Point(111, 87);
            btnFrameRight.Name = "btnFrameRight";
            btnFrameRight.Size = new Size(106, 38);
            btnFrameRight.TabIndex = 2;
            btnFrameRight.Text = ">";
            btnFrameRight.UseVisualStyleBackColor = true;
            btnFrameRight.Click += btnFrameRight_Click;
            // 
            // btnFrameLeft
            // 
            btnFrameLeft.Location = new Point(3, 87);
            btnFrameLeft.Name = "btnFrameLeft";
            btnFrameLeft.Size = new Size(102, 38);
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
            pnlData.Name = "pnlData";
            pnlData.Size = new Size(220, 64);
            pnlData.TabIndex = 2;
            // 
            // lblThrottleDetail
            // 
            lblThrottleDetail.ImageAlign = ContentAlignment.MiddleRight;
            lblThrottleDetail.Location = new Point(57, 39);
            lblThrottleDetail.Name = "lblThrottleDetail";
            lblThrottleDetail.Size = new Size(46, 15);
            lblThrottleDetail.TabIndex = 5;
            lblThrottleDetail.Text = "수치";
            lblThrottleDetail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAngleDetail
            // 
            lblAngleDetail.ImageAlign = ContentAlignment.MiddleRight;
            lblAngleDetail.Location = new Point(57, 10);
            lblAngleDetail.Name = "lblAngleDetail";
            lblAngleDetail.Size = new Size(46, 15);
            lblAngleDetail.TabIndex = 4;
            lblAngleDetail.Text = "수치";
            lblAngleDetail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblThrottle
            // 
            lblThrottle.Location = new Point(6, 35);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(45, 23);
            lblThrottle.TabIndex = 3;
            lblThrottle.Text = "속도";
            lblThrottle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAngle
            // 
            lblAngle.Location = new Point(6, 6);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(45, 23);
            lblAngle.TabIndex = 2;
            lblAngle.Text = "각도";
            lblAngle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // prgAngle
            // 
            prgAngle.Location = new Point(109, 6);
            prgAngle.Name = "prgAngle";
            prgAngle.Size = new Size(108, 23);
            prgAngle.TabIndex = 0;
            // 
            // prgThrottle
            // 
            prgThrottle.Location = new Point(109, 35);
            prgThrottle.Name = "prgThrottle";
            prgThrottle.Size = new Size(108, 23);
            prgThrottle.TabIndex = 1;
            // 
            // pnlImage
            // 
            pnlImage.BorderStyle = BorderStyle.FixedSingle;
            pnlImage.Controls.Add(picImage);
            pnlImage.Controls.Add(trkProgress);
            pnlImage.Controls.Add(pnlTimeStamp);
            pnlImage.Dock = DockStyle.Fill;
            pnlImage.Location = new Point(0, 32);
            pnlImage.Name = "pnlImage";
            pnlImage.Size = new Size(530, 306);
            pnlImage.TabIndex = 7;
            // 
            // picImage
            // 
            picImage.BackColor = Color.Black;
            picImage.Dock = DockStyle.Fill;
            picImage.Location = new Point(0, 0);
            picImage.Name = "picImage";
            picImage.Size = new Size(528, 238);
            picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            picImage.TabIndex = 2;
            picImage.TabStop = false;
            // 
            // trkProgress
            // 
            trkProgress.AutoSize = false;
            trkProgress.Dock = DockStyle.Bottom;
            trkProgress.LargeChange = 1;
            trkProgress.Location = new Point(0, 238);
            trkProgress.Name = "trkProgress";
            trkProgress.Size = new Size(528, 21);
            trkProgress.TabIndex = 1;
            trkProgress.TickStyle = TickStyle.None;
            // 
            // pnlTimeStamp
            // 
            pnlTimeStamp.Dock = DockStyle.Bottom;
            pnlTimeStamp.Location = new Point(0, 259);
            pnlTimeStamp.Margin = new Padding(2, 2, 2, 2);
            pnlTimeStamp.Name = "pnlTimeStamp";
            pnlTimeStamp.Size = new Size(528, 45);
            pnlTimeStamp.TabIndex = 12;
            // 
            // TubManagerUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlImage);
            Controls.Add(pnlSub);
            Controls.Add(pnlSetting);
            Controls.Add(pnlFile);
            Controls.Add(pnlChart);
            Name = "TubManagerUI";
            Size = new Size(750, 560);
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
    }
}
