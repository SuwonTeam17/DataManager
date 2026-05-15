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
            btnApplyBlackWhite = new CheckBox();
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
            pnlChart.Location = new Point(0, 428);
            pnlChart.Name = "pnlChart";
            pnlChart.Size = new Size(750, 132);
            pnlChart.TabIndex = 3;
            // 
            // chtData
            // 
            chtData.BackColor = SystemColors.Control;
            chartArea1.Name = "ChartArea1";
            chtData.ChartAreas.Add(chartArea1);
            chtData.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chtData.Legends.Add(legend1);
            chtData.Location = new Point(0, 0);
            chtData.Name = "chtData";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "각도";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "속도";
            chtData.Series.Add(series1);
            chtData.Series.Add(series2);
            chtData.Size = new Size(748, 130);
            chtData.TabIndex = 0;
            chtData.Text = "chart1";
            // 
            // btnFileLoad
            // 
            btnFileLoad.Location = new Point(3, 3);
            btnFileLoad.Name = "btnFileLoad";
            btnFileLoad.Size = new Size(106, 48);
            btnFileLoad.TabIndex = 0;
            btnFileLoad.Text = "파일 가져오기";
            btnFileLoad.UseVisualStyleBackColor = true;
            // 
            // btnNewFolder
            // 
            btnNewFolder.Location = new Point(115, 3);
            btnNewFolder.Name = "btnNewFolder";
            btnNewFolder.Size = new Size(106, 48);
            btnNewFolder.TabIndex = 1;
            btnNewFolder.Text = "새 폴더 생성";
            btnNewFolder.UseVisualStyleBackColor = true;
            // 
            // btnSaveRoute
            // 
            btnSaveRoute.Location = new Point(339, 3);
            btnSaveRoute.Name = "btnSaveRoute";
            btnSaveRoute.Size = new Size(106, 48);
            btnSaveRoute.TabIndex = 3;
            btnSaveRoute.Text = "저장 경로 지정";
            btnSaveRoute.UseVisualStyleBackColor = true;
            // 
            // btnSaveData
            // 
            btnSaveData.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveData.Location = new Point(639, 3);
            btnSaveData.Name = "btnSaveData";
            btnSaveData.Size = new Size(106, 48);
            btnSaveData.TabIndex = 5;
            btnSaveData.Text = "데이터 저장";
            btnSaveData.UseVisualStyleBackColor = true;
            // 
            // btnDelFolder
            // 
            btnDelFolder.Location = new Point(227, 3);
            btnDelFolder.Name = "btnDelFolder";
            btnDelFolder.Size = new Size(106, 48);
            btnDelFolder.TabIndex = 2;
            btnDelFolder.Text = "폴더 삭제";
            btnDelFolder.UseVisualStyleBackColor = true;
            // 
            // lblSaveRoute
            // 
            lblSaveRoute.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSaveRoute.Location = new Point(451, 3);
            lblSaveRoute.Name = "lblSaveRoute";
            lblSaveRoute.Size = new Size(182, 48);
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
            pnlFile.Name = "pnlFile";
            pnlFile.Size = new Size(750, 55);
            pnlFile.TabIndex = 1;
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
            btnInitFillterSet.Size = new Size(135, 28);
            btnInitFillterSet.TabIndex = 8;
            btnInitFillterSet.Text = "변경 사항 설정 초기화";
            btnInitFillterSet.UseVisualStyleBackColor = true;
            // 
            // btnCancelFillter
            // 
            btnCancelFillter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancelFillter.Location = new Point(517, 5);
            btnCancelFillter.Name = "btnCancelFillter";
            btnCancelFillter.Size = new Size(95, 28);
            btnCancelFillter.TabIndex = 7;
            btnCancelFillter.Text = "변경 사항 취소";
            btnCancelFillter.UseVisualStyleBackColor = true;
            // 
            // btnApplyFillter
            // 
            btnApplyFillter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnApplyFillter.Location = new Point(421, 5);
            btnApplyFillter.Name = "btnApplyFillter";
            btnApplyFillter.Size = new Size(95, 28);
            btnApplyFillter.TabIndex = 6;
            btnApplyFillter.Text = "변경 사항 적용";
            btnApplyFillter.UseVisualStyleBackColor = true;
            // 
            // lblSelectedRange
            // 
            lblSelectedRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblSelectedRange.Location = new Point(275, 5);
            lblSelectedRange.Name = "lblSelectedRange";
            lblSelectedRange.Size = new Size(160, 28);
            lblSelectedRange.TabIndex = 2;
            lblSelectedRange.Text = "선택된 범위 (0, 0)";
            lblSelectedRange.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAllRange
            // 
            btnAllRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnAllRange.Location = new Point(206, 5);
            btnAllRange.Name = "btnAllRange";
            btnAllRange.Size = new Size(67, 28);
            btnAllRange.TabIndex = 5;
            btnAllRange.Text = "전체 선택";
            btnAllRange.UseVisualStyleBackColor = true;
            // 
            // btnRightRange
            // 
            btnRightRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnRightRange.Location = new Point(98, 5);
            btnRightRange.Name = "btnRightRange";
            btnRightRange.Size = new Size(107, 28);
            btnRightRange.TabIndex = 1;
            btnRightRange.Text = "오른쪽 범위 선택";
            btnRightRange.UseVisualStyleBackColor = true;
            // 
            // btnLeftRange
            // 
            btnLeftRange.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnLeftRange.Location = new Point(2, 5);
            btnLeftRange.Name = "btnLeftRange";
            btnLeftRange.Size = new Size(95, 28);
            btnLeftRange.TabIndex = 0;
            btnLeftRange.Text = "왼쪽 범위 선택";
            btnLeftRange.UseVisualStyleBackColor = true;
            // 
            // pnlFillter
            // 
            pnlFillter.BackColor = Color.White;
            pnlFillter.BorderStyle = BorderStyle.FixedSingle;
            pnlFillter.Controls.Add(chkSetBlur);
            pnlFillter.Controls.Add(trkSetBlur);
            pnlFillter.Controls.Add(chkSetBright);
            pnlFillter.Controls.Add(trkSetBright);
            pnlFillter.Controls.Add(btnApplyBlackWhite);
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
            // btnApplyBlackWhite
            // 
            btnApplyBlackWhite.BackColor = SystemColors.Control;
            btnApplyBlackWhite.Location = new Point(483, 3);
            btnApplyBlackWhite.Name = "btnApplyBlackWhite";
            btnApplyBlackWhite.Size = new Size(78, 21);
            btnApplyBlackWhite.TabIndex = 7;
            btnApplyBlackWhite.Text = "흑백 적용";
            btnApplyBlackWhite.UseVisualStyleBackColor = false;
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
            pnlSub.Location = new Point(530, 55);
            pnlSub.Name = "pnlSub";
            pnlSub.Size = new Size(220, 283);
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
            pnlControl.Size = new Size(220, 219);
            pnlControl.TabIndex = 3;
            // 
            // lblSpeed
            // 
            lblSpeed.Location = new Point(3, 58);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(102, 23);
            lblSpeed.TabIndex = 9;
            lblSpeed.Text = "Speed";
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
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(3, 175);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(102, 38);
            btnPlay.TabIndex = 5;
            btnPlay.Text = "재생";
            btnPlay.UseVisualStyleBackColor = true;
            // 
            // btn5FrameRight
            // 
            btn5FrameRight.Location = new Point(111, 131);
            btn5FrameRight.Name = "btn5FrameRight";
            btn5FrameRight.Size = new Size(106, 38);
            btn5FrameRight.TabIndex = 4;
            btn5FrameRight.Text = ">>>";
            btn5FrameRight.UseVisualStyleBackColor = true;
            // 
            // btn5FrameLeft
            // 
            btn5FrameLeft.Location = new Point(3, 131);
            btn5FrameLeft.Name = "btn5FrameLeft";
            btn5FrameLeft.Size = new Size(102, 38);
            btn5FrameLeft.TabIndex = 3;
            btn5FrameLeft.Text = "<<<";
            btn5FrameLeft.UseVisualStyleBackColor = true;
            // 
            // btnFrameRight
            // 
            btnFrameRight.Location = new Point(111, 87);
            btnFrameRight.Name = "btnFrameRight";
            btnFrameRight.Size = new Size(106, 38);
            btnFrameRight.TabIndex = 2;
            btnFrameRight.Text = ">";
            btnFrameRight.UseVisualStyleBackColor = true;
            // 
            // btnFrameLeft
            // 
            btnFrameLeft.Location = new Point(3, 87);
            btnFrameLeft.Name = "btnFrameLeft";
            btnFrameLeft.Size = new Size(102, 38);
            btnFrameLeft.TabIndex = 1;
            btnFrameLeft.Text = "<";
            btnFrameLeft.UseVisualStyleBackColor = true;
            // 
            // pnlData
            // 
            pnlData.BorderStyle = BorderStyle.FixedSingle;
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
            // lblThrottle
            // 
            lblThrottle.Location = new Point(26, 38);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(45, 23);
            lblThrottle.TabIndex = 3;
            lblThrottle.Text = "속도";
            lblThrottle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAngle
            // 
            lblAngle.Location = new Point(26, 6);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(45, 23);
            lblAngle.TabIndex = 2;
            lblAngle.Text = "각도";
            lblAngle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // prgAngle
            // 
            prgAngle.Location = new Point(97, 6);
            prgAngle.Name = "prgAngle";
            prgAngle.Size = new Size(120, 23);
            prgAngle.TabIndex = 0;
            // 
            // prgThrottle
            // 
            prgThrottle.Location = new Point(97, 35);
            prgThrottle.Name = "prgThrottle";
            prgThrottle.Size = new Size(120, 23);
            prgThrottle.TabIndex = 1;
            // 
            // pnlImage
            // 
            pnlImage.BorderStyle = BorderStyle.FixedSingle;
            pnlImage.Controls.Add(picImage);
            pnlImage.Controls.Add(trkProgress);
            pnlImage.Dock = DockStyle.Fill;
            pnlImage.Location = new Point(0, 55);
            pnlImage.Name = "pnlImage";
            pnlImage.Size = new Size(530, 283);
            pnlImage.TabIndex = 7;
            // 
            // picImage
            // 
            picImage.BackColor = Color.Black;
            picImage.Dock = DockStyle.Fill;
            picImage.Location = new Point(0, 0);
            picImage.Name = "picImage";
            picImage.Size = new Size(528, 260);
            picImage.TabIndex = 2;
            picImage.TabStop = false;
            // 
            // trkProgress
            // 
            trkProgress.AutoSize = false;
            trkProgress.Dock = DockStyle.Bottom;
            trkProgress.LargeChange = 1;
            trkProgress.Location = new Point(0, 260);
            trkProgress.Name = "trkProgress";
            trkProgress.Size = new Size(528, 21);
            trkProgress.TabIndex = 1;
            trkProgress.TickStyle = TickStyle.None;
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
        private CheckBox btnApplyBlackWhite;
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
    }
}
