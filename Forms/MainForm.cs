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

            // ?� ?庖
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


        public void AddLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            ListViewItem lvi = new ListViewItem(currentTime);
            lvi.SubItems.Add(type);
            lvi.SubItems.Add(message);

            if (lvwLogBox.InvokeRequired)
            {
                lvwLogBox.Invoke(new Action(() => {
                    lvwLogBox.Items.Add(lvi);
                    lvwLogBox.EnsureVisible(lvwLogBox.Items.Count - 1);
                }));
            }
            else
            {
                lvwLogBox.Items.Add(lvi);
                lvwLogBox.EnsureVisible(lvwLogBox.Items.Count - 1);
            }
        }
    }
}
