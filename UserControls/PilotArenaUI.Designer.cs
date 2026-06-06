namespace DataManager.UserControls
{
    partial class PilotArenaUI
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
            lblPilotTest = new Label();
            btnPlay = new Button();
            btn5FrameRight = new Button();
            btn5FrameLeft = new Button();
            btnFrameRight = new Button();
            btnFrameLeft = new Button();
            pnlSetting = new Panel();
            btnLoadTub = new Button();
            comboBox1 = new ComboBox();
            btnModelAdd = new Button();
            trkBlur = new TrackBar();
            lblBlur = new Label();
            trkBright = new TrackBar();
            lblBright = new Label();
            btnResetTransform = new Button();
            trkProgress = new TrackBar();
            lblSpeed = new Label();
            flpModule = new Panel();
            pnlSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trkBlur).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkBright).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trkProgress).BeginInit();
            SuspendLayout();
            // 
            // lblPilotTest
            // 
            lblPilotTest.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblPilotTest.Location = new Point(4, 4);
            lblPilotTest.Name = "lblPilotTest";
            lblPilotTest.Size = new Size(200, 38);
            lblPilotTest.TabIndex = 0;
            lblPilotTest.Text = "모델 테스트";
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.FromArgb(72, 175, 120);
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnPlay.ForeColor = Color.White;
            btnPlay.Location = new Point(839, 4);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(70, 30);
            btnPlay.TabIndex = 11;
            btnPlay.Text = "▶ 재생";
            btnPlay.UseVisualStyleBackColor = false;
            // 
            // btn5FrameRight
            // 
            btn5FrameRight.BackColor = Color.FromArgb(80, 130, 195);
            btn5FrameRight.Cursor = Cursors.Hand;
            btn5FrameRight.FlatAppearance.BorderSize = 0;
            btn5FrameRight.FlatStyle = FlatStyle.Flat;
            btn5FrameRight.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btn5FrameRight.ForeColor = Color.White;
            btn5FrameRight.Location = new Point(783, 4);
            btn5FrameRight.Name = "btn5FrameRight";
            btn5FrameRight.Size = new Size(52, 30);
            btn5FrameRight.TabIndex = 10;
            btn5FrameRight.Text = ">>>";
            btn5FrameRight.UseVisualStyleBackColor = false;
            // 
            // btn5FrameLeft
            // 
            btn5FrameLeft.BackColor = Color.FromArgb(80, 130, 195);
            btn5FrameLeft.Cursor = Cursors.Hand;
            btn5FrameLeft.FlatAppearance.BorderSize = 0;
            btn5FrameLeft.FlatStyle = FlatStyle.Flat;
            btn5FrameLeft.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btn5FrameLeft.ForeColor = Color.White;
            btn5FrameLeft.Location = new Point(671, 4);
            btn5FrameLeft.Name = "btn5FrameLeft";
            btn5FrameLeft.Size = new Size(52, 30);
            btn5FrameLeft.TabIndex = 9;
            btn5FrameLeft.Text = "<<<";
            btn5FrameLeft.UseVisualStyleBackColor = false;
            // 
            // btnFrameRight
            // 
            btnFrameRight.BackColor = Color.FromArgb(100, 150, 210);
            btnFrameRight.Cursor = Cursors.Hand;
            btnFrameRight.FlatAppearance.BorderSize = 0;
            btnFrameRight.FlatStyle = FlatStyle.Flat;
            btnFrameRight.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFrameRight.ForeColor = Color.White;
            btnFrameRight.Location = new Point(727, 4);
            btnFrameRight.Name = "btnFrameRight";
            btnFrameRight.Size = new Size(52, 30);
            btnFrameRight.TabIndex = 8;
            btnFrameRight.Text = ">";
            btnFrameRight.UseVisualStyleBackColor = false;
            // 
            // btnFrameLeft
            // 
            btnFrameLeft.BackColor = Color.FromArgb(100, 150, 210);
            btnFrameLeft.Cursor = Cursors.Hand;
            btnFrameLeft.FlatAppearance.BorderSize = 0;
            btnFrameLeft.FlatStyle = FlatStyle.Flat;
            btnFrameLeft.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFrameLeft.ForeColor = Color.White;
            btnFrameLeft.Location = new Point(615, 4);
            btnFrameLeft.Name = "btnFrameLeft";
            btnFrameLeft.Size = new Size(52, 30);
            btnFrameLeft.TabIndex = 7;
            btnFrameLeft.Text = "<";
            btnFrameLeft.UseVisualStyleBackColor = false;
            // 
            // pnlSetting
            // 
            pnlSetting.BackColor = Color.FromArgb(250, 251, 253);
            pnlSetting.Controls.Add(btnLoadTub);
            pnlSetting.Controls.Add(comboBox1);
            pnlSetting.Controls.Add(btnModelAdd);
            pnlSetting.Controls.Add(trkBlur);
            pnlSetting.Controls.Add(lblBlur);
            pnlSetting.Controls.Add(trkBright);
            pnlSetting.Controls.Add(lblBright);
            pnlSetting.Controls.Add(btnResetTransform);
            pnlSetting.Controls.Add(trkProgress);
            pnlSetting.Controls.Add(lblPilotTest);
            pnlSetting.Controls.Add(btnPlay);
            pnlSetting.Controls.Add(btnFrameRight);
            pnlSetting.Controls.Add(btn5FrameRight);
            pnlSetting.Controls.Add(btnFrameLeft);
            pnlSetting.Controls.Add(btn5FrameLeft);
            pnlSetting.Controls.Add(lblSpeed);
            pnlSetting.Dock = DockStyle.Top;
            pnlSetting.Location = new Point(0, 0);
            pnlSetting.Margin = new Padding(0);
            pnlSetting.Name = "pnlSetting";
            pnlSetting.Size = new Size(950, 70);
            pnlSetting.TabIndex = 5;
            // 
            // btnLoadTub
            // 
            btnLoadTub.BackColor = Color.FromArgb(100, 150, 210);
            btnLoadTub.Cursor = Cursors.Hand;
            btnLoadTub.FlatAppearance.BorderSize = 0;
            btnLoadTub.FlatStyle = FlatStyle.Flat;
            btnLoadTub.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnLoadTub.ForeColor = Color.White;
            btnLoadTub.Location = new Point(90, 44);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(143, 24);
            btnLoadTub.TabIndex = 21;
            btnLoadTub.Text = "주행 데이터 가져오기";
            btnLoadTub.UseVisualStyleBackColor = false;
            btnLoadTub.Click += btnLoadTub_Click;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.White;
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox1.Font = new Font("맑은 고딕", 9.5F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "0.25", "0.50", "1.00", "2.00", "3.00", "5.00", "10.00" });
            comboBox1.Location = new Point(643, 38);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(62, 25);
            comboBox1.TabIndex = 19;
            comboBox1.Text = "1.00";
            // 
            // btnModelAdd
            // 
            btnModelAdd.BackColor = Color.FromArgb(72, 175, 120);
            btnModelAdd.Cursor = Cursors.Hand;
            btnModelAdd.FlatAppearance.BorderSize = 0;
            btnModelAdd.FlatStyle = FlatStyle.Flat;
            btnModelAdd.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnModelAdd.ForeColor = Color.White;
            btnModelAdd.Location = new Point(4, 44);
            btnModelAdd.Name = "btnModelAdd";
            btnModelAdd.Size = new Size(80, 24);
            btnModelAdd.TabIndex = 18;
            btnModelAdd.Text = "모델 추가";
            btnModelAdd.UseVisualStyleBackColor = false;
            btnModelAdd.Click += btnModelAdd_Click;
            // 
            // trkBlur
            // 
            trkBlur.AutoSize = false;
            trkBlur.LargeChange = 1;
            trkBlur.Location = new Point(310, 10);
            trkBlur.Name = "trkBlur";
            trkBlur.Size = new Size(154, 22);
            trkBlur.TabIndex = 17;
            trkBlur.TickStyle = TickStyle.None;
            // 
            // lblBlur
            // 
            lblBlur.Font = new Font("맑은 고딕", 9.5F);
            lblBlur.ForeColor = Color.FromArgb(60, 72, 92);
            lblBlur.Location = new Point(234, 8);
            lblBlur.Name = "lblBlur";
            lblBlur.Size = new Size(80, 22);
            lblBlur.TabIndex = 16;
            lblBlur.Text = "흐림 : 0.00";
            lblBlur.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trkBright
            // 
            trkBright.AutoSize = false;
            trkBright.LargeChange = 1;
            trkBright.Location = new Point(310, 42);
            trkBright.Name = "trkBright";
            trkBright.Size = new Size(154, 22);
            trkBright.TabIndex = 15;
            trkBright.TickStyle = TickStyle.None;
            // 
            // lblBright
            // 
            lblBright.Font = new Font("맑은 고딕", 9.5F);
            lblBright.ForeColor = Color.FromArgb(60, 72, 92);
            lblBright.Location = new Point(234, 40);
            lblBright.Name = "lblBright";
            lblBright.Size = new Size(80, 22);
            lblBright.TabIndex = 14;
            lblBright.Text = "밝기 : 0.00";
            lblBright.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnResetTransform
            // 
            btnResetTransform.BackColor = Color.FromArgb(140, 100, 185);
            btnResetTransform.Cursor = Cursors.Hand;
            btnResetTransform.FlatAppearance.BorderSize = 0;
            btnResetTransform.FlatStyle = FlatStyle.Flat;
            btnResetTransform.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
            btnResetTransform.ForeColor = Color.White;
            btnResetTransform.Location = new Point(467, 34);
            btnResetTransform.Name = "btnResetTransform";
            btnResetTransform.Size = new Size(105, 30);
            btnResetTransform.TabIndex = 22;
            btnResetTransform.Text = "변경사항 초기화";
            btnResetTransform.UseVisualStyleBackColor = false;
            btnResetTransform.Click += BtnResetTransform_Click;
            // 
            // trkProgress
            // 
            trkProgress.AutoSize = false;
            trkProgress.LargeChange = 1;
            trkProgress.Location = new Point(704, 38);
            trkProgress.Name = "trkProgress";
            trkProgress.Size = new Size(213, 22);
            trkProgress.TabIndex = 13;
            trkProgress.TickStyle = TickStyle.None;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Font = new Font("맑은 고딕", 9.5F);
            lblSpeed.ForeColor = Color.FromArgb(60, 72, 92);
            lblSpeed.Location = new Point(605, 42);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(34, 17);
            lblSpeed.TabIndex = 20;
            lblSpeed.Text = "배속";
            lblSpeed.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flpModule
            // 
            flpModule.BackColor = Color.FromArgb(235, 238, 244);
            flpModule.Dock = DockStyle.Fill;
            flpModule.Location = new Point(0, 70);
            flpModule.Margin = new Padding(0);
            flpModule.Name = "flpModule";
            flpModule.Size = new Size(950, 696);
            flpModule.TabIndex = 6;
            // 
            // PilotArenaUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(flpModule);
            Controls.Add(pnlSetting);
            Name = "PilotArenaUI";
            Size = new Size(950, 766);
            pnlSetting.ResumeLayout(false);
            pnlSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trkBlur).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkBright).EndInit();
            ((System.ComponentModel.ISupportInitialize)trkProgress).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label lblPilotTest;
        private Button btnLoadModel;
        private ComboBox cboModelType;
        private Button btnPlay;
        private Button btn5FrameRight;
        private Button btn5FrameLeft;
        private Button btnFrameRight;
        private Button btnFrameLeft;
        private Panel pnlSetting;
        private TrackBar trkProgress;
        private Panel flpModule;
        private Label lblBright;
        private TrackBar trkBright;
        private TrackBar trkBlur;
        private Label lblBlur;
        private Button btnModelAdd;
        private Label lblSpeed;
        private ComboBox comboBox1;
        private Button btnLoadTub;
        private Button btnResetTransform;
    }
}
