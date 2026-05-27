namespace DataManager
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnChgTubForm = new Button();
            btnChgTrainerForm = new Button();
            btnChgPilotForm = new Button();
            pnlChgForm = new Panel();
            pnlLog = new Panel();
            lvwLogBox = new ListView();
            시간 = new ColumnHeader();
            타입 = new ColumnHeader();
            메시지 = new ColumnHeader();
            pnlMain = new Panel();
            pnlChgForm.SuspendLayout();
            pnlLog.SuspendLayout();
            SuspendLayout();
            // 
            // btnChgTubForm
            // 
            btnChgTubForm.BackColor = Color.FromArgb(67, 130, 220);
            btnChgTubForm.Cursor = Cursors.Hand;
            btnChgTubForm.FlatAppearance.BorderSize = 0;
            btnChgTubForm.FlatStyle = FlatStyle.Flat;
            btnChgTubForm.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnChgTubForm.ForeColor = Color.White;
            btnChgTubForm.Location = new Point(0, 0);
            btnChgTubForm.Margin = new Padding(0);
            btnChgTubForm.Name = "btnChgTubForm";
            btnChgTubForm.Size = new Size(160, 40);
            btnChgTubForm.TabIndex = 0;
            btnChgTubForm.Text = "이미지 편집";
            btnChgTubForm.UseVisualStyleBackColor = false;
            btnChgTubForm.Click += btnChgTubForm_Click;
            // 
            // btnChgTrainerForm
            // 
            btnChgTrainerForm.BackColor = Color.FromArgb(100, 110, 130);
            btnChgTrainerForm.Cursor = Cursors.Hand;
            btnChgTrainerForm.FlatAppearance.BorderSize = 0;
            btnChgTrainerForm.FlatStyle = FlatStyle.Flat;
            btnChgTrainerForm.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnChgTrainerForm.ForeColor = Color.White;
            btnChgTrainerForm.Location = new Point(160, 0);
            btnChgTrainerForm.Margin = new Padding(0);
            btnChgTrainerForm.Name = "btnChgTrainerForm";
            btnChgTrainerForm.Size = new Size(160, 40);
            btnChgTrainerForm.TabIndex = 1;
            btnChgTrainerForm.Text = "모델 훈련";
            btnChgTrainerForm.UseVisualStyleBackColor = false;
            btnChgTrainerForm.Click += btnChgTrainerForm_Click;
            // 
            // btnChgPilotForm
            // 
            btnChgPilotForm.BackColor = Color.FromArgb(100, 110, 130);
            btnChgPilotForm.Cursor = Cursors.Hand;
            btnChgPilotForm.FlatAppearance.BorderSize = 0;
            btnChgPilotForm.FlatStyle = FlatStyle.Flat;
            btnChgPilotForm.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnChgPilotForm.ForeColor = Color.White;
            btnChgPilotForm.Location = new Point(320, 0);
            btnChgPilotForm.Margin = new Padding(0);
            btnChgPilotForm.Name = "btnChgPilotForm";
            btnChgPilotForm.Size = new Size(160, 40);
            btnChgPilotForm.TabIndex = 2;
            btnChgPilotForm.Text = "모델 테스트";
            btnChgPilotForm.UseVisualStyleBackColor = false;
            btnChgPilotForm.Click += btnChgPilotForm_Click;
            // 
            // pnlChgForm
            // 
            pnlChgForm.BackColor = Color.FromArgb(40, 50, 70);
            pnlChgForm.Controls.Add(btnChgTrainerForm);
            pnlChgForm.Controls.Add(btnChgTubForm);
            pnlChgForm.Controls.Add(btnChgPilotForm);
            pnlChgForm.Dock = DockStyle.Top;
            pnlChgForm.Location = new Point(0, 0);
            pnlChgForm.Margin = new Padding(0);
            pnlChgForm.Name = "pnlChgForm";
            pnlChgForm.Size = new Size(950, 40);
            pnlChgForm.TabIndex = 4;
            // 
            // pnlLog
            // 
            pnlLog.BackColor = Color.FromArgb(245, 247, 250);
            pnlLog.Controls.Add(lvwLogBox);
            pnlLog.Dock = DockStyle.Bottom;
            pnlLog.Location = new Point(0, 806);
            pnlLog.Margin = new Padding(0);
            pnlLog.Name = "pnlLog";
            pnlLog.Size = new Size(950, 130);
            pnlLog.TabIndex = 6;
            // 
            // lvwLogBox
            // 
            lvwLogBox.BackColor = Color.FromArgb(245, 247, 250);
            lvwLogBox.BorderStyle = BorderStyle.None;
            lvwLogBox.Columns.AddRange(new ColumnHeader[] { 시간, 타입, 메시지 });
            lvwLogBox.Dock = DockStyle.Fill;
            lvwLogBox.Font = new Font("맑은 고딕", 9.5F);
            lvwLogBox.ForeColor = Color.FromArgb(55, 68, 90);
            lvwLogBox.FullRowSelect = true;
            lvwLogBox.GridLines = true;
            lvwLogBox.Location = new Point(0, 0);
            lvwLogBox.Name = "lvwLogBox";
            lvwLogBox.Size = new Size(950, 130);
            lvwLogBox.TabIndex = 0;
            lvwLogBox.UseCompatibleStateImageBehavior = false;
            lvwLogBox.View = View.Details;
            // 
            // 시간
            // 
            시간.Text = "시간";
            시간.Width = 120;
            // 
            // 타입
            // 
            타입.Text = "타입";
            타입.Width = 100;
            // 
            // 메시지
            // 
            메시지.Text = "메시지";
            메시지.Width = 700;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(245, 247, 250);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 40);
            pnlMain.Margin = new Padding(0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(950, 766);
            pnlMain.TabIndex = 7;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(950, 936);
            Controls.Add(pnlMain);
            Controls.Add(pnlLog);
            Controls.Add(pnlChgForm);
            Name = "MainForm";
            Text = "Donkey UI";
            pnlChgForm.ResumeLayout(false);
            pnlLog.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button btnChgTubForm;
        private Button btnChgTrainerForm;
        private Button btnChgPilotForm;
        private Panel pnlChgForm;
        private Panel pnlLog;
        private Panel pnlMain;
        private ListView lvwLogBox;
        private ColumnHeader 시간;
        private ColumnHeader 타입;
        private ColumnHeader 메시지;
    }
}