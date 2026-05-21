

namespace DataManager.UserControls
{
    public partial class TrainerUI : UserControl
    {
        public TrainerUI()
        {
            InitializeComponent();

            this.cboAddConfCount.SelectedIndexChanged += new System.EventHandler(this.cboAddConfCount_SelectedIndexChanged);
        }

        private int GetItemsPerRow()
        {
            if (cboAddConfCount.SelectedItem != null && int.TryParse(cboAddConfCount.SelectedItem.ToString(), out int result))
            {
                return result <= 0 ? 1 : result;
            }
            return 1;
        }

        // ====================================================================
        // 4. [내 구성 저장] 버튼 클릭 이벤트 (Application.OpenForms 활용)
        // ====================================================================
        private void btnSaveMyConf_Click(object sender, EventArgs e)
        {
            // ★ [치트키] 현재 프로그램에서 열려있는 모든 폼 중 "MainForm"이라는 이름의 진짜 주소를 직접 찾아옵니다.
            MainForm mainForm = null;
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is MainForm)
                {
                    mainForm = (MainForm)openForm;
                    break;
                }
            }

            string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\mycar_real\myconfig.py"));

            // 1. 파일이 없을 때 메인 폼의 로그박스 원격 호출
            if (!File.Exists(filePath))
            {
                mainForm?.AddLog("오류", $"myconfig.py 파일을 찾을 수 없습니다. 경로: {filePath}");
                return;
            }

            Dictionary<string, string> configMapping = new Dictionary<string, string>()
    {
        { "상단 자르기", "ROI_CROP_TOP" },
        { "하단 자르기", "ROI_CROP_BOTTOM" },
        { "우측 자르기", "ROI_CROP_RIGHT" },
        { "좌측 자르기", "ROI_CROP_LEFT" },
        { "반복 횟수", "MAX_EPOCHS" },
        { "학습 한번에 쓸 사진 수", "BATCH_SIZE" }
    };

            Dictionary<string, string> userSettings = new Dictionary<string, string>();

            foreach (Control rowControl in flpConfCon.Controls)
            {
                if (rowControl is Panel rowPanel)
                {
                    ComboBox cbo = rowPanel.Controls["cboSelConf"] as ComboBox;
                    TextBox txt = rowPanel.Controls["txtSetConf"] as TextBox;

                    if (cbo != null && txt != null && cbo.SelectedItem != null)
                    {
                        string koreanKey = cbo.SelectedItem.ToString();
                        string englishKey = configMapping[koreanKey];
                        string value = txt.Text.Trim();

                        if (!string.IsNullOrEmpty(value))
                        {
                            userSettings[englishKey] = value;
                        }
                    }
                }
            }

            try
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();

                int markerIndex = lines.FindIndex(line => line.Contains("# --- GUI User Settings ---"));
                if (markerIndex != -1)
                {
                    lines.RemoveRange(markerIndex, lines.Count - markerIndex);
                }

                lines.Add("# --- GUI User Settings ---");
                foreach (var setting in userSettings)
                {
                    lines.Add($"{setting.Key} = {setting.Value}");
                }

                File.WriteAllLines(filePath, lines);

                // 2. 저장 성공 시 메인 폼의 로그박스 원격 호출
                mainForm?.AddLog("알림", "myconfig.py 파일에 새로운 설정값을 성공적으로 저장했습니다.");
            }
            catch (Exception ex)
            {
                // 3. 예외 발생 시 메인 폼의 로그박스 원격 호출
                mainForm?.AddLog("오류", $"파일 저장 중 시스템 오류 발생: {ex.Message}");
            }
        }


        // 2. ★ [동기화 메서드] 콤보박스와 텍스트박스 비율 동시 조절
        private void SyncAllPanelSizes()
        {
            int itemsPerRow = GetItemsPerRow();
            int availableWidth = flpConfCon.Width - 55;
            int newWidth = (availableWidth / itemsPerRow) - 12;

            foreach (Control control in flpConfCon.Controls)
            {
                if (control is Panel rowPanel)
                {
                    rowPanel.Width = newWidth;

                    Control cbo = rowPanel.Controls["cboSelConf"];
                    Control txt = rowPanel.Controls["txtSetConf"];
                    Control btn = rowPanel.Controls["btnDelete"];

                    // ★ 핵심: 콤보박스가 패널 너비의 45%를 차지하게 하되, 최대 180px까지만 커지도록 제한 (Math.Min 사용)
                    int cboWidth = Math.Min(180, (int)(newWidth * 0.45));
                    int txtPosX = cboWidth + 15; // 콤보박스 시작위치(5) + 콤보박스 너비 + 사이 간격(10)

                    if (cbo != null)
                    {
                        cbo.Width = cboWidth;
                    }
                    if (txt != null)
                    {
                        txt.Location = new Point(txtPosX, 13);
                        // 전체 너비에서 텍스트박스 시작점과 우측 버튼 공간(55px)을 뺀 나머지를 텍스트박스에 올인
                        txt.Width = newWidth - txtPosX - 55;
                    }
                    if (btn != null)
                    {
                        btn.Location = new Point(newWidth - 50, 7);
                    }
                }
            }
        }

        // ====================================================================
        // 3. [구성 설정 추가 (+)] 버튼 클릭 이벤트
        // ====================================================================
        private void btnAddConf_Click(object sender, EventArgs e)
        {
            flpConfCon.FlowDirection = FlowDirection.LeftToRight;
            flpConfCon.WrapContents = true;

            Panel rowPanel = new Panel();
            rowPanel.Height = 50;
            rowPanel.Margin = new Padding(4, 4, 4, 4);

            ComboBox cboSelConf_New = new ComboBox();
            cboSelConf_New.Name = "cboSelConf";
            cboSelConf_New.Location = new Point(5, 13);
            cboSelConf_New.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelConf_New.Items.AddRange(new string[] {
        "상단 자르기", "하단 자르기", "우측 자르기", "좌측 자르기", "반복 횟수", "학습 한번에 쓸 사진 수"
    });
            cboSelConf_New.SelectedIndex = 0;

            Button btnDelete = new Button();
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "X";
            btnDelete.Width = 45;
            btnDelete.Height = 35;
            btnDelete.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            btnDelete.ForeColor = Color.Red;
            btnDelete.Padding = new Padding(0);

            btnDelete.Click += (s, args) => {
                flpConfCon.Controls.Remove(rowPanel);
                rowPanel.Dispose();
                SyncAllPanelSizes();
            };

            TextBox txtSetConf_New = new TextBox();
            txtSetConf_New.Name = "txtSetConf";

            rowPanel.Controls.Add(cboSelConf_New);
            rowPanel.Controls.Add(txtSetConf_New);
            rowPanel.Controls.Add(btnDelete);

            flpConfCon.Controls.Add(rowPanel);

            // 어차피 여기서 SyncAllPanelSizes()가 불리면서 정확한 크기와 위치가 재계산되므로
            // 초기 생성 시의 Width나 Location 수학 계산은 과감히 생략했습니다.
            SyncAllPanelSizes();
        }

        // ====================================================================
        // 4. [한 줄 개수 콤보박스] 값이 바뀔 때 실행되는 이벤트
        // ====================================================================
        private void cboAddConfCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            flpConfCon.FlowDirection = FlowDirection.LeftToRight;
            flpConfCon.WrapContents = true;

            // 콤보박스 값이 바뀔 때도 전체 동기화 메서드 한방으로 끝!
            SyncAllPanelSizes();
        }
    }
}
