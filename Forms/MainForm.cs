using System;
using System.Windows.Forms;
using DataManager.UserControls;
 
namespace DataManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            ShowUI(new InitUI());
        }


        public void ShowUI(UserControl ui)
        {
            pnlMain.Controls.Clear();

            ui.Dock = DockStyle.Fill;

            if (ui is TubManagerUI tubUI)
            {
                tubUI.OnLogReported += AppendLogToListView;
            }
            else if (ui is TrainerUI trainerUI)
            {
                trainerUI.OnLogReported += AppendLogToListView;
            }
            else if (ui is PilotArenaUI pilotArenaUI)
            {
                pilotArenaUI.OnLogReported += AppendLogToListView;
            }

            pnlMain.Controls.Add(ui);
        }

        private void AppendLogToListView(string _time, string _type, string _message)
        {
            if (lvwLogBox.InvokeRequired)
            {
                lvwLogBox.Invoke(new Action(() => AppendLogToListView(_time, _type, _message)));
                return;
            }

            ListViewItem _item = new ListViewItem(_time);
            
            _item.SubItems.Add(_type);
            _item.SubItems.Add(_message);
            lvwLogBox.Items.Add(_item);
            
            lvwLogBox.EnsureVisible(lvwLogBox.Items.Count - 1);
        }

        private void SetActiveTab(Button activeBtn)
        {
            // 모든 탭 버튼을 비활성 색으로 초기화
            btnChgTubForm.BackColor = Color.FromArgb(100, 110, 130);
            btnChgTrainerForm.BackColor = Color.FromArgb(100, 110, 130);
            btnChgPilotForm.BackColor = Color.FromArgb(100, 110, 130);

            // 클릭된 버튼만 활성 색으로
            activeBtn.BackColor = Color.FromArgb(67, 130, 220);
        }

        private void btnChgTubForm_Click(object sender, EventArgs e)
        {
            SetActiveTab(btnChgTubForm);
            ShowUI(new TubManagerUI());
        }

        private void btnChgTrainerForm_Click(object sender, EventArgs e)
        {
            SetActiveTab(btnChgTrainerForm);
            ShowUI(new TrainerUI());
        }

        private void btnChgPilotForm_Click(object sender, EventArgs e)
        {
            SetActiveTab(btnChgPilotForm);
            ShowUI(new PilotArenaUI());
        }
    }
}
