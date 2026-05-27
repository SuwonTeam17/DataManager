namespace DataManager.UserControls
{
    partial class TrainerUI
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
            pnlConfEditor = new Panel();
            pnlConfig = new Panel();
            flpConfCon = new FlowLayoutPanel();
            btnSaveMyConf = new Button();
            cboAddConfCount = new ComboBox();
            btnAddConf = new Button();
            lblAddConfSetter = new Label();
            lblConfEditor = new Label();
            pnlTrainer = new Panel();
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
            colPilot = new ColumnHeader();
            colType = new ColumnHeader();
            colTubs = new ColumnHeader();
            colTime = new ColumnHeader();
            colTransfer = new ColumnHeader();
            colComment = new ColumnHeader();
            pnlLabel = new Panel();
            lblViewerAndEditor = new Label();
            pnlButton = new Panel();
            btnTrainningHistory = new Button();
            btnShowConf = new Button();
            btnChgComment = new Button();
            btnDelete = new Button();
            label2 = new Label();
            pnlConfEditor.SuspendLayout();
            pnlConfig.SuspendLayout();
            pnlTrainer.SuspendLayout();
            grpSetTrainSetting.SuspendLayout();
            pnlViewerAndEditor.SuspendLayout();
            pnlListView.SuspendLayout();
            pnlLabel.SuspendLayout();
            pnlButton.SuspendLayout();
            SuspendLayout();
            // 
            // pnlConfEditor
            // 
            pnlConfEditor.BorderStyle = BorderStyle.FixedSingle;
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
            pnlConfEditor.Size = new Size(1286, 213);
            pnlConfEditor.TabIndex = 0;
            // 
            // pnlConfig
            // 
            pnlConfig.Controls.Add(flpConfCon);
            pnlConfig.Dock = DockStyle.Bottom;
            pnlConfig.Location = new Point(0, 117);
            pnlConfig.Margin = new Padding(5, 6, 5, 6);
            pnlConfig.Name = "pnlConfig";
            pnlConfig.Padding = new Padding(17, 0, 17, 0);
            pnlConfig.Size = new Size(1284, 94);
            pnlConfig.TabIndex = 9;
            // 
            // flpConfCon
            // 
            flpConfCon.AutoScroll = true;
            flpConfCon.Dock = DockStyle.Fill;
            flpConfCon.FlowDirection = FlowDirection.TopDown;
            flpConfCon.Location = new Point(17, 0);
            flpConfCon.Margin = new Padding(3, 4, 3, 4);
            flpConfCon.Name = "flpConfCon";
            flpConfCon.Size = new Size(1250, 94);
            flpConfCon.TabIndex = 8;
            flpConfCon.WrapContents = false;
            // 
            // btnSaveMyConf
            // 
            btnSaveMyConf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveMyConf.Font = new Font("맑은 고딕", 12F);
            btnSaveMyConf.Location = new Point(1061, 6);
            btnSaveMyConf.Margin = new Padding(5, 6, 5, 6);
            btnSaveMyConf.Name = "btnSaveMyConf";
            btnSaveMyConf.Size = new Size(219, 78);
            btnSaveMyConf.TabIndex = 4;
            btnSaveMyConf.Text = "설정값 저장";
            btnSaveMyConf.UseVisualStyleBackColor = true;
            btnSaveMyConf.Click += btnSaveMyConf_Click;
            // 
            // cboAddConfCount
            // 
            cboAddConfCount.FormattingEnabled = true;
            cboAddConfCount.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cboAddConfCount.Location = new Point(851, 41);
            cboAddConfCount.Margin = new Padding(5, 6, 5, 6);
            cboAddConfCount.Name = "cboAddConfCount";
            cboAddConfCount.Size = new Size(76, 38);
            cboAddConfCount.TabIndex = 3;
            cboAddConfCount.Text = "1";
            // 
            // btnAddConf
            // 
            btnAddConf.Font = new Font("맑은 고딕", 11F);
            btnAddConf.Location = new Point(490, 30);
            btnAddConf.Margin = new Padding(5, 6, 5, 6);
            btnAddConf.Name = "btnAddConf";
            btnAddConf.Size = new Size(46, 60);
            btnAddConf.TabIndex = 2;
            btnAddConf.Text = "+";
            btnAddConf.TextAlign = ContentAlignment.TopLeft;
            btnAddConf.UseVisualStyleBackColor = true;
            btnAddConf.Click += btnAddConf_Click;
            // 
            // lblAddConfSetter
            // 
            lblAddConfSetter.Font = new Font("맑은 고딕", 11F);
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
            pnlTrainer.BorderStyle = BorderStyle.FixedSingle;
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
            pnlTrainer.Location = new Point(0, 213);
            pnlTrainer.Margin = new Padding(5, 6, 5, 6);
            pnlTrainer.Name = "pnlTrainer";
            pnlTrainer.Size = new Size(1286, 262);
            pnlTrainer.TabIndex = 1;
            // 
            // grpSetTrainSetting
            // 
            grpSetTrainSetting.Controls.Add(rdoUseCPU);
            grpSetTrainSetting.Controls.Add(rdoUseGPU);
            grpSetTrainSetting.Location = new Point(732, 115);
            grpSetTrainSetting.Name = "grpSetTrainSetting";
            grpSetTrainSetting.Size = new Size(533, 78);
            grpSetTrainSetting.TabIndex = 9;
            grpSetTrainSetting.TabStop = false;
            grpSetTrainSetting.Text = "학습 방법";
            // 
            // rdoUseCPU
            // 
            rdoUseCPU.AutoSize = true;
            rdoUseCPU.Location = new Point(291, 38);
            rdoUseCPU.Name = "rdoUseCPU";
            rdoUseCPU.Size = new Size(148, 34);
            rdoUseCPU.TabIndex = 1;
            rdoUseCPU.TabStop = true;
            rdoUseCPU.Text = "CPU만 사용";
            rdoUseCPU.UseVisualStyleBackColor = true;
            // 
            // rdoUseGPU
            // 
            rdoUseGPU.AutoSize = true;
            rdoUseGPU.Location = new Point(50, 38);
            rdoUseGPU.Name = "rdoUseGPU";
            rdoUseGPU.Size = new Size(129, 34);
            rdoUseGPU.TabIndex = 0;
            rdoUseGPU.TabStop = true;
            rdoUseGPU.Text = "GPU 사용";
            rdoUseGPU.UseVisualStyleBackColor = true;
            // 
            // lblTransferWarning
            // 
            lblTransferWarning.AutoSize = true;
            lblTransferWarning.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTransferWarning.ForeColor = Color.Red;
            lblTransferWarning.Location = new Point(17, 153);
            lblTransferWarning.Name = "lblTransferWarning";
            lblTransferWarning.Size = new Size(692, 30);
            lblTransferWarning.TabIndex = 8;
            lblTransferWarning.Text = "전이학습을 하는 동안 모델 종류를 바꿀 수 없습니다. (바꿀시 오류 발생)";
            lblTransferWarning.Visible = false;
            // 
            // prgTrain
            // 
            prgTrain.Location = new Point(228, 199);
            prgTrain.Margin = new Padding(5, 6, 5, 6);
            prgTrain.Name = "prgTrain";
            prgTrain.Size = new Size(1037, 50);
            prgTrain.TabIndex = 7;
            // 
            // btnTrain
            // 
            btnTrain.Location = new Point(17, 199);
            btnTrain.Margin = new Padding(5, 6, 5, 6);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(201, 50);
            btnTrain.TabIndex = 6;
            btnTrain.Text = "훈련 시작";
            btnTrain.UseVisualStyleBackColor = true;
            btnTrain.Click += btnTrain_Click;
            // 
            // txtComment
            // 
            txtComment.Location = new Point(848, 29);
            txtComment.Margin = new Padding(5, 6, 5, 6);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(403, 77);
            txtComment.TabIndex = 0;
            // 
            // lblComment
            // 
            lblComment.Location = new Point(732, 36);
            lblComment.Margin = new Padding(5, 0, 5, 0);
            lblComment.Name = "lblComment";
            lblComment.Size = new Size(106, 62);
            lblComment.TabIndex = 5;
            lblComment.Text = "모델 메모";
            lblComment.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSelectTransferModel
            // 
            lblSelectTransferModel.Location = new Point(164, 94);
            lblSelectTransferModel.Margin = new Padding(5, 0, 5, 0);
            lblSelectTransferModel.Name = "lblSelectTransferModel";
            lblSelectTransferModel.Size = new Size(206, 46);
            lblSelectTransferModel.TabIndex = 4;
            lblSelectTransferModel.Text = "전이 학습 모델 선택";
            lblSelectTransferModel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cboSelectTransferModel
            // 
            cboSelectTransferModel.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelectTransferModel.FormattingEnabled = true;
            cboSelectTransferModel.Location = new Point(391, 99);
            cboSelectTransferModel.Margin = new Padding(5, 6, 5, 6);
            cboSelectTransferModel.Name = "cboSelectTransferModel";
            cboSelectTransferModel.Size = new Size(318, 38);
            cboSelectTransferModel.TabIndex = 3;
            cboSelectTransferModel.SelectedIndexChanged += cboSelectTransferModel_SelectedIndexChanged;
            // 
            // cboSelectModelType
            // 
            cboSelectModelType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelectModelType.FormattingEnabled = true;
            cboSelectModelType.Location = new Point(340, 41);
            cboSelectModelType.Margin = new Padding(5, 6, 5, 6);
            cboSelectModelType.Name = "cboSelectModelType";
            cboSelectModelType.Size = new Size(369, 38);
            cboSelectModelType.TabIndex = 2;
            // 
            // lblSelectModelType
            // 
            lblSelectModelType.Location = new Point(164, 36);
            lblSelectModelType.Margin = new Padding(5, 0, 5, 0);
            lblSelectModelType.Name = "lblSelectModelType";
            lblSelectModelType.Size = new Size(154, 46);
            lblSelectModelType.TabIndex = 1;
            lblSelectModelType.Text = "모델 종류 선택";
            lblSelectModelType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            label1.Location = new Point(0, 10);
            label1.Margin = new Padding(5, 6, 5, 6);
            label1.Name = "label1";
            label1.Size = new Size(168, 78);
            label1.TabIndex = 0;
            label1.Text = "학습기";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlViewerAndEditor
            // 
            pnlViewerAndEditor.BorderStyle = BorderStyle.FixedSingle;
            pnlViewerAndEditor.Controls.Add(pnlListView);
            pnlViewerAndEditor.Controls.Add(pnlLabel);
            pnlViewerAndEditor.Controls.Add(pnlButton);
            pnlViewerAndEditor.Dock = DockStyle.Fill;
            pnlViewerAndEditor.Location = new Point(0, 475);
            pnlViewerAndEditor.Margin = new Padding(5, 6, 5, 6);
            pnlViewerAndEditor.Name = "pnlViewerAndEditor";
            pnlViewerAndEditor.Size = new Size(1286, 645);
            pnlViewerAndEditor.TabIndex = 2;
            // 
            // pnlListView
            // 
            pnlListView.Controls.Add(lvwModel);
            pnlListView.Dock = DockStyle.Fill;
            pnlListView.Location = new Point(0, 106);
            pnlListView.Margin = new Padding(5, 6, 5, 6);
            pnlListView.Name = "pnlListView";
            pnlListView.Padding = new Padding(17, 20, 17, 20);
            pnlListView.Size = new Size(1284, 457);
            pnlListView.TabIndex = 12;
            // 
            // lvwModel
            // 
            lvwModel.Columns.AddRange(new ColumnHeader[] { colName, colPilot, colType, colTubs, colTime, colTransfer, colComment });
            lvwModel.Dock = DockStyle.Fill;
            lvwModel.FullRowSelect = true;
            lvwModel.GridLines = true;
            lvwModel.Location = new Point(17, 20);
            lvwModel.Margin = new Padding(5, 6, 5, 6);
            lvwModel.Name = "lvwModel";
            lvwModel.Size = new Size(1250, 417);
            lvwModel.TabIndex = 0;
            lvwModel.UseCompatibleStateImageBehavior = false;
            lvwModel.View = View.Details;
            // 
            // colName
            // 
            colName.Text = "모델 이름";
            colName.Width = 180;
            // 
            // colPilot
            // 
            colPilot.Text = "모델 파일명";
            colPilot.Width = 175;
            // 
            // colType
            // 
            colType.Text = "모델 종류";
            colType.Width = 175;
            // 
            // colTubs
            // 
            colTubs.Text = "사용한 데이터셋";
            colTubs.Width = 180;
            // 
            // colTime
            // 
            colTime.Text = "학습 시간";
            colTime.Width = 180;
            // 
            // colTransfer
            // 
            colTransfer.Text = "전이학습 여부";
            colTransfer.Width = 150;
            // 
            // colComment
            // 
            colComment.Text = "메모";
            colComment.Width = 200;
            // 
            // pnlLabel
            // 
            pnlLabel.Controls.Add(lblViewerAndEditor);
            pnlLabel.Dock = DockStyle.Top;
            pnlLabel.Location = new Point(0, 0);
            pnlLabel.Margin = new Padding(0);
            pnlLabel.Name = "pnlLabel";
            pnlLabel.Size = new Size(1284, 106);
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
            // pnlButton
            // 
            pnlButton.Controls.Add(btnTrainningHistory);
            pnlButton.Controls.Add(btnShowConf);
            pnlButton.Controls.Add(btnChgComment);
            pnlButton.Controls.Add(btnDelete);
            pnlButton.Dock = DockStyle.Bottom;
            pnlButton.Location = new Point(0, 563);
            pnlButton.Margin = new Padding(5, 6, 5, 6);
            pnlButton.Name = "pnlButton";
            pnlButton.Size = new Size(1284, 80);
            pnlButton.TabIndex = 9;
            // 
            // btnTrainningHistory
            // 
            btnTrainningHistory.Location = new Point(1023, 10);
            btnTrainningHistory.Margin = new Padding(9, 10, 34, 10);
            btnTrainningHistory.Name = "btnTrainningHistory";
            btnTrainningHistory.Size = new Size(228, 60);
            btnTrainningHistory.TabIndex = 3;
            btnTrainningHistory.Text = "훈련 기록";
            btnTrainningHistory.UseVisualStyleBackColor = true;
            btnTrainningHistory.Click += btnTrainningHistory_Click;
            // 
            // btnShowConf
            // 
            btnShowConf.Location = new Point(699, 10);
            btnShowConf.Margin = new Padding(9, 10, 9, 10);
            btnShowConf.Name = "btnShowConf";
            btnShowConf.Size = new Size(228, 60);
            btnShowConf.TabIndex = 2;
            btnShowConf.Text = "구성 표시";
            btnShowConf.UseVisualStyleBackColor = true;
            btnShowConf.Click += btnShowConf_Click;
            // 
            // btnChgComment
            // 
            btnChgComment.Location = new Point(363, 10);
            btnChgComment.Margin = new Padding(9, 10, 9, 10);
            btnChgComment.Name = "btnChgComment";
            btnChgComment.Size = new Size(228, 60);
            btnChgComment.TabIndex = 1;
            btnChgComment.Text = "메모 변경";
            btnChgComment.UseVisualStyleBackColor = true;
            btnChgComment.Click += btnChgComment_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(34, 10);
            btnDelete.Margin = new Padding(34, 10, 9, 10);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(228, 60);
            btnDelete.TabIndex = 0;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 11.1428576F, FontStyle.Regular, GraphicsUnit.Point, 129);
            label2.Location = new Point(578, 39);
            label2.Name = "label2";
            label2.Size = new Size(260, 37);
            label2.TabIndex = 3;
            label2.Text = "한줄에 띄울 설정 수";
            // 
            // TrainerUI
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlViewerAndEditor);
            Controls.Add(pnlTrainer);
            Controls.Add(pnlConfEditor);
            Margin = new Padding(5, 6, 5, 6);
            Name = "TrainerUI";
            Size = new Size(1286, 1120);
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
            pnlButton.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlConfEditor;
        private Label lblConfEditor;
        private Panel pnlTrainer;
        private Panel pnlViewerAndEditor;
        private Label lblAddConfSetter;
        private Button btnAddConf;
        private ComboBox cboAddConfCount;
        private Button btnSaveMyConf;
        private Label label1;
        private Label lblSelectModelType;
        private ComboBox cboSelectModelType;
        private ComboBox cboSelectTransferModel;
        private Label lblSelectTransferModel;
        private Label lblComment;
        private TextBox txtComment;
        private Button btnTrain;
        private Label lblViewerAndEditor;
        private Panel pnlButton;
        private Button btnTrainningHistory;
        private Button btnShowConf;
        private Button btnChgComment;
        private Button btnDelete;
        private Panel pnlLabel;
        private Panel pnlListView;
        private ProgressBar prgTrain;
        private FlowLayoutPanel flpConfCon;
        private ListView lvwModel;
        private ColumnHeader colName;
        private ColumnHeader colPilot;
        private ColumnHeader colType;
        private ColumnHeader colTubs;
        private ColumnHeader colTime;
        private ColumnHeader colTransfer;
        private ColumnHeader colComment;
        private Label lblTransferWarning;
        private Panel pnlConfig;
        private GroupBox grpSetTrainSetting;
        private RadioButton rdoUseCPU;
        private RadioButton rdoUseGPU;
        private Label label2;
    }
}
