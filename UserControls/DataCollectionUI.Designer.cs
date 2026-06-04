namespace DataManager.UserControls
{
    partial class DataCollectionUI
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
            pnlConnect.Controls.Add(cboMapList);
            pnlConnect.Controls.Add(chkRecording);
            pnlConnect.Controls.Add(btnStartPython);
            pnlConnect.Controls.Add(btnStartSim);
            pnlConnect.Controls.Add(btnConnect);
            pnlConnect.Dock = DockStyle.Top;
            pnlConnect.Location = new Point(0, 0);
            pnlConnect.Name = "pnlConnect";
            pnlConnect.Size = new Size(750, 96);
            pnlConnect.TabIndex = 1;
            // 
            // cboMapList
            // 
            cboMapList.FormattingEnabled = true;
            cboMapList.Location = new Point(109, 54);
            cboMapList.Name = "cboMapList";
            cboMapList.Size = new Size(121, 23);
            cboMapList.TabIndex = 0;
            // 
            // chkRecording
            // 
            chkRecording.AutoSize = true;
            chkRecording.Location = new Point(3, 54);
            chkRecording.Name = "chkRecording";
            chkRecording.Size = new Size(80, 19);
            chkRecording.TabIndex = 3;
            chkRecording.Text = "Recording";
            chkRecording.UseVisualStyleBackColor = true;
            chkRecording.CheckedChanged += chkRecording_CheckedChanged;
            // 
            // btnStartPython
            // 
            btnStartPython.Location = new Point(109, 3);
            btnStartPython.Name = "btnStartPython";
            btnStartPython.Size = new Size(100, 45);
            btnStartPython.TabIndex = 2;
            btnStartPython.Text = "Start Python Server";
            btnStartPython.UseVisualStyleBackColor = true;
            btnStartPython.Click += btnStartPython_Click;
            // 
            // btnStartSim
            // 
            btnStartSim.Location = new Point(3, 3);
            btnStartSim.Name = "btnStartSim";
            btnStartSim.Size = new Size(100, 45);
            btnStartSim.TabIndex = 1;
            btnStartSim.Text = "Start Simulator";
            btnStartSim.UseVisualStyleBackColor = true;
            btnStartSim.Click += btnStartSim_Click;
            // 
            // btnConnect
            // 
            btnConnect.Enabled = false;
            btnConnect.Location = new Point(215, 3);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(100, 45);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "Connect Server";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // pnlData
            // 
            pnlData.Controls.Add(btnUnSelectFolder);
            pnlData.Controls.Add(btnDelFolder);
            pnlData.Controls.Add(lblSelectedFolderRoute);
            pnlData.Controls.Add(btnNewFolder);
            pnlData.Controls.Add(btnSelectFolder);
            pnlData.Dock = DockStyle.Bottom;
            pnlData.Location = new Point(0, 520);
            pnlData.Name = "pnlData";
            pnlData.Size = new Size(750, 80);
            pnlData.TabIndex = 2;
            // 
            // btnUnSelectFolder
            // 
            btnUnSelectFolder.Location = new Point(3, 42);
            btnUnSelectFolder.Name = "btnUnSelectFolder";
            btnUnSelectFolder.Size = new Size(120, 35);
            btnUnSelectFolder.TabIndex = 4;
            btnUnSelectFolder.Text = "UnSelect Folder";
            btnUnSelectFolder.UseVisualStyleBackColor = true;
            btnUnSelectFolder.Click += btnUnSelectFolder_Click;
            // 
            // btnDelFolder
            // 
            btnDelFolder.Location = new Point(129, 3);
            btnDelFolder.Name = "btnDelFolder";
            btnDelFolder.Size = new Size(120, 35);
            btnDelFolder.TabIndex = 3;
            btnDelFolder.Text = "Delect Folder";
            btnDelFolder.UseVisualStyleBackColor = true;
            btnDelFolder.Click += btnDelFolder_Click;
            // 
            // lblSelectedFolderRoute
            // 
            lblSelectedFolderRoute.Location = new Point(255, 42);
            lblSelectedFolderRoute.Name = "lblSelectedFolderRoute";
            lblSelectedFolderRoute.Size = new Size(361, 35);
            lblSelectedFolderRoute.TabIndex = 2;
            lblSelectedFolderRoute.Text = "Selected Folder Route";
            lblSelectedFolderRoute.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnNewFolder
            // 
            btnNewFolder.Location = new Point(3, 3);
            btnNewFolder.Name = "btnNewFolder";
            btnNewFolder.Size = new Size(120, 35);
            btnNewFolder.TabIndex = 1;
            btnNewFolder.Text = "Create New Folder";
            btnNewFolder.UseVisualStyleBackColor = true;
            btnNewFolder.Click += btnNewFolder_Click;
            // 
            // btnSelectFolder
            // 
            btnSelectFolder.Location = new Point(129, 42);
            btnSelectFolder.Name = "btnSelectFolder";
            btnSelectFolder.Size = new Size(120, 35);
            btnSelectFolder.TabIndex = 0;
            btnSelectFolder.Text = "Select Folder";
            btnSelectFolder.UseVisualStyleBackColor = true;
            btnSelectFolder.Click += btnSelectFolder_Click;
            // 
            // pnlControl
            // 
            pnlControl.Controls.Add(pnlControlJoystick);
            pnlControl.Controls.Add(pnlView);
            pnlControl.Dock = DockStyle.Right;
            pnlControl.Location = new Point(490, 96);
            pnlControl.Name = "pnlControl";
            pnlControl.Size = new Size(260, 424);
            pnlControl.TabIndex = 3;
            // 
            // pnlControlJoystick
            // 
            pnlControlJoystick.Dock = DockStyle.Fill;
            pnlControlJoystick.Location = new Point(0, 123);
            pnlControlJoystick.Name = "pnlControlJoystick";
            pnlControlJoystick.Size = new Size(260, 301);
            pnlControlJoystick.TabIndex = 1;
            // 
            // pnlView
            // 
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
            lblThrottle.Location = new Point(130, 0);
            lblThrottle.Name = "lblThrottle";
            lblThrottle.Size = new Size(130, 57);
            lblThrottle.TabIndex = 3;
            lblThrottle.Text = "Throttle";
            lblThrottle.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblAngle
            // 
            lblAngle.Dock = DockStyle.Left;
            lblAngle.Location = new Point(0, 0);
            lblAngle.Name = "lblAngle";
            lblAngle.Size = new Size(130, 57);
            lblAngle.TabIndex = 2;
            lblAngle.Text = "Angle";
            lblAngle.TextAlign = ContentAlignment.TopCenter;
            // 
            // pnlControlType
            // 
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
            btnKeyBoard.Location = new Point(91, 0);
            btnKeyBoard.Name = "btnKeyBoard";
            btnKeyBoard.Size = new Size(81, 34);
            btnKeyBoard.TabIndex = 1;
            btnKeyBoard.Text = "KeyBoard";
            btnKeyBoard.UseVisualStyleBackColor = true;
            btnKeyBoard.Click += btnKeyBoard_Click;
            // 
            // btnGamePad
            // 
            btnGamePad.Location = new Point(178, 0);
            btnGamePad.Name = "btnGamePad";
            btnGamePad.Size = new Size(82, 34);
            btnGamePad.TabIndex = 2;
            btnGamePad.Text = "GamePad";
            btnGamePad.UseVisualStyleBackColor = true;
            btnGamePad.Click += btnGamePad_Click;
            // 
            // btnJoyStick
            // 
            btnJoyStick.Location = new Point(3, 0);
            btnJoyStick.Name = "btnJoyStick";
            btnJoyStick.Size = new Size(82, 34);
            btnJoyStick.TabIndex = 0;
            btnJoyStick.Text = "JoyStick";
            btnJoyStick.UseVisualStyleBackColor = true;
            btnJoyStick.Click += btnJoyStick_Click;
            // 
            // pnlSetThrottle
            // 
            pnlSetThrottle.Controls.Add(lblSetThrottle);
            pnlSetThrottle.Controls.Add(cboThrottleMax);
            pnlSetThrottle.Controls.Add(cboThrottleType);
            pnlSetThrottle.Dock = DockStyle.Bottom;
            pnlSetThrottle.Location = new Point(0, 91);
            pnlSetThrottle.Name = "pnlSetThrottle";
            pnlSetThrottle.Size = new Size(260, 32);
            pnlSetThrottle.TabIndex = 0;
            // 
            // lblSetThrottle
            // 
            lblSetThrottle.AutoSize = true;
            lblSetThrottle.Location = new Point(16, 9);
            lblSetThrottle.Name = "lblSetThrottle";
            lblSetThrottle.Size = new Size(69, 15);
            lblSetThrottle.TabIndex = 2;
            lblSetThrottle.Text = "Set Throttle";
            lblSetThrottle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboThrottleMax
            // 
            cboThrottleMax.FormattingEnabled = true;
            cboThrottleMax.Items.AddRange(new object[] { "100%", "90%", "80%", "70%", "60%", "50%", "40%", "30%", "20%", "10%" });
            cboThrottleMax.Location = new Point(196, 6);
            cboThrottleMax.Name = "cboThrottleMax";
            cboThrottleMax.Size = new Size(58, 23);
            cboThrottleMax.TabIndex = 1;
            cboThrottleMax.Text = "100%";
            // 
            // cboThrottleType
            // 
            cboThrottleType.FormattingEnabled = true;
            cboThrottleType.Items.AddRange(new object[] { "Maximum", "Constant" });
            cboThrottleType.Location = new Point(98, 6);
            cboThrottleType.Name = "cboThrottleType";
            cboThrottleType.Size = new Size(92, 23);
            cboThrottleType.TabIndex = 0;
            cboThrottleType.Text = "Maximum";
            // 
            // pnlCamera
            // 
            pnlCamera.Controls.Add(picCamera);
            pnlCamera.Dock = DockStyle.Fill;
            pnlCamera.Location = new Point(0, 96);
            pnlCamera.Name = "pnlCamera";
            pnlCamera.Size = new Size(490, 424);
            pnlCamera.TabIndex = 4;
            // 
            // picCamera
            // 
            picCamera.BackColor = Color.Black;
            picCamera.Dock = DockStyle.Fill;
            picCamera.Location = new Point(0, 0);
            picCamera.Name = "picCamera";
            picCamera.Size = new Size(490, 424);
            picCamera.SizeMode = PictureBoxSizeMode.Zoom;
            picCamera.TabIndex = 0;
            picCamera.TabStop = false;
            // 
            // DataCollectionUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlCamera);
            Controls.Add(pnlControl);
            Controls.Add(pnlData);
            Controls.Add(pnlConnect);
            Name = "DataCollectionUI";
            Size = new Size(750, 600);
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
