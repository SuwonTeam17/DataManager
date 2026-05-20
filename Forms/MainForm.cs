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

            // 시작 화면
            ShowUI(new InitUI());
        }


        public void ShowUI(UserControl ui)
        {
            pnlMain.Controls.Clear();

            ui.Dock = DockStyle.Fill;

            //  새로 띄우려는 화면이 TubManagerUI라면 로그 이벤트를 연결
            if (ui is TubManagerUI tubUI)
            {
                tubUI.OnLogReported += AppendLogToListView;
            }

            pnlMain.Controls.Add(ui);
        }

        //  TubManagerUI에서 보낸 로그를 lvwLogBox에 출력하는 함수
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

        private void btnChgTubForm_Click(object sender, EventArgs e)
        {
            ShowUI(new TubManagerUI());
        }

        private void btnChgTrainerForm_Click(object sender, EventArgs e)
        {
            ShowUI(new TrainerUI());
        }

        private void btnChgPilotForm_Click(object sender, EventArgs e)
        {
            ShowUI(new PilotArenaUI());
        }
    }
}
