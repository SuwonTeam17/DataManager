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
            pnlConfig = new Panel();
            flpConfCon = new FlowLayoutPanel();
            panel2 = new Panel();
            lblConfEditor = new Label();
            btnConfigHelp = new Button();
            btnSaveMyConf = new Button();
            lblConfigHelp = new Label();
            lblAddConfSetter = new Label();
            btnAddConf = new Button();
            label2 = new Label();
            cboAddConfCount = new ComboBox();
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
            tblButton = new TableLayoutPanel();
            btnDelete = new Button();
            btnRename = new Button();
            btnChgComment = new Button();
            btnShowConf = new Button();
            btnTrainningHistory = new Button();
            pnlLabel = new Panel();
            lblViewerAndEditor = new Label();
            pnlField = new Panel();
            pnlGraph = new Panel();
            pnlTrainServiceTab = new Panel();
            btnViewerAndEditorTab = new Button();
            btnConfigEditorTab = new Button();
            btnChartTab = new Button();
            pnlConfEditor.SuspendLayout();
            pnlConfig.SuspendLayout();
            panel2.SuspendLayout();
            pnlTrainer.SuspendLayout();
            grpSetTrainSetting.SuspendLayout();
            pnlViewerAndEditor.SuspendLayout();
            pnlListView.SuspendLayout();
            tblButton.SuspendLayout();
            pnlLabel.SuspendLayout();
            pnlField.SuspendLayout();
            pnlTrainServiceTab.SuspendLayout();
            SuspendLayout();
            // 
            // pnlConfEditor
            // 
            pnlConfEditor.BackColor = Color.FromArgb(250, 251, 253);
            pnlConfEditor.Controls.Add(pnlConfig);
            pnlConfEditor.Controls.Add(panel2);
            pnlConfEditor.Location = new Point(34, 6);
            pnlConfEditor.Margin = new Padding(5, 6, 5, 6);
            pnlConfEditor.Name = "pnlConfEditor";
            pnlConfEditor.Size = new Size(1590, 316);
            pnlConfEditor.TabIndex = 0;
            // 
            // pnlConfig
            // 
            pnlConfig.BackColor = Color.FromArgb(250, 251, 253);
            pnlConfig.Controls.Add(flpConfCon);
            pnlConfig.Dock = DockStyle.Fill;
            pnlConfig.Location = new Point(0, 88);
            pnlConfig.Margin = new Padding(5, 6, 5, 6);
            pnlConfig.Name = "pnlConfig";
            pnlConfig.Padding = new Padding(17, 0, 17, 0);
            pnlConfig.Size = new Size(1590, 228);
            pnlConfig.TabIndex = 9;
            // 
            // flpConfCon
            // 
            flpConfCon.AutoScroll = true;
            flpConfCon.BackColor = Color.FromArgb(250, 251, 253);
            flpConfCon.Dock = DockStyle.Fill;
            flpConfCon.FlowDirection = FlowDirection.TopDown;
            flpConfCon.Location = new Point(17, 0);
            flpConfCon.Margin = new Padding(3, 4, 3, 4);
            flpConfCon.Name = "flpConfCon";
            flpConfCon.Size = new Size(1556, 228);
            flpConfCon.TabIndex = 8;
            flpConfCon.WrapContents = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblConfEditor);
            panel2.Controls.Add(btnConfigHelp);
            panel2.Controls.Add(btnSaveMyConf);
            panel2.Controls.Add(lblConfigHelp);
            panel2.Controls.Add(lblAddConfSetter);
            panel2.Controls.Add(btnAddConf);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(cboAddConfCount);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1590, 88);
            panel2.TabIndex = 0;
            // 
            // lblConfEditor
            // 
            lblConfEditor.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblConfEditor.Location = new Point(3, 4);
            lblConfEditor.Margin = new Padding(5, 6, 5, 6);
            lblConfEditor.Name = "lblConfEditor";
            lblConfEditor.Size = new Size(345, 78);
            lblConfEditor.TabIndex = 0;
            lblConfEditor.Text = "학습 상세 설정";
            lblConfEditor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnConfigHelp
            // 
            btnConfigHelp.Font = new Font("맑은 고딕", 15.8571434F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnConfigHelp.Location = new Point(1238, 20);
            btnConfigHelp.Margin = new Padding(3, 4, 3, 4);
            btnConfigHelp.Name = "btnConfigHelp";
            btnConfigHelp.Size = new Size(51, 54);
            btnConfigHelp.TabIndex = 10;
            btnConfigHelp.Text = "？";
            btnConfigHelp.UseVisualStyleBackColor = true;
            btnConfigHelp.Click += btnConfigHelp_Click;
            // 
            // btnSaveMyConf
            // 
            btnSaveMyConf.BackColor = Color.FromArgb(72, 175, 120);
            btnSaveMyConf.Cursor = Cursors.Hand;
            btnSaveMyConf.FlatAppearance.BorderSize = 0;
            btnSaveMyConf.FlatStyle = FlatStyle.Flat;
            btnSaveMyConf.Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
            btnSaveMyConf.ForeColor = Color.White;
            btnSaveMyConf.Location = new Point(1333, 4);
            btnSaveMyConf.Margin = new Padding(5, 6, 5, 6);
            btnSaveMyConf.Name = "btnSaveMyConf";
            btnSaveMyConf.Size = new Size(238, 78);
            btnSaveMyConf.TabIndex = 4;
            btnSaveMyConf.Text = "학습 설정 저장";
            btnSaveMyConf.UseVisualStyleBackColor = false;
            btnSaveMyConf.Click += btnSaveMyConf_Click;
            // 
            // lblConfigHelp
            // 
            lblConfigHelp.AutoSize = true;
            lblConfigHelp.Font = new Font("맑은 고딕", 11F);
            lblConfigHelp.ForeColor = Color.FromArgb(60, 72, 92);
            lblConfigHelp.Location = new Point(982, 27);
            lblConfigHelp.Name = "lblConfigHelp";
            lblConfigHelp.Size = new Size(250, 36);
            lblConfigHelp.TabIndex = 11;
            lblConfigHelp.Text = "각 설정에 대한 설명";
            // 
            // lblAddConfSetter
            // 
            lblAddConfSetter.Font = new Font("맑은 고딕", 11F);
            lblAddConfSetter.ForeColor = Color.FromArgb(60, 72, 92);
            lblAddConfSetter.Location = new Point(340, 22);
            lblAddConfSetter.Margin = new Padding(5, 0, 5, 0);
            lblAddConfSetter.Name = "lblAddConfSetter";
            lblAddConfSetter.Size = new Size(195, 46);
            lblAddConfSetter.TabIndex = 1;
            lblAddConfSetter.Text = "학습 설정 추가";
            lblAddConfSetter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnAddConf
            // 
            btnAddConf.BackColor = Color.FromArgb(67, 130, 220);
            btnAddConf.Cursor = Cursors.Hand;
            btnAddConf.FlatAppearance.BorderSize = 0;
            btnAddConf.FlatStyle = FlatStyle.Flat;
            btnAddConf.Font = new Font("맑은 고딕", 11F);
            btnAddConf.ForeColor = Color.White;
            btnAddConf.Location = new Point(545, 22);
            btnAddConf.Margin = new Padding(5, 6, 5, 6);
            btnAddConf.Name = "btnAddConf";
            btnAddConf.Size = new Size(46, 60);
            btnAddConf.TabIndex = 2;
            btnAddConf.Text = "+";
            btnAddConf.TextAlign = ContentAlignment.TopLeft;
            btnAddConf.UseVisualStyleBackColor = false;
            btnAddConf.Click += btnAddConf_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 11F);
            label2.ForeColor = Color.FromArgb(60, 72, 92);
            label2.Location = new Point(622, 27);
            label2.Name = "label2";
            label2.Size = new Size(250, 36);
            label2.TabIndex = 3;
            label2.Text = "한줄에 띄울 설정 수";
            // 
            // cboAddConfCount
            // 
            cboAddConfCount.BackColor = Color.White;
            cboAddConfCount.FlatStyle = FlatStyle.Flat;
            cboAddConfCount.Font = new Font("맑은 고딕", 9.5F);
            cboAddConfCount.FormattingEnabled = true;
            cboAddConfCount.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cboAddConfCount.Location = new Point(880, 25);
            cboAddConfCount.Margin = new Padding(5, 6, 5, 6);
            cboAddConfCount.Name = "cboAddConfCount";
            cboAddConfCount.Size = new Size(76, 38);
            cboAddConfCount.TabIndex = 3;
            cboAddConfCount.Text = "1";
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
            pnlTrainer.Margin = new Padding(5, 6, 5, 6);
            pnlTrainer.Name = "pnlTrainer";
            pnlTrainer.Size = new Size(1629, 410);
            pnlTrainer.TabIndex = 1;
            // 
            // btnShowLog
            // 
            btnShowLog.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnShowLog.Location = new Point(820, 226);
            btnShowLog.Margin = new Padding(3, 4, 3, 4);
            btnShowLog.Name = "btnShowLog";
            btnShowLog.Size = new Size(786, 52);
            btnShowLog.TabIndex = 17;
            btnShowLog.Text = "로그 보기";
            btnShowLog.UseVisualStyleBackColor = true;
            btnShowLog.Click += btnShowLog_Click;
            // 
            // btnModelTypeHelp
            // 
            btnModelTypeHelp.Font = new Font("맑은 고딕", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnModelTypeHelp.Location = new Point(730, 113);
            btnModelTypeHelp.Margin = new Padding(3, 4, 3, 4);
            btnModelTypeHelp.Name = "btnModelTypeHelp";
            btnModelTypeHelp.Size = new Size(53, 46);
            btnModelTypeHelp.TabIndex = 12;
            btnModelTypeHelp.Text = "？";
            btnModelTypeHelp.UseVisualStyleBackColor = true;
            btnModelTypeHelp.Click += btnModelTypeHelp_Click;
            // 
            // lblMinValLoss
            // 
            lblMinValLoss.AutoSize = true;
            lblMinValLoss.Font = new Font("맑은 고딕", 11.1428576F);
            lblMinValLoss.Location = new Point(1209, 353);
            lblMinValLoss.Name = "lblMinValLoss";
            lblMinValLoss.Size = new Size(347, 37);
            lblMinValLoss.TabIndex = 15;
            lblMinValLoss.Text = "최소 테스트 오차율: 0.0000";
            // 
            // lblValLoss
            // 
            lblValLoss.AutoSize = true;
            lblValLoss.Font = new Font("맑은 고딕", 11.1428576F);
            lblValLoss.Location = new Point(774, 353);
            lblValLoss.Name = "lblValLoss";
            lblValLoss.Size = new Size(284, 37);
            lblValLoss.TabIndex = 14;
            lblValLoss.Text = "테스트 오차율: 0.0000";
            // 
            // lblLoss
            // 
            lblLoss.AutoSize = true;
            lblLoss.Font = new Font("맑은 고딕", 11.1428576F);
            lblLoss.Location = new Point(326, 353);
            lblLoss.Name = "lblLoss";
            lblLoss.Size = new Size(266, 37);
            lblLoss.TabIndex = 13;
            lblLoss.Text = "학습 오차율 : 0.0000";
            // 
            // lblEpoch
            // 
            lblEpoch.AutoSize = true;
            lblEpoch.Font = new Font("맑은 고딕", 11.1428576F);
            lblEpoch.Location = new Point(44, 353);
            lblEpoch.Name = "lblEpoch";
            lblEpoch.Size = new Size(190, 37);
            lblEpoch.TabIndex = 12;
            lblEpoch.Text = "반복횟수 : 0/0";
            // 
            // lblSetModelName
            // 
            lblSetModelName.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblSetModelName.ForeColor = Color.FromArgb(60, 72, 92);
            lblSetModelName.Location = new Point(153, 48);
            lblSetModelName.Margin = new Padding(9, 0, 9, 0);
            lblSetModelName.Name = "lblSetModelName";
            lblSetModelName.Size = new Size(228, 50);
            lblSetModelName.TabIndex = 11;
            lblSetModelName.Text = "모델 이름 (선택)";
            lblSetModelName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtModelName
            // 
            txtModelName.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtModelName.Location = new Point(384, 59);
            txtModelName.Margin = new Padding(5, 6, 5, 6);
            txtModelName.Name = "txtModelName";
            txtModelName.Size = new Size(399, 42);
            txtModelName.TabIndex = 10;
            // 
            // grpSetTrainSetting
            // 
            grpSetTrainSetting.BackColor = Color.FromArgb(250, 251, 253);
            grpSetTrainSetting.Controls.Add(rdoUseCPU);
            grpSetTrainSetting.Controls.Add(rdoUseGPU);
            grpSetTrainSetting.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            grpSetTrainSetting.ForeColor = Color.FromArgb(60, 72, 92);
            grpSetTrainSetting.Location = new Point(820, 140);
            grpSetTrainSetting.Margin = new Padding(3, 4, 3, 4);
            grpSetTrainSetting.Name = "grpSetTrainSetting";
            grpSetTrainSetting.Padding = new Padding(3, 4, 3, 4);
            grpSetTrainSetting.Size = new Size(786, 78);
            grpSetTrainSetting.TabIndex = 9;
            grpSetTrainSetting.TabStop = false;
            grpSetTrainSetting.Text = "학습 방법";
            // 
            // rdoUseCPU
            // 
            rdoUseCPU.AutoSize = true;
            rdoUseCPU.Font = new Font("맑은 고딕", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 129);
            rdoUseCPU.ForeColor = Color.FromArgb(60, 72, 92);
            rdoUseCPU.Location = new Point(518, 34);
            rdoUseCPU.Margin = new Padding(3, 4, 3, 4);
            rdoUseCPU.Name = "rdoUseCPU";
            rdoUseCPU.Size = new Size(165, 36);
            rdoUseCPU.TabIndex = 1;
            rdoUseCPU.TabStop = true;
            rdoUseCPU.Text = "CPU만 사용";
            rdoUseCPU.UseVisualStyleBackColor = true;
            // 
            // rdoUseGPU
            // 
            rdoUseGPU.AutoSize = true;
            rdoUseGPU.Font = new Font("맑은 고딕", 9.857143F, FontStyle.Regular, GraphicsUnit.Point, 129);
            rdoUseGPU.ForeColor = Color.FromArgb(60, 72, 92);
            rdoUseGPU.Location = new Point(149, 34);
            rdoUseGPU.Margin = new Padding(3, 4, 3, 4);
            rdoUseGPU.Name = "rdoUseGPU";
            rdoUseGPU.Size = new Size(143, 36);
            rdoUseGPU.TabIndex = 0;
            rdoUseGPU.TabStop = true;
            rdoUseGPU.Text = "GPU 사용";
            rdoUseGPU.UseVisualStyleBackColor = true;
            // 
            // lblTransferWarning
            // 
            lblTransferWarning.AutoSize = true;
            lblTransferWarning.Font = new Font("맑은 고딕", 9.857143F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTransferWarning.ForeColor = Color.FromArgb(210, 70, 70);
            lblTransferWarning.Location = new Point(31, 238);
            lblTransferWarning.Name = "lblTransferWarning";
            lblTransferWarning.Size = new Size(790, 32);
            lblTransferWarning.TabIndex = 8;
            lblTransferWarning.Text = "전이학습을 하는 동안 모델 종류를 바꿀 수 없습니다. (바꿀시 오류 발생)";
            lblTransferWarning.Visible = false;
            // 
            // prgTrain
            // 
            prgTrain.BackColor = Color.FromArgb(220, 228, 240);
            prgTrain.ForeColor = Color.FromArgb(72, 175, 120);
            prgTrain.Location = new Point(230, 295);
            prgTrain.Margin = new Padding(5, 6, 5, 6);
            prgTrain.Name = "prgTrain";
            prgTrain.Size = new Size(1374, 52);
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
            btnTrain.Location = new Point(17, 295);
            btnTrain.Margin = new Padding(5, 6, 5, 6);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(203, 52);
            btnTrain.TabIndex = 6;
            btnTrain.Text = "▶ 훈련 시작";
            btnTrain.UseVisualStyleBackColor = false;
            btnTrain.Click += btnTrain_Click;
            // 
            // txtComment
            // 
            txtComment.BackColor = Color.White;
            txtComment.BorderStyle = BorderStyle.FixedSingle;
            txtComment.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            txtComment.Location = new Point(982, 35);
            txtComment.Margin = new Padding(5, 6, 5, 6);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(624, 95);
            txtComment.TabIndex = 0;
            // 
            // lblComment
            // 
            lblComment.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblComment.ForeColor = Color.FromArgb(60, 72, 92);
            lblComment.Location = new Point(820, 57);
            lblComment.Margin = new Padding(5, 0, 5, 0);
            lblComment.Name = "lblComment";
            lblComment.Size = new Size(143, 54);
            lblComment.TabIndex = 5;
            lblComment.Text = "모델 메모";
            lblComment.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSelectTransferModel
            // 
            lblSelectTransferModel.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblSelectTransferModel.ForeColor = Color.FromArgb(60, 72, 92);
            lblSelectTransferModel.Location = new Point(153, 172);
            lblSelectTransferModel.Margin = new Padding(5, 0, 5, 0);
            lblSelectTransferModel.Name = "lblSelectTransferModel";
            lblSelectTransferModel.Size = new Size(270, 46);
            lblSelectTransferModel.TabIndex = 4;
            lblSelectTransferModel.Text = "전이 학습 모델 선택";
            lblSelectTransferModel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cboSelectTransferModel
            // 
            cboSelectTransferModel.BackColor = Color.White;
            cboSelectTransferModel.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelectTransferModel.FlatStyle = FlatStyle.Flat;
            cboSelectTransferModel.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            cboSelectTransferModel.FormattingEnabled = true;
            cboSelectTransferModel.Location = new Point(465, 174);
            cboSelectTransferModel.Margin = new Padding(5, 6, 5, 6);
            cboSelectTransferModel.Name = "cboSelectTransferModel";
            cboSelectTransferModel.Size = new Size(318, 44);
            cboSelectTransferModel.TabIndex = 3;
            cboSelectTransferModel.SelectedIndexChanged += cboSelectTransferModel_SelectedIndexChanged;
            // 
            // cboSelectModelType
            // 
            cboSelectModelType.BackColor = Color.White;
            cboSelectModelType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelectModelType.FlatStyle = FlatStyle.Flat;
            cboSelectModelType.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            cboSelectModelType.FormattingEnabled = true;
            cboSelectModelType.Location = new Point(366, 113);
            cboSelectModelType.Margin = new Padding(5, 6, 5, 6);
            cboSelectModelType.Name = "cboSelectModelType";
            cboSelectModelType.Size = new Size(355, 44);
            cboSelectModelType.TabIndex = 2;
            // 
            // lblSelectModelType
            // 
            lblSelectModelType.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblSelectModelType.ForeColor = Color.FromArgb(60, 72, 92);
            lblSelectModelType.Location = new Point(153, 113);
            lblSelectModelType.Margin = new Padding(5, 0, 5, 0);
            lblSelectModelType.Name = "lblSelectModelType";
            lblSelectModelType.Size = new Size(203, 46);
            lblSelectModelType.TabIndex = 1;
            lblSelectModelType.Text = "모델 종류 선택";
            lblSelectModelType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            label1.Location = new Point(-2, 0);
            label1.Margin = new Padding(5, 6, 5, 6);
            label1.Name = "label1";
            label1.Size = new Size(168, 78);
            label1.TabIndex = 0;
            label1.Text = "학습기";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlViewerAndEditor
            // 
            pnlViewerAndEditor.BackColor = Color.White;
            pnlViewerAndEditor.Controls.Add(pnlListView);
            pnlViewerAndEditor.Controls.Add(tblButton);
            pnlViewerAndEditor.Controls.Add(pnlLabel);
            pnlViewerAndEditor.Location = new Point(32, 451);
            pnlViewerAndEditor.Margin = new Padding(5, 6, 5, 6);
            pnlViewerAndEditor.Name = "pnlViewerAndEditor";
            pnlViewerAndEditor.Size = new Size(1575, 579);
            pnlViewerAndEditor.TabIndex = 2;
            // 
            // pnlListView
            // 
            pnlListView.BackColor = Color.White;
            pnlListView.Controls.Add(lvwModel);
            pnlListView.Dock = DockStyle.Fill;
            pnlListView.Location = new Point(0, 106);
            pnlListView.Margin = new Padding(5, 6, 5, 6);
            pnlListView.Name = "pnlListView";
            pnlListView.Padding = new Padding(17, 20, 17, 20);
            pnlListView.Size = new Size(1575, 393);
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
            lvwModel.Location = new Point(17, 20);
            lvwModel.Margin = new Padding(5, 6, 5, 6);
            lvwModel.Name = "lvwModel";
            lvwModel.Size = new Size(1541, 353);
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
            tblButton.Location = new Point(0, 499);
            tblButton.Margin = new Padding(5, 6, 5, 6);
            tblButton.Name = "tblButton";
            tblButton.Padding = new Padding(9, 10, 9, 10);
            tblButton.RowCount = 1;
            tblButton.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblButton.Size = new Size(1575, 80);
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
            btnDelete.Location = new Point(18, 10);
            btnDelete.Margin = new Padding(9, 0, 9, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(293, 60);
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
            btnRename.Location = new Point(329, 10);
            btnRename.Margin = new Padding(9, 0, 9, 0);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(293, 60);
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
            btnChgComment.Location = new Point(640, 10);
            btnChgComment.Margin = new Padding(9, 0, 9, 0);
            btnChgComment.Name = "btnChgComment";
            btnChgComment.Size = new Size(293, 60);
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
            btnShowConf.Location = new Point(951, 10);
            btnShowConf.Margin = new Padding(9, 0, 9, 0);
            btnShowConf.Name = "btnShowConf";
            btnShowConf.Size = new Size(293, 60);
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
            btnTrainningHistory.Location = new Point(1262, 10);
            btnTrainningHistory.Margin = new Padding(9, 0, 9, 0);
            btnTrainningHistory.Name = "btnTrainningHistory";
            btnTrainningHistory.Size = new Size(295, 60);
            btnTrainningHistory.TabIndex = 3;
            btnTrainningHistory.Text = "훈련 기록 (Ctrl + 4)";
            btnTrainningHistory.UseVisualStyleBackColor = false;
            btnTrainningHistory.Click += btnTrainningHistory_Click;
            // 
            // pnlLabel
            // 
            pnlLabel.BackColor = Color.FromArgb(250, 251, 253);
            pnlLabel.Controls.Add(lblViewerAndEditor);
            pnlLabel.Dock = DockStyle.Top;
            pnlLabel.Location = new Point(0, 0);
            pnlLabel.Margin = new Padding(0);
            pnlLabel.Name = "pnlLabel";
            pnlLabel.Size = new Size(1575, 106);
            pnlLabel.TabIndex = 11;
            // 
            // lblViewerAndEditor
            // 
            lblViewerAndEditor.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblViewerAndEditor.Location = new Point(0, 16);
            lblViewerAndEditor.Margin = new Padding(5, 0, 5, 0);
            lblViewerAndEditor.Name = "lblViewerAndEditor";
            lblViewerAndEditor.Size = new Size(492, 76);
            lblViewerAndEditor.TabIndex = 8;
            lblViewerAndEditor.Text = "파일럿 뷰어 및 편집기";
            lblViewerAndEditor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlField
            // 
            pnlField.Controls.Add(pnlGraph);
            pnlField.Controls.Add(pnlViewerAndEditor);
            pnlField.Controls.Add(pnlConfEditor);
            pnlField.Dock = DockStyle.Fill;
            pnlField.Location = new Point(0, 410);
            pnlField.Margin = new Padding(5, 6, 5, 6);
            pnlField.Name = "pnlField";
            pnlField.Size = new Size(1629, 1042);
            pnlField.TabIndex = 3;
            // 
            // pnlGraph
            // 
            pnlGraph.Location = new Point(32, 331);
            pnlGraph.Name = "pnlGraph";
            pnlGraph.Size = new Size(350, 111);
            pnlGraph.TabIndex = 3;
            // 
            // pnlTrainServiceTab
            // 
            pnlTrainServiceTab.Controls.Add(btnViewerAndEditorTab);
            pnlTrainServiceTab.Controls.Add(btnConfigEditorTab);
            pnlTrainServiceTab.Controls.Add(btnChartTab);
            pnlTrainServiceTab.Dock = DockStyle.Bottom;
            pnlTrainServiceTab.Location = new Point(0, 1452);
            pnlTrainServiceTab.Margin = new Padding(5, 6, 5, 6);
            pnlTrainServiceTab.Name = "pnlTrainServiceTab";
            pnlTrainServiceTab.Size = new Size(1629, 80);
            pnlTrainServiceTab.TabIndex = 4;
            // 
            // btnViewerAndEditorTab
            // 
            btnViewerAndEditorTab.BackColor = Color.FromArgb(100, 110, 130);
            btnViewerAndEditorTab.Cursor = Cursors.Hand;
            btnViewerAndEditorTab.FlatAppearance.BorderSize = 0;
            btnViewerAndEditorTab.FlatStyle = FlatStyle.Flat;
            btnViewerAndEditorTab.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnViewerAndEditorTab.ForeColor = Color.White;
            btnViewerAndEditorTab.Location = new Point(549, 0);
            btnViewerAndEditorTab.Margin = new Padding(0);
            btnViewerAndEditorTab.Name = "btnViewerAndEditorTab";
            btnViewerAndEditorTab.Size = new Size(274, 80);
            btnViewerAndEditorTab.TabIndex = 6;
            btnViewerAndEditorTab.Text = "파일럿 뷰어 및 편집기";
            btnViewerAndEditorTab.UseVisualStyleBackColor = false;
            btnViewerAndEditorTab.Click += btnViewerAndEditorTab_Click;
            // 
            // btnConfigEditorTab
            // 
            btnConfigEditorTab.BackColor = Color.FromArgb(100, 110, 130);
            btnConfigEditorTab.Cursor = Cursors.Hand;
            btnConfigEditorTab.FlatAppearance.BorderSize = 0;
            btnConfigEditorTab.FlatStyle = FlatStyle.Flat;
            btnConfigEditorTab.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnConfigEditorTab.ForeColor = Color.White;
            btnConfigEditorTab.Location = new Point(274, 0);
            btnConfigEditorTab.Margin = new Padding(0);
            btnConfigEditorTab.Name = "btnConfigEditorTab";
            btnConfigEditorTab.Size = new Size(274, 80);
            btnConfigEditorTab.TabIndex = 5;
            btnConfigEditorTab.Text = "학습 상세 설정";
            btnConfigEditorTab.UseVisualStyleBackColor = false;
            btnConfigEditorTab.Click += btnConfigEditorTab_Click;
            // 
            // btnChartTab
            // 
            btnChartTab.BackColor = Color.FromArgb(67, 130, 220);
            btnChartTab.Cursor = Cursors.Hand;
            btnChartTab.FlatAppearance.BorderSize = 0;
            btnChartTab.FlatStyle = FlatStyle.Flat;
            btnChartTab.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            btnChartTab.ForeColor = Color.White;
            btnChartTab.Location = new Point(0, 0);
            btnChartTab.Margin = new Padding(0);
            btnChartTab.Name = "btnChartTab";
            btnChartTab.Size = new Size(274, 80);
            btnChartTab.TabIndex = 4;
            btnChartTab.Text = "학습 그래프";
            btnChartTab.UseVisualStyleBackColor = false;
            btnChartTab.Click += btnChartTab_Click;
            // 
            // TrainerUI
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlField);
            Controls.Add(pnlTrainer);
            Controls.Add(pnlTrainServiceTab);
            Margin = new Padding(5, 6, 5, 6);
            Name = "TrainerUI";
            Size = new Size(1629, 1532);
            Load += TrainerUI_Load;
            pnlConfEditor.ResumeLayout(false);
            pnlConfig.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            pnlTrainer.ResumeLayout(false);
            pnlTrainer.PerformLayout();
            grpSetTrainSetting.ResumeLayout(false);
            grpSetTrainSetting.PerformLayout();
            pnlViewerAndEditor.ResumeLayout(false);
            pnlListView.ResumeLayout(false);
            tblButton.ResumeLayout(false);
            pnlLabel.ResumeLayout(false);
            pnlField.ResumeLayout(false);
            pnlTrainServiceTab.ResumeLayout(false);
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
        private Panel pnlTrainServiceTab;
        private Button btnViewerAndEditorTab;
        private Button btnConfigEditorTab;
        private Button btnChartTab;
        private Panel pnlGraph;
        private Panel panel2;
    }
}