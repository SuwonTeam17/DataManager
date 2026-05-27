namespace DataManager.UserControls
{
    partial class PilotArenaUI
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
            lblPilotTest = new Label();
            btnStop = new Button();
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
            lblPilotTest.Location = new Point(0, 0);
            lblPilotTest.Margin = new Padding(3);
            lblPilotTest.Name = "lblPilotTest";
            lblPilotTest.Size = new Size(161, 39);
            lblPilotTest.TabIndex = 0;
            lblPilotTest.Text = "모델 테스트";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(693, 3);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(52, 32);
            btnStop.TabIndex = 12;
            btnStop.Text = "중지";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(642, 3);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(48, 32);
            btnPlay.TabIndex = 11;
            btnPlay.Text = "재생";
            btnPlay.UseVisualStyleBackColor = true;
            // 
            // btn5FrameRight
            // 
            btn5FrameRight.Location = new Point(532, 3);
            btn5FrameRight.Name = "btn5FrameRight";
            btn5FrameRight.Size = new Size(52, 32);
            btn5FrameRight.TabIndex = 10;
            btn5FrameRight.Text = ">>>";
            btn5FrameRight.UseVisualStyleBackColor = true;
            // 
            // btn5FrameLeft
            // 
            btn5FrameLeft.Location = new Point(481, 3);
            btn5FrameLeft.Name = "btn5FrameLeft";
            btn5FrameLeft.Size = new Size(48, 32);
            btn5FrameLeft.TabIndex = 9;
            btn5FrameLeft.Text = "<<<";
            btn5FrameLeft.UseVisualStyleBackColor = true;
            // 
            // btnFrameRight
            // 
            btnFrameRight.Location = new Point(587, 3);
            btnFrameRight.Name = "btnFrameRight";
            btnFrameRight.Size = new Size(52, 32);
            btnFrameRight.TabIndex = 8;
            btnFrameRight.Text = ">";
            btnFrameRight.UseVisualStyleBackColor = true;
            // 
            // btnFrameLeft
            // 
            btnFrameLeft.Location = new Point(430, 3);
            btnFrameLeft.Name = "btnFrameLeft";
            btnFrameLeft.Size = new Size(48, 32);
            btnFrameLeft.TabIndex = 7;
            btnFrameLeft.Text = "<";
            btnFrameLeft.UseVisualStyleBackColor = true;
            // 
            // pnlSetting
            // 
            pnlSetting.BorderStyle = BorderStyle.FixedSingle;
            pnlSetting.Controls.Add(btnLoadTub);
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
            pnlSetting.Size = new Size(750, 63);
            pnlSetting.TabIndex = 5;
            // 
            // btnLoadTub
            // 
            btnLoadTub.Location = new Point(81, 38);
            btnLoadTub.Name = "btnLoadTub";
            btnLoadTub.Size = new Size(68, 23);
            btnLoadTub.TabIndex = 21;
            btnLoadTub.Text = "Tub 열기";
            btnLoadTub.UseVisualStyleBackColor = true;
            btnLoadTub.Click += btnLoadTub_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "0.25", "0.50", "1.00", "2.00", "3.00", "5.00", "10.00" });
            comboBox1.Location = new Point(480, 36);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(50, 23);
            comboBox1.TabIndex = 19;
            comboBox1.Text = "1.00";
            // 
            // btnModelAdd
            // 
            btnModelAdd.Location = new Point(3, 38);
            btnModelAdd.Name = "btnModelAdd";
            btnModelAdd.Size = new Size(73, 23);
            btnModelAdd.TabIndex = 18;
            btnModelAdd.Text = "모델 추가";
            btnModelAdd.UseVisualStyleBackColor = true;
            btnModelAdd.Click += btnModelAdd_Click;
            // 
            // trkBlur
            // 
            trkBlur.AutoSize = false;
            trkBlur.LargeChange = 1;
            trkBlur.Location = new Point(228, 11);
            trkBlur.Name = "trkBlur";
            trkBlur.Size = new Size(184, 21);
            trkBlur.TabIndex = 17;
            trkBlur.TickStyle = TickStyle.None;
            // 
            // lblBlur
            // 
            lblBlur.Location = new Point(158, 11);
            lblBlur.Name = "lblBlur";
            lblBlur.Size = new Size(71, 22);
            lblBlur.TabIndex = 16;
            lblBlur.Text = "흐림 : 0.00";
            lblBlur.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trkBright
            // 
            trkBright.AutoSize = false;
            trkBright.LargeChange = 1;
            trkBright.Location = new Point(228, 38);
            trkBright.Name = "trkBright";
            trkBright.Size = new Size(184, 21);
            trkBright.TabIndex = 15;
            trkBright.TickStyle = TickStyle.None;
            // 
            // lblBright
            // 
            lblBright.Location = new Point(158, 38);
            lblBright.Name = "lblBright";
            lblBright.Size = new Size(71, 22);
            lblBright.TabIndex = 14;
            lblBright.Text = "밝기 : 0.00";
            lblBright.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trkProgress
            // 
            trkProgress.AutoSize = false;
            trkProgress.LargeChange = 1;
            trkProgress.Location = new Point(532, 37);
            trkProgress.Name = "trkProgress";
            trkProgress.Size = new Size(213, 21);
            trkProgress.TabIndex = 13;
            trkProgress.TickStyle = TickStyle.None;
            // 
            // lblSpeed
            // 
            lblSpeed.AutoSize = true;
            lblSpeed.Location = new Point(442, 39);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(31, 15);
            lblSpeed.TabIndex = 20;
            lblSpeed.Text = "배속";
            lblSpeed.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flpModule
            // 
            flpModule.Dock = DockStyle.Fill;
            flpModule.Location = new Point(0, 63);
            flpModule.Margin = new Padding(0);
            flpModule.Name = "flpModule";
            flpModule.Size = new Size(750, 537);
            flpModule.TabIndex = 6;
            // 
            // PilotArenaUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flpModule);
            Controls.Add(pnlSetting);
            Name = "PilotArenaUI";
            Size = new Size(750, 600);
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
        }
    }
