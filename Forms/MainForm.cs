using DataManager.UserControls;

namespace DataManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // ?쒖옉 ?붾㈃
            ShowUI(new InitUI());
        }


        public void ShowUI(UserControl ui)
        {
            pnlMain.Controls.Clear();

            ui.Dock = DockStyle.Fill;

            pnlMain.Controls.Add(ui);
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

        /// <summary>
        /// TrainerUI 등 다른 화면에서 원격으로 호출하여 메인 로그박스에 줄을 추가하는 메서드
        /// </summary>
        public void AddLog(string type, string message)
        {
            // 1. 현재 시간을 "HH:mm:ss" 형태로 포맷팅 (예: 22:42:05)
            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            // 2. 리스트뷰에 들어갈 한 줄(Row) 항목 생성 및 서브아이템 추가
            ListViewItem lvi = new ListViewItem(currentTime); // 첫 번째 컬럼: 시간
            lvi.SubItems.Add(type);                           // 두 번째 컬럼: 타입 (INFO, ERROR 등)
            lvi.SubItems.Add(message);                        // 세 번째 컬럼: 내용

            // 3. [중요] 크로스 스레드 방지 및 실시간 UI 업데이트 처리
            if (lvwLogBox.InvokeRequired)
            {
                // 백그라운드(다른 스레드)에서 호출된 경우, 메인 UI 스레드에게 안전하게 대리 토스
                lvwLogBox.Invoke(new Action(() => {
                    lvwLogBox.Items.Add(lvi);
                    lvwLogBox.EnsureVisible(lvwLogBox.Items.Count - 1); // 스크롤을 맨 아래(최신 로그)로 자동 고정
                }));
            }
            else
            {
                // 메인 UI 스레드에서 직접 호출된 경우 바로 추가
                lvwLogBox.Items.Add(lvi);
                lvwLogBox.EnsureVisible(lvwLogBox.Items.Count - 1);
            }
        }
    }
}
