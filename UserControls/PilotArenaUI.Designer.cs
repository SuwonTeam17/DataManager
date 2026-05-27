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
            btnStop = new Button();
            btnPlay = new Button();
            btn5FrameRight = new Button();
            btn5FrameLeft = new Button();
            btnFrameRight = new Button();
            btnFrameLeft = new Button();
            pnlSetting = new Panel();
            btnLoadTub = new Button();
            btnFullGraph = new Button();
            comboBox1 = new ComboBox();
            btnModelAdd = new Button();
            trkBlur = new TrackBar();
            lblBlur = new Label();
            trkBright = new TrackBar();
            lblBright = new Label();
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
            // btnModelAdd
            // 
            btnModelAdd.BackColor = Color.FromArgb(72, 175, 120);
            btnModelAdd.FlatStyle = FlatStyle.Flat;
            btnModelAdd.FlatAppearance.BorderSize = 0;
            btnModelAdd.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnModelAdd.ForeColor = Color.White;
            btnModelAdd.Location = new Point(4, 44);
            btnModelAdd.Name = "btnModelAdd";
            btnModelAdd.Size = new Size(90, 24);
            btnModelAdd.TabIndex = 18;
            btnModelAdd.Text = "모델 추가";
            btnModelAdd.UseVisualStyleBackColor = false;
            btnModelAdd.Cursor = Cursors.Hand;
            btnModelAdd.Click += btnModelAdd_Click;
            // 
            // btnLoadTub
            // 
            btnLoadTub.BackColor = Color.FromArgb(100, 150, 210);
            btnLoadTub.FlatStyle = FlatStyle.Flat;
            btnLoadTub.FlatAppearance.BorderSize = 0;
            btnLoadTub.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnLoadTub.ForeColor = Color.White;
            btnLoadTub.Location = new Point(100, 44);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(90, 24);
            btnLoadTub.TabIndex = 21;
            btnLoadTub.Text = "Tub 열기";
            btnLoadTub.UseVisualStyleBackColor = false;
            btnLoadTub.Cursor = Cursors.Hand;
            btnLoadTub.Click += btnLoadTub_Click;
            //
            // btnFullGraph
            //
            btnFullGraph.BackColor = Color.FromArgb(130, 100, 200);
            btnFullGraph.FlatStyle = FlatStyle.Flat;
            btnFullGraph.FlatAppearance.BorderSize = 0;
            btnFullGraph.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFullGraph.ForeColor = Color.White;
            btnFullGraph.Location = new Point(196, 44);
            btnFullGraph.Name = "btnFullGraph";
            btnFullGraph.Size = new Size(90, 24);
            btnFullGraph.TabIndex = 22;
            btnFullGraph.Text = "전체 그래프";
            btnFullGraph.UseVisualStyleBackColor = false;
            btnFullGraph.Cursor = Cursors.Hand;
            btnFullGraph.Click += btnFullGraph_Click;
            //
            // lblBlur
            // 
            lblBlur.Font = new Font("맑은 고딕", 9.5F);
            lblBlur.ForeColor = Color.FromArgb(60, 72, 92);
            lblBlur.Location = new Point(200, 8);
            lblBlur.Name = "lblBlur";
            lblBlur.Size = new Size(80, 22);
            lblBlur.TabIndex = 16;
            lblBlur.Text = "흐림 : 0.00";
            lblBlur.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trkBlur
            // 
            trkBlur.AutoSize = false;
            trkBlur.LargeChange = 1;
            trkBlur.Location = new Point(284, 6);
            trkBlur.Name = "trkBlur";
            trkBlur.Size = new Size(184, 22);
            trkBlur.TabIndex = 17;
            trkBlur.TickStyle = TickStyle.None;
            // 
            // lblBright
            // 
            lblBright.Font = new Font("맑은 고딕", 9.5F);
            lblBright.ForeColor = Color.FromArgb(60, 72, 92);
            lblBright.Location = new Point(200, 40);
            lblBright.Name = "lblBright";
            lblBright.Size = new Size(80, 22);
            lblBright.TabIndex = 14;
            lblBright.Text = "밝기 : 0.00";
            lblBright.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trkBright
            // 
            trkBright.AutoSize = false;
            trkBright.LargeChange = 1;
            trkBright.Location = new Point(284, 38);
            trkBright.Name = "trkBright";
            trkBright.Size = new Size(184, 22);
            trkBright.TabIndex = 15;
            trkBright.TickStyle = TickStyle.None;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Font = new Font("맑은 고딕", 9.5F);
            lblSpeed.ForeColor = Color.FromArgb(60, 72, 92);
            lblSpeed.Location = new Point(476, 42);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.TabIndex = 20;
            lblSpeed.Text = "배속";
            lblSpeed.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.White;
            comboBox1.FlatStyle = FlatStyle.Flat;
            comboBox1.Font = new Font("맑은 고딕", 9.5F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "0.25", "0.50", "1.00", "2.00", "3.00", "5.00", "10.00" });
            comboBox1.Location = new Point(514, 38);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(62, 25);
            comboBox1.TabIndex = 19;
            comboBox1.Text = "1.00";
            // 
            // trkProgress
            // 
            trkProgress.AutoSize = false;
            trkProgress.LargeChange = 1;
            trkProgress.Location = new Point(582, 38);
            trkProgress.Name = "trkProgress";
            trkProgress.Size = new Size(213, 22);
            trkProgress.TabIndex = 13;
            trkProgress.TickStyle = TickStyle.None;
            // 
            // btnFrameLeft  "<"
            // 
            btnFrameLeft.BackColor = Color.FromArgb(100, 150, 210);
            btnFrameLeft.FlatStyle = FlatStyle.Flat;
            btnFrameLeft.FlatAppearance.BorderSize = 0;
            btnFrameLeft.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFrameLeft.ForeColor = Color.White;
            btnFrameLeft.Location = new Point(476, 4);
            btnFrameLeft.Name = "btnFrameLeft";
            btnFrameLeft.Size = new Size(52, 30);
            btnFrameLeft.TabIndex = 7;
            btnFrameLeft.Text = "<";
            btnFrameLeft.UseVisualStyleBackColor = false;
            btnFrameLeft.Cursor = Cursors.Hand;
            // 
            // btn5FrameLeft  "<<<"
            // 
            btn5FrameLeft.BackColor = Color.FromArgb(80, 130, 195);
            btn5FrameLeft.FlatStyle = FlatStyle.Flat;
            btn5FrameLeft.FlatAppearance.BorderSize = 0;
            btn5FrameLeft.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btn5FrameLeft.ForeColor = Color.White;
            btn5FrameLeft.Location = new Point(532, 4);
            btn5FrameLeft.Name = "btn5FrameLeft";
            btn5FrameLeft.Size = new Size(52, 30);
            btn5FrameLeft.TabIndex = 9;
            btn5FrameLeft.Text = "<<<";
            btn5FrameLeft.UseVisualStyleBackColor = false;
            btn5FrameLeft.Cursor = Cursors.Hand;
            // 
            // btnFrameRight  ">"
            // 
            btnFrameRight.BackColor = Color.FromArgb(100, 150, 210);
            btnFrameRight.FlatStyle = FlatStyle.Flat;
            btnFrameRight.FlatAppearance.BorderSize = 0;
            btnFrameRight.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnFrameRight.ForeColor = Color.White;
            btnFrameRight.Location = new Point(588, 4);
            btnFrameRight.Name = "btnFrameRight";
            btnFrameRight.Size = new Size(52, 30);
            btnFrameRight.TabIndex = 8;
            btnFrameRight.Text = ">";
            btnFrameRight.UseVisualStyleBackColor = false;
            btnFrameRight.Cursor = Cursors.Hand;
            // 
            // btn5FrameRight  ">>>"
            // 
            btn5FrameRight.BackColor = Color.FromArgb(80, 130, 195);
            btn5FrameRight.FlatStyle = FlatStyle.Flat;
            btn5FrameRight.FlatAppearance.BorderSize = 0;
            btn5FrameRight.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btn5FrameRight.ForeColor = Color.White;
            btn5FrameRight.Location = new Point(644, 4);
            btn5FrameRight.Name = "btn5FrameRight";
            btn5FrameRight.Size = new Size(52, 30);
            btn5FrameRight.TabIndex = 10;
            btn5FrameRight.Text = ">>>";
            btn5FrameRight.UseVisualStyleBackColor = false;
            btn5FrameRight.Cursor = Cursors.Hand;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.FromArgb(72, 175, 120);
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnPlay.ForeColor = Color.White;
            btnPlay.Location = new Point(700, 4);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(70, 30);
            btnPlay.TabIndex = 11;
            btnPlay.Text = "▶ 재생";
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Cursor = Cursors.Hand;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(210, 70, 70);
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(774, 4);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(70, 30);
            btnStop.TabIndex = 12;
            btnStop.Text = "■ 중지";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Cursor = Cursors.Hand;
            // 
            // pnlSetting
            // 
            pnlSetting.BackColor = Color.FromArgb(250, 251, 253);
            pnlSetting.BorderStyle = BorderStyle.None;
            pnlSetting.Controls.Add(btnLoadTub);
            pnlSetting.Controls.Add(btnFullGraph);
            pnlSetting.Controls.Add(comboBox1);
            pnlSetting.Controls.Add(btnModelAdd);
            pnlSetting.Controls.Add(trkBlur);
            pnlSetting.Controls.Add(lblBlur);
            pnlSetting.Controls.Add(trkBright);
            pnlSetting.Controls.Add(lblBright);
            pnlSetting.Controls.Add(trkProgress);
            pnlSetting.Controls.Add(btnStop);
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
            // flpModule
            // 
            flpModule.BackColor = Color.FromArgb(235, 238, 244);
            flpModule.Dock = DockStyle.Fill;
            flpModule.Location = new Point(0, 70);
            flpModule.Margin = new Padding(0);
            flpModule.Name = "flpModule";
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
        private Button btnStop;
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
        private Button btnFullGraph;
    }
}
