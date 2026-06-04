
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;

namespace DataManager.UserControls
{
    public partial class TrainerUI : UserControl
    {

        // ⭐ 실시간 힌트를 띄워줄 백그라운드 타이머
        private System.Windows.Forms.Timer placeholderTimer;

        private double minValLoss = double.MaxValue;

        // 클래스 내부 최상단 멤버 변수 구역에 넣어주세요
        public event Action<string, string, string> OnLogReported;

        // 클래스 맨 위 전역 변수 영역
        private Process trainProcess = null;
        private bool isTraining = false; // ⭐ 현재 훈련 중인지 기억하는 변수
        private bool isUserStopped = false;   // ⭐ 사용자가 의도적으로 정지했는지 확인하는 깃발

        // 클래스 전역 변수 영역
        private List<double> historyLoss = new List<double>();
        private List<double> historyValLoss = new List<double>();

        //  전역 변수로 즉석 창과 리스트뷰를 담을 그릇만 만들어 둡니다.
        private Form dynamicLogWindow;
        private ListView dynamicLvLogs;

        // 마우스를 꾹 누르고 드래그 중인지 기억하는 스위치
        private bool isDraggingModels = false;

        // ⭐ 한글 이름과 파이썬 변수명을 연결해 주는 공통 사전
        private readonly Dictionary<string, string> configMapping = new Dictionary<string, string>()
        {
            { "상단 자르기", "ROI_CROP_TOP" },
            { "하단 자르기", "ROI_CROP_BOTTOM" },
            { "우측 자르기", "ROI_CROP_RIGHT" },
            { "좌측 자르기", "ROI_CROP_LEFT" },
            { "반복 횟수", "MAX_EPOCHS" },
            { "학습 한번에 쓸 사진 수", "BATCH_SIZE" }
        };

        private Dictionary<string, string> configHelpDocs = new Dictionary<string, string>()
        {
            { "상단 자르기", "이미지 위쪽(하늘, 천장 등)에서 잘라낼 픽셀 수입니다. 불필요한 배경을 가려 AI가 트랙에만 집중하게 만듭니다." },
            { "하단 자르기", "이미지 아래쪽(자동차 보닛 등)에서 잘라낼 픽셀 수입니다. 트랙이 아닌 자동차 앞부분이 학습에 방해되는 것을 막아줍니다." },
            { "우측 자르기", "이미지 오른쪽 가장자리에서 잘라낼 픽셀 수입니다. 카메라가 한쪽으로 치우쳐 있을 때 시야를 보정합니다." },
            { "좌측 자르기", "이미지 왼쪽 가장자리에서 잘라낼 픽셀 수입니다. 마찬가지로 시야의 중심을 맞추는 데 사용합니다." },
            { "반복 횟수", "준비된 사진 데이터 전체를 AI가 처음부터 끝까지 복습할 최대 횟수(에포크)입니다. 보통 100을 설정하며, 얼리 스토핑이 작동하면 다 채우지 않고 끝날 수 있습니다." },
            { "학습 한번에 쓸 사진 수", "AI가 한 번에 꺼내서 볼 사진의 묶음(배치) 개수입니다. 보통 64나 128을 사용하며, 너무 크면 그래픽카드 메모리가 부족해집니다." }
        };

        // ==============================================================
        // 🔒 훈련 상태에 따른 UI 전체 잠금/해제 제어 스위치
        // ==============================================================
        private void ToggleUIState(bool isTraining)
        {
            // 훈련 중(true)이면 컨트롤들을 비활성화(false)해야 하므로 값을 반대로 뒤집습니다.
            bool enableControls = !isTraining;

            // 1. 기본 정보 입력칸 잠금
            // (하리님의 실제 텍스트박스 이름에 맞게 수정해주세요)
            txtModelName.Enabled = enableControls;
            cboSelectModelType.Enabled = enableControls;
            cboSelectTransferModel.Enabled = enableControls;
            txtComment .Enabled = enableControls;
            chkShowTrainLog .Enabled = enableControls;
            grpSetTrainSetting .Enabled = enableControls;

            // 2. ⭐ 방금 만든 동적 구성 설정 패널 전체를 한 방에 잠금!
            // 패널(flpConfCon) 자체를 꺼버리면 그 안의 모든 콤보박스와 텍스트박스도 같이 잠깁니다.
            flpConfCon.Enabled = enableControls;
            btnAddConf.Enabled = enableControls;
            cboAddConfCount.Enabled = enableControls;
            btnSaveMyConf.Enabled = enableControls;
        }

        // ⭐ 파이썬 폴더의 정체를 밝혀내는 탐지기 함수
        private string DetectPythonEnvironmentType(string pythonFolderPath)
        {
            if (!Directory.Exists(pythonFolderPath))
            {
                return "경로 없음";
            }

            // 1. 가상환경(venv) 검사: pyvenv.cfg 파일이 있는지 확인
            string venvConfigPath = Path.Combine(pythonFolderPath, "pyvenv.cfg");
            if (File.Exists(venvConfigPath))
            {
                return "가상환경(venv)";
            }

            // 2. 임베더블 패키지 검사: ._pth 파일이 있는지 확인
            // (파이썬 버전에 따라 python39._pth, python310._pth 등으로 이름이 다르므로 확장자로 찾습니다)
            string[] pthFiles = Directory.GetFiles(pythonFolderPath, "*._pth");
            if (pthFiles.Length > 0)
            {
                return "임베더블(Embeddable)";
            }

            // 3. 둘 다 아니면 알 수 없는 일반 폴더
            return "알 수 없음";
        }

        private void TrainerUI_Load(object sender, EventArgs e)
        {
            // 1. 모델 종류 콤보박스 세팅
            cboSelectModelType.Items.Clear();
            cboSelectModelType.Items.Add("기본 주행 (Linear)");
            cboSelectModelType.Items.Add("분류형 주행 (Categorical)");
            cboSelectModelType.Items.Add("기억형 주행 (RNN)");
            cboSelectModelType.Items.Add("추론형 주행 (Inferred)"); // ⭐ Behavior 대신 Inferred로 변경
            cboSelectModelType.Items.Add("입체 시각 주행 (3D)");
            cboSelectModelType.SelectedIndex = 0;
            lblTransferWarning.Visible = false; // 전이학습 경고 메시지 숨기기

            // 2. 기존 학습된 모델 불러오기 (리스트뷰 & 전이학습 콤보박스 세팅)
            LoadExistingModels();

            rdoUseCPU.Checked = true; // 기본값은 안전하게 CPU 모드로 켜지게 세팅

            LoadConfigToUI();
            CleanUpZombieFolders();

            // ⭐ 타이머 세팅: 1초(1000ms)마다 갱신
            placeholderTimer = new System.Windows.Forms.Timer();
            placeholderTimer.Interval = 1000;
            placeholderTimer.Tick += PlaceholderTimer_Tick; // 1초마다 실행할 함수 연결
            placeholderTimer.Start(); // 타이머 시작!

            // 시작하자마자 바로 한 번 띄워주기 (1초 기다리는 것 방지)
            UpdatePlaceholder();
            txtComment.PlaceholderText = "모델 설명 (예: 50에포크, 직선코스 학습)";
        }

        // ⭐ 모든 메서드가 동일한 myconfig.py 경로를 바라보도록 만드는 안전한 경로 탐색기
        private string GetMyConfigPath()
        {
            string currentPath = Application.StartupPath;
            while (!Directory.Exists(Path.Combine(currentPath, "env")))
            {
                DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                if (parentInfo == null) break;
                currentPath = parentInfo.FullName;
            }
            return Path.Combine(currentPath, "mycar", "myconfig.py"); // mycar_real 대신 공식 폴더로 통일!
        }

        private void CleanUpZombieFolders()
        {
            try
            {
                string currentPath = Application.StartupPath;
                while (!Directory.Exists(Path.Combine(currentPath, "env")))
                {
                    DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                    if (parentInfo == null) return;
                    currentPath = parentInfo.FullName;
                }

                string modelsPath = Path.Combine(currentPath, "mycar", "models");
                if (!Directory.Exists(modelsPath)) return;

                DirectoryInfo di = new DirectoryInfo(modelsPath);
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    string modelName = dir.Name;

                    // 1. 옛날 방식: 바깥 폴더에 pb가 있는지 확인
                    string oldPbPath = Path.Combine(dir.FullName, "saved_model.pb");
                    // 2. 새 방식: 안쪽에 '모델명'과 똑같은 이름의 중첩 폴더가 있는지 확인
                    string nestedFolderPath = Path.Combine(dir.FullName, modelName);

                    // ⭐ 두 가지 흔적이 모두 없을 때만(진짜 껍데기일 때만) 암살!
                    if (!File.Exists(oldPbPath) && !Directory.Exists(nestedFolderPath))
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        private ComboBox lastFocusedComboBox = null;

        public TrainerUI()
        {
            InitializeComponent();

            this.cboAddConfCount.SelectedIndexChanged += new System.EventHandler(this.cboAddConfCount_SelectedIndexChanged);

            // ==========================================================
            // ⭐ [해결책] FlowLayoutPanel의 크기가 변할 때(창 크기 조절 시) 실시간으로 비율 재계산!
            this.flpConfCon.SizeChanged += (s, args) => SyncAllPanelSizes();
            // ==========================================================
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
            string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\mycar\myconfig.py"));

            // 1. 파일이 없을 때 메인 폼의 로그박스 원격 호출
            if (!File.Exists(filePath))
            {
                // 1. 디버깅의 생명줄 (개발자를 위한 기록)
                ReportLog("오류", $"설정 파일 누락: myconfig.py 파일을 찾을 수 없음. (탐색 경로: {filePath})");

                // 2. 사용자에게 알림 (사용자를 위한 경고)
                MessageBox.Show($"동키카 설정 파일(myconfig.py)을 찾을 수 없습니다.\n아래 경로에 파일이 존재하는지 확인해 주세요.\n\n경로: {filePath}",
                                "설정 파일 누락",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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
                ReportLog("알림", "myconfig.py 파일에 새로운 설정값을 성공적으로 저장했습니다.");
            }
            catch (Exception ex)
            {
                // 1. 개발자용: 나중에 디버깅을 위해 상세한 예외 전체 경로를 기록합니다.
                ReportLog("오류", $"파일 저장 중 시스템 오류 발생: {ex.ToString()}");

                // 2. 사용자용: 저장이 안 되었음을 알리고, 흔한 원인을 짚어줍니다.
                MessageBox.Show("파일을 저장하는 중에 시스템 오류가 발생했습니다.\n" +
                                "디스크 용량이 부족하거나 저장할 폴더에 쓰기 권한이 없을 수 있습니다.\n\n" +
                                $"[상세 원인] {ex.Message}",
                                "파일 저장 실패",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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

                    // ==========================================================
                    // ⭐ 핵심 수정: 콤보박스의 최대 폭 제한(Math.Min)을 아예 없애버렸습니다!
                    // 무조건 패널 너비의 60%를 콤보박스가 차지하도록 시원하게 몰아줍니다.
                    int cboWidth = (int)(newWidth * 0.60);
                    int txtPosX = cboWidth + 10; // 콤보박스와 텍스트박스 사이의 간격은 10px로 밀착
                    // ==========================================================

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

            // ⭐ 콤보박스를 마우스로 클릭(Enter)하거나 값을 바꿀 때마다 자신을 기억시킵니다.
            cboSelConf_New.Enter += (s, args) => { lastFocusedComboBox = (ComboBox)s; };
            cboSelConf_New.SelectedIndexChanged += (s, args) => { lastFocusedComboBox = (ComboBox)s; };

            Button btnDelete = new Button();
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "X";
            btnDelete.Width = 45;
            btnDelete.Height = 35;
            btnDelete.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            btnDelete.ForeColor = Color.Red;
            btnDelete.Padding = new Padding(0);

            btnDelete.Click += (s, args) =>
            {
                // ⭐ 방어 코드: 만약 지우려는 줄이 마지막으로 선택했던 콤보박스라면, 기억을 비워줍니다. (에러 방지)
                if (lastFocusedComboBox == cboSelConf_New)
                {
                    lastFocusedComboBox = null;
                }

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

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }


        private void btnTrain_Click(object sender, EventArgs e)
        {
            // ==============================================================
            // 🛑 1. [정지 모드] 현재 훈련 중인데 버튼을 눌렀을 때 (강제 중지)
            // ==============================================================
            if (isTraining)
            {
                DialogResult result = MessageBox.Show(
                    "훈련을 강제로 중지하시겠습니까?\n(현재까지 저장된 최고 기록 모델과 설정 영수증은 안전하게 보존됩니다.)",
                    "훈련 중지", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    btnTrain.Enabled = false;
                    btnTrain.Text = "정지 중...";

                    // ⭐ "에러가 아니라 사용자가 끈 거야!" 라고 깃발 꽂기
                    isUserStopped = true;

                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "taskkill",
                            Arguments = $"/PID {trainProcess.Id} /T /F",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        }).WaitForExit();
                    }
                    catch { }
                }
                return; // 정지 로직만 실행하고 함수를 빠져나갑니다.
            }

            // ==============================================================
            // 🚀 2. [시작 모드] 새로 훈련을 시작할 때
            // ==============================================================

            // ⭐ 시작 상태 세팅
            isTraining = true;
            isUserStopped = false;
            btnTrain.Enabled = false;
            historyLoss.Clear();    // ⭐ 새 훈련 시작 전 수첩 초기화
            historyValLoss.Clear(); // ⭐ 새 훈련 시작 전 수첩 초기화
            btnTrain.Text = "🛑 학습 정지"; // 버튼을 정지 버튼으로 변신
            btnTrain.ForeColor = Color.Red;
            ToggleUIState(true);

            // 1. 역대 최저점 초기화 (가장 중요! 다시 무한대로 돌려놓습니다)
            minValLoss = double.MaxValue;

            // 2. 라벨 텍스트 초기화
            lblEpoch.Text = "반복횟수 : 0/0";
            lblLoss.Text = "학습 오차율 : 0.0000";
            lblValLoss.Text = "테스트 오차율 : 0.0000";
            lblMinValLoss.Text = "최소 테스트 오차율 : 0.0000";

            // 4. 라벨 색상 및 폰트 굵기 원상 복구 (초록색 굵은 글씨 해제)
            lblMinValLoss.ForeColor = Color.Black;
            lblMinValLoss.Font = new Font(lblMinValLoss.Font, FontStyle.Regular);

            string tubPath = "";

            // 1. 데이터 폴더 선택
            string trainRoot = AppPaths.EditedData;
            if (!Directory.Exists(trainRoot))
                Directory.CreateDirectory(trainRoot);

            using (var browser = new CustomFolderBrowser(trainRoot, "학습할 데이터(tub) 폴더 선택"))
            {
                browser.AllowFileSelection = true;

                if (browser.ShowDialog(this) == DialogResult.OK)
                {
                    tubPath = browser.SelectedPath;
                }
                else
                {
                    isTraining = false;
                    btnTrain.Enabled = true;
                    btnTrain.Text = "▶ 훈련 시작";
                    btnTrain.ForeColor = Color.White;
                    ToggleUIState(false);
                    return;
                }
            }

            // 2. 작업 경로 찾기
            string currentPath = Application.StartupPath;
            while (!Directory.Exists(Path.Combine(currentPath, "env")))
            {
                DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                if (parentInfo == null)
                {
                    ReportLog("치명적 오류", "파이썬 엔진 누락: 'env' 폴더를 찾을 수 없음. (경로 탐색 실패)");
                    MessageBox.Show("AI 훈련을 위한 핵심 파이썬 엔진('env' 폴더)을 찾을 수 없습니다.\n프로그램이 정상적으로 설치되었는지 확인해 주세요.",
                                    "시스템 치명적 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    isTraining = false;
                    btnTrain.Enabled = true;
                    btnTrain.Text = "▶ 훈련 시작";
                    btnTrain.ForeColor = Color.White;
                    ToggleUIState(false);
                    return;
                }
                currentPath = parentInfo.FullName;
            }
            string rootPath = currentPath;
            string mycarPath = $@"{rootPath}\mycar";

            // 3. 자동 모델 이름 생성 및 메모 박제
            string timestamp = DateTime.Now.ToString("yyMMdd_HHmmss");
            string defaultMemo = "";

            if (cboSelectTransferModel.SelectedIndex > 0)
            {
                string baseModel = cboSelectTransferModel.SelectedItem.ToString();
                defaultMemo = $"[{DateTime.Now.ToString("MM/dd HH:mm")}] '{baseModel}' 기반 이어서 학습";
            }
            else
            {
                defaultMemo = $"[{DateTime.Now.ToString("MM/dd HH:mm")}] 신규 베이스 모델";
            }

            string curMemo = string.IsNullOrWhiteSpace(txtComment.Text) ? defaultMemo : txtComment.Text.Trim();

            // 4. 모델 종류(--type) 파라미터 번역
            string selectedType = "linear";
            string displayType = cboSelectModelType.SelectedItem.ToString();

            if (displayType.Contains("Categorical")) selectedType = "categorical";
            else if (displayType.Contains("RNN")) selectedType = "rnn";
            else if (displayType.Contains("Inferred")) selectedType = "inferred"; // ⭐ Contains와 변수명 변경
            else if (displayType.Contains("3D")) selectedType = "3d";

            // 5. 전이학습(--transfer) 파라미터 확인
            string curTransfer = "X";
            string transferCommand = "";

            if (cboSelectTransferModel.SelectedIndex > 0)
            {
                string selectedBaseModel = cboSelectTransferModel.SelectedItem.ToString();
                string modelsBaseDir = Path.Combine(mycarPath, "models");

                string nestedPath = Path.Combine(modelsBaseDir, selectedBaseModel, selectedBaseModel);
                string flatPath = Path.Combine(modelsBaseDir, selectedBaseModel);
                string targetTransferPath = "";

                if (Directory.Exists(nestedPath) || File.Exists(nestedPath))
                {
                    targetTransferPath = $"models\\{selectedBaseModel}\\{selectedBaseModel}";
                }
                else if (Directory.Exists(flatPath) || File.Exists(flatPath))
                {
                    targetTransferPath = $"models\\{selectedBaseModel}";
                }
                else if (File.Exists(flatPath + ".h5"))
                {
                    targetTransferPath = $"models\\{selectedBaseModel}.h5";
                }
                else
                {
                    MessageBox.Show($"'{selectedBaseModel}' 베이스 모델 파일을 찾을 수 없습니다.",
                                    "모델 찾기 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isTraining = false;
                    btnTrain.Enabled = true;
                    btnTrain.Text = "▶ 훈련 시작";
                    btnTrain.ForeColor = Color.White;
                    ToggleUIState(false);
                    return;
                }

                transferCommand = $" --transfer \"{targetTransferPath}\"";
                curTransfer = selectedBaseModel;
            }

            string cudaCommand = rdoUseCPU.Checked
                ? "set CUDA_VISIBLE_DEVICES=-1 && set DML_VISIBLE_DEVICES=-1 && "
                : "";

            // 7. 모델 이름 결정하기
            string customName = txtModelName.Text.Trim();
            string modelName = "";

            if (!string.IsNullOrEmpty(customName))
            {
                if (!Regex.IsMatch(customName, @"^[a-zA-Z0-9_]+$"))
                {
                    ReportLog("경고", $"이름 형식 오류 차단됨: 입력값 '{customName}' (영문, 숫자, 언더바 이외 문자 포함)");
                    MessageBox.Show("안정적인 AI 훈련을 위해 모델 이름은\n영어, 숫자, 언더바(_)만 사용할 수 있습니다.",
                                    "이름 형식 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isTraining = false;
                    btnTrain.Enabled = true;
                    btnTrain.Text = "▶ 훈련 시작";
                    btnTrain.ForeColor = Color.White;
                    ToggleUIState(false);
                    return;
                }
                modelName = customName;
            }
            else
            {
                modelName = $"mypilot_{DateTime.Now.ToString("yyMMdd_HHmmss")}";
            }

            // 방어막: 덮어쓰기 방지
            string modelFolderPath = Path.Combine(mycarPath, "models", modelName);
            if (Directory.Exists(modelFolderPath))
            {
                ReportLog("오류", $"이름 중복: '{modelName}' 모델이 이미 존재함.");
                MessageBox.Show($"'{modelName}'(이)라는 모델이 이미 존재합니다.\n다른 이름을 지정해주세요.",
                                "이름 중복", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isTraining = false;
                btnTrain.Enabled = true;
                btnTrain.Text = "▶ 훈련 시작";
                btnTrain.ForeColor = Color.White;
                ToggleUIState(false);
                return;
            }

            if (!Directory.Exists(modelFolderPath))
            {
                Directory.CreateDirectory(modelFolderPath);
            }

            string envPath = Path.GetFullPath(Path.Combine(mycarPath, "..", "env"));
            string envType = DetectPythonEnvironmentType(envPath);
            string windowsCommand = "";


            if (envType == "가상환경(venv)")
            {
                // 🟢 [기존(가상 환경) 방식] 코드 적용
                windowsCommand = $"cd /d \"{mycarPath}\" && \"..\\env\\Scripts\\activate\" && {cudaCommand}donkey train --tub \"{tubPath}\" --model \"models\\{modelName}\\{modelName}\" --type {selectedType}{transferCommand}";
            }
            else
            {
                // 🔵 [신규(임베더블 패키지) 방식] 다이렉트 실행 코드 적용!
                string donkeyPath = Path.GetFullPath(Path.Combine(mycarPath, "..", "env", "Scripts", "donkey.exe"));
                windowsCommand = $"cd /d \"{mycarPath}\" && {cudaCommand}\"{donkeyPath}\" train --tub \"{tubPath}\" --model \"models\\{modelName}\\{modelName}\" --type {selectedType}{transferCommand}";
            }

            // (테스트용 메시지 박스는 필요하시면 주석 해제하세요)
            MessageBox.Show(windowsCommand, "명령어 복사하기");

            // 7. 백그라운드 프로세스 세팅
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.Arguments = $"/c \"{windowsCommand}\"";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            string pythonErrorMessage = "";

            // ⭐ 지역 변수 대신 전역 변수 trainProcess 사용!
            trainProcess = new Process();
            trainProcess.StartInfo = psi;

            // ==============================================================
            // 📺 [추가된 코드] 체크박스가 켜져 있다면 실시간 로그 창 즉석 생성!
            // ==============================================================
            if (chkShowTrainLog.Checked)
            {
                if (dynamicLogWindow == null || dynamicLogWindow.IsDisposed)
                {
                    // 껍데기 창(Form) 만들기
                    dynamicLogWindow = new Form();
                    dynamicLogWindow.Text = $"실시간 훈련 로그 모니터 [{modelName}]";

                    // ⭐ FHD 모니터 안에도 쏙 들어가는 안전한 와이드 사이즈
                    dynamicLogWindow.Size = new Size(1500, 600);
                    dynamicLogWindow.StartPosition = FormStartPosition.CenterScreen;

                    // 알맹이(ListView) 만들기
                    dynamicLvLogs = new ListView();
                    dynamicLvLogs.View = View.Details;
                    dynamicLvLogs.Dock = DockStyle.Fill;
                    dynamicLvLogs.Columns.Add("시간", 80);

                    // ⭐ 창 크기에 맞춰서 시스템 메시지 너비도 확 늘려줍니다.
                    dynamicLvLogs.Columns.Add("시스템 메시지", 1500);

                    dynamicLogWindow.Controls.Add(dynamicLvLogs);
                    dynamicLogWindow.Show();
                }
            }
            // ==============================================================

            prgTrain.Value = 0;
            prgTrain.Maximum = 100;

            // 훈련 시작 버튼 코드 어딘가에 있는 ErrorDataReceived 함수 수정!
            trainProcess.ErrorDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    // 1. 눈에 안 보이는 특수문자 싹 다 지우기
                    string cleanError = Regex.Replace(args.Data, @"\x1B(?:[@-Z\\-_]|\[[0-?]*[ -/]*[@-~])", "").Trim();

                    // ⭐ [스팸 필터] 뒷문으로 들어오는 텐서플로우 경고 100% 차단!
                    if (cleanError.Contains("WARNING:absl:") ||
                        cleanError.Contains("Found untraced functions") ||
                        cleanError.Contains("These functions will not be directly callable"))
                    {
                        return; // 로그 창에도 안 띄우고, 에러 수집도 안 하고 쿨하게 무시!
                    }

                    pythonErrorMessage += cleanError + Environment.NewLine;

                    // 실시간 로그 창 띄우기
                    if (chkShowTrainLog.Checked && dynamicLogWindow != null && !dynamicLogWindow.IsDisposed)
                    {
                        dynamicLogWindow.BeginInvoke((MethodInvoker)delegate
                        {
                            ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
                            item.SubItems.Add(cleanError);

                            // ⭐ 3단 색상 분류기 
                            if (cleanError.Contains("Error") || cleanError.Contains("Exception") || cleanError.Contains("Traceback"))
                            {
                                // 진짜 에러나 프로그램 터짐은 빨간색!
                                item.ForeColor = Color.Red;
                            }
                            else if (cleanError.Contains("WARNING") || cleanError.Contains("W tensorflow"))
                            {
                                // 텐서플로우 경고 메시지는 주황색!
                                item.ForeColor = Color.DarkOrange;
                            }
                            else
                            {
                                // INFO 로딩 메시지나 기타 정상적인 텍스트는 원래 색상(검은색)으로!
                                item.ForeColor = Color.Black;
                            }

                            dynamicLvLogs.Items.Add(item);
                            dynamicLvLogs.Items[dynamicLvLogs.Items.Count - 1].EnsureVisible();
                        });
                    }
                }
            };

            // 9. 파이썬 학습 종료 이벤트 
            trainProcess.EnableRaisingEvents = true;
            trainProcess.Exited += (s, args) =>
            {
                int savedExitCode = trainProcess.ExitCode;

                // 🚨 [에러 발생 시] 종료 코드가 0이 아니고, 사용자가 강제종료한 것도 아닐 때
                if (savedExitCode != 0 && !isUserStopped)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        ReportLog("오류", $"학습 중 오류 발생! (종료 코드: {savedExitCode})");

                        string displayError = string.IsNullOrWhiteSpace(pythonErrorMessage)
                                              ? "알 수 없는 이유로 프로세스가 강제 종료되었습니다. (메모리 부족 등)"
                                              : pythonErrorMessage.Trim();

                        MessageBox.Show(displayError, "훈련 실패 원인", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        prgTrain.Value = 0;

                        if (Directory.Exists(modelFolderPath))
                        {
                            try { Directory.Delete(modelFolderPath, true); } catch { }
                        }

                        isTraining = false;
                        btnTrain.Enabled = true;
                        btnTrain.Text = "▶ 훈련 시작";
                        btnTrain.ForeColor = Color.White;
                        ToggleUIState(false);
                    });

                    trainProcess.Dispose();
                    return;
                }

                // 🎉 [정상 종료 OR 사용자 강제 중지] 영수증 작업 진행
                this.BeginInvoke((MethodInvoker)async delegate
                {
                    int step = 200;
                    for (int i = prgTrain.Value; i <= prgTrain.Maximum; i += step)
                    {
                        if (i > prgTrain.Maximum) i = prgTrain.Maximum;
                        prgTrain.Value = i;
                        await Task.Delay(1);
                    }
                    prgTrain.Value = prgTrain.Maximum;

                    // ⭐ 깃발에 따라 메시지 다르게 띄우기
                    if (isUserStopped)
                    {
                        ReportLog("알림", $"학습이 중지되었습니다. (저장된 지점까지 '{modelName}' 이름으로 보존됩니다.)");
                    }
                    else
                    {
                        ReportLog("알림", $"학습 완료! '{modelName}' 이름으로 저장되었습니다.");
                    }

                    // 4줄 영수증 기록
                    string curDataset = Path.GetFileName(tubPath);
                    string modelFolder = Path.Combine(mycarPath, "models", modelName);
                    string metaFilePath = Path.Combine(modelFolder, "meta.txt");

                    try { File.WriteAllLines(metaFilePath, new string[] { curDataset, displayType, curTransfer, curMemo }); }
                    catch { }

                    // =========================================================
                    // ⭐ 동키카가 사진을 안 만들었을 경우를 대비해 C#이 직접 구워냅니다!
                    string backupGraphPath = Path.Combine(modelFolder, "loss_graph_manual.png");

                    // 굳이 동키카가 그렸는지 안 그렸는지 찾을 필요 없이, 
                    // C#이 무조건 이 예쁜 그래프를 만들어서 폴더에 넣어줍니다.
                    try { CreateAndSaveGraph(backupGraphPath); } catch { }
                    // =========================================================

                    // [영구 박제 로직] 설정 파일 병합
                    string baseConfigPath = Path.Combine(mycarPath, "config.py");
                    string myConfigPath = Path.Combine(mycarPath, "myconfig.py");
                    string savedConfigPath = Path.Combine(modelFolder, "final_config.txt");

                    try
                    {
                        var finalConfig = ParseConfigToDict(baseConfigPath);
                        var customConfig = ParseConfigToDict(myConfigPath);
                        foreach (var kvp in customConfig)
                        {
                            finalConfig[kvp.Key] = kvp.Value;
                        }

                        List<string> saveLines = new List<string>();
                        var sortedKeys = finalConfig.Keys.ToList();
                        sortedKeys.Sort();

                        foreach (var key in sortedKeys)
                        {
                            saveLines.Add($"{key} = {finalConfig[key]}");
                        }

                        File.WriteAllLines(savedConfigPath, saveLines);
                    }
                    catch { }

                    // 리스트뷰 업데이트
                    ListViewItem newItem = new ListViewItem(modelName);
                    newItem.SubItems.Add(displayType);
                    newItem.SubItems.Add(curDataset);
                    newItem.SubItems.Add(DateTime.Now.ToString("yy-MM-dd HH:mm"));
                    newItem.SubItems.Add(curTransfer);
                    newItem.SubItems.Add(curMemo);
                    newItem.ToolTipText = curMemo;

                    lvwModel.Items.Add(newItem);

                    // 콤보박스 추가 및 폼 초기화
                    cboSelectTransferModel.Items.Add(modelName);
                    txtComment.Clear();
                    cboSelectTransferModel.SelectedIndex = 0;
                    cboSelectModelType.SelectedIndex = 0;

                    txtModelName.Clear();
                    txtComment.PlaceholderText = "모델 설명 (예: 50에포크, 직선코스 학습)";

                    cboSelectModelType.Enabled = true;
                    lblTransferWarning.Visible = false;

                    await Task.Delay(1500);
                    prgTrain.Value = 0;

                    isTraining = false;
                    btnTrain.Enabled = true;
                    btnTrain.Text = "▶ 훈련 시작";
                    btnTrain.ForeColor = Color.White;
                    ToggleUIState(false);
                });

                trainProcess.Dispose();
            };

            // 10. 엔진 가동 시작!
            try
            {
                // 버튼을 활성화시켜서 취소(정지)할 수 있게 풀어줌
                btnTrain.Enabled = true;

                trainProcess.Start();
                trainProcess.BeginErrorReadLine();

                // ⭐ 실시간 스트림 가로채기 (비동기)
                Task.Run(() => ReadPythonStream(trainProcess.StandardOutput));

                ReportLog("알림", $"'{modelName}' 모델 학습 엔진 가동을 시작했습니다.");
            }
            catch (Exception ex)
            {
                ReportLog("오류", $"훈련 엔진 가동 실패: {ex.Message}");

                MessageBox.Show($"훈련 엔진을 시작하는 중에 에러가 발생했습니다.\n" +
                    $"파이썬 가상환경(env)이나 데이터 폴더에 문제가 있을 수 있습니다.",
                    "훈련 가동 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (Directory.Exists(modelFolderPath))
                {
                    try { Directory.Delete(modelFolderPath, true); } catch { }
                }

                isTraining = false;
                btnTrain.Enabled = true;
                btnTrain.Text = "▶ 훈련 시작";
                btnTrain.ForeColor = Color.White;
                ToggleUIState(false);
            }
        }

        private void LoadExistingModels()
        {
            lvwModel.Items.Clear();
            lvwModel.ShowItemToolTips = true;

            cboSelectTransferModel.Items.Clear();
            cboSelectTransferModel.Items.Add("None");
            cboSelectTransferModel.SelectedIndex = 0;

            string currentPath = Application.StartupPath;
            while (!Directory.Exists(Path.Combine(currentPath, "env")))
            {
                DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                if (parentInfo == null) return;
                currentPath = parentInfo.FullName;
            }

            string modelsPath = Path.Combine(currentPath, "mycar", "models");
            if (!Directory.Exists(modelsPath)) return;

            DirectoryInfo di = new DirectoryInfo(modelsPath);
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                string modelName = dir.Name;

                // ⭐ 핵심 수정 부분: 모델 폴더 안에 똑같은 이름의 중첩 폴더 경로 생성
                // 예: mycar\models\테스트모델\테스트모델
                string nestedFolderPath = Path.Combine(dir.FullName, modelName);

                // 안쪽 방(중첩 폴더)에 saved_model.pb가 있는지, 혹은 폴더 자체가 존재하는지 검사
                string pbFilePath = Path.Combine(nestedFolderPath, "saved_model.pb");

                // 만약 중첩 구조로 저장되었거나, 기존 방식대로 바깥에 pb가 있는 경우 모두 커버
                if (Directory.Exists(nestedFolderPath) || File.Exists(Path.Combine(dir.FullName, "saved_model.pb")))
                {
                    string dataset = "알 수 없음";
                    string modelType = "기본 주행 (Linear)"; // 기본값
                    string isTransfer = "-";
                    string memo = "저장되어 있던 모델";

                    // 영수증(meta.txt)은 보통 바깥 폴더(dir.FullName)에 있으므로 유지
                    string metaFilePath = Path.Combine(dir.FullName, "meta.txt");
                    if (File.Exists(metaFilePath))
                    {
                        try
                        {
                            string[] lines = File.ReadAllLines(metaFilePath);

                            // 3줄짜리 구형 영수증
                            if (lines.Length == 3)
                            {
                                dataset = lines[0];
                                isTransfer = lines[1];
                                memo = lines[2];
                            }
                            // 4줄짜리 신형 영수증 (종류 포함)
                            else if (lines.Length >= 4)
                            {
                                dataset = lines[0];
                                modelType = lines[1]; // 2번째 줄에서 종류 읽어오기
                                isTransfer = lines[2];
                                memo = lines[3];
                            }
                        }
                        catch { }
                    }

                    ListViewItem item = new ListViewItem(modelName);
                    item.SubItems.Add(modelType);
                    item.SubItems.Add(dataset);
                    item.SubItems.Add(dir.CreationTime.ToString("yy-MM-dd HH:mm"));
                    item.SubItems.Add(isTransfer);
                    item.SubItems.Add(memo);

                    // 🛠️ 수정된 코드
                    item.ToolTipText = modelName;
                    lvwModel.Items.Add(item);

                    cboSelectTransferModel.Items.Add(modelName);
                }
            }
        }

        private void cboSelectTransferModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBaseModel = cboSelectTransferModel.SelectedItem.ToString();

            // 1. 전이학습을 안 할 때 ("None" 선택)
            if (selectedBaseModel == "None")
            {
                cboSelectModelType.Enabled = true; // 콤보박스 잠금 해제
                lblTransferWarning.Visible = false; // 경고 숨기기
                return;
            }

            // 2. 전이학습할 모델을 선택했을 때
            foreach (ListViewItem item in lvwModel.Items)
            {
                if (item.Text == selectedBaseModel)
                {
                    string originalType = item.SubItems[1].Text;

                    for (int i = 0; i < cboSelectModelType.Items.Count; i++)
                    {
                        if (cboSelectModelType.Items[i].ToString() == originalType)
                        {
                            cboSelectModelType.SelectedIndex = i;
                            break;
                        }
                    }

                    cboSelectModelType.Enabled = false; // 콤보박스 잠금
                    lblTransferWarning.Visible = true; // 경고 띄우기!
                    break;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 1. [UX 방어막] 리스트뷰에서 아무것도 선택하지 않고 버튼을 눌렀을 때 컷!
            int selectedCount = lvwModel.SelectedItems.Count;
            if (selectedCount == 0)
            {
                MessageBox.Show("삭제할 모델을 먼저 선택해 주세요.", "선택 확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. [UX 방어막] 진짜 지울 것인지 사용자에게 다시 한번 물어보기
            // 단일 삭제와 대량 삭제일 때의 메시지를 유연하게 분기해 줍니다.
            string confirmMsg = selectedCount == 1
                ? $"'{lvwModel.SelectedItems[0].Text}' 모델을 정말로 삭제하시겠습니까?\n하드디스크의 실제 파일과 영수증이 모두 영구 삭제됩니다."
                : $"선택한 {selectedCount}개의 모델을 정말로 일괄 삭제하시겠습니까?\n하드디스크의 실제 파일과 영수증이 모두 영구 삭제됩니다.";

            DialogResult result = MessageBox.Show(confirmMsg, "모델 완전 삭제 경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No) return; // '아니오'를 누르면 삭제 취소

            // 3. 실제 컴퓨터의 모델 폴더 기준 경로 추적
            // ⭐ [성능 튜닝] 루프 안에서 매번 경로를 계산하면 느려지므로, 바깥에서 딱 한 번만 실행합니다.
            string currentPath = Application.StartupPath;
            while (!Directory.Exists(Path.Combine(currentPath, "env")))
            {
                DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                if (parentInfo == null) break;
                currentPath = parentInfo.FullName;
            }

            // 4. ⭐ [화면 버벅임 방지] 대량 삭제 시 리스트뷰가 깜빡거리는 현상을 잠급니다.
            lvwModel.BeginUpdate();

            int successCount = 0;

            try
            {
                // 5. ⭐ [핵심] 인덱스 꼬임 방지를 위한 역순(Reverse) for문!
                // SelectedIndices의 맨 마지막 원소부터 거꾸로 걸어 내려옵니다.
                for (int i = lvwModel.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    int targetIndex = lvwModel.SelectedIndices[i];
                    ListViewItem item = lvwModel.Items[targetIndex];
                    string modelName = item.Text; // 지울 모델명 추출

                    // 바깥쪽 폴더 경로 (mycar\models\모델명)
                    string modelFolderPath = Path.Combine(currentPath, "mycar", "models", modelName);

                    // 하드디스크에서 폴더 통째로 완전 삭제
                    if (Directory.Exists(modelFolderPath))
                    {
                        Directory.Delete(modelFolderPath, true);
                    }

                    // 폴더 밖(models)에 숨어있는 보너스 파일(.tflite, .png)도 추적해서 암살!
                    string tflitePath = Path.Combine(currentPath, "mycar", "models", $"{modelName}.tflite");
                    string pngPath = Path.Combine(currentPath, "mycar", "models", $"{modelName}.png");

                    if (File.Exists(tflitePath)) File.Delete(tflitePath);
                    if (File.Exists(pngPath)) File.Delete(pngPath);

                    // 6. UI 리스트뷰(표)에서 해당 인덱스의 줄 지우기
                    lvwModel.Items.RemoveAt(targetIndex);

                    // 7. 전이학습 선택 콤보박스 목록에서도 똑같이 지워줘서 동기화하기!
                    if (cboSelectTransferModel.Items.Contains(modelName))
                    {
                        cboSelectTransferModel.Items.Remove(modelName);
                    }

                    successCount++;
                }

                ReportLog("알림", $"{successCount}개의 모델이 성공적으로 완전 삭제되었습니다.");
            }
            catch (Exception ex)
            {
                // 에러가 나면 어느 모델을 지우다 터졌는지 역추적이 가능하도록 ex.ToString()을 남깁니다.
                ReportLog("오류", $"모델 일괄 삭제 중 일부 실패: {ex.ToString()}");

                MessageBox.Show("모델들을 삭제하는 중 에러가 발생했습니다.\n" +
                                "특정 모델의 폴더가 열려있거나, 파이썬 백그라운드 프로세스가 파일을 사용 중일 수 있습니다.\n" +
                                "폴더를 닫거나 잠시 후 다시 시도해 주세요.\n\n" +
                                $"[상세 원인] {ex.Message}",
                                "삭제 실패",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                // 8. ⭐ 작업이 다 끝났으니 잠갔던 리스트뷰 화면을 풀고 한 번에 새로고침합니다.
                lvwModel.EndUpdate();
            }
        }

        private void btnChgComment_Click(object sender, EventArgs e)
        {
            // 1. 리스트뷰에서 아무것도 선택하지 않았을 때 컷!
            if (lvwModel.SelectedItems.Count == 0)
            {
                MessageBox.Show("메모를 수정할 모델을 먼저 선택해 주세요.", "선택 확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. 선택된 줄(모델) 정보 가져오기
            ListViewItem selectedItem = lvwModel.SelectedItems[0];
            string modelName = selectedItem.Text;

            // 5번째 칸(index 5)에서 가져옵니다.
            string currentMemo = selectedItem.SubItems[5].Text;

            // 3. 수제 팝업창을 띄워서 새로운 메모 입력받기
            string newMemo = ShowInputBox("새로운 메모를 입력하세요:", "메모 수정", currentMemo);

            // 4. 내용이 바뀌었고, 빈칸이 아니라면 업데이트 진행!
            if (!string.IsNullOrWhiteSpace(newMemo) && newMemo != currentMemo)
            {
                try
                {
                    // 실제 컴퓨터의 meta.txt 파일 경로 추적
                    string currentPath = Application.StartupPath;
                    while (!Directory.Exists(Path.Combine(currentPath, "env")))
                    {
                        DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                        if (parentInfo == null) break;
                        currentPath = parentInfo.FullName;
                    }

                    // ⭐ 원래 사용자님 코드 그대로! 바깥쪽 폴더의 meta.txt를 정확히 조준합니다.
                    string metaFilePath = Path.Combine(currentPath, "mycar", "models", modelName, "meta.txt");

                    if (File.Exists(metaFilePath))
                    {
                        string[] lines = File.ReadAllLines(metaFilePath);

                        // 4줄 이상일 때 4번째 줄(인덱스 3) 덮어쓰기
                        if (lines.Length >= 4)
                        {
                            lines[3] = newMemo;
                            File.WriteAllLines(metaFilePath, lines);
                        }
                    }

                    // 화면의 리스트뷰 표 업데이트
                    selectedItem.SubItems[5].Text = newMemo;
                    selectedItem.ToolTipText = newMemo;

                    ReportLog("알림", "메모가 성공적으로 수정되었습니다.");
                }
                catch (Exception ex)
                {
                    // 1. 개발자용: 이번에도 역시 ex.Message가 아닌 ex.ToString()을 남겨서 어느 줄에서 터졌는지 기록합니다.
                    ReportLog("오류", $"메모 수정 실패: {ex.ToString()}");

                    // 2. 사용자용: 윈도우에서 가장 자주 발생하는 에러 원인을 친절하게 짚어줍니다.
                    MessageBox.Show("메모를 수정하는 중 에러가 발생했습니다.\n" +
                                    "해당 모델의 폴더가 열려있거나, 파이썬 백그라운드 프로세스가 파일을 사용 중일 수 있습니다.\n" +
                                    "폴더를 닫거나 잠시 후 다시 시도해 주세요.\n\n" +
                                    $"[상세 원인] {ex.Message}",
                                    "수정 실패",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        // ==========================================================
        // ⭐ C#용 수제 입력창(InputBox) 도우미 함수 
        // (반드시 btnChgComment_Click 함수 바깥쪽에 위치해야 합니다!)
        // ⭐ 화면 잘림을 원천 차단한 C#용 수제 입력창(InputBox)
        private string ShowInputBox(string text, string caption, string defaultValue)
        {
            Form prompt = new Form()
            {
                // 세로 높이를 130에서 160으로 아주 넉넉하게 늘렸습니다.
                ClientSize = new Size(380, 160),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                // 화면 배율이 달라도 비율을 유지하도록 돕는 옵션
                AutoScaleMode = AutoScaleMode.Dpi
            };

            Label textLabel = new Label() { Left = 16, Top = 20, Text = text, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 16, Top = 50, Width = 346, Text = defaultValue };

            // 버튼의 높이(Height = 35)를 명시적으로 주고 위치를 잡아줍니다.
            Button confirmation = new Button()
            {
                Text = "확인",
                Left = 262,
                Top = 105,
                Width = 100,
                Height = 35,
                DialogResult = DialogResult.OK
            };

            // ⭐ 핵심 마법: 윈도우가 창 크기를 제멋대로 늘리더라도 
            // 텍스트 박스는 가로로 늘어나고, 확인 버튼은 우측 하단에 찰싹 붙어있게 만듭니다!
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            confirmation.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);

            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void btnShowConf_Click(object sender, EventArgs e)
        {
            // 1. 리스트뷰에서 모델 선택 확인
            if (lvwModel.SelectedItems.Count == 0)
            {
                MessageBox.Show("설정을 확인할 모델을 먼저 선택해주세요.", "선택 확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string modelName = lvwModel.SelectedItems[0].Text;

            // 2. 경로 찾기 
            string currentPath = Application.StartupPath;
            while (!Directory.Exists(Path.Combine(currentPath, "env")))
            {
                DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                if (parentInfo == null) break;
                currentPath = parentInfo.FullName;
            }

            // ⭐ 훈련할 때 만들어둔 통합 박제 파일 경로!
            string savedConfigPath = Path.Combine(currentPath, "mycar", "models", modelName, "final_config.txt");

            if (!File.Exists(savedConfigPath))
            {
                // 1. 디버깅용 로그 남기기 (어떤 모델이 불량인지 정확히 기록!)
                ReportLog("오류", $"설정 파일 누락: '{modelName}' 모델에 final_config.txt가 존재하지 않음.");

                // 2. 사용자에게 알려주고 행동 멈추기
                MessageBox.Show($"'{modelName}' 모델의 통합 설정 파일(final_config.txt)을 찾을 수 없습니다.\n파일이 삭제되었거나 손상되었을 수 있습니다.",
                                "설정 파일 누락",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error); // 해당 모델을 당장 쓸 수 없으므로 Error 아이콘이 적절합니다.
                return;
            }

            // 3. 박제된 텍스트 파일을 딕셔너리로 변환 (아까 만든 ParseConfigToDict 함수 재활용!)
            var configData = ParseConfigToDict(savedConfigPath);

            // 4. 표(ListView) 뷰어 호출!
            ShowConfigListViewer(modelName, configData);
        }

        // ==========================================================
        // ⭐ 1. 파이썬 설정 파일 완벽 분석기 (멀티라인 & 주석 완벽 대응)
        private Dictionary<string, string> ParseConfigToDict(string filePath)
        {
            var dict = new Dictionary<string, string>();
            if (!File.Exists(filePath)) return dict;

            string[] lines = File.ReadAllLines(filePath);
            string currentKey = null;
            string currentValue = "";
            int bracketCount = 0; // 괄호의 짝을 추적합니다.

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                // 괄호가 다 닫혀있는데 빈 줄이거나 주석이면 그냥 통과
                if (bracketCount == 0 && (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")))
                    continue;

                // 방해되는 인라인 주석 제거 (문자열 안의 # 기호는 보호됨)
                string cleanLine = RemoveInlineComment(line);
                if (string.IsNullOrWhiteSpace(cleanLine)) continue;

                // 1. 새로운 '변수 = 값' 시작
                if (bracketCount == 0)
                {
                    int eqIndex = cleanLine.IndexOf('=');
                    if (eqIndex > 0)
                    {
                        currentKey = cleanLine.Substring(0, eqIndex).Trim();
                        currentValue = cleanLine.Substring(eqIndex + 1).Trim();
                    }
                    else continue;
                }
                // 2. 멀티라인 이어서 읽기 (딕셔너리나 리스트가 여러 줄일 때)
                else
                {
                    currentValue += " " + cleanLine; // 윗줄과 아랫줄을 한 줄로 예쁘게 합칩니다!
                }

                // 괄호가 열리고 닫힌 횟수를 셉니다.
                bracketCount = CountBrackets(currentValue);

                // 괄호가 완벽하게 모두 닫혔다면, 비로소 사전에 저장!
                if (bracketCount <= 0 && currentKey != null)
                {
                    dict[currentKey] = currentValue;
                    currentKey = null;
                    bracketCount = 0;
                }
            }
            return dict;
        }

        // ⭐ 2. 인라인 주석(#)만 정밀 타격해서 지워주는 도우미
        private string RemoveInlineComment(string line)
        {
            bool inSingle = false, inDouble = false;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '\'' && !inDouble) inSingle = !inSingle;
                else if (line[i] == '"' && !inSingle) inDouble = !inDouble;
                else if (line[i] == '#' && !inSingle && !inDouble)
                    return line.Substring(0, i).Trim(); // 진짜 주석을 만난 순간 그 앞부분까지만 자르기!
            }
            return line.Trim();
        }

        // ⭐ 3. 괄호의 짝을 계산해 주는 도우미
        private int CountBrackets(string text)
        {
            int count = 0;
            bool inSingle = false, inDouble = false;
            foreach (char c in text)
            {
                if (c == '\'' && !inDouble) inSingle = !inSingle;
                else if (c == '"' && !inSingle) inDouble = !inDouble;
                else if (!inSingle && !inDouble)
                {
                    if (c == '{' || c == '[' || c == '(') count++;
                    else if (c == '}' || c == ']' || c == ')') count--;
                }
            }
            return count;
        }
        // ==========================================================

        // ⭐ 2. 추출된 설정을 표(ListView) 형태로 예쁘게 띄워주는 뷰어 창
        private void ShowConfigListViewer(string modelName, Dictionary<string, string> configData)
        {
            Form viewer = new Form()
            {
                Text = $"[{modelName}] 최종 적용된 환경 설정",
                ClientSize = new Size(600, 700),
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false
            };

            // 텍스트 박스 대신 깔끔한 리스트뷰(표) 생성
            ListView lvConfig = new ListView()
            {
                View = View.Details,
                GridLines = true,       // 엑셀처럼 선 긋기
                FullRowSelect = true,   // 한 줄 전체 선택
                Dock = DockStyle.Fill,  // 창에 꽉 차게
                Font = new Font("Consolas", 10) // 개발자 폰트
            };

            // 열(Column) 2개 추가
            lvConfig.Columns.Add("항목 (Setting)", 250);
            lvConfig.Columns.Add("값 (Value)", 320);

            // 항목 이름(A-Z) 순서대로 정렬해서 보기 편하게 만들기
            var sortedKeys = configData.Keys.ToList();
            sortedKeys.Sort();

            // 표에 데이터 채워 넣기
            foreach (var key in sortedKeys)
            {
                ListViewItem item = new ListViewItem(key); // 첫 번째 칸 (항목)
                item.SubItems.Add(configData[key]);        // 두 번째 칸 (값)
                lvConfig.Items.Add(item);
            }

            viewer.Controls.Add(lvConfig);
            viewer.ShowDialog();
        }

        // ⭐ C#용 수제 그래프(성적표) 뷰어 도우미 함수
        // ⭐ 크기를 확 키우고 '전체 화면(최대화)' 기능을 켠 그래프 뷰어
        private void ShowGraphViewer(string modelName, string imagePath)
        {
            Form viewer = new Form()
            {
                Text = $"[{modelName}] 훈련 성적표 (Loss Graph)",
                // 기본 크기를 600x450에서 900x600으로 시원하게 키웠습니다!
                ClientSize = new Size(900, 600),
                StartPosition = FormStartPosition.CenterParent,
                // 이제 최대화 버튼(ㅁ)을 눌러서 모니터 꽉 차게 볼 수 있습니다.
                MaximizeBox = true,
                MinimizeBox = false,
                BackColor = Color.White
            };

            PictureBox pbGraph = new PictureBox()
            {
                Dock = DockStyle.Fill,
                // 창 크기를 드래그해서 늘리거나 줄여도 사진 비율이 깨지지 않고 자동으로 맞춰집니다.
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // 파일을 잠그지 않고 메모리로 읽어오는 안전한 방식 유지
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                pbGraph.Image = Image.FromStream(fs);
            }

            viewer.Controls.Add(pbGraph);
            viewer.ShowDialog();
        }

        private void btnTrainningHistory_Click(object sender, EventArgs e)
        {
            // 1. 리스트뷰에서 모델 선택 확인
            if (lvwModel.SelectedItems.Count == 0)
            {
                MessageBox.Show("그래프를 확인할 모델을 먼저 선택해주세요.", "선택 확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string modelName = lvwModel.SelectedItems[0].Text;

            // 2. 모델 폴더 경로 추적
            string currentPath = Application.StartupPath;
            while (!Directory.Exists(Path.Combine(currentPath, "env")))
            {
                DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                if (parentInfo == null) break;
                currentPath = parentInfo.FullName;
            }

            // 3. 3가지 버전의 png 파일 경로 준비
            // ① 정상 종료 시 동키카가 새로 뱉는 경로 (mycar\models\모델명\모델명.png)
            string pngPath = Path.Combine(currentPath, "mycar", "models", modelName, $"{modelName}.png");

            // ② 호환성 (예전 구형 모델 경로)
            string oldPngPath = Path.Combine(currentPath, "mycar", "models", $"{modelName}.png");

            // ⭐ ③ 우리가 새로 만든 '강제 종료 대비용 수제 그래프' 경로!
            string manualPngPath = Path.Combine(currentPath, "mycar", "models", modelName, "loss_graph_manual.png");

            // 4. 사진 파일이 있으면 뷰어 띄우기 (우리가 만든 수제 그래프를 1순위로 찾습니다!)
            if (File.Exists(manualPngPath))
            {
                // ⭐ 1순위: 우리가 직접 C#으로 그린 예쁜 한글 그래프
                ShowGraphViewer(modelName, manualPngPath);
            }
            else if (File.Exists(pngPath))
            {
                // 2순위: 동키카가 만든 오리지널 그래프 (신규 경로)
                ShowGraphViewer(modelName, pngPath);
            }
            else if (File.Exists(oldPngPath))
            {
                // 3순위: 동키카가 만든 오리지널 그래프 (구형 경로)
                ShowGraphViewer(modelName, oldPngPath);
            }
            else
            {
                // 파일이 셋 다 없을 때 (1에포크도 못 돌고 죽은 경우)
                ReportLog("경고", $"'{modelName}' 모델의 훈련 그래프 파일(png)이 존재하지 않습니다.");

                MessageBox.Show($"'{modelName}' 모델의 훈련 그래프(png) 파일이 존재하지 않습니다.\n학습이 도중에 너무 일찍 중단되었거나 파일이 이동되었을 수 있습니다.",
                                "그래프 파일 없음",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        // ⭐ 프로그램이 켜질 때 myconfig.py를 분석해서 Panel(flpConfCon)에 동적 생성해 주는 함수
        private void LoadConfigToUI()
        {
            string filePath = GetMyConfigPath();
            flpConfCon.Controls.Clear(); // 일단 UI 깨끗하게 비우기

            // 만약 처음 실행해서 파일이 없거나, 초기화 상태를 원하시면 아래 주석을 해제하세요.
            // File.WriteAllText(filePath, "# --- GUI User Settings ---\n"); return; // 💡 완전 초기화 모드

            if (!File.Exists(filePath)) return;

            // 스마트 파서 엔진으로 파이썬 파일 읽어오기
            var savedConfig = ParseConfigToDict(filePath);

            foreach (var setting in savedConfig)
            {
                // 파이썬 영어 변수명(예: MAX_EPOCHS)으로 한글 UI 이름(예: 반복 횟수)을 역추적합니다.
                string koreanKey = configMapping.FirstOrDefault(x => x.Value == setting.Key).Key;

                // 우리가 지원하는 설정값이고 값이 비어있지 않다면 UI 패널에 한 줄 추가!
                if (!string.IsNullOrEmpty(koreanKey))
                {
                    CreateConfigRowWithData(koreanKey, setting.Value);
                }
            }
        }

        // 복원 전용 동적 UI 생성 도우미 (btnAddConf_Click의 로직과 완벽히 일치함)
        private void CreateConfigRowWithData(string selectedKoreanKey, string savedValue)
        {
            flpConfCon.FlowDirection = FlowDirection.LeftToRight;
            flpConfCon.WrapContents = true;

            Panel rowPanel = new Panel { Height = 50, Margin = new Padding(4, 4, 4, 4) };

            ComboBox cboSelConf_New = new ComboBox
            {
                Name = "cboSelConf",
                Location = new Point(5, 13),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboSelConf_New.Items.AddRange(configMapping.Keys.ToArray());
            cboSelConf_New.SelectedItem = selectedKoreanKey; // 저장되어 있던 항목 선택!

            TextBox txtSetConf_New = new TextBox
            {
                Name = "txtSetConf",
                Text = savedValue // 저장되어 있던 수치 복원!
            };

            Button btnDelete = new Button
            {
                Name = "btnDelete",
                Text = "X",
                Width = 45,
                Height = 35,
                Font = new Font("맑은 고딕", 10, FontStyle.Bold),
                ForeColor = Color.Red,
                Padding = new Padding(0)
            };
            btnDelete.Click += (s, args) =>
            {
                flpConfCon.Controls.Remove(rowPanel);
                rowPanel.Dispose();
                SyncAllPanelSizes();
            };

            rowPanel.Controls.Add(cboSelConf_New);
            rowPanel.Controls.Add(txtSetConf_New);
            rowPanel.Controls.Add(btnDelete);
            flpConfCon.Controls.Add(rowPanel);

            SyncAllPanelSizes();
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            // 1. 리스트뷰에서 모델 선택 확인
            if (lvwModel.SelectedItems.Count == 0)
            {
                MessageBox.Show("이름을 변경할 모델을 먼저 선택해주세요.", "선택 확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ListViewItem selectedItem = lvwModel.SelectedItems[0];
            string oldName = selectedItem.Text;

            // ⭐ 1. 작성하신 루트 경로 동적 탐색 로직 
            string currentPath = Application.StartupPath;
            while (!Directory.Exists(Path.Combine(currentPath, "env")))
            {
                DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                if (parentInfo == null)
                {
                    // 1. 디버깅을 위한 영구적인 증거(로그)를 남깁니다.
                    ReportLog("치명적 오류", "파이썬 엔진 누락: 'env' 폴더를 찾을 수 없음. (경로 탐색 실패)");

                    // 2. 사용자에게 가장 강력한(Error) 시각적 경고를 줍니다.
                    MessageBox.Show("AI 훈련을 위한 핵심 파이썬 엔진('env' 폴더)을 찾을 수 없습니다.\n프로그램이 정상적으로 설치되었는지 확인해 주세요.",
                                    "시스템 치명적 오류",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error); // ⬅️ Warning이나 Information이 아닌 Error(빨간 X) 사용!
                    return;
                }
                currentPath = parentInfo.FullName;
            }

            // ⭐ 2. 찾아낸 루트 경로를 기반으로 mycarPath 완성!
            string mycarPath = Path.Combine(currentPath, "mycar");

            // 3. 커스텀 입력창 호출
            string rawInput = ShowInputBox("새로운 모델 이름을 입력하세요:", "모델 이름 변경", oldName);
            // ... 커스텀 입력창 호출 이후 ...
            string newName = rawInput.Trim();

            if (string.IsNullOrEmpty(newName) || oldName == newName) return;

            // ⭐ 강력한 방어막: 정규표현식으로 영어, 숫자, 언더바(_)만 통과시킵니다.
            if (!Regex.IsMatch(newName, @"^[a-zA-Z0-9_]+$"))
            {
                // 1. 백그라운드 로그 기록 (어떤 글자 때문에 막혔는지 증거를 남김)
                ReportLog("경고", $"이름 형식 오류 차단됨: 입력값 '{newName}' (영문, 숫자, 언더바 이외 문자 포함)");

                // 2. 사용자에게는 팝업창 띄우기
                MessageBox.Show("안정적인 AI 훈련을 위해 모델 이름은\n영어, 숫자, 언더바(_)만 사용할 수 있습니다.",
                                "이름 형식 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 즉시 중단
            }

            // 중복 체크 및 연쇄 이동 로직 시작...

            // 4. 경로 조립 및 중복 체크
            string modelsBaseDir = Path.Combine(mycarPath, "models");
            string oldFullDir = Path.Combine(modelsBaseDir, oldName);
            string newFullDir = Path.Combine(modelsBaseDir, newName);

            if (Directory.Exists(newFullDir))
            {
                ReportLog("오류", $"'{newName}'(이)라는 모델이 이미 존재합니다.");

                MessageBox.Show($"'{newName}'(이)라는 모델이 이미 존재합니다.\n다른 이름을 지정해주세요.",
                                "이름 중복",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 5. [연쇄 이동 시작] 바깥 폴더 -> 안쪽 폴더 -> 내부 파일들
                Directory.Move(oldFullDir, newFullDir);

                string oldInnerModelPath = Path.Combine(newFullDir, oldName);
                string newInnerModelPath = Path.Combine(newFullDir, newName);
                if (Directory.Exists(oldInnerModelPath))
                {
                    Directory.Move(oldInnerModelPath, newInnerModelPath);
                }

                DirectoryInfo di = new DirectoryInfo(newFullDir);
                foreach (FileInfo file in di.GetFiles())
                {
                    if (file.Name.StartsWith(oldName))
                    {
                        string newFileName = file.Name.Replace(oldName, newName);
                        file.MoveTo(Path.Combine(newFullDir, newFileName));
                    }
                }

                selectedItem.Text = newName;
                LoadExistingModels(); // (여기 내부에도 mycarPath 찾는 로직이 이미 동일하게 들어있겠군요!)

                ReportLog("알림", "이름이 성공적으로 변경되었습니다.");
            }
            catch (Exception ex)
            {
                // 1. 개발자용: 이번에도 역시 ex.Message가 아닌 ex.ToString()을 남겨서 어느 줄에서 터졌는지 기록합니다.
                ReportLog("오류", $"이름 변경 실패: {ex.ToString()}");

                // 2. 사용자용: 윈도우에서 가장 자주 발생하는 에러 원인을 친절하게 짚어줍니다.
                MessageBox.Show("모델 이름을 변경하는 중 에러가 발생했습니다.\n" +
                                "해당 모델의 폴더가 열려있거나, 파이썬 백그라운드 프로세스가 파일을 사용 중일 수 있습니다.\n" +
                                "폴더를 닫거나 잠시 후 다시 시도해 주세요.\n\n" +
                                $"[상세 원인] {ex.Message}",
                                "이름 변경 실패",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // 1초마다 타이머가 호출하는 함수
        private void PlaceholderTimer_Tick(object sender, EventArgs e)
        {
            UpdatePlaceholder();
        }

        // 회색 힌트 글씨를 현재 시간으로 새로고침 하는 함수
        private void UpdatePlaceholder()
        {
            // 초 단위(yyMMddHHmm) 실시간 포맷
            txtModelName.PlaceholderText = $"mypilot_{DateTime.Now.ToString("yyMMdd_HHmmss")}";
        }

        private void ReadPythonStream(StreamReader reader)
        {
            StringBuilder sb = new StringBuilder();
            int charCode;

            while ((charCode = reader.Read()) > -1)
            {
                char c = (char)charCode;

                if (c == '\r' || c == '\n')
                {
                    string line = sb.ToString();
                    sb.Clear();

                    // ⭐ [핵심] 텐서플로우가 몰래 넣은 특수 기호(ANSI 코드) 완벽 제거!
                    string cleanLine = Regex.Replace(line, @"\x1B(?:[@-Z\\-_]|\[[0-?]*[ -/]*[@-~])", "").Trim();

                    if (string.IsNullOrWhiteSpace(cleanLine)) continue;

                    // 일반 스트림 앞문 방어
                    if (cleanLine.Contains("WARNING:") || cleanLine.Contains("OneAPI")) continue;

                    // ==============================================================
                    // 📺 로그 창에 데이터 쏘기 (진행률 바 스마트 덮어쓰기)
                    // ==============================================================
                    if (chkShowTrainLog.Checked && dynamicLogWindow != null && !dynamicLogWindow.IsDisposed)
                    {
                        bool isProgressBar = cleanLine.Contains("[=") || cleanLine.Contains("ETA:") || cleanLine.Contains("step - loss:");

                        dynamicLogWindow.BeginInvoke((MethodInvoker)delegate
                        {
                            if (isProgressBar)
                            {
                                // ⭐ [핵심] 찌꺼기 방지 역탐색 추적 시스템!
                                // 무조건 마지막 줄만 보는 게 아니라, 최근 5줄을 뒤져서 '아직 안 끝난(ETA:)' 프로그레스 바를 찾습니다.
                                ListViewItem targetItem = null;
                                for (int i = dynamicLvLogs.Items.Count - 1; i >= Math.Max(0, dynamicLvLogs.Items.Count - 5); i--)
                                {
                                    // "ETA:" 글자가 포함되어 있다면 아직 달리고 있는(미완성) 프로그레스 바입니다.
                                    if (dynamicLvLogs.Items[i].SubItems[1].Text.Contains("ETA:"))
                                    {
                                        targetItem = dynamicLvLogs.Items[i];
                                        break; // 찾았으면 더 위로 올라갈 필요 없이 중지!
                                    }
                                }

                                if (targetItem != null)
                                {
                                    // 새치기한 INFO 로그들이 밑에 깔려 있더라도, 기어코 찾아내서 텍스트만 깔끔하게 갈아끼웁니다!
                                    targetItem.SubItems[1].Text = cleanLine;
                                }
                                else
                                {
                                    // 안 끝난 프로그레스 바가 없으면 (새로운 에포크의 시작이라면) 쿨하게 새 줄 추가
                                    ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
                                    item.SubItems.Add(cleanLine);
                                    dynamicLvLogs.Items.Add(item);
                                }
                            }
                            else
                            {
                                // 프로그레스 바가 아닌 일반 텍스트(Epoch, INFO 등)는 무조건 새 줄로 추가
                                ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
                                item.SubItems.Add(cleanLine);
                                dynamicLvLogs.Items.Add(item);
                            }

                            // 항상 최신 로그가 보이도록 스크롤
                            dynamicLvLogs.Items[dynamicLvLogs.Items.Count - 1].EnsureVisible();
                        });
                    }
                    // ==============================================================

                    // 깨끗해진 텍스트를 하리님의 실시간 파싱 함수로 보냅니다!
                    ParseRealTimeStep(cleanLine);
                }
                else
                {
                    sb.Append(c);
                }
            }
        }

        private void ParseRealTimeStep(string line)
        {
            // 1. 에포크 전체 진행도 (프로그레스 바 갱신)
            Match epochMatch = Regex.Match(line, @"Epoch (\d+)/(\d+)");
            if (epochMatch.Success)
            {
                int currentEpoch = int.Parse(epochMatch.Groups[1].Value);
                int totalEpoch = int.Parse(epochMatch.Groups[2].Value);

                this.BeginInvoke((MethodInvoker)async delegate
                {
                    lblEpoch.Text = $"반복횟수 : {currentEpoch}/{totalEpoch}";

                    int targetMax = totalEpoch * 100;
                    int targetValue = currentEpoch * 100;

                    if (prgTrain.Maximum != targetMax) prgTrain.Maximum = targetMax;
                    if (prgTrain.Value > targetValue) prgTrain.Value = targetValue;

                    for (int i = prgTrain.Value; i <= targetValue; i += 2)
                    {
                        if (i > prgTrain.Maximum) i = prgTrain.Maximum;
                        prgTrain.Value = i;
                        await Task.Delay(1);
                    }
                });
                return;
            }

            // ⭐ 2. 신기록 달성 낚아채기 (Keras 자체 출력 활용)
            Match improveMatch = Regex.Match(line, @"val_loss improved from .* to ([0-9.]+)");
            if (improveMatch.Success)
            {
                double minVal = double.Parse(improveMatch.Groups[1].Value);
                this.BeginInvoke((MethodInvoker)delegate
                {
                    lblMinValLoss.Text = $"최소 테스트 오차율 : {minVal:F4}";
                    lblMinValLoss.ForeColor = Color.Green;
                    lblMinValLoss.Font = new Font(lblMinValLoss.Font, FontStyle.Bold);
                });
                return;
            }

            // ⭐ 3. 정체 구간 낚아채기 (기록 경신 실패 시 색상 원상 복구)
            Match notImproveMatch = Regex.Match(line, @"val_loss did not improve from ([0-9.]+)");
            if (notImproveMatch.Success)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    lblMinValLoss.ForeColor = Color.Black;
                    lblMinValLoss.Font = new Font(lblMinValLoss.Font, FontStyle.Regular);
                });
                return;
            }

            // 4. 에포크 완료 시점의 최종 오차율 갱신
            if (line.Contains("- loss:") && line.Contains("- val_loss:"))
            {
                Match lossMatch = Regex.Match(line, @"-\s*loss:\s*([0-9.]+)");
                Match valLossMatch = Regex.Match(line, @"-\s*val_loss:\s*([0-9.]+)");

                if (lossMatch.Success && valLossMatch.Success)
                {
                    double finalLoss = double.Parse(lossMatch.Groups[1].Value);
                    double valLoss = double.Parse(valLossMatch.Groups[1].Value);

                    // ⭐ 파싱한 숫자를 수첩에 순서대로 추가합니다!
                    historyLoss.Add(finalLoss);
                    historyValLoss.Add(valLoss);

                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        lblLoss.Text = $"학습 오차율 : {finalLoss:F4}";
                        lblValLoss.Text = $"테스트 오차율 : {valLoss:F4}";
                    });
                }
                return;
            }

            // 5. 실시간 스텝 진행 중 (val_loss 없이 loss만 갱신될 때)
            if (line.Contains("- loss:"))
            {
                Match stepMatch = Regex.Match(line, @"-\s*loss:\s*([0-9.]+)");
                if (stepMatch.Success)
                {
                    double currentLoss = double.Parse(stepMatch.Groups[1].Value);
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        lblLoss.Text = $"학습 오차율 : {currentLoss:F4}";
                    });
                }
            }
        }

        private void btnConfigHelp_Click(object sender, EventArgs e)
        {
            // 긴 문자열을 예쁘게 조립하기 위한 StringBuilder
            StringBuilder helpMessage = new StringBuilder();
            helpMessage.AppendLine("[ ⚙️ 전체 구성 설정(Config) 가이드 ]\n");

            // 1. 화면의 콤보박스를 뒤질 필요 없이, 백과사전(딕셔너리) 자체를 처음부터 끝까지 순회합니다.
            // configHelpDocs에 저장된 모든 Key(설정 이름)와 Value(설명)를 싹 다 가져옵니다.
            foreach (var doc in configHelpDocs)
            {
                helpMessage.AppendLine($"[{doc.Key}]");
                helpMessage.AppendLine(doc.Value);
                helpMessage.AppendLine(); // 줄바꿈으로 단락 분리
            }

            // 2. 만약 백과사전이 텅 비어있을 경우의 안전 장치
            if (configHelpDocs.Count == 0)
            {
                MessageBox.Show("등록된 도움말 데이터가 없습니다.", "도움말", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 3. 싹 다 모은 설명들을 하나의 팝업창으로 시원하게 띄워줍니다!
            MessageBox.Show(helpMessage.ToString(), "전체 설정 도움말", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateAndSaveGraph(string savePath)
        {
            // 점수가 아예 없으면(1에포크도 못 돌았으면) 그리지 않고 함수 종료
            if (historyLoss.Count == 0) return;

            // 1. 메모리 상에 투명한 도화지(Chart)를 생성합니다.
            using (var chart = new System.Windows.Forms.DataVisualization.Charting.Chart())
            {
                chart.Width = 800;  // 사진 가로 크기
                chart.Height = 600; // 사진 세로 크기
                chart.BackColor = Color.White; // 폰트가 잘 보이도록 배경색을 흰색으로 고정

                var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();

                // X축 (가로) 한국어 설정 + 폰트 키우기
                chartArea.AxisX.Title = "반복 횟수 (Epoch)";
                chartArea.AxisX.TitleFont = new Font("맑은 고딕", 12, FontStyle.Bold);
                chartArea.AxisX.MajorGrid.Enabled = false; // ⭐ 가로 눈금선 제거!

                // Y축 (세로) 한국어 설정 + 폰트 키우기
                chartArea.AxisY.Title = "오차율 (Loss)";
                chartArea.AxisY.TitleFont = new Font("맑은 고딕", 12, FontStyle.Bold);
                chartArea.AxisY.MajorGrid.Enabled = false; // ⭐ 세로 눈금선 제거!

                chart.ChartAreas.Add(chartArea);

                // ⭐ 그래프 맨 위에 멋진 전체 제목 달아주기
                chart.Titles.Add(new System.Windows.Forms.DataVisualization.Charting.Title(
                    "학습 및 테스트 오차율 변화",
                    System.Windows.Forms.DataVisualization.Charting.Docking.Top,
                    new Font("맑은 고딕", 15, FontStyle.Bold),
                    Color.Black
                ));

                // ⭐ 범례(Legend) 폰트도 한글이 잘 보이게 수정
                var legend = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend1");
                legend.Font = new Font("맑은 고딕", 10, FontStyle.Regular);
                legend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top; // 범례를 위쪽으로 올려서 공간 확보
                chart.Legends.Add(legend);

                // 2. 파란선 (학습 오차율) 세팅
                var seriesLoss = new System.Windows.Forms.DataVisualization.Charting.Series("학습 오차율 (Train Loss)");
                seriesLoss.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                seriesLoss.Color = Color.Blue;
                seriesLoss.BorderWidth = 3;

                // 3. 주황선 (테스트 오차율) 세팅
                var seriesValLoss = new System.Windows.Forms.DataVisualization.Charting.Series("테스트 오차율 (Val Loss)");
                seriesValLoss.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                seriesValLoss.Color = Color.DarkOrange;
                seriesValLoss.BorderWidth = 3;

                // 4. 수첩에 적어둔 점수를 하나씩 꺼내서 점을 찍습니다.
                for (int i = 0; i < historyLoss.Count; i++)
                {
                    seriesLoss.Points.AddXY(i + 1, historyLoss[i]);

                    // 혹시라도 배열 길이가 달라서 에러가 나는 것을 방지
                    if (i < historyValLoss.Count)
                    {
                        seriesValLoss.Points.AddXY(i + 1, historyValLoss[i]);
                    }
                }

                chart.Series.Add(seriesLoss);
                chart.Series.Add(seriesValLoss);

                // 5. 완성된 도화지를 지정된 경로에 PNG 파일로 구워냅니다!
                chart.SaveImage(savePath, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            }
        }

        // ⭐ 리스트박스에서 키보드가 눌렸을 때 실행되는 이벤트
        private void lstModels_KeyDown(object sender, KeyEventArgs e)
        {
            // 1. 눌린 키가 'Delete' 키인지 확인
            if (e.KeyCode == Keys.Delete)
            {
                // ⭐ 기존에 만들어둔 삭제 버튼을 "프로그램이 대신 마우스로 클릭" 해줍니다!
                btnDelete.PerformClick();
            }

            // 💡 컨트롤(Ctrl) 키가 눌려있는 상태일 때만 내부 조사를 시작합니다.
            if (e.Control)
            {
                // 1️⃣ [Ctrl + 1] ➔ 이름 변경 (btnRename)
                if ((e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && btnRename.Enabled)
                {
                    btnRename_Click(sender, e);
                    SetKeyHandled(e);
                }

                // 2️⃣ [Ctrl + 2] ➔ 메모 변경 (btnChgComment)
                else if ((e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2) && btnChgComment.Enabled)
                {
                    btnChgComment_Click(sender, e);
                    SetKeyHandled(e);
                }

                // 3️⃣ [Ctrl + 3] ➔ 구성 표시 (btnShowConf)
                else if ((e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3) && btnShowConf.Enabled)
                {
                    btnShowConf_Click(sender, e);
                    SetKeyHandled(e);
                }

                // 4️⃣ [Ctrl + 4] ➔ 훈련 기록 보기 (btnTrainningHistory)
                else if ((e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4) && btnTrainningHistory.Enabled)
                {
                    btnTrainningHistory_Click(sender, e);
                    SetKeyHandled(e);
                }
            }
        }

        private void btnModelTypeHelp_Click(object sender, EventArgs e)
        {
            // 문자열을 깔끔하게 조립하기 위한 StringBuilder
            StringBuilder modelHelp = new StringBuilder();

            modelHelp.AppendLine("[ 🚗 동키카 인공지능 주행 모델 가이드 ]\n");

            modelHelp.AppendLine("■ 1. 기본 주행 (Linear)");
            modelHelp.AppendLine(" - 가장 무난한 표준 모델입니다. 사진 1장을 보고 핸들(조향)과 엑셀(속도)을 동시에 학습합니다.");
            modelHelp.AppendLine(" - 일반적인 트랙에서 가장 안정적인 결과를 보여줍니다.\n");

            modelHelp.AppendLine("■ 2. 분류형 주행 (Categorical)");
            modelHelp.AppendLine(" - 핸들 각도를 수십 개의 칸으로 쪼개서 확률로 계산합니다.");
            modelHelp.AppendLine(" - 사람이 부드럽게 꺾지 못하고 뚝뚝 끊어서 조종했어도 커브를 매끄럽게 잘 돕니다.\n");

            modelHelp.AppendLine("■ 3. 기억형 주행 (RNN)");
            modelHelp.AppendLine(" - 과거 3~4장의 사진 흐름을 기억하여 차체의 궤적과 속도감을 인지합니다.");
            modelHelp.AppendLine(" - 훈련 속도가 조금 느리며, 라즈베리파이 사양에 따라 실제 주행 시 약간의 렉이 있을 수 있습니다.\n");

            modelHelp.AppendLine("■ 4. 추론형 주행 (Inferred) ★강력 추천");
            modelHelp.AppendLine(" - 인간의 불안정한 속도 데이터는 버리고, 오직 '핸들링'만 집중 학습합니다.");
            modelHelp.AppendLine(" - 속도는 핸들을 많이 꺾으면 알아서 줄어들도록 수학 공식이 제어하므로 주행이 아주 스마트합니다.\n");

            modelHelp.AppendLine("■ 5. 입체 시각 주행 (3D)");
            modelHelp.AppendLine(" - 시간과 공간을 입체적으로 분석하는 끝판왕 모델입니다.");
            modelHelp.AppendLine(" - 뇌 용량이 너무 커서 적은 데이터로는 트랙을 통째로 '암기'해버려 조기 종료(Early Stop)가 빨리 일어납니다. 아주 방대한 데이터가 필요합니다.");

            // 깔끔하고 신뢰감 주는 정보(Information) 아이콘으로 메시지박스 팝업!
            MessageBox.Show(modelHelp.ToString(), "훈련 모델 상세 안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ① 마우스를 꾹 눌렀을 때
        private void lvwModel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) isDraggingModels = true;
        }

        // ② 마우스를 누른 채로 슉슉 움직일 때 (여기가 핵심!)
        private void lvwModel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingModels)
            {
                // 마우스 커서가 지나가는 위치의 아이템(모델 이름)을 찾습니다.
                ListViewItem item = lvwModel.GetItemAt(e.X, e.Y);

                if (item != null)
                {
                    // 💡 지나가는 자리의 모델을 파란색(선택 상태)으로 칠합니다!
                    item.Selected = true;
                }
            }
        }

        // ③ 마우스에서 손을 뗐을 때
        private void lvwModel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) isDraggingModels = false;
        }

        private void lvwModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 현재 선택된 항목 개수
            int selectedCount = lvwModel.SelectedItems.Count;

            // ⭐ 0개(선택 없음)이거나 1개일 때만 true!
            // 즉, 2개 이상 다중 드래그했을 때만 이름/메모 변경을 락(lock) 겁니다.
            bool allowSingleActions = (selectedCount <= 1);

            // 0개일 때는 이벤트 내부의 방어 코드가 작동하고, 1개일 때는 정상 작동합니다!
            btnRename.Enabled = allowSingleActions;           // 이름 변경
            btnChgComment.Enabled = allowSingleActions;       // 메모 변경
            btnShowConf.Enabled = allowSingleActions;         // 구성 표시
            btnTrainningHistory.Enabled = allowSingleActions; // 훈련 기록


            // 삭제 버튼은 0개일 때도 방어 코드가 있으니 켜두고, 1개 이상일 때도 당연히 켜둡니다.
            // 결론적으로 삭제 버튼은 어떤 상황에서든 항상 켜두면 됩니다!
            btnDelete.Enabled = true;
        }

        // 💡 중복되는 키 처리 완료 코드를 깔끔하게 묶어주는 헬퍼 함수
        private void SetKeyHandled(KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true; // 윈도우 특유의 '띵~' 거리는 경고음 방지
        }
    }
}
