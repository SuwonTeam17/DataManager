

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

        private void btnSaveMConf_Click(object sender, EventArgs e)
        {
            // 1. 요청하신 6가지 한글 설정명과 실제 파이썬 변수명 매핑 딕셔너리 정의
            Dictionary<string, string> configMapping = new Dictionary<string, string>()
    {
        { "상단 자르기", "ROI_CROP_TOP" },
        { "하단 자르기", "ROI_CROP_BOTTOM" },
        { "우측 자르기", "ROI_CROP_RIGHT" },
        { "좌측 자르기", "ROI_CROP_LEFT" },
        { "반복 횟수", "MAX_EPOCHS" },
        { "학습 한번에 쓸 사진 수", "BATCH_SIZE" }
    };

            // 2. UI에서 사용자가 입력한 동적 값들을 수집할 딕셔너리
            Dictionary<string, string> userSettings = new Dictionary<string, string>();

            // flpConfCon 내부의 모든 행 패널을 순회합니다.
            foreach (Control rowControl in flpConfCon.Controls)
            {
                if (rowControl is Panel rowPanel)
                {
                    // Name 속성으로 심어두었던 콤보박스와 텍스트박스 검색
                    ComboBox cbo = rowPanel.Controls["cboSelConf"] as ComboBox;
                    TextBox txt = rowPanel.Controls["txtSetConf"] as TextBox;

                    if (cbo != null && txt != null && cbo.SelectedItem != null)
                    {
                        string koreanKey = cbo.SelectedItem.ToString();
                        string englishKey = configMapping[koreanKey];
                        string value = txt.Text.Trim();

                        // 사용자가 값을 입력한 경우에만 수집 목록에 추가
                        if (!string.IsNullOrEmpty(value))
                        {
                            // 만약 동일한 설정을 중복 추가했다면, 가장 아래에 있는 설정값으로 덮어씁니다.
                            userSettings[englishKey] = value;
                        }
                    }
                }
            }

            // 3. 파일 경로 지정 (사용하시는 실제 동키카 프로젝트 폴더의 myconfig.py 경로를 적어주세요)
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "myconfig.py");

            try
            {
                // 파일이 존재하지 않는 경우 예외 처리
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("myconfig.py 파일을 찾을 수 없습니다. 경로를 확인해 주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 4. 기존 파일의 모든 줄을 읽어옵니다.
                List<string> lines = File.ReadAllLines(filePath).ToList();

                // 5. 이전에 GUI를 통해 저장했던 블록이 있는지 확인하고 있으면 삭제 (중복 누적 방지 청소 작업)
                int markerIndex = lines.FindIndex(line => line.Contains("# --- GUI User Settings ---"));
                if (markerIndex != -1)
                {
                    // 마커가 위치한 줄부터 파일 맨 끝줄까지의 과거 데이터를 통째로 날립니다.
                    lines.RemoveRange(markerIndex, lines.Count - markerIndex);
                }

                // 6. 청소 완료된 파일 내용 뒤에 새로운 마커와 설정 데이터 주입
                lines.Add("# --- GUI User Settings ---");
                foreach (var setting in userSettings)
                {
                    // 예시 출력 형태: ROI_CROP_TOP = 45
                    lines.Add($"{setting.Key} = {setting.Value}");
                }

                // 7. 파이썬 파일에 최종적으로 덮어쓰기 완료
                File.WriteAllLines(filePath, lines);
                MessageBox.Show("구성이 myconfig.py에 성공적으로 반영되었습니다!", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"파일 저장 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // 1. [구성 설정 추가 (+)] 버튼 클릭 이벤트
        // ====================================================================
        private void btnAddConf_Click(object sender, EventArgs e)
        {
            flpConfCon.FlowDirection = FlowDirection.LeftToRight;
            flpConfCon.WrapContents = true;

            int itemsPerRow = GetItemsPerRow();

            // ★ 핵심: 스크롤바 두께(약 25px)와 테두리 여유 공간을 위해 45px을 통째로 뺍니다.
            int availableWidth = flpConfCon.ClientSize.Width - 45;
            // 각 항목의 좌우 마진(4+4=8px) 및 결함 방지 버퍼를 위해 12px을 추가로 뺍니다.
            int newWidth = (availableWidth / itemsPerRow) - 12;

            Panel rowPanel = new Panel();
            rowPanel.Width = newWidth;
            rowPanel.Height = 40;
            rowPanel.Margin = new Padding(4, 4, 4, 4); // 고정 마진

            // 콤보박스 너비를 130으로 조절 (4분할 시 텍스트박스 공간 확보)
            ComboBox cboSelConf_New = new ComboBox();
            cboSelConf_New.Name = "cboSelConf";
            cboSelConf_New.Width = 130;
            cboSelConf_New.Location = new Point(5, 8);
            cboSelConf_New.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSelConf_New.Items.AddRange(new string[] {
                "상단 자르기", "하단 자르기", "우측 자르기", "좌측 자르기", "반복 횟수", "학습 한번에 쓸 사진 수"
            });
            cboSelConf_New.SelectedIndex = 0;

            // 삭제 버튼 (-) 크기 및 위치 최적화
            Button btnDelete = new Button();
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "-";
            btnDelete.Width = 32;
            btnDelete.Height = 24;
            btnDelete.Location = new Point(rowPanel.Width - 38, 7);
            btnDelete.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            btnDelete.ForeColor = Color.Red;

            btnDelete.Click += (s, args) => {
                flpConfCon.Controls.Remove(rowPanel);
                rowPanel.Dispose();
                flpConfCon.PerformLayout();
            };

            // 텍스트박스 너비 비율 재조정
            TextBox txtSetConf_New = new TextBox();
            txtSetConf_New.Name = "txtSetConf";
            txtSetConf_New.Width = rowPanel.Width - 180;
            txtSetConf_New.Location = new Point(140, 8);

            rowPanel.Controls.Add(cboSelConf_New);
            rowPanel.Controls.Add(txtSetConf_New);
            rowPanel.Controls.Add(btnDelete);

            flpConfCon.Controls.Add(rowPanel);
        }

        // ====================================================================
        // 2. [한 줄 개수 콤보박스] 값이 바뀔 때 실행되는 이벤트
        // ====================================================================
        private void cboAddConfCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            flpConfCon.FlowDirection = FlowDirection.LeftToRight;
            flpConfCon.WrapContents = true;

            int itemsPerRow = GetItemsPerRow();

            // 위의 생성 공식과 완벽히 일치 시켜 픽셀 오차를 차단합니다.
            int availableWidth = flpConfCon.ClientSize.Width - 45;
            int newWidth = (availableWidth / itemsPerRow) - 12;

            foreach (Control control in flpConfCon.Controls)
            {
                if (control is Panel rowPanel)
                {
                    rowPanel.Width = newWidth;

                    Control txt = rowPanel.Controls["txtSetConf"];
                    Control btn = rowPanel.Controls["btnDelete"];

                    if (txt != null)
                    {
                        txt.Width = newWidth - 180;
                    }
                    if (btn != null)
                    {
                        btn.Location = new Point(newWidth - 38, 7);
                    }
                }
            }

            flpConfCon.PerformLayout();
        }
    }
}
