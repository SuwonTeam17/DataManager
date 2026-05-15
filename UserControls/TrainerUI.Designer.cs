namespace DonkeyUI.UserControls
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
            btnSaveMyConf = new Button();
            cboAddConfCount = new ComboBox();
            btnAddConf = new Button();
            lblAddConfSetter = new Label();
            lblConfEditor = new Label();
            pnlTrainer = new Panel();
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
            lvw = new ListView();
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
            pnlConfEditor.SuspendLayout();
            pnlTrainer.SuspendLayout();
            pnlViewerAndEditor.SuspendLayout();
            pnlListView.SuspendLayout();
            pnlLabel.SuspendLayout();
            pnlButton.SuspendLayout();
            SuspendLayout();
            // 
            // pnlConfEditor
            // 
            pnlConfEditor.BorderStyle = BorderStyle.FixedSingle;
            pnlConfEditor.Controls.Add(btnSaveMyConf);
            pnlConfEditor.Controls.Add(cboAddConfCount);
            pnlConfEditor.Controls.Add(btnAddConf);
            pnlConfEditor.Controls.Add(lblAddConfSetter);
            pnlConfEditor.Controls.Add(lblConfEditor);
            pnlConfEditor.Dock = DockStyle.Top;
            pnlConfEditor.Location = new Point(0, 0);
            pnlConfEditor.Name = "pnlConfEditor";
            pnlConfEditor.Size = new Size(750, 126);
            pnlConfEditor.TabIndex = 0;
            // 
            // btnSaveMyConf
            // 
            btnSaveMyConf.Font = new Font("맑은 고딕", 12F);
            btnSaveMyConf.Location = new Point(619, 3);
            btnSaveMyConf.Name = "btnSaveMyConf";
            btnSaveMyConf.Size = new Size(128, 39);
            btnSaveMyConf.TabIndex = 4;
            btnSaveMyConf.Text = "내 구성 저장";
            btnSaveMyConf.UseVisualStyleBackColor = true;
            // 
            // cboAddConfCount
            // 
            cboAddConfCount.FormattingEnabled = true;
            cboAddConfCount.Location = new Point(168, 61);
            cboAddConfCount.Name = "cboAddConfCount";
            cboAddConfCount.Size = new Size(46, 23);
            cboAddConfCount.TabIndex = 3;
            cboAddConfCount.Text = "1";
            // 
            // btnAddConf
            // 
            btnAddConf.Font = new Font("맑은 고딕", 11F);
            btnAddConf.Location = new Point(135, 57);
            btnAddConf.Name = "btnAddConf";
            btnAddConf.Size = new Size(27, 30);
            btnAddConf.TabIndex = 2;
            btnAddConf.Text = "+";
            btnAddConf.TextAlign = ContentAlignment.TopLeft;
            btnAddConf.UseVisualStyleBackColor = true;
            // 
            // lblAddConfSetter
            // 
            lblAddConfSetter.Font = new Font("맑은 고딕", 11F);
            lblAddConfSetter.Location = new Point(15, 61);
            lblAddConfSetter.Name = "lblAddConfSetter";
            lblAddConfSetter.Size = new Size(115, 23);
            lblAddConfSetter.TabIndex = 1;
            lblAddConfSetter.Text = "구성 설정 추가";
            lblAddConfSetter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblConfEditor
            // 
            lblConfEditor.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblConfEditor.Location = new Point(3, 3);
            lblConfEditor.Margin = new Padding(3);
            lblConfEditor.Name = "lblConfEditor";
            lblConfEditor.Size = new Size(149, 39);
            lblConfEditor.TabIndex = 0;
            lblConfEditor.Text = "구성 편집";
            lblConfEditor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlTrainer
            // 
            pnlTrainer.BorderStyle = BorderStyle.FixedSingle;
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
            pnlTrainer.Location = new Point(0, 126);
            pnlTrainer.Name = "pnlTrainer";
            pnlTrainer.Size = new Size(750, 107);
            pnlTrainer.TabIndex = 1;
            // 
            // prgTrain
            // 
            prgTrain.Location = new Point(133, 75);
            prgTrain.Name = "prgTrain";
            prgTrain.Size = new Size(605, 25);
            prgTrain.TabIndex = 7;
            // 
            // btnTrain
            // 
            btnTrain.Location = new Point(10, 75);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(117, 25);
            btnTrain.TabIndex = 6;
            btnTrain.Text = "훈련 시작";
            btnTrain.UseVisualStyleBackColor = true;
            // 
            // txtComment
            // 
            txtComment.Location = new Point(489, 14);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(237, 52);
            txtComment.TabIndex = 0;
            // 
            // lblComment
            // 
            lblComment.Location = new Point(421, 14);
            lblComment.Name = "lblComment";
            lblComment.Size = new Size(62, 52);
            lblComment.TabIndex = 5;
            lblComment.Text = "모델 메모";
            lblComment.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSelectTransferModel
            // 
            lblSelectTransferModel.Location = new Point(114, 43);
            lblSelectTransferModel.Name = "lblSelectTransferModel";
            lblSelectTransferModel.Size = new Size(90, 23);
            lblSelectTransferModel.TabIndex = 4;
            lblSelectTransferModel.Text = "환승 모델 선택";
            lblSelectTransferModel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cboSelectTransferModel
            // 
            cboSelectTransferModel.FormattingEnabled = true;
            cboSelectTransferModel.Location = new Point(210, 43);
            cboSelectTransferModel.Name = "cboSelectTransferModel";
            cboSelectTransferModel.Size = new Size(203, 23);
            cboSelectTransferModel.TabIndex = 3;
            // 
            // cboSelectModelType
            // 
            cboSelectModelType.FormattingEnabled = true;
            cboSelectModelType.Location = new Point(210, 14);
            cboSelectModelType.Name = "cboSelectModelType";
            cboSelectModelType.Size = new Size(203, 23);
            cboSelectModelType.TabIndex = 2;
            // 
            // lblSelectModelType
            // 
            lblSelectModelType.Location = new Point(114, 14);
            lblSelectModelType.Name = "lblSelectModelType";
            lblSelectModelType.Size = new Size(90, 23);
            lblSelectModelType.TabIndex = 1;
            lblSelectModelType.Text = "모델 종류 선택";
            lblSelectModelType.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            label1.Location = new Point(3, 3);
            label1.Margin = new Padding(3);
            label1.Name = "label1";
            label1.Size = new Size(98, 39);
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
            pnlViewerAndEditor.Location = new Point(0, 233);
            pnlViewerAndEditor.Name = "pnlViewerAndEditor";
            pnlViewerAndEditor.Size = new Size(750, 327);
            pnlViewerAndEditor.TabIndex = 2;
            // 
            // pnlListView
            // 
            pnlListView.Controls.Add(lvw);
            pnlListView.Dock = DockStyle.Fill;
            pnlListView.Location = new Point(0, 53);
            pnlListView.Name = "pnlListView";
            pnlListView.Padding = new Padding(10);
            pnlListView.Size = new Size(748, 232);
            pnlListView.TabIndex = 12;
            // 
            // lvw
            // 
            lvw.Columns.AddRange(new ColumnHeader[] { colName, colPilot, colType, colTubs, colTime, colTransfer, colComment });
            lvw.Dock = DockStyle.Fill;
            lvw.FullRowSelect = true;
            lvw.GridLines = true;
            lvw.Location = new Point(10, 10);
            lvw.Name = "lvw";
            lvw.Size = new Size(728, 212);
            lvw.TabIndex = 0;
            lvw.UseCompatibleStateImageBehavior = false;
            lvw.View = View.Details;
            // 
            // colName
            // 
            colName.Text = "모델 이름";
            colName.Width = 80;
            // 
            // colPilot
            // 
            colPilot.Text = "모델 파일명";
            colPilot.Width = 90;
            // 
            // colType
            // 
            colType.Text = "모델 종류";
            colType.Width = 80;
            // 
            // colTubs
            // 
            colTubs.Text = "사용한 데이터셋";
            colTubs.Width = 100;
            // 
            // colTime
            // 
            colTime.Text = "학습 시간";
            colTime.Width = 70;
            // 
            // colTransfer
            // 
            colTransfer.Text = "전이학습 여부";
            colTransfer.Width = 90;
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
            pnlLabel.Size = new Size(748, 53);
            pnlLabel.TabIndex = 11;
            // 
            // lblViewerAndEditor
            // 
            lblViewerAndEditor.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
            lblViewerAndEditor.Location = new Point(-1, 0);
            lblViewerAndEditor.Name = "lblViewerAndEditor";
            lblViewerAndEditor.Size = new Size(287, 38);
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
            pnlButton.Location = new Point(0, 285);
            pnlButton.Name = "pnlButton";
            pnlButton.Size = new Size(748, 40);
            pnlButton.TabIndex = 9;
            // 
            // btnTrainningHistory
            // 
            btnTrainningHistory.Location = new Point(597, 5);
            btnTrainningHistory.Margin = new Padding(5, 5, 20, 5);
            btnTrainningHistory.Name = "btnTrainningHistory";
            btnTrainningHistory.Size = new Size(133, 30);
            btnTrainningHistory.TabIndex = 3;
            btnTrainningHistory.Text = "훈련 기록";
            btnTrainningHistory.UseVisualStyleBackColor = true;
            // 
            // btnShowConf
            // 
            btnShowConf.Location = new Point(408, 5);
            btnShowConf.Margin = new Padding(5);
            btnShowConf.Name = "btnShowConf";
            btnShowConf.Size = new Size(133, 30);
            btnShowConf.TabIndex = 2;
            btnShowConf.Text = "구성 표시";
            btnShowConf.UseVisualStyleBackColor = true;
            // 
            // btnChgComment
            // 
            btnChgComment.Location = new Point(212, 5);
            btnChgComment.Margin = new Padding(5);
            btnChgComment.Name = "btnChgComment";
            btnChgComment.Size = new Size(133, 30);
            btnChgComment.TabIndex = 1;
            btnChgComment.Text = "메모 변경";
            btnChgComment.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(20, 5);
            btnDelete.Margin = new Padding(20, 5, 5, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(133, 30);
            btnDelete.TabIndex = 0;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // TrainerUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlViewerAndEditor);
            Controls.Add(pnlTrainer);
            Controls.Add(pnlConfEditor);
            Name = "TrainerUI";
            Size = new Size(750, 560);
            pnlConfEditor.ResumeLayout(false);
            pnlTrainer.ResumeLayout(false);
            pnlTrainer.PerformLayout();
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
        private ListView lvw;
        private ProgressBar prgTrain;
        private ColumnHeader colName;
        private ColumnHeader colPilot;
        private ColumnHeader colType;
        private ColumnHeader colTubs;
        private ColumnHeader colTime;
        private ColumnHeader colTransfer;
        private ColumnHeader colComment;
    }
}
