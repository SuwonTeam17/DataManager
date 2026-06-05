namespace DataManager.UserControls
{
    partial class DataCollectionUI
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
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
            pnlConnect = new Panel();
            cboMapList = new ComboBox();
            chkRecording = new CheckBox();
            btnStartPython = new Button();
            btnStartSim = new Button();
            btnConnect = new Button();
            pnlData = new Panel();
            btnUnSelectFolder = new Button();
            btnDelFolder = new Button();
            lblSelectedFolderRoute = new Label();
            btnNewFolder = new Button();
            btnSelectFolder = new Button();
            pnlControl = new Panel();
            pnlControlJoystick = new Panel();
            pnlView = new Panel();
            pnlDirection = new Panel();
            lblThrottle = new Label();
            lblAngle = new Label();
            pnlControlType = new Panel();
            btnKeyBoard = new Button();
            btnGamePad = new Button();
            btnJoyStick = new Button();
            pnlSetThrottle = new Panel();
            lblSetThrottle = new Label();
            cboThrottleMax = new ComboBox();
            cboThrottleType = new ComboBox();
            pnlCamera = new Panel();
            picCamera = new PictureBox();
            pnlConnect.SuspendLayout();
            pnlData.SuspendLayout();
            pnlControl.SuspendLayout();
            pnlView.SuspendLayout();
            pnlDirection.SuspendLayout();
            pnlControlType.SuspendLayout();
            pnlSetThrottle.SuspendLayout();
            pnlCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCamera).BeginInit();
            SuspendLayout();
            // 
            // pnlConnect
            // 
            pnlConnect.BackColor = Color.FromArgb(250, 251, 253);
            pnlConnect.Controls.Add(cboMapList);
            pnlConnect.Controls.Add(chkRecording);
            pnlConnect.Controls.Add(btnStartPython);
            pnlConnect.Controls.Add(btnStartSim);
            pnlConnect.Controls.Add(btnConnect);
            pnlConnect.Dock = DockStyle.Top;
            pnlConnect.Location = new Point(0, 0);
            pnlConnect.Name = "pnlConnect";
            pnlConnect.Size = new Size(950, 50);
            pnlConnect.TabIndex = 1;
            // 
            // cboMapList
            // 
            cboMapList.BackColor = Color.White;
            cboMapList.FlatStyle = FlatStyle.Flat;
            cboMapList.Font = new Font("맑은 고딕", 9.5F);
            cboMapList.FormattingEnabled = true;
            cboMapList.Location = new Point(378, 12);
            cboMapList.Name = "cboMapList";
            cboMapList.Size = new Size(130, 25);
            cboMapList.TabIndex = 0;
            // 
            // chkRecording
            // 
            chkRecording.AutoSize = true;
            chkRecording.Enabled = false;
            chkRecording.Font = new Font("맑은 고딕", 9.5F);
            chkRecording.ForeColor = Color.FromArgb(60, 72, 92);
            chkRecording.Location = new Point(517, 14);
            chkRecording.Name = "chkRecording";
            chkRecording.Size = new Size(71, 21);
            chkRecording.TabIndex = 3;
            chkRecording.Text = "녹화 중";
            chkRecording.UseVisualStyleBackColor = true;
            chkRecording.CheckedChanged += chkRecording_CheckedChanged;
            // 
            // btnStartPython
            // 
            btnStartPython.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnStartPython.BackColor = Color.FromArgb(67, 130, 220);
            btnStartPython.Cursor = Cursors.Hand;
            btnStartPython.FlatAppearance.BorderSize = 0;
            btnStartPython.FlatStyle = FlatStyle.Flat;
            btnStartPython.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnStartPython.ForeColor = Color.White;
            btnStartPython.Location = new Point(132, 10);
            btnStartPython.Name = "btnStartPython";
            btnStartPython.Size = new Size(120, 30);
            btnStartPython.TabIndex = 2;
            btnStartPython.Text = "파이썬 서버 시작";
            btnStartPython.UseVisualStyleBackColor = false;
            btnStartPython.Click += btnStartPython_Click;
            // 
            // btnStartSim
            // 
            btnStartSim.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnStartSim.BackColor = Color.FromArgb(67, 130, 220);
            btnStartSim.Cursor = Cursors.Hand;
            btnStartSim.FlatAppearance.BorderSize = 0;
            btnStartSim.FlatStyle = FlatStyle.Flat;
            btnStartSim.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnStartSim.ForeColor = Color.White;
            btnStartSim.Location = new Point(4, 10);
            btnStartSim.Name = "btnStartSim";
            btnStartSim.Size = new Size(122, 30);
            btnStartSim.TabIndex = 1;
            btnStartSim.Text = "시뮬레이터 시작";
            btnStartSim.UseVisualStyleBackColor = false;
            btnStartSim.Click += btnStartSim_Click;
            // 
            // btnConnect
            // 
            btnConnect.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnConnect.BackColor = Color.FromArgb(72, 175, 120);
            btnConnect.Cursor = Cursors.Hand;
            btnConnect.Enabled = false;
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.Location = new Point(258, 10);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(110, 30);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "서버 연결";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            // 
            // pnlData
            // 
            pnlData.BackColor = Color.FromArgb(250, 251, 253);
            pnlData.Controls.Add(btnUnSelectFolder);
            pnlData.Controls.Add(btnDelFolder);
            pnlData.Controls.Add(lblSelectedFolderRoute);
            pnlData.Controls.Add(btnNewFolder);
            pnlData.Controls.Add(btnSelectFolder);
            pnlData.Dock = DockStyle.Bottom;
            pnlData.Location = new Point(0, 716);
            pnlData.Name = "pnlData";
            pnlData.Size = new Size(950, 50);
            pnlData.TabIndex = 2;
            // 
            // btnUnSelectFolder
            // 
            btnUnSelectFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnUnSelectFolder.BackColor = Color.FromArgb(140, 148, 160);
            btnUnSelectFolder.Cursor = Cursors.Hand;
            btnUnSelectFolder.FlatAppearance.BorderSize = 0;
            btnUnSelectFolder.FlatStyle = FlatStyle.Flat;
            btnUnSelectFolder.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnUnSelectFolder.ForeColor = Color.White;
            btnUnSelectFolder.Location = new Point(356, 10);
            btnUnSelectFolder.Name = "btnUnSelectFolder";
            btnUnSelectFolder.Size = new Size(90, 30);
            btnUnSelectFolder.TabIndex = 4;
            btnUnSelectFolder.Text = "선택 해제";
            btnUnSelectFolder.UseVisualStyleBackColor = false;
            btnUnSelectFolder.Click += btnUnSelectFolder_Click;
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
            btnDelFolder.Location = new Point(136, 10);
            btnDelFolder.Name = "btnDelFolder";
            btnDelFolder.Size = new Size(90, 30);
            btnDelFolder.TabIndex = 3;
            btnDelFolder.Text = "폴더 삭제";
            btnDelFolder.UseVisualStyleBackColor = false;
            btnDelFolder.Click += btnDelFolder_Click;
            // 
            // lblSelectedFolderRoute
            // 
            lblSelectedFolderRoute.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblSelectedFolderRoute.Font = new Font("맑은 고딕", 9F);
            lblSelectedFolderRoute.ForeColor = Color.FromArgb(120, 130, 150);
            lblSelectedFolderRoute.Location = new Point(454, 10);
            lblSelectedFolderRoute.Name = "lblSelectedFolderRoute";
            lblSelectedFolderRoute.Size = new Size(330, 30);
            lblSelectedFolderRoute.TabIndex = 2;
            lblSelectedFolderRoute.Text = "(선택된 폴더 경로)";
            lblSelectedFolderRoute.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnNewFolder
            // 
            btnNewFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnNewFolder.BackColor = Color.FromArgb(67, 130, 220);
            btnNewFolder.Cursor = Cursors.Hand;
            btnNewFolder.FlatAppearance.BorderSize = 0;
            btnNewFolder.FlatStyle = FlatStyle.Flat;
            btnNewFolder.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnNewFolder.ForeColor = Color.White;
            btnNewFolder.Location = new Point(4, 10);
            btnNewFolder.Name = "btnNewFolder";
            btnNewFolder.Size = new Size(126, 30);
            btnNewFolder.TabIndex = 1;
            btnNewFolder.Text = "새 저장 폴더 생성";
            btnNewFolder.UseVisualStyleBackColor = false;
            btnNewFolder.Click += btnNewFolder_Click;
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnSelectFolder.BackColor = Color.FromArgb(210, 140, 40);
            btnSelectFolder.Cursor = Cursors.Hand;
            btnSelectFolder.FlatAppearance.BorderSize = 0;
            btnSelectFolder.FlatStyle = FlatStyle.Flat;
            btnSelectFolder.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnSelectFolder.ForeColor = Color.White;
            btnSelectFolder.Location = new Point(232, 10);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(118, 30);
            btnSelectFolder.TabIndex = 0;
            btnSelectFolder.Text = "저장 경로 지정";
            btnSelectFolder.UseVisualStyleBackColor = false;
            btnSelectFolder.Click += btnSelectFolder_Click;
            // 
            // pnlControl
            // 
            pnlControl.BackColor = Color.FromArgb(245, 247, 250);
            pnlControl.Controls.Add(pnlControlJoystick);
            pnlControl.Controls.Add(pnlView);
            pnlControl.Dock = DockStyle.Right;
            pnlControl.Location = new Point(690, 50);
            pnlControl.Name = "pnlControl";
            pnlControl.Size = new Size(260, 666);
            pnlControl.TabIndex = 3;
            // 
            // pnlControlJoystick
            // 
            pnlControlJoystick.BackColor = Color.FromArgb(30, 30, 30);
            pnlControlJoystick.Dock = DockStyle.Fill;
            pnlControlJoystick.Location = new Point(0, 123);
            pnlControlJoystick.Name = "pnlControlJoystick";
            pnlControlJoystick.Size = new Size(260, 543);
            pnlControlJoystick.TabIndex = 1;
            // 
            // pnlView
            // 
            pnlView.BackColor = Color.FromArgb(250, 251, 253);
            pnlView.Controls.Add(pnlDirection);
            pnlView.Controls.Add(pnlControlType);
            pnlView.Controls.Add(pnlSetThrottle);
            pnlView.Dock = DockStyle.Top;
            pnlView.Location = new Point(0, 0);
            pnlView.Name = "pnlView";
            pnlView.Size = new Size(260, 123);
            pnlView.TabIndex = 0;
            // 
            // pnlDirection
            // 
            pnlDirection.Controls.Add(lblThrottle);
            pnlDirection.Controls.Add(lblAngle);
            pnlDirection.Dock = DockStyle.Fill;
            pnlDirection.Location = new Point(0, 34);
            pnlDirection.Name = "pnlDirection";
            pnlDirection.Size = new Size(260, 57);
            pnlDirection.TabIndex = 1;
            // 
            // lblThrottle
            // 
            lblThrottle.Dock = DockStyle.Fill;
            lblThrottle.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblThrottle.ForeColor = Color.FromArgb(72, 175, 120);
            lblThrottle.Location = new Point(130, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(130, 57);
            lblThrottle.TabIndex = 3;
            lblThrottle.Text = "스로틀";
            lblThrottle.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblAngle
            // 
            lblAngle.Dock = DockStyle.Left;
            lblAngle.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            lblAngle.ForeColor = Color.FromArgb(67, 130, 220);
            lblAngle.Location = new Point(0, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(130, 57);
            lblAngle.TabIndex = 2;
            lblAngle.Text = "조향각";
            lblAngle.TextAlign = ContentAlignment.TopCenter;
            // 
            // pnlControlType
            // 
            pnlControlType.BackColor = Color.FromArgb(250, 251, 253);
            pnlControlType.Controls.Add(btnKeyBoard);
            pnlControlType.Controls.Add(btnGamePad);
            pnlControlType.Controls.Add(btnJoyStick);
            pnlControlType.Dock = DockStyle.Top;
            pnlControlType.Location = new Point(0, 0);
            pnlControlType.Name = "pnlControlType";
            pnlControlType.Size = new Size(260, 34);
            pnlControlType.TabIndex = 0;
            // 
            // btnKeyBoard
            // 
            btnKeyBoard.BackColor = Color.FromArgb(100, 150, 210);
            btnKeyBoard.Cursor = Cursors.Hand;
            btnKeyBoard.FlatAppearance.BorderSize = 0;
            btnKeyBoard.FlatStyle = FlatStyle.Flat;
            btnKeyBoard.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnKeyBoard.ForeColor = Color.White;
            btnKeyBoard.Location = new Point(88, 0);
            btnKeyBoard.Name = "btnKeyBoard";
            btnKeyBoard.Size = new Size(86, 34);
            btnKeyBoard.TabIndex = 1;
            btnKeyBoard.Text = "키보드";
            btnKeyBoard.UseVisualStyleBackColor = false;
            btnKeyBoard.Click += btnKeyBoard_Click;
            // 
            // btnGamePad
            // 
            btnGamePad.BackColor = Color.FromArgb(100, 150, 210);
            btnGamePad.Cursor = Cursors.Hand;
            btnGamePad.FlatAppearance.BorderSize = 0;
            btnGamePad.FlatStyle = FlatStyle.Flat;
            btnGamePad.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnGamePad.ForeColor = Color.White;
            btnGamePad.Location = new Point(176, 0);
            btnGamePad.Name = "btnGamePad";
            btnGamePad.Size = new Size(84, 34);
            btnGamePad.TabIndex = 2;
            btnGamePad.Text = "게임패드";
            btnGamePad.UseVisualStyleBackColor = false;
            btnGamePad.Click += btnGamePad_Click;
            // 
            // btnJoyStick
            // 
            btnJoyStick.BackColor = Color.FromArgb(100, 150, 210);
            btnJoyStick.Cursor = Cursors.Hand;
            btnJoyStick.FlatAppearance.BorderSize = 0;
            btnJoyStick.FlatStyle = FlatStyle.Flat;
            btnJoyStick.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnJoyStick.ForeColor = Color.White;
            btnJoyStick.Location = new Point(0, 0);
            btnJoyStick.Name = "btnJoyStick";
            btnJoyStick.Size = new Size(86, 34);
            btnJoyStick.TabIndex = 0;
            btnJoyStick.Text = "조이스틱";
            btnJoyStick.UseVisualStyleBackColor = false;
            btnJoyStick.Click += btnJoyStick_Click;
            // 
            // pnlSetThrottle
            // 
            pnlSetThrottle.BackColor = Color.FromArgb(250, 251, 253);
            pnlSetThrottle.Controls.Add(lblSetThrottle);
            pnlSetThrottle.Controls.Add(cboThrottleMax);
            pnlSetThrottle.Controls.Add(cboThrottleType);
            pnlSetThrottle.Dock = DockStyle.Bottom;
            pnlSetThrottle.Location = new Point(0, 91);
            pnlSetThrottle.Name = "pnlSetThrottle";
            pnlSetThrottle.Size = new Size(260, 32);
            pnlSetThrottle.TabIndex = 2;
            // 
            // lblSetThrottle
            // 
            lblSetThrottle.AutoSize = true;
            lblSetThrottle.Font = new Font("맑은 고딕", 9.5F);
            lblSetThrottle.ForeColor = Color.FromArgb(60, 72, 92);
            lblSetThrottle.Location = new Point(6, 9);
            lblSetThrottle.Name = "lblSetThrottle";
            lblSetThrottle.Size = new Size(78, 17);
            lblSetThrottle.TabIndex = 2;
            lblSetThrottle.Text = "스로틀 설정";
            lblSetThrottle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboThrottleMax
            // 
            cboThrottleMax.BackColor = Color.White;
            cboThrottleMax.FlatStyle = FlatStyle.Flat;
            cboThrottleMax.Font = new Font("맑은 고딕", 9F);
            cboThrottleMax.FormattingEnabled = true;
            cboThrottleMax.Items.AddRange(new object[] { "100%", "90%", "80%", "70%", "60%", "50%", "40%", "30%", "20%", "10%" });
            cboThrottleMax.Location = new Point(188, 6);
            cboThrottleMax.Name = "cboThrottleMax";
            cboThrottleMax.Size = new Size(66, 23);
            cboThrottleMax.TabIndex = 1;
            cboThrottleMax.Text = "100%";
            // 
            // cboThrottleType
            // 
            cboThrottleType.BackColor = Color.White;
            cboThrottleType.FlatStyle = FlatStyle.Flat;
            cboThrottleType.Font = new Font("맑은 고딕", 9F);
            cboThrottleType.FormattingEnabled = true;
            cboThrottleType.Items.AddRange(new object[] { "최댓값", "고정값" });
            cboThrottleType.Location = new Point(90, 6);
            cboThrottleType.Name = "cboThrottleType";
            cboThrottleType.Size = new Size(92, 23);
            cboThrottleType.TabIndex = 0;
            cboThrottleType.Text = "최댓값";
            // 
            // pnlCamera
            // 
            pnlCamera.BackColor = Color.FromArgb(20, 20, 30);
            pnlCamera.Controls.Add(picCamera);
            pnlCamera.Dock = DockStyle.Fill;
            pnlCamera.Location = new Point(0, 50);
            pnlCamera.Name = "pnlCamera";
            pnlCamera.Size = new Size(690, 666);
            pnlCamera.TabIndex = 4;
            // 
            // picCamera
            // 
            picCamera.BackColor = Color.FromArgb(20, 20, 30);
            picCamera.Dock = DockStyle.Fill;
            picCamera.Location = new Point(0, 0);
            picCamera.Name = "picCamera";
            picCamera.Size = new Size(690, 666);
            picCamera.SizeMode = PictureBoxSizeMode.Zoom;
            picCamera.TabIndex = 0;
            picCamera.TabStop = false;
            // 
            // DataCollectionUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlCamera);
            Controls.Add(pnlControl);
            Controls.Add(pnlData);
            Controls.Add(pnlConnect);
            Name = "DataCollectionUI";
            Size = new Size(950, 766);
            pnlConnect.ResumeLayout(false);
            pnlConnect.PerformLayout();
            pnlData.ResumeLayout(false);
            pnlControl.ResumeLayout(false);
            pnlView.ResumeLayout(false);
            pnlDirection.ResumeLayout(false);
            pnlControlType.ResumeLayout(false);
            pnlSetThrottle.ResumeLayout(false);
            pnlSetThrottle.PerformLayout();
            pnlCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCamera).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlConnect;
        private Panel pnlData;
        private Panel pnlControl;
        private Panel pnlCamera;
        private PictureBox picCamera;
        private Button btnConnect;
        private Button btnStartPython;
        private Button btnStartSim;
        private Panel pnlControlJoystick;
        private Panel pnlView;
        private Panel pnlDirection;
        private Panel pnlControlType;
        private Label lblThrottle;
        private Label lblAngle;
        private Button btnGamePad;
        private Button btnKeyBoard;
        private Button btnJoyStick;
        private CheckBox chkRecording;
        private ComboBox cboMapList;
        private Button btnSelectFolder;
        private Button btnNewFolder;
        private Label lblSelectedFolderRoute;
        private Panel pnlSetThrottle;
        private ComboBox cboThrottleType;
        private ComboBox cboThrottleMax;
        private Label lblSetThrottle;
        private Button btnUnSelectFolder;
        private Button btnDelFolder;
    }
}