namespace DonkeyUI
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
            btnChgTubForm.Location = new Point(100, 0);
            btnChgTubForm.Margin = new Padding(0);
            btnChgTubForm.Name = "btnChgTubForm";
            btnChgTubForm.Size = new Size(100, 25);
            btnChgTubForm.TabIndex = 0;
            btnChgTubForm.Text = "Tub Manager";
            btnChgTubForm.UseVisualStyleBackColor = true;
            btnChgTubForm.Click += btnChgTubForm_Click;
            // 
            // btnChgTrainerForm
            // 
            btnChgTrainerForm.Location = new Point(200, 0);
            btnChgTrainerForm.Margin = new Padding(0);
            btnChgTrainerForm.Name = "btnChgTrainerForm";
            btnChgTrainerForm.Size = new Size(100, 25);
            btnChgTrainerForm.TabIndex = 1;
            btnChgTrainerForm.Text = "Trainer";
            btnChgTrainerForm.UseVisualStyleBackColor = true;
            btnChgTrainerForm.Click += btnChgTrainerForm_Click;
            // 
            // btnChgPilotForm
            // 
            btnChgPilotForm.Location = new Point(300, 0);
            btnChgPilotForm.Margin = new Padding(0);
            btnChgPilotForm.Name = "btnChgPilotForm";
            btnChgPilotForm.Size = new Size(100, 25);
            btnChgPilotForm.TabIndex = 2;
            btnChgPilotForm.Text = "Pilot Arena";
            btnChgPilotForm.UseVisualStyleBackColor = true;
            btnChgPilotForm.Click += btnChgPilotForm_Click;
            // 
            // pnlChgForm
            // 
            pnlChgForm.Controls.Add(btnChgTrainerForm);
            pnlChgForm.Controls.Add(btnChgTubForm);
            pnlChgForm.Controls.Add(btnChgPilotForm);
            pnlChgForm.Dock = DockStyle.Top;
            pnlChgForm.Location = new Point(0, 0);
            pnlChgForm.Margin = new Padding(0);
            pnlChgForm.Name = "pnlChgForm";
            pnlChgForm.Size = new Size(750, 25);
            pnlChgForm.TabIndex = 4;
            // 
            // pnlLog
            // 
            pnlLog.Controls.Add(lvwLogBox);
            pnlLog.Dock = DockStyle.Bottom;
            pnlLog.Location = new Point(0, 545);
            pnlLog.Margin = new Padding(0);
            pnlLog.Name = "pnlLog";
            pnlLog.Size = new Size(750, 80);
            pnlLog.TabIndex = 6;
            // 
            // lvwLogBox
            // 
            lvwLogBox.Columns.AddRange(new ColumnHeader[] { 시간, 타입, 메시지 });
            lvwLogBox.Dock = DockStyle.Fill;
            lvwLogBox.FullRowSelect = true;
            lvwLogBox.GridLines = true;
            lvwLogBox.Location = new Point(0, 0);
            lvwLogBox.Name = "lvwLogBox";
            lvwLogBox.Size = new Size(750, 80);
            lvwLogBox.TabIndex = 0;
            lvwLogBox.UseCompatibleStateImageBehavior = false;
            lvwLogBox.View = View.Details;
            // 
            // 시간
            // 
            시간.Text = "시간";
            시간.Width = 100;
            // 
            // 타입
            // 
            타입.Text = "타입";
            // 
            // 메시지
            // 
            메시지.Text = "메시지";
            메시지.Width = 580;
            // 
            // pnlMain
            // 
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 25);
            pnlMain.Margin = new Padding(0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(750, 520);
            pnlMain.TabIndex = 7;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(750, 625);
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
