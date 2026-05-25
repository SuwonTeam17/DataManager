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

            // OnLogReported 이벤트가 존재하면 구독
            var eventInfo = ui.GetType().GetEvent("OnLogReported");
            if (eventInfo != null && eventInfo.EventHandlerType == typeof(Action<string, string, string>))
            {
                var addLogMethod = this.GetType().GetMethod(nameof(AddLog), new[] { typeof(string), typeof(string), typeof(string) });
                if (addLogMethod != null)
                {
                    var del = Delegate.CreateDelegate(typeof(Action<string, string, string>), this, addLogMethod);
                    eventInfo.AddEventHandler(ui, del);
                }
            }

            pnlMain.Controls.Add(ui);
        }

        public void AddLog(string time, string level, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AddLog(time, level, message)));
                return;
            }

            var item = new ListViewItem(time);
            item.SubItems.Add(level);
            item.SubItems.Add(message);
            lvwLogBox.Items.Add(item);

            // 항상 마지막 항목을 볼 수 있도록 처리
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
