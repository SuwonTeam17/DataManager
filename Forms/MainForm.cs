using System;
using System.Drawing;
using System.Windows.Forms;
using DataManager.UserControls;

namespace DataManager
{
    public partial class MainForm : Form
    {
        // ⭐ 1. 화면들을 클래스 전역 변수로 딱 한 번만 미리 선언해 둡니다.
        private TubManagerUI tubUI;
        private TrainerUI trainerUI;
        private PilotArenaUI pilotArenaUI;

        public MainForm()
        {
            InitializeComponent();

            // ⭐ 2. 프로그램 켜질 때 화면들을 미리 생성하고 세팅합니다.
            InitializeScreens();

            // 처음 켜졌을 때는 InitUI를 보여줍니다.
            SwitchScreen(tubUI);
        }

        // 화면들을 최초 1회 생성하고 설정하는 함수
        private void InitializeScreens()
        {
            tubUI = new TubManagerUI();
            trainerUI = new TrainerUI();
            pilotArenaUI = new PilotArenaUI();

            // 로그 이벤트 연결 (한 번만 하면 됨)
            tubUI.OnLogReported += AppendLogToListView;
            trainerUI.OnLogReported += AppendLogToListView;
            pilotArenaUI.OnLogReported += AppendLogToListView;

            // 모든 화면을 메인 패널에 꽉 채워서 넣고 일단 다 숨깁니다.
            AddScreenToPanel(tubUI);
            AddScreenToPanel(trainerUI);
            AddScreenToPanel(pilotArenaUI);
        }

        // 패널에 컨트롤을 추가하고 숨기는 보조 함수
        private void AddScreenToPanel(UserControl ui)
        {
            ui.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(ui);
            ui.Hide(); // 처음에 무조건 다 숨김
        }

        // ⭐ 3. Clear() 대신 Hide()와 Show()를 사용해 화면을 교체하는 핵심 함수
        private void SwitchScreen(UserControl activeUI)
        {
            // 현재 패널에 있는 모든 화면을 일단 숨깁니다. (삭제하지 않음!)
            foreach (Control ctrl in pnlMain.Controls)
            {
                if (ctrl is UserControl uc)
                {
                    uc.Hide();
                }
            }

            // 내가 보고 싶은 화면만 보여주고, 겹치지 않게 맨 앞으로 끌어옵니다.
            activeUI.Show();
            activeUI.BringToFront();
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
            // 모든 탭 버튼을 비활성 색상으로 초기화
            btnChgTubForm.BackColor = Color.FromArgb(100, 110, 130);
            btnChgTrainerForm.BackColor = Color.FromArgb(100, 110, 130);
            btnChgPilotForm.BackColor = Color.FromArgb(100, 110, 130);

            // 클릭한 버튼만 활성 색상으로 변경
            activeBtn.BackColor = Color.FromArgb(67, 130, 220);
        }

        // ⭐ 4. 이제 버튼을 누를 때 new 객체를 만들지 않고, 아까 만든 변수를 넘깁니다.
        private void btnChgTubForm_Click(object sender, EventArgs e)
        {
            SetActiveTab(btnChgTubForm);
            SwitchScreen(tubUI);
        }

        private void btnChgTrainerForm_Click(object sender, EventArgs e)
        {
            SetActiveTab(btnChgTrainerForm);
            SwitchScreen(trainerUI);
        }

        private void btnChgPilotForm_Click(object sender, EventArgs e)
        {
            SetActiveTab(btnChgPilotForm);
            SwitchScreen(pilotArenaUI);
        }
    }
}