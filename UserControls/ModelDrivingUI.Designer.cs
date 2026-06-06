namespace DataManager.UserControls
{
    partial class ModelDrivingUI
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
            panel1 = new Panel();
            cboMapList = new ComboBox();
            btnDrive = new Button();
            lblLoadedModel = new Label();
            btnLoadModel = new Button();
            btnInit = new Button();
            btnConnect = new Button();
            btnStartSim = new Button();
            lblModelDriving = new Label();
            panel2 = new Panel();
            picCamera = new PictureBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCamera).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(cboMapList);
            panel1.Controls.Add(btnDrive);
            panel1.Controls.Add(lblLoadedModel);
            panel1.Controls.Add(btnLoadModel);
            panel1.Controls.Add(btnInit);
            panel1.Controls.Add(btnConnect);
            panel1.Controls.Add(btnStartSim);
            panel1.Controls.Add(lblModelDriving);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(950, 88);
            panel1.TabIndex = 0;
            // 
            // cboMapList
            // 
            cboMapList.BackColor = Color.White;
            cboMapList.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMapList.FlatStyle = FlatStyle.Flat;
            cboMapList.Font = new Font("맑은 고딕", 9.5F);
            cboMapList.FormattingEnabled = true;
            cboMapList.Items.AddRange(new object[] { "generated_track", "generated_road", "warehouse", "sparkfun_avc", "mountain_track", "roboracingleague_1", "mini_monaco", "warren", "circuit_launch", "waveshare" });
            cboMapList.Location = new Point(482, 13);
            cboMapList.Name = "cboMapList";
            cboMapList.Size = new Size(162, 25);
            cboMapList.TabIndex = 1;
            cboMapList.SelectedIndexChanged += cboMapList_SelectedIndexChanged;
            // 
            // btnDrive
            // 
            btnDrive.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnDrive.BackColor = Color.FromArgb(72, 175, 120);
            btnDrive.Cursor = Cursors.Hand;
            btnDrive.Enabled = false;
            btnDrive.FlatAppearance.BorderSize = 0;
            btnDrive.FlatStyle = FlatStyle.Flat;
            btnDrive.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnDrive.ForeColor = Color.White;
            btnDrive.Location = new Point(832, 10);
            btnDrive.Name = "btnDrive";
            btnDrive.Size = new Size(103, 30);
            btnDrive.TabIndex = 9;
            btnDrive.Text = "▶ 주행 시작";
            btnDrive.UseVisualStyleBackColor = false;
            btnDrive.Click += btnDrive_Click;
            // 
            // lblLoadedModel
            // 
            lblLoadedModel.Location = new Point(263, 51);
            lblLoadedModel.Name = "lblLoadedModel";
            lblLoadedModel.Size = new Size(190, 30);
            lblLoadedModel.TabIndex = 7;
            lblLoadedModel.Text = "tflite 파일을 선택해주세요";
            // 
            // btnLoadModel
            // 
            btnLoadModel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoadModel.BackColor = Color.FromArgb(67, 130, 220);
            btnLoadModel.Cursor = Cursors.Hand;
            btnLoadModel.Enabled = false;
            btnLoadModel.FlatAppearance.BorderSize = 0;
            btnLoadModel.FlatStyle = FlatStyle.Flat;
            btnLoadModel.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnLoadModel.ForeColor = Color.White;
            btnLoadModel.Location = new Point(154, 48);
            btnLoadModel.Name = "btnLoadModel";
            btnLoadModel.Size = new Size(103, 30);
            btnLoadModel.TabIndex = 6;
            btnLoadModel.Text = "모델 가져오기";
            btnLoadModel.UseVisualStyleBackColor = false;
            btnLoadModel.Click += btnLoadModel_Click;
            // 
            // btnInit
            // 
            btnInit.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            btnInit.BackColor = Color.FromArgb(210, 70, 70);
            btnInit.Cursor = Cursors.Hand;
            btnInit.FlatAppearance.BorderSize = 0;
            btnInit.FlatStyle = FlatStyle.Flat;
            btnInit.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnInit.ForeColor = Color.White;
            btnInit.Location = new Point(382, 10);
            btnInit.Name = "btnInit";
            btnInit.Size = new Size(94, 30);
            btnInit.TabIndex = 5;
            btnInit.Text = "초기화";
            btnInit.UseVisualStyleBackColor = false;
            btnInit.Click += btnInit_Click;
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
            btnConnect.Location = new Point(282, 10);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 30);
            btnConnect.TabIndex = 3;
            btnConnect.Text = "서버 연결";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
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
            btnStartSim.Location = new Point(154, 10);
            btnStartSim.Name = "btnStartSim";
            btnStartSim.Size = new Size(122, 30);
            btnStartSim.TabIndex = 2;
            btnStartSim.Text = "시뮬레이터 시작";
            btnStartSim.UseVisualStyleBackColor = false;
            btnStartSim.Click += btnStartSim_Click;
            // 
            // lblModelDriving
            // 
            lblModelDriving.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblModelDriving.Location = new Point(0, 0);
            lblModelDriving.Name = "lblModelDriving";
            lblModelDriving.Size = new Size(139, 47);
            lblModelDriving.TabIndex = 0;
            lblModelDriving.Text = "모델 주행";
            lblModelDriving.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.Controls.Add(picCamera);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 88);
            panel2.Name = "panel2";
            panel2.Size = new Size(950, 678);
            panel2.TabIndex = 1;
            // 
            // picCamera
            // 
            picCamera.BackColor = Color.FromArgb(20, 20, 30);
            picCamera.Dock = DockStyle.Fill;
            picCamera.Location = new Point(0, 0);
            picCamera.Name = "picCamera";
            picCamera.Size = new Size(950, 678);
            picCamera.SizeMode = PictureBoxSizeMode.Zoom;
            picCamera.TabIndex = 0;
            picCamera.TabStop = false;
            // 
            // ModelDrivingUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ModelDrivingUI";
            Size = new Size(950, 766);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCamera).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lblModelDriving;
        private Button btnStartSim;
        private Button btnConnect;
        private Button btnInit;
        private Button btnLoadModel;
        private Label lblLoadedModel;
        private Panel panel2;
        private PictureBox picCamera;
        private Button btnDrive;
        private ComboBox cboMapList;
    }
}
