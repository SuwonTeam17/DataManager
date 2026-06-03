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
            pnlConfEditor.SuspendLayout();
            pnlConfig.SuspendLayout();
            pnlTrainer.SuspendLayout();
            grpSetTrainSetting.SuspendLayout();
            pnlViewerAndEditor.SuspendLayout();
            pnlListView.SuspendLayout();
            pnlLabel.SuspendLayout();
            tblButton.SuspendLayout();
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
            pnlConfEditor.Dock = DockStyle.Top;
            pnlConfEditor.Location = new Point(0, 0);
            pnlConfEditor.Margin = new Padding(5, 6, 5, 6);
            pnlConfEditor.Name = "pnlConfEditor";
            pnlConfEditor.Size = new Size(1629, 325);
            pnlConfEditor.TabIndex = 0;
            // 
            // lblConfigHelp
            // 
            lblConfigHelp.AutoSize = true;
            lblConfigHelp.Font = new Font("맑은 고딕", 11F);
            lblConfigHelp.ForeColor = Color.FromArgb(60, 72, 92);
            lblConfigHelp.Location = new Point(956, 40);
            lblConfigHelp.Name = "lblConfigHelp";
            lblConfigHelp.Size = new Size(311, 36);
            lblConfigHelp.TabIndex = 11;
            lblConfigHelp.Text = "각 구성 설정에 대한 설명";
            // 
            // btnConfigHelp
            // 
            btnConfigHelp.Font = new Font("맑은 고딕", 15.8571434F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnConfigHelp.Location = new Point(1273, 30);
            btnConfigHelp.Name = "btnConfigHelp";
            btnConfigHelp.Size = new Size(52, 54);
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
            label2.Location = new Point(578, 40);
            label2.Name = "label2";
            label2.Size = new Size(250, 36);
            label2.TabIndex = 3;
            label2.Text = "한줄에 띄울 설정 수";
            // 
            // pnlConfig
            // 
            pnlConfig.BackColor = Color.FromArgb(250, 251, 253);
            pnlConfig.Controls.Add(flpConfCon);
            pnlConfig.Dock = DockStyle.Bottom;
            pnlConfig.Location = new Point(0, 109);
            pnlConfig.Margin = new Padding(5, 6, 5, 6);
            pnlConfig.Name = "pnlConfig";
            pnlConfig.Padding = new Padding(17, 0, 17, 0);
            pnlConfig.Size = new Size(1629, 216);
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
            flpConfCon.Size = new Size(1595, 216);
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
            btnSaveMyConf.Location = new Point(1404, 6);
            btnSaveMyConf.Margin = new Padding(5, 6, 5, 6);
            btnSaveMyConf.Name = "btnSaveMyConf";
            btnSaveMyConf.Size = new Size(219, 78);
            btnSaveMyConf.TabIndex = 4;
            btnSaveMyConf.Text = "구성값 저장";
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
            cboAddConfCount.Location = new Point(850, 40);
            cboAddConfCount.Margin = new Padding(5, 6, 5, 6);
            cboAddConfCount.Name = "cboAddConfCount";
            cboAddConfCount.Size = new Size(76, 38);
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
            btnAddConf.Location = new Point(490, 30);
            btnAddConf.Margin = new Padding(5, 6, 5, 6);
            btnAddConf.Name = "btnAddConf";
            btnAddConf.Size = new Size(46, 60);
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
            lblAddConfSetter.Location = new Point(283, 30);
            lblAddConfSetter.Margin = new Padding(5, 0, 5, 0);
            lblAddConfSetter.Name = "lblAddConfSetter";
            lblAddConfSetter.Size = new Size(197, 46);
            lblAddConfSetter.TabIndex = 1;
            lblAddConfSetter.Text = "구성 설정 추가";
            lblAddConfSetter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblConfEditor
            // 
            lblConfEditor.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblConfEditor.Location = new Point(5, 6);
            lblConfEditor.Margin = new Padding(5, 6, 5, 6);
            lblConfEditor.Name = "lblConfEditor";
            lblConfEditor.Size = new Size(255, 78);
            lblConfEditor.TabIndex = 0;
            lblConfEditor.Text = "구성 편집";
            lblConfEditor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlTrainer
            // 
            pnlTrainer.BackColor = Color.FromArgb(250, 251, 253);
            pnlTrainer.BorderStyle = BorderStyle.FixedSingle;
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
            pnlTrainer.Location = new Point(0, 325);
            pnlTrainer.Margin = new Padding(5, 6, 5, 6);
            pnlTrainer.Name = "pnlTrainer";
            pnlTrainer.Size = new Size(1629, 313);
            pnlTrainer.TabIndex = 1;
            // 
            // lblMinValLoss
            // 
            lblMinValLoss.AutoSize = true;
            lblMinValLoss.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblMinValLoss.Location = new Point(1249, 251);
            lblMinValLoss.Name = "lblMinValLoss";
            lblMinValLoss.Size = new Size(347, 37);
            lblMinValLoss.TabIndex = 15;
            lblMinValLoss.Text = "최소 테스트 오차율: 0.0000";
            // 
            // lblValLoss
            // 
            lblValLoss.AutoSize = true;
            lblValLoss.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblValLoss.Location = new Point(1249, 172);
            lblValLoss.Name = "lblValLoss";
            lblValLoss.Size = new Size(284, 37);
            lblValLoss.TabIndex = 14;
            lblValLoss.Text = "테스트 오차율: 0.0000";
            // 
            // lblLoss
            // 
            lblLoss.AutoSize = true;
            lblLoss.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblLoss.Location = new Point(1249, 93);
            lblLoss.Name = "lblLoss";
            lblLoss.Size = new Size(266, 37);
            lblLoss.TabIndex = 13;
            lblLoss.Text = "학습 오차율 : 0.0000";
            // 
            // lblEpoch
            // 
            lblEpoch.AutoSize = true;
            lblEpoch.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            lblEpoch.Location = new Point(1249, 14);
            lblEpoch.Name = "lblEpoch";
            lblEpoch.Size = new Size(190, 37);
            lblEpoch.TabIndex = 12;
            lblEpoch.Text = "반복횟수 : 0/0";
            // 
            // lblSetModelName
            // 
            lblSetModelName.Font = new Font("맑은 고딕", 9.5F);
            lblSetModelName.ForeColor = Color.FromArgb(60, 72, 92);
            lblSetModelName.Location = new Point(153, 24);
            lblSetModelName.Margin = new Padding(9, 0, 9, 0);
            lblSetModelName.Name = "lblSetModelName";
            lblSetModelName.Size = new Size(190, 50);
            lblSetModelName.TabIndex = 11;
            lblSetModelName.Text = "모델 이름 (선택)";
            lblSetModelName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtModelName
            // 
            txtModelName.Location = new Point(346, 39);
            txtModelName.Margin = new Padding(5, 6, 5, 6);
            txtModelName.Name = "txtModelName";
            txtModelName.Size = new Size(354, 35);
            txtModelName.TabIndex = 10;
            // 
            // grpSetTrainSetting
            // 
            grpSetTrainSetting.BackColor = Color.FromArgb(250, 251, 253);
            grpSetTrainSetting.Controls.Add(rdoUseCPU);
            grpSetTrainSetting.Controls.Add(rdoUseGPU);
            grpSetTrainSetting.Font = new Font("맑은 고딕", 9.5F);
            grpSetTrainSetting.ForeColor = Color.FromArgb(60, 72, 92);
            grpSetTrainSetting.Location = new Point(715, 168);
            grpSetTrainSetting.Margin = new Padding(3, 4, 3, 4);
            grpSetTrainSetting.Name = "grpSetTrainSetting";
            grpSetTrainSetting.Padding = new Padding(3, 4, 3, 4);
            grpSetTrainSetting.Size = new Size(527, 78);
            grpSetTrainSetting.TabIndex = 9;
            grpSetTrainSetting.TabStop = false;
            grpSetTrainSetting.Text = "학습 방법";
            // 
            // rdoUseCPU
            // 
            rdoUseCPU.AutoSize = true;
            rdoUseCPU.Font = new Font("맑은 고딕", 9.5F);
            rdoUseCPU.ForeColor = Color.FromArgb(60, 72, 92);
            rdoUseCPU.Location = new Point(283, 35);
            rdoUseCPU.Margin = new Padding(3, 4, 3, 4);
            rdoUseCPU.Name = "rdoUseCPU";
            rdoUseCPU.Size = new Size(160, 35);
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
            rdoUseGPU.Location = new Point(48, 38);
            rdoUseGPU.Margin = new Padding(3, 4, 3, 4);
            rdoUseGPU.Name = "rdoUseGPU";
            rdoUseGPU.Size = new Size(138, 35);
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
            lblTransferWarning.Location = new Point(17, 220);
            lblTransferWarning.Name = "lblTransferWarning";
            lblTransferWarning.Size = new Size(692, 30);
            lblTransferWarning.TabIndex = 8;
            lblTransferWarning.Text = "전이학습을 하는 동안 모델 종류를 바꿀 수 없습니다. (바꿀시 오류 발생)";
            lblTransferWarning.Visible = false;
            // 
            // prgTrain
            // 
            prgTrain.BackColor = Color.FromArgb(220, 228, 240);
            prgTrain.ForeColor = Color.FromArgb(72, 175, 120);
            prgTrain.Location = new Point(216, 256);
            prgTrain.Margin = new Padding(5, 6, 5, 6);
            prgTrain.Name = "prgTrain";
            prgTrain.Size = new Size(1026, 50);
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
            btnTrain.Location = new Point(5, 256);
            btnTrain.Margin = new Padding(5, 6, 5, 6);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(201, 50);
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
            txtComment.Location = new Point(831, 40);
            txtComment.Margin = new Padding(5, 6, 5, 6);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(400, 106);
            txtComment.TabIndex = 0;
            // 
            // lblComment
            // 
            lblComment.Font = new Font("맑은 고딕", 9.5F);
            lblComment.ForeColor = Color.FromArgb(60, 72, 92);
            lblComment.Location = new Point(710, 70);
            lblComment.Margin = new Padding(5, 0, 5, 0);
            lblComment.Name = "lblComment";
            lblComment.Size = new Size(118, 36);
            lblComment.TabIndex = 5;
            lblComment.Text = "모델 메모";
            lblComment.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSelectTransferModel
            // 
            lblSelectTransferModel.Font = new Font("맑은 고딕", 9.5F);
            lblSelectTransferModel.ForeColor = Color.FromArgb(60, 72, 92);
            lblSelectTransferModel.Location = new Point(138, 151);
            lblSelectTransferModel.Margin = new Padding(5, 0, 5, 0);
            lblSelectTransferModel.Name = "lblSelectTransferModel";
            lblSelectTransferModel.Size = new Size(219, 46);
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
            cboSelectTransferModel.Location = new Point(382, 156);
            cboSelectTransferModel.Margin = new Padding(5, 6, 5, 6);
            cboSelectTransferModel.Name = "cboSelectTransferModel";
            cboSelectTransferModel.Size = new Size(318, 38);
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
            cboSelectModelType.Location = new Point(331, 89);
            cboSelectModelType.Margin = new Padding(5, 6, 5, 6);
            cboSelectModelType.Name = "cboSelectModelType";
            cboSelectModelType.Size = new Size(369, 38);
            cboSelectModelType.TabIndex = 2;
            // 
            // lblSelectModelType
            // 
            lblSelectModelType.Font = new Font("맑은 고딕", 9.5F);
            lblSelectModelType.ForeColor = Color.FromArgb(60, 72, 92);
            lblSelectModelType.Location = new Point(153, 84);
            lblSelectModelType.Margin = new Padding(5, 0, 5, 0);
            lblSelectModelType.Name = "lblSelectModelType";
            lblSelectModelType.Size = new Size(168, 46);
            lblSelectModelType.TabIndex = 1;
            lblSelectModelType.Text = "모델 종류 선택";
            lblSelectModelType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            label1.Location = new Point(-1, 0);
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
            pnlViewerAndEditor.Controls.Add(pnlLabel);
            pnlViewerAndEditor.Controls.Add(tblButton);
            pnlViewerAndEditor.Dock = DockStyle.Fill;
            pnlViewerAndEditor.Location = new Point(0, 638);
            pnlViewerAndEditor.Margin = new Padding(5, 6, 5, 6);
            pnlViewerAndEditor.Name = "pnlViewerAndEditor";
            pnlViewerAndEditor.Size = new Size(1629, 894);
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
            pnlListView.Size = new Size(1629, 708);
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
            lvwModel.Size = new Size(1595, 668);
            lvwModel.TabIndex = 0;
            lvwModel.UseCompatibleStateImageBehavior = false;
            lvwModel.View = View.Details;
            lvwModel.KeyDown += lstModels_KeyDown;
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
            pnlLabel.Size = new Size(1629, 106);
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
            tblButton.Location = new Point(0, 814);
            tblButton.Margin = new Padding(5, 6, 5, 6);
            tblButton.Name = "tblButton";
            tblButton.Padding = new Padding(9, 10, 9, 10);
            tblButton.RowCount = 1;
            tblButton.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblButton.Size = new Size(1629, 80);
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
            btnDelete.Size = new Size(304, 60);
            btnDelete.TabIndex = 0;
            btnDelete.Text = "삭제";
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
            btnRename.Location = new Point(340, 10);
            btnRename.Margin = new Padding(9, 0, 9, 0);
            btnRename.Name = "btnRename";
            btnRename.Size = new Size(304, 60);
            btnRename.TabIndex = 4;
            btnRename.Text = "이름 변경";
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
            btnChgComment.Location = new Point(662, 10);
            btnChgComment.Margin = new Padding(9, 0, 9, 0);
            btnChgComment.Name = "btnChgComment";
            btnChgComment.Size = new Size(304, 60);
            btnChgComment.TabIndex = 1;
            btnChgComment.Text = "메모 변경";
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
            btnShowConf.Location = new Point(984, 10);
            btnShowConf.Margin = new Padding(9, 0, 9, 0);
            btnShowConf.Name = "btnShowConf";
            btnShowConf.Size = new Size(304, 60);
            btnShowConf.TabIndex = 2;
            btnShowConf.Text = "구성 표시";
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
            btnTrainningHistory.Location = new Point(1306, 10);
            btnTrainningHistory.Margin = new Padding(9, 0, 9, 0);
            btnTrainningHistory.Name = "btnTrainningHistory";
            btnTrainningHistory.Size = new Size(305, 60);
            btnTrainningHistory.TabIndex = 3;
            btnTrainningHistory.Text = "훈련 기록";
            btnTrainningHistory.UseVisualStyleBackColor = false;
            btnTrainningHistory.Click += btnTrainningHistory_Click;
            // 
            // TrainerUI
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            Controls.Add(pnlViewerAndEditor);
            Controls.Add(pnlTrainer);
            Controls.Add(pnlConfEditor);
            Margin = new Padding(5, 6, 5, 6);
            Name = "TrainerUI";
            Size = new Size(1629, 1532);
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
    }
}