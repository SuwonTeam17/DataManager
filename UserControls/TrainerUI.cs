
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions; // 이거 하나만 맨 위에 추가해주세요!

namespace DataManager.UserControls
{
    public partial class TrainerUI : UserControl
    {

        // ⭐ 실시간 힌트를 띄워줄 백그라운드 타이머
        private System.Windows.Forms.Timer placeholderTimer;

        // 클래스 내부 최상단 멤버 변수 구역에 넣어주세요
        public event Action<string, string, string> OnLogReported;

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

        private void TrainerUI_Load(object sender, EventArgs e)
        {
            // 1. 모델 종류 콤보박스 세팅
            cboSelectModelType.Items.Clear();
            cboSelectModelType.Items.Add("기본 주행 (Linear)");
            cboSelectModelType.Items.Add("분류형 주행 (Categorical)");
            cboSelectModelType.Items.Add("기억형 주행 (RNN)");
            cboSelectModelType.Items.Add("지시형 주행 (Behavior)");
            cboSelectModelType.Items.Add("센서 융합 주행 (IMU)");
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
            btnTrain.Enabled = false;
            btnTrain.Text = "학습 중...";

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
                    btnTrain.Enabled = true;
                    btnTrain.Text = "▶ 훈련 시작";
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
            string rootPath = currentPath;
            string mycarPath = $@"{rootPath}\mycar";

            // 3. 자동 모델 이름 생성 및 메모 박제
            string timestamp = DateTime.Now.ToString("yyMMdd_HHmmss");
            string defaultMemo = "";

            // 1. 전이학습 콤보박스에서 특정 모델을 선택했다면? (0번 인덱스가 '선택 안 함'이라고 가정)
            if (cboSelectTransferModel.SelectedIndex > 0)
            {
                string baseModel = cboSelectTransferModel.SelectedItem.ToString();
                defaultMemo = $"[{DateTime.Now.ToString("MM/dd HH:mm")}] '{baseModel}' 기반 이어서 학습";
            }
            else
            {
                // 2. 아무것도 선택하지 않은 백지상태 학습이라면?
                defaultMemo = $"[{DateTime.Now.ToString("MM/dd HH:mm")}] 신규 베이스 모델";
            }

            // 3. 최종 결정 (사용자가 쓴 글이 있으면 그걸 쓰고, 없으면 방금 조립한 defaultMemo를 씁니다)
            string curMemo = string.IsNullOrWhiteSpace(txtComment.Text) ? defaultMemo : txtComment.Text.Trim();

            // 4. 모델 종류(--type) 파라미터 번역
            string selectedType = "linear";
            string displayType = cboSelectModelType.SelectedItem.ToString();

            if (displayType.Contains("Categorical")) selectedType = "categorical";
            else if (displayType.Contains("RNN")) selectedType = "rnn";
            else if (displayType.Contains("Behavior")) selectedType = "behavior";
            else if (displayType.Contains("IMU")) selectedType = "imu";
            else if (displayType.Contains("3D")) selectedType = "3d";

            // ⭐ 5. 전이학습(--transfer) 파라미터 확인 (꼼수 제거, 공식 옵션 적용!)
            string curTransfer = "X";
            string transferCommand = "";

            // 2. 콤보박스에서 1번째 이상(실제 모델)을 선택했을 때만 경로 탐색을 시작합니다.
            if (cboSelectTransferModel.SelectedIndex > 0)
            {
                string selectedBaseModel = cboSelectTransferModel.SelectedItem.ToString();
                string modelsBaseDir = Path.Combine(mycarPath, "models");

                string nestedPath = Path.Combine(modelsBaseDir, selectedBaseModel, selectedBaseModel);
                string flatPath = Path.Combine(modelsBaseDir, selectedBaseModel);

                string targetTransferPath = "";

                // 3. 모델 구조 탐색 (아까 작성한 3중 방어막)
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
                    return; // 훈련 중단
                }

                // 4. 전이학습을 한다고 했으니 명령어를 채워줍니다.
                transferCommand = $" --transfer \"{targetTransferPath}\"";
                curTransfer = selectedBaseModel;
            }

            // CPU 모드일 때는 CUDA와 DML 양쪽 모두에게 -1을 주어 GPU를 완벽하게 숨깁니다.
            string cudaCommand = rdoUseCPU.Checked
                ? "set CUDA_VISIBLE_DEVICES=-1 && set DML_VISIBLE_DEVICES=-1 && "
                : "";

            // 7. 모델 이름 결정하기
            string customName = txtModelName.Text.Trim();
            string modelName = "";

            if (!string.IsNullOrEmpty(customName))
            {
                // ⭐ 새로 추가된 강력한 방어막 (영어, 숫자, 언더바만 허용)
                if (!Regex.IsMatch(customName, @"^[a-zA-Z0-9_]+$"))
                {
                    // 1. 백그라운드 로그 기록 (어떤 글자 때문에 막혔는지 증거를 남김)
                    ReportLog("경고", $"이름 형식 오류 차단됨: 입력값 '{customName}' (영문, 숫자, 언더바 이외 문자 포함)");

                    // 2. 사용자에게는 팝업창 띄우기
                    MessageBox.Show("안정적인 AI 훈련을 위해 모델 이름은\n영어, 숫자, 언더바(_)만 사용할 수 있습니다.",
                                    "이름 형식 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnTrain.Enabled = true;
                    btnTrain.Text = "학습 시작";
                    return; // 훈련 즉시 중단
                }
                modelName = customName;
            }
            else
            {
                // 빈칸이면 안전한 날짜 형식으로 자동 생성
                modelName = $"mypilot_{DateTime.Now.ToString("yyMMdd_HHmmss")}";
            }

            // 🚨 방어막 2: 사용자가 적은 이름의 모델이 이미 존재하는지 검사 (덮어쓰기 방지)
            string modelFolderPath = Path.Combine(mycarPath, "models", modelName);
            if (Directory.Exists(modelFolderPath))
            {
                // 1. 시스템 기록용으로 로그를 조용히 남깁니다.
                ReportLog("오류", $"이름 중복: '{modelName}' 모델이 이미 존재함.");

                // 2. 사용자에게는 강력하게 경고창을 띄워 행동을 멈춥니다.
                MessageBox.Show($"'{modelName}'(이)라는 모델이 이미 존재합니다.\n다른 이름을 지정해주세요.",
                                "이름 중복",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                btnTrain.Enabled = true;
                btnTrain.Text = "학습 시작";
                return; // 훈련 중단
            }

            // 2. Directory.Exists로 폴더가 '없는' 상태인지 확인합니다.
            if (!Directory.Exists(modelFolderPath))
            {
                // 3. 폴더가 없다면 새로 만듭니다. (상위 폴더가 없다면 상위 폴더까지 한 번에 다 만들어 줍니다)
                Directory.CreateDirectory(modelFolderPath);
            }



            // ⭐ 6. 최종 명령어 조립 (python train.py -> donkey train 으로 변경)
            string windowsCommand = $"cd /d \"{mycarPath}\" && \"..\\env\\Scripts\\activate\" && {cudaCommand}donkey train --tub \"{tubPath}\" --model \"models\\{modelName}\\{modelName}\" --type {selectedType}{transferCommand}";


            // 7. 백그라운드 프로세스 세팅
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.Arguments = $"/c \"{windowsCommand}\"";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            string pythonErrorMessage = "";

            Process process = new Process();
            process.StartInfo = psi;

            prgTrain.Value = 0;
            prgTrain.Maximum = 100;

            // 8. 훈련 게이지 가로채기
            process.OutputDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Match match = Regex.Match(args.Data, @"Epoch (\d+)/(\d+)");
                    if (match.Success)
                    {
                        int currentEpoch = int.Parse(match.Groups[1].Value);
                        int totalEpoch = int.Parse(match.Groups[2].Value);

                        this.BeginInvoke((MethodInvoker)async delegate
                        {
                            int targetMax = totalEpoch * 100;
                            int targetValue = currentEpoch * 100;

                            if (prgTrain.Maximum != targetMax) prgTrain.Maximum = targetMax;

                            for (int i = prgTrain.Value; i <= targetValue; i += 2)
                            {
                                if (i > prgTrain.Maximum) i = prgTrain.Maximum;
                                prgTrain.Value = i;
                                await Task.Delay(1);
                            }
                        });
                    }
                }
            };

            process.ErrorDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    // 파일에 안 쓰고 메모리(변수)에 바로 누적시킵니다.
                    pythonErrorMessage += args.Data + Environment.NewLine;
                }
            };

            // 9. 파이썬 학습 종료 이벤트 
            process.EnableRaisingEvents = true;
            process.Exited += (s, args) =>
            {
                // ⭐ 핵심 수정: 프로세스가 메모리에서 날아가기 전에 '종료 코드'를 미리 안전하게 복사해 둡니다!
                int savedExitCode = process.ExitCode;

                // 🚨 [방어막] 비정상 종료 시(에러 등)
                if (savedExitCode != 0)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        // 화면의 로그 기록에는 안전하게 빼둔 savedExitCode를 사용합니다.
                        ReportLog("Error", $"학습 중 오류 발생! (종료 코드: {savedExitCode})");

                        string displayError = string.IsNullOrWhiteSpace(pythonErrorMessage)
                                              ? "알 수 없는 이유로 프로세스가 강제 종료되었습니다. (메모리 부족 등)"
                                              : pythonErrorMessage.Trim();

                        MessageBox.Show(
                            displayError,
                            "훈련 실패 원인",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        prgTrain.Value = 0;

                        // 껍데기 폴더는 예정대로 미련 없이 삭제
                        if (Directory.Exists(modelFolderPath))
                        {
                            try { Directory.Delete(modelFolderPath, true); } catch { }
                        }

                        btnTrain.Enabled = true;
                        btnTrain.Text = "학습 시작";
                    });

                    // 화면 스레드가 변수를 읽든 말든, 본체는 여기서 안전하게 삭제됩니다.
                    process.Dispose();
                    return;
                }

                // 정상 종료 시 마무리 작업
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

                    ReportLog("Success", $"학습 완료! '{modelName}' 이름으로 저장되었습니다.");

                    // 4줄 영수증 기록
                    string curDataset = Path.GetFileName(tubPath);
                    string modelFolder = Path.Combine(mycarPath, "models", modelName);
                    string metaFilePath = Path.Combine(modelFolder, "meta.txt");

                    try { File.WriteAllLines(metaFilePath, new string[] { curDataset, displayType, curTransfer, curMemo }); }
                    catch { }

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

                    btnTrain.Enabled = true;
                    btnTrain.Text = "학습 시작";
                });

                process.Dispose();
            };

            // 10. 엔진 가동 시작!
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                ReportLog("알림", $"'{modelName}' 모델 학습 엔진 가동을 시작했습니다.");
            }
            catch (Exception ex)
            {
                ReportLog("오류", $"훈련 엔진 가동 실패: {ex.Message}");

                MessageBox.Show($"훈련 엔진을 시작하는 중에 에러가 발생했습니다.\n" +
                    $"파이썬 가상환경(env)이나 데이터 폴더에 문제가 있을 수 있습니다.",
                    "훈련 가동 실패",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // ⭐ [추가된 로직] 애초에 엔진 가동조차 실패했을 때도 껍데기 폴더 삭제!
                if (Directory.Exists(modelFolderPath))
                {
                    try { Directory.Delete(modelFolderPath, true); } catch { }
                }

                btnTrain.Enabled = true;
                btnTrain.Text = "학습 시작";
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
            if (lvwModel.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 모델을 먼저 선택해 주세요.", "선택 확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. 선택된 줄(모델) 정보 가져오기
            ListViewItem selectedItem = lvwModel.SelectedItems[0];
            string modelName = selectedItem.Text; // 첫 번째 칸 (모델명)

            // 3. [UX 방어막] 진짜 지울 것인지 사용자에게 다시 한번 물어보기
            DialogResult result = MessageBox.Show(
                $"'{modelName}' 모델을 정말로 삭제하시겠습니까?\n하드디스크의 실제 파일과 영수증이 모두 영구 삭제됩니다.",
                "모델 완전 삭제 경고",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No) return; // '아니오'를 누르면 삭제 취소

            try
            {
                // 4. 실제 컴퓨터의 모델 폴더 경로 추적
                string currentPath = Application.StartupPath;
                while (!Directory.Exists(Path.Combine(currentPath, "env")))
                {
                    DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                    if (parentInfo == null) break;
                    currentPath = parentInfo.FullName;
                }

                // 바깥쪽 폴더 경로 (mycar\models\모델명)
                string modelFolderPath = Path.Combine(currentPath, "mycar", "models", modelName);

                // 5. 하드디스크에서 폴더 통째로 완전 삭제
                if (Directory.Exists(modelFolderPath))
                {
                    // ⭐ true 옵션: 바깥 폴더를 지우면 그 안의 meta.txt와 '중첩된 모델 폴더'까지 한 방에 날아갑니다.
                    Directory.Delete(modelFolderPath, true);
                }

                // ==========================================================
                // 5-1. 폴더 밖(models)에 숨어있는 보너스 파일(.tflite, .png)도 추적해서 암살!
                string tflitePath = Path.Combine(currentPath, "mycar", "models", $"{modelName}.tflite");
                string pngPath = Path.Combine(currentPath, "mycar", "models", $"{modelName}.png");

                if (File.Exists(tflitePath))
                {
                    File.Delete(tflitePath);
                }
                if (File.Exists(pngPath))
                {
                    File.Delete(pngPath);
                }
                // ==========================================================

                // 6. 화면의 리스트뷰(표)에서 해당 줄 지우기
                lvwModel.Items.Remove(selectedItem);

                // 7. 전이학습 선택 콤보박스 목록에서도 똑같이 지워줘서 동기화하기!
                if (cboSelectTransferModel.Items.Contains(modelName))
                {
                    cboSelectTransferModel.Items.Remove(modelName);
                }

                ReportLog("알림", $"'{modelName}' 모델이 성공적으로 삭제되었습니다.");
            }
            catch (Exception ex)
            {
                // 1. 개발자용: 이번에도 역시 ex.Message가 아닌 ex.ToString()을 남겨서 어느 줄에서 터졌는지 기록합니다.
                ReportLog("오류", $"모델 삭제 실패: {ex.ToString()}");

                // 2. 사용자용: 윈도우에서 가장 자주 발생하는 에러 원인을 친절하게 짚어줍니다.
                MessageBox.Show("모델을 삭제하는 중 에러가 발생했습니다.\n" +
                                "해당 모델의 폴더가 열려있거나, 파이썬 백그라운드 프로세스가 파일을 사용 중일 수 있습니다.\n" +
                                "폴더를 닫거나 잠시 후 다시 시도해 주세요.\n\n" +
                                $"[상세 원인] {ex.Message}",
                                "삭제 실패",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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

            // ⭐ 수정된 부분: 바뀐 훈련 명령어(--model)에 맞춰 png 파일의 위치를 안쪽으로 조준합니다.
            // 새 경로: mycar\models\모델명\모델명.png
            string pngPath = Path.Combine(currentPath, "mycar", "models", modelName, $"{modelName}.png");

            // [호환성] 예전에 바깥에 저장했던 구형 모델들의 사진 경로도 남겨둡니다.
            // 옛날 경로: mycar\models\모델명.png
            string oldPngPath = Path.Combine(currentPath, "mycar", "models", $"{modelName}.png");

            // 3. 사진 파일이 있으면 뷰어 띄우기 (새 경로 먼저 찾고, 없으면 옛날 경로 찾기)
            if (File.Exists(pngPath))
            {
                ShowGraphViewer(modelName, pngPath);
            }
            else if (File.Exists(oldPngPath))
            {
                ShowGraphViewer(modelName, oldPngPath);
            }
            else
            {
                // 1. 어떤 모델의 그래프가 없는지 변수(modelName)를 넣어 로그를 남깁니다.
                // 등급은 대형 에러가 아니므로 "경고" 또는 "정보"가 좋습니다.
                ReportLog("경고", $"'{modelName}' 모델의 훈련 그래프 파일(png)이 존재하지 않습니다.");

                // 2. 사용자에게는 경고 아이콘과 함께 안내합니다.
                MessageBox.Show($"'{modelName}' 모델의 훈련 그래프(png) 파일이 존재하지 않습니다.\n학습이 도중에 중단되었거나 파일이 이동되었을 수 있습니다.",
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
            if (lvwModel.SelectedItems.Count == 0) return;
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
    }
}
