namespace DataManager.UserControls
{
    partial class TrainerUI
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
            pnlConfEditor = new Panel();
            lblConfigHelp = new Label();
            btnConfigHelp = new Button();
            label2 = new Label();
            pnlConfig = new Panel();
            flpConfCon = new FlowLayoutPanel();
            btnSaveMyConf = new Button();
            cboAddConfCount = new ComboBox();
            btnAddConf = new Button();
            lblAddConfSetter = new Label();
            lblConfEditor = new Label();
            pnlTrainer = new Panel();
            btnShowLog = new Button();
            btnModelTypeHelp = new Button();
            lblMinValLoss = new Label();
            lblValLoss = new Label();
            lblLoss = new Label();
            lblEpoch = new Label();
            lblSetModelName = new Label();
            txtModelName = new TextBox();
            grpSetTrainSetting = new GroupBox();
            rdoUseCPU = new RadioButton();
            rdoUseGPU = new RadioButton();
            lblTransferWarning = new Label();
            prgTrain = new ProgressBar();
            btnTrain = new Button();
            txtComment = new TextBox();
            lblComment = new Label();
            lblSelectTransferModel = new Label();
            cboSelectTransferModel = new ComboBox();
            cboSelectModelType = new ComboBox();
            lblSelectModelType = new Label();
            label1 = new Label();
            pnlViewerAndEditor = new Panel();
            pnlListView = new Panel();
            lvwModel = new ListView();
            colName = new ColumnHeader();
            colType = new ColumnHeader();
            colTubs = new ColumnHeader();
            colTime = new ColumnHeader();
            colTransfer = new ColumnHeader();
            colComment = new ColumnHeader();
            pnlLabel = new Panel();
            lblViewerAndEditor = new Label();
            tblButton = new TableLayoutPanel();
            btnDelete = new Button();
            btnRename = new Button();
            btnChgComment = new Button();
            btnShowConf = new Button();
            btnTrainningHistory = new Button();
            pnlField = new Panel();
            panel1 = new Panel();
            button2 = new Button();
            button1 = new Button();
            btnChart = new Button();
            pnlConfEditor.SuspendLayout();
            pnlConfig.SuspendLayout();
            pnlTrainer.SuspendLayout();
            grpSetTrainSetting.SuspendLayout();
            pnlViewerAndEditor.SuspendLayout();
            pnlListView.SuspendLayout();
            pnlLabel.SuspendLayout();
            tblButton.SuspendLayout();
            pnlField.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlConfEditor
            // 
            pnlConfEditor.BackColor = Color.FromArgb(250, 251, 253);
            pnlConfEditor.Controls.Add(lblConfigHelp);
            pnlConfEditor.Controls.Add(btnConfigHelp);
            pnlConfEditor.Controls.Add(label2);
            pnlConfEditor.Controls.Add(pnlConfig);
            pnlConfEditor.Controls.Add(btnSaveMyConf);
            pnlConfEditor.Controls.Add(cboAddConfCount);
            pnlConfEditor.Controls.Add(btnAddConf);
            pnlConfEditor.Controls.Add(lblAddConfSetter);
            pnlConfEditor.Controls.Add(lblConfEditor);
            pnlConfEditor.Location = new Point(663, 141);
            pnlConfEditor.Name = "pnlConfEditor";
            pnlConfEditor.Size = new Size(232, 158);
            pnlConfEditor.TabIndex = 0;
            // 
            // lblConfigHelp
            // 
            lblConfigHelp.AutoSize = true;
            lblConfigHelp.Font = new Font("맑은 고딕", 11F);
            lblConfigHelp.ForeColor = Color.FromArgb(60, 72, 92);
            lblConfigHelp.Location = new Point(596, 18);
            lblConfigHelp.Margin = new Padding(2, 0, 2, 0);
            lblConfigHelp.Name = "lblConfigHelp";
            lblConfigHelp.Size = new Size(144, 20);
            lblConfigHelp.TabIndex = 11;
            lblConfigHelp.Text = "각 설정에 대한 설명";
            // 
            // btnConfigHelp
            // 
            btnConfigHelp.Font = new Font("맑은 고딕", 15.8571434F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnConfigHelp.Location = new Point(745, 14);
            btnConfigHelp.Margin = new Padding(2);
            btnConfigHelp.Name = "btnConfigHelp";
            btnConfigHelp.Size = new Size(30, 27);
            btnConfigHelp.TabIndex = 10;
            btnConfigHelp.Text = "？";
            btnConfigHelp.UseVisualStyleBackColor = true;
            btnConfigHelp.Click += btnConfigHelp_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 11F);
            label2.ForeColor = Color.FromArgb(60, 72, 92);
            label2.Location = new Point(380, 18);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(144, 20);
            label2.TabIndex = 3;
            label2.Text = "한줄에 띄울 설정 수";
            // 
            // pnlConfig
            // 
            pnlConfig.BackColor = Color.FromArgb(250, 251, 253);
            pnlConfig.Controls.Add(flpConfCon);
            pnlConfig.Dock = DockStyle.Bottom;
            pnlConfig.Location = new Point(0, 50);
            pnlConfig.Name = "pnlConfig";
            pnlConfig.Padding = new Padding(10, 0, 10, 0);
            pnlConfig.Size = new Size(232, 108);
            pnlConfig.TabIndex = 9;
            // 
            // flpConfCon
            // 
            flpConfCon.AutoScroll = true;
            flpConfCon.BackColor = Color.FromArgb(250, 251, 253);
            flpConfCon.Dock = DockStyle.Fill;
            flpConfCon.FlowDirection = FlowDirection.TopDown;
            flpConfCon.Location = new Point(10, 0);
            flpConfCon.Margin = new Padding(2);
            flpConfCon.Name = "flpConfCon";
            flpConfCon.Size = new Size(212, 108);
            flpConfCon.TabIndex = 8;
            flpConfCon.WrapContents = false;
            // 
            // btnSaveMyConf
            // 
            btnSaveMyConf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveMyConf.BackColor = Color.FromArgb(72, 175, 120);
            btnSaveMyConf.Cursor = Cursors.Hand;
            btnSaveMyConf.FlatAppearance.BorderSize = 0;
            btnSaveMyConf.FlatStyle = FlatStyle.Flat;
            btnSaveMyConf.Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
            btnSaveMyConf.ForeColor = Color.White;
            btnSaveMyConf.Location = new Point(90, 3);
            btnSaveMyConf.Name = "btnSaveMyConf";
            btnSaveMyConf.Size = new Size(139, 39);
            btnSaveMyConf.TabIndex = 4;
            btnSaveMyConf.Text = "학습 설정 저장";
            btnSaveMyConf.UseVisualStyleBackColor = false;
            btnSaveMyConf.Click += btnSaveMyConf_Click;
            // 
            // cboAddConfCount
            // 
            cboAddConfCount.BackColor = Color.White;
            cboAddConfCount.FlatStyle = FlatStyle.Flat;
            cboAddConfCount.Font = new Font("맑은 고딕", 9.5F);
            cboAddConfCount.FormattingEnabled = true;
            cboAddConfCount.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cboAddConfCount.Location = new Point(533, 19);
            cboAddConfCount.Name = "cboAddConfCount";
            cboAddConfCount.Size = new Size(46, 25);
            cboAddConfCount.TabIndex = 3;
            cboAddConfCount.Text = "1";
            // 
            // btnAddConf
            // 
            btnAddConf.BackColor = Color.FromArgb(67, 130, 220);
            btnAddConf.Cursor = Cursors.Hand;
            btnAddConf.FlatAppearance.BorderSize = 0;
            btnAddConf.FlatStyle = FlatStyle.Flat;
            btnAddConf.Font = new Font("맑은 고딕", 11F);
            btnAddConf.ForeColor = Color.White;
            btnAddConf.Location = new Point(331, 11);
            btnAddConf.Name = "btnAddConf";
            btnAddConf.Size = new Size(27, 30);
            btnAddConf.TabIndex = 2;
            btnAddConf.Text = "+";
            btnAddConf.TextAlign = ContentAlignment.TopLeft;
            btnAddConf.UseVisualStyleBackColor = false;
            btnAddConf.Click += btnAddConf_Click;
            // 
            // lblAddConfSetter
            // 
            lblAddConfSetter.Font = new Font("맑은 고딕", 11F);
            lblAddConfSetter.ForeColor = Color.FromArgb(60, 72, 92);
            lblAddConfSetter.Location = new Point(211, 11);
            lblAddConfSetter.Name = "lblAddConfSetter";
            lblAddConfSetter.Size = new Size(114, 23);
            lblAddConfSetter.TabIndex = 1;
            lblAddConfSetter.Text = "학습 설정 추가";
            lblAddConfSetter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblConfEditor
            // 
            lblConfEditor.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblConfEditor.Location = new Point(3, 3);
            lblConfEditor.Margin = new Padding(3);
            lblConfEditor.Name = "lblConfEditor";
            lblConfEditor.Size = new Size(211, 39);
            lblConfEditor.TabIndex = 0;
            lblConfEditor.Text = "학습 상세 설정";
            lblConfEditor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlTrainer
            // 
            pnlTrainer.BackColor = Color.FromArgb(250, 251, 253);
            pnlTrainer.BorderStyle = BorderStyle.FixedSingle;
            pnlTrainer.Controls.Add(btnShowLog);
            pnlTrainer.Controls.Add(btnModelTypeHelp);
            pnlTrainer.Controls.Add(lblMinValLoss);
            pnlTrainer.Controls.Add(lblValLoss);
            pnlTrainer.Controls.Add(lblLoss);
            pnlTrainer.Controls.Add(lblEpoch);
            pnlTrainer.Controls.Add(lblSetModelName);
            pnlTrainer.Controls.Add(txtModelName);
            pnlTrainer.Controls.Add(grpSetTrainSetting);
            pnlTrainer.Controls.Add(lblTransferWarning);
            pnlTrainer.Controls.Add(prgTrain);
            pnlTrainer.Controls.Add(btnTrain);
            pnlTrainer.Controls.Add(txtComment);
            pnlTrainer.Controls.Add(lblComment);
            pnlTrainer.Controls.Add(lblSelectTransferModel);
            pnlTrainer.Controls.Add(cboSelectTransferModel);
            pnlTrainer.Controls.Add(cboSelectModelType);
            pnlTrainer.Controls.Add(lblSelectModelType);
            pnlTrainer.Controls.Add(label1);
            pnlTrainer.Dock = DockStyle.Top;
            pnlTrainer.Location = new Point(0, 0);
            pnlTrainer.Name = "pnlTrainer";
            pnlTrainer.Size = new Size(950, 206);
            pnlTrainer.TabIndex = 1;
            // 
            // btnShowLog
            // 
            btnShowLog.Location = new Point(417, 104);
            btnShowLog.Margin = new Padding(2);
            btnShowLog.Name = "btnShowLog";
            btnShowLog.Size = new Size(304, 20);
            btnShowLog.TabIndex = 17;
            btnShowLog.Text = "로그 보기";
            btnShowLog.UseVisualStyleBackColor = true;
            btnShowLog.Click += btnShowLog_Click;
            // 
            // btnModelTypeHelp
            // 
            btnModelTypeHelp.Font = new Font("맑은 고딕", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnModelTypeHelp.Location = new Point(390, 43);
            btnModelTypeHelp.Margin = new Padding(2);
            btnModelTypeHelp.Name = "btnModelTypeHelp";
            btnModelTypeHelp.Size = new Size(23, 20);
            btnModelTypeHelp.TabIndex = 12;
            btnModelTypeHelp.Text = "？";
            btnModelTypeHelp.UseVisualStyleBackColor = true;
            btnModelTypeHelp.Click += btnModelTypeHelp_Click;
            // 
            // lblMinValLoss
            // 
            lblMinValLoss.AutoSize = true;
            lblMinValLoss.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblMinValLoss.Location = new Point(729, 126);
            lblMinValLoss.Margin = new Padding(2, 0, 2, 0);
            lblMinValLoss.Name = "lblMinValLoss";
            lblMinValLoss.Size = new Size(190, 20);
            lblMinValLoss.TabIndex = 15;
            lblMinValLoss.Text = "최소 테스트 오차율: 0.0000";
            // 
            // lblValLoss
            // 
            lblValLoss.AutoSize = true;
            lblValLoss.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblValLoss.Location = new Point(729, 86);
            lblValLoss.Margin = new Padding(2, 0, 2, 0);
            lblValLoss.Name = "lblValLoss";
            lblValLoss.Size = new Size(155, 20);
            lblValLoss.TabIndex = 14;
            lblValLoss.Text = "테스트 오차율: 0.0000";
            // 
            // lblLoss
            // 
            lblLoss.AutoSize = true;
            lblLoss.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblLoss.Location = new Point(729, 46);
            lblLoss.Margin = new Padding(2, 0, 2, 0);
            lblLoss.Name = "lblLoss";
            lblLoss.Size = new Size(145, 20);
            lblLoss.TabIndex = 13;
            lblLoss.Text = "학습 오차율 : 0.0000";
            // 
            // lblEpoch
            // 
            lblEpoch.AutoSize = true;
            lblEpoch.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblEpoch.Location = new Point(729, 7);
            lblEpoch.Margin = new Padding(2, 0, 2, 0);
            lblEpoch.Name = "lblEpoch";
            lblEpoch.Size = new Size(104, 20);
            lblEpoch.TabIndex = 12;
            lblEpoch.Text = "반복횟수 : 0/0";
            // 
            // lblSetModelName
            // 
            lblSetModelName.Font = new Font("맑은 고딕", 9.5F);
            lblSetModelName.ForeColor = Color.FromArgb(60, 72, 92);
            lblSetModelName.Location = new Point(89, 12);
            lblSetModelName.Margin = new Padding(5, 0, 5, 0);
            lblSetModelName.Name = "lblSetModelName";
            lblSetModelName.Size = new Size(111, 25);
            lblSetModelName.TabIndex = 11;
            lblSetModelName.Text = "모델 이름 (선택)";
            lblSetModelName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtModelName
            // 
            txtModelName.Location = new Point(198, 16);
            txtModelName.Name = "txtModelName";
            txtModelName.Size = new Size(208, 23);
            txtModelName.TabIndex = 10;
            // 
            // grpSetTrainSetting
            // 
            grpSetTrainSetting.BackColor = Color.FromArgb(250, 251, 253);
            grpSetTrainSetting.Controls.Add(rdoUseCPU);
            grpSetTrainSetting.Controls.Add(rdoUseGPU);
            grpSetTrainSetting.Font = new Font("맑은 고딕", 9.5F);
            grpSetTrainSetting.ForeColor = Color.FromArgb(60, 72, 92);
            grpSetTrainSetting.Location = new Point(417, 60);
            grpSetTrainSetting.Margin = new Padding(2);
            grpSetTrainSetting.Name = "grpSetTrainSetting";
            grpSetTrainSetting.Padding = new Padding(2);
            grpSetTrainSetting.Size = new Size(304, 39);
            grpSetTrainSetting.TabIndex = 9;
            grpSetTrainSetting.TabStop = false;
            grpSetTrainSetting.Text = "학습 방법";
            // 
            // rdoUseCPU
            // 
            rdoUseCPU.AutoSize = true;
            rdoUseCPU.Font = new Font("맑은 고딕", 9.5F);
            rdoUseCPU.ForeColor = Color.FromArgb(60, 72, 92);
            rdoUseCPU.Location = new Point(165, 18);
            rdoUseCPU.Margin = new Padding(2);
            rdoUseCPU.Name = "rdoUseCPU";
            rdoUseCPU.Size = new Size(94, 21);
            rdoUseCPU.TabIndex = 1;
            rdoUseCPU.TabStop = true;
            rdoUseCPU.Text = "CPU만 사용";
            rdoUseCPU.UseVisualStyleBackColor = true;
            // 
            // rdoUseGPU
            // 
            rdoUseGPU.AutoSize = true;
            rdoUseGPU.Font = new Font("맑은 고딕", 9.5F);
            rdoUseGPU.ForeColor = Color.FromArgb(60, 72, 92);
            rdoUseGPU.Location = new Point(28, 19);
            rdoUseGPU.Margin = new Padding(2);
            rdoUseGPU.Name = "rdoUseGPU";
            rdoUseGPU.Size = new Size(82, 21);
            rdoUseGPU.TabIndex = 0;
            rdoUseGPU.TabStop = true;
            rdoUseGPU.Text = "GPU 사용";
            rdoUseGPU.UseVisualStyleBackColor = true;
            // 
            // lblTransferWarning
            // 
            lblTransferWarning.AutoSize = true;
            lblTransferWarning.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTransferWarning.ForeColor = Color.FromArgb(210, 70, 70);
            lblTransferWarning.Location = new Point(10, 110);
            lblTransferWarning.Margin = new Padding(2, 0, 2, 0);
            lblTransferWarning.Name = "lblTransferWarning";
            lblTransferWarning.Size = new Size(394, 15);
            lblTransferWarning.TabIndex = 8;
            lblTransferWarning.Text = "전이학습을 하는 동안 모델 종류를 바꿀 수 없습니다. (바꿀시 오류 발생)";
            lblTransferWarning.Visible = false;
            // 
            // prgTrain
            // 
            prgTrain.BackColor = Color.FromArgb(220, 228, 240);
            prgTrain.ForeColor = Color.FromArgb(72, 175, 120);
            prgTrain.Location = new Point(126, 128);
            prgTrain.Name = "prgTrain";
            prgTrain.Size = new Size(598, 25);
            prgTrain.TabIndex = 7;
            // 
            // btnTrain
            // 
            btnTrain.BackColor = Color.FromArgb(67, 130, 220);
            btnTrain.Cursor = Cursors.Hand;
            btnTrain.FlatAppearance.BorderSize = 0;
            btnTrain.FlatStyle = FlatStyle.Flat;
            btnTrain.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnTrain.ForeColor = Color.White;
            btnTrain.Location = new Point(3, 128);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(117, 25);
            btnTrain.TabIndex = 6;
            btnTrain.Text = "▶ 훈련 시작";
            btnTrain.UseVisualStyleBackColor = false;
            btnTrain.Click += btnTrain_Click;
            // 
            // txtComment
            // 
            txtComment.BackColor = Color.White;
            txtComment.BorderStyle = BorderStyle.FixedSingle;
            txtComment.Font = new Font("맑은 고딕", 9.5F);
            txtComment.Location = new Point(488, 12);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(234, 44);
            txtComment.TabIndex = 0;
            // 
            // lblComment
            // 
            lblComment.Font = new Font("맑은 고딕", 9.5F);
            lblComment.ForeColor = Color.FromArgb(60, 72, 92);
            lblComment.Location = new Point(414, 28);
            lblComment.Name = "lblComment";
            lblComment.Size = new Size(69, 18);
            lblComment.TabIndex = 5;
            lblComment.Text = "모델 메모";
            lblComment.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSelectTransferModel
            // 
            lblSelectTransferModel.Font = new Font("맑은 고딕", 9.5F);
            lblSelectTransferModel.ForeColor = Color.FromArgb(60, 72, 92);
            lblSelectTransferModel.Location = new Point(80, 76);
            lblSelectTransferModel.Name = "lblSelectTransferModel";
            lblSelectTransferModel.Size = new Size(128, 23);
            lblSelectTransferModel.TabIndex = 4;
            lblSelectTransferModel.Text = "전이 학습 모델 선택";
            lblSelectTransferModel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cboSelectTransferModel
            // 
            cboSelectTransferModel.BackColor = Color.White;
            cboSelectTransferModel.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelectTransferModel.FlatStyle = FlatStyle.Flat;
            cboSelectTransferModel.Font = new Font("맑은 고딕", 9.5F);
            cboSelectTransferModel.FormattingEnabled = true;
            cboSelectTransferModel.Location = new Point(219, 78);
            cboSelectTransferModel.Name = "cboSelectTransferModel";
            cboSelectTransferModel.Size = new Size(187, 25);
            cboSelectTransferModel.TabIndex = 3;
            cboSelectTransferModel.SelectedIndexChanged += cboSelectTransferModel_SelectedIndexChanged;
            // 
            // cboSelectModelType
            // 
            cboSelectModelType.BackColor = Color.White;
            cboSelectModelType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelectModelType.FlatStyle = FlatStyle.Flat;
            cboSelectModelType.Font = new Font("맑은 고딕", 9.5F);
            cboSelectModelType.FormattingEnabled = true;
            cboSelectModelType.Location = new Point(193, 44);
            cboSelectModelType.Name = "cboSelectModelType";
            cboSelectModelType.Size = new Size(194, 25);
            cboSelectModelType.TabIndex = 2;
            // 
            // lblSelectModelType
            // 
            lblSelectModelType.Font = new Font("맑은 고딕", 9.5F);
            lblSelectModelType.ForeColor = Color.FromArgb(60, 72, 92);
            lblSelectModelType.Location = new Point(89, 42);
            lblSelectModelType.Name = "lblSelectModelType";
            lblSelectModelType.Size = new Size(98, 23);
            lblSelectModelType.TabIndex = 1;
            lblSelectModelType.Text = "모델 종류 선택";
            lblSelectModelType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            label1.Location = new Point(-1, 0);
            label1.Margin = new Padding(3);
            label1.Name = "label1";
            label1.Size = new Size(98, 39);
            label1.TabIndex = 0;
            label1.Text = "학습기";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlViewerAndEditor
            // 
            pnlViewerAndEditor.BackColor = Color.White;
            pnlViewerAndEditor.Controls.Add(pnlListView);
            pnlViewerAndEditor.Controls.Add(pnlLabel);
            pnlViewerAndEditor.Controls.Add(tblButton);
            pnlViewerAndEditor.Location = new Point(279, 144);
            pnlViewerAndEditor.Name = "pnlViewerAndEditor";
            pnlViewerAndEditor.Size = new Size(249, 163);
            pnlViewerAndEditor.TabIndex = 2;
            // 
            // pnlListView
            // 
            pnlListView.BackColor = Color.White;
            pnlListView.Controls.Add(lvwModel);
            pnlListView.Dock = DockStyle.Fill;
            pnlListView.Location = new Point(0, 53);
            pnlListView.Name = "pnlListView";
            pnlListView.Padding = new Padding(10);
            pnlListView.Size = new Size(249, 70);
            pnlListView.TabIndex = 12;
            // 
            // lvwModel
            // 
            lvwModel.BackColor = Color.White;
            lvwModel.BorderStyle = BorderStyle.None;
            lvwModel.Columns.AddRange(new ColumnHeader[] { colName, colType, colTubs, colTime, colTransfer, colComment });
            lvwModel.Dock = DockStyle.Fill;
            lvwModel.Font = new Font("맑은 고딕", 9.5F);
            lvwModel.ForeColor = Color.FromArgb(60, 72, 92);
            lvwModel.FullRowSelect = true;
            lvwModel.GridLines = true;
            lvwModel.Location = new Point(10, 10);
            lvwModel.Name = "lvwModel";
            lvwModel.Size = new Size(229, 50);
            lvwModel.TabIndex = 0;
            lvwModel.UseCompatibleStateImageBehavior = false;
            lvwModel.View = View.Details;
            lvwModel.SelectedIndexChanged += lvwModel_SelectedIndexChanged;
            lvwModel.MouseDown += lvwModel_MouseDown;
            lvwModel.MouseMove += lvwModel_MouseMove;
            lvwModel.MouseUp += lvwModel_MouseUp;
            // 
            // colName
            // 
            colName.Text = "모델 이름";
            colName.Width = 240;
            // 
            // colType
            // 
            colType.Text = "모델 종류";
            colType.Width = 200;
            // 
            // colTubs
            // 
            colTubs.Text = "사용한 데이터셋";
            colTubs.Width = 200;
            // 
            // colTime
            // 
            colTime.Text = "학습 시간";
            colTime.Width = 180;
            // 
            // colTransfer
            // 
            colTransfer.Text = "전이학습 여부";
            colTransfer.Width = 180;
            // 
            // colComment
            // 
            colComment.Text = "메모";
            colComment.Width = 240;
            // 
            // pnlLabel
            // 
            pnlLabel.BackColor = Color.FromArgb(250, 251, 253);
            pnlLabel.Controls.Add(lblViewerAndEditor);
            pnlLabel.Dock = DockStyle.Top;
            pnlLabel.Location = new Point(0, 0);
            pnlLabel.Margin = new Padding(0);
            pnlLabel.Name = "pnlLabel";
            pnlLabel.Size = new Size(249, 53);
            pnlLabel.TabIndex = 11;
            // 
            // lblViewerAndEditor
            // 
            lblViewerAndEditor.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblViewerAndEditor.Location = new Point(0, 8);
            lblViewerAndEditor.Name = "lblViewerAndEditor";
            lblViewerAndEditor.Size = new Size(287, 38);
            lblViewerAndEditor.TabIndex = 8;
            lblViewerAndEditor.Text = "파일럿 뷰어 및 편집기";
            lblViewerAndEditor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tblButton
            // 
            tblButton.BackColor = Color.FromArgb(250, 251, 253);
            tblButton.ColumnCount = 5;
            tblButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblButton.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblButton.Controls.Add(btnDelete, 0, 0);
            tblButton.Controls.Add(btnRename, 1, 0);
            tblButton.Controls.Add(btnChgComment, 2, 0);
            tblButton.Controls.Add(btnShowConf, 3, 0);
            tblButton.Controls.Add(btnTrainningHistory, 4, 0);
            tblButton.Dock = DockStyle.Bottom;
            tblButton.Location = new Point(0, 123);
            tblButton.Name = "tblButton";
            tblButton.Padding = new Padding(5);
            tblButton.RowCount = 1;
            tblButton.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblButton.Size = new Size(249, 40);
            tblButton.TabIndex = 9;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(210, 70, 70);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Dock = DockStyle.Fill;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(10, 5);
            btnDelete.Margin = new Padding(5, 0, 5, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(37, 30);
            btnDelete.TabIndex = 0;
            btnDelete.Text = "삭제 (Del)";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRename
            // 
            btnRename.BackColor = Color.FromArgb(72, 175, 120);
            btnRename.Cursor = Cursors.Hand;
            btnRename.Dock = DockStyle.Fill;
            btnRename.FlatStyle = FlatStyle.Flat;
            btnRename.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnRename.ForeColor = Color.White;
            btnRename.Location = new Point(57, 5);
            btnRename.Margin = new Padding(5, 0, 5, 0);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(37, 30);
            btnRename.TabIndex = 4;
            btnRename.Text = "이름 변경 (Ctrl + 1)";
            btnRename.UseVisualStyleBackColor = false;
            btnRename.Click += btnRename_Click;
            // 
            // btnChgComment
            // 
            btnChgComment.BackColor = Color.FromArgb(210, 140, 40);
            btnChgComment.Cursor = Cursors.Hand;
            btnChgComment.Dock = DockStyle.Fill;
            btnChgComment.FlatAppearance.BorderSize = 0;
            btnChgComment.FlatStyle = FlatStyle.Flat;
            btnChgComment.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnChgComment.ForeColor = Color.White;
            btnChgComment.Location = new Point(104, 5);
            btnChgComment.Margin = new Padding(5, 0, 5, 0);
            btnChgComment.Name = "btnChgComment";
            btnChgComment.Size = new Size(37, 30);
            btnChgComment.TabIndex = 1;
            btnChgComment.Text = "메모 변경 (Ctrl + 2)";
            btnChgComment.UseVisualStyleBackColor = false;
            btnChgComment.Click += btnChgComment_Click;
            // 
            // btnShowConf
            // 
            btnShowConf.BackColor = Color.FromArgb(67, 130, 220);
            btnShowConf.Cursor = Cursors.Hand;
            btnShowConf.Dock = DockStyle.Fill;
            btnShowConf.FlatAppearance.BorderSize = 0;
            btnShowConf.FlatStyle = FlatStyle.Flat;
            btnShowConf.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnShowConf.ForeColor = Color.White;
            btnShowConf.Location = new Point(151, 5);
            btnShowConf.Margin = new Padding(5, 0, 5, 0);
            btnShowConf.Name = "btnShowConf";
            btnShowConf.Size = new Size(37, 30);
            btnShowConf.TabIndex = 2;
            btnShowConf.Text = "구성 표시 (Ctrl + 3)";
            btnShowConf.UseVisualStyleBackColor = false;
            btnShowConf.Click += btnShowConf_Click;
            // 
            // btnTrainningHistory
            // 
            btnTrainningHistory.BackColor = Color.FromArgb(130, 90, 200);
            btnTrainningHistory.Cursor = Cursors.Hand;
            btnTrainningHistory.Dock = DockStyle.Fill;
            btnTrainningHistory.FlatAppearance.BorderSize = 0;
            btnTrainningHistory.FlatStyle = FlatStyle.Flat;
            btnTrainningHistory.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            btnTrainningHistory.ForeColor = Color.White;
            btnTrainningHistory.Location = new Point(198, 5);
            btnTrainningHistory.Margin = new Padding(5, 0, 5, 0);
            btnTrainningHistory.Name = "btnTrainningHistory";
            btnTrainningHistory.Size = new Size(41, 30);
            btnTrainningHistory.TabIndex = 3;
            btnTrainningHistory.Text = "훈련 기록 (Ctrl + 4)";
            btnTrainningHistory.UseVisualStyleBackColor = false;
            btnTrainningHistory.Click += btnTrainningHistory_Click;
            // 
            // pnlField
            // 
            pnlField.Controls.Add(pnlViewerAndEditor);
            pnlField.Controls.Add(pnlConfEditor);
            pnlField.Dock = DockStyle.Fill;
            pnlField.Location = new Point(0, 206);
            pnlField.Name = "pnlField";
            pnlField.Size = new Size(950, 560);
            pnlField.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnChart);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 726);
            panel1.Name = "panel1";
            panel1.Size = new Size(950, 40);
            panel1.TabIndex = 4;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(100, 110, 130);
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            button2.ForeColor = Color.White;
            button2.Location = new Point(320, 0);
            button2.Margin = new Padding(0);
            button2.Name = "button2";
            button2.Size = new Size(160, 40);
            button2.TabIndex = 6;
            button2.Text = "파일럿 뷰어 및 편집기";
            button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(100, 110, 130);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(160, 0);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(160, 40);
            button1.TabIndex = 5;
            button1.Text = "학습 상세 설정";
            button1.UseVisualStyleBackColor = false;
            // 
            // btnChart
            // 
            btnChart.BackColor = Color.FromArgb(67, 130, 220);
            btnChart.Cursor = Cursors.Hand;
            btnChart.FlatAppearance.BorderSize = 0;
            btnChart.FlatStyle = FlatStyle.Flat;
            btnChart.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnChart.ForeColor = Color.White;
            btnChart.Location = new Point(0, 0);
            btnChart.Margin = new Padding(0);
            btnChart.Name = "btnChart";
            btnChart.Size = new Size(160, 40);
            btnChart.TabIndex = 4;
            btnChart.Text = "학습 그래프";
            btnChart.UseVisualStyleBackColor = false;
            // 
            // TrainerUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(panel1);
            Controls.Add(pnlField);
            Controls.Add(pnlTrainer);
            Name = "TrainerUI";
            Size = new Size(950, 766);
            Load += TrainerUI_Load;
            pnlConfEditor.ResumeLayout(false);
            pnlConfEditor.PerformLayout();
            pnlConfig.ResumeLayout(false);
            pnlTrainer.ResumeLayout(false);
            pnlTrainer.PerformLayout();
            grpSetTrainSetting.ResumeLayout(false);
            grpSetTrainSetting.PerformLayout();
            pnlViewerAndEditor.ResumeLayout(false);
            pnlListView.ResumeLayout(false);
            pnlLabel.ResumeLayout(false);
            tblButton.ResumeLayout(false);
            pnlField.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlConfEditor;
        private Panel pnlConfig;
        private FlowLayoutPanel flpConfCon;
        private Button btnSaveMyConf;
        private ComboBox cboAddConfCount;
        private Button btnAddConf;
        private Label lblAddConfSetter;
        private Label label2;
        private Label lblConfEditor;
        private Panel pnlTrainer;
        private GroupBox grpSetTrainSetting;
        private RadioButton rdoUseCPU;
        private RadioButton rdoUseGPU;
        private Label lblTransferWarning;
        private ProgressBar prgTrain;
        private Button btnTrain;
        private TextBox txtComment;
        private Label lblComment;
        private Label lblSelectTransferModel;
        private ComboBox cboSelectTransferModel;
        private ComboBox cboSelectModelType;
        private Label lblSelectModelType;
        private Label label1;
        private Panel pnlViewerAndEditor;
        private Panel pnlListView;
        private ListView lvwModel;
        private ColumnHeader colName;
        private ColumnHeader colType;
        private ColumnHeader colTubs;
        private ColumnHeader colTime;
        private ColumnHeader colTransfer;
        private ColumnHeader colComment;
        private Panel pnlLabel;
        private Label lblViewerAndEditor;
        private TableLayoutPanel tblButton;
        private Button btnTrainningHistory;
        private Button btnShowConf;
        private Button btnChgComment;
        private Button btnDelete;
        private Label lblSetModelName;
        private TextBox txtModelName;
        private Button btnRename;
        private Label lblLoss;
        private Label lblEpoch;
        private Label lblValLoss;
        private Label lblMinValLoss;
        private Button btnConfigHelp;
        private Label lblConfigHelp;
        private Button btnModelTypeHelp;
        private Button btnShowLog;
        private Panel pnlField;
        private Panel panel1;
        private Button button2;
        private Button button1;
        private Button btnChart;
    }
}