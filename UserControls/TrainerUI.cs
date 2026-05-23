

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DataManager.UserControls
{
    public partial class TrainerUI : UserControl
    {
        public event Action<string, string, string> OnLogReported;

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
            string filePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\mycar_real\myconfig.py"));

            // 1. 파일이 없을 때 메인 폼의 로그박스 원격 호출
            if (!File.Exists(filePath))
            {
                ReportLog("오류", $"myconfig.py 파일을 찾을 수 없습니다. 경로: {filePath}");
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
                // 3. 예외 발생 시 메인 폼의 로그박스 원격 호출
                ReportLog("오류", $"파일 저장 중 시스템 오류 발생: {ex.Message}");
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
            string tubPath = "";

            // 1. 데이터 폴더 선택
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "학습시킬 주행 데이터(tub) 폴더를 선택해주세요.";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    tubPath = fbd.SelectedPath;
                }
                else
                {
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
                    ReportLog("Error", "'env' 파이썬 엔진 폴더를 찾을 수 없습니다.");
                    return;
                }
                currentPath = parentInfo.FullName;
            }
            string rootPath = currentPath;
            string mycarPath = $@"{rootPath}\mycar";

            // 3. 자동 모델 이름 생성 및 메모 박제
            string timestamp = DateTime.Now.ToString("yyMMdd_HHmmss");
            string modelName = $"mypilot_{timestamp}";
            string curMemo = string.IsNullOrWhiteSpace(txtComment.Text) ? "새로 학습된 모델입니다." : txtComment.Text;

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

            if (cboSelectTransferModel.SelectedItem != null && cboSelectTransferModel.SelectedItem.ToString() != "None")
            {
                string selectedBaseModel = cboSelectTransferModel.SelectedItem.ToString();
                curTransfer = selectedBaseModel;

                // 폴더 복사 꼼수 대신 파이썬 명령어에 직접 --transfer 옵션을 추가합니다.
                transferCommand = $" --transfer \"models\\{selectedBaseModel}\"";
            }

            // ⭐ 6. 최종 명령어 조립 (python train.py -> donkey train 으로 변경)
            string windowsCommand = $"cd /d \"{mycarPath}\" && \"..\\env\\Scripts\\activate\" && set CUDA_VISIBLE_DEVICES=-1 && donkey train --tub \"{tubPath}\" --model \"models\\{modelName}\" --type {selectedType}{transferCommand}";

            // 7. 백그라운드 프로세스 세팅
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.Arguments = $"/c \"{windowsCommand}\"";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

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

            // 파이썬 에러 가로채기 (경고 메시지도 포함되어 출력될 수 있습니다)
            process.ErrorDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        ReportLog("Python", args.Data);
                    });
                }
            };

            // 9. 파이썬 학습 종료 이벤트 
            process.EnableRaisingEvents = true;
            process.Exited += (s, args) =>
            {
                // 🚨 [방어막] 비정상 종료 시(에러 등) 여기서 컷!
                if (process.ExitCode != 0)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        ReportLog("Error", "훈련 중 문제가 발생하여 종료되었습니다. 로그를 확인하세요.");
                        prgTrain.Value = 0;
                    });
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

                    // 리스트뷰 업데이트
                    ListViewItem newItem = new ListViewItem(modelName);
                    newItem.SubItems.Add($"{modelName}");
                    newItem.SubItems.Add(displayType);
                    newItem.SubItems.Add(curDataset);
                    newItem.SubItems.Add(DateTime.Now.ToString("yy-MM-dd HH:mm"));
                    newItem.SubItems.Add(curTransfer);
                    newItem.SubItems.Add(curMemo);
                    newItem.ToolTipText = curMemo;

                    lvwModel.Items.Add(newItem);

                    // 콤보박스에 새 모델 추가 및 폼 초기화
                    cboSelectTransferModel.Items.Add(modelName);
                    txtComment.Clear();
                    cboSelectTransferModel.SelectedIndex = 0;
                    cboSelectModelType.SelectedIndex = 0;

                    // ⭐ 다음 훈련을 위해 잠가뒀던 모델 종류 콤보박스 다시 풀기
                    cboSelectModelType.Enabled = true;
                    lblTransferWarning.Visible = false;

                    await Task.Delay(1500);
                    prgTrain.Value = 0;
                });

                process.Dispose();
            };

            // 10. 엔진 가동 시작!
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                ReportLog("Error", $"훈련 엔진 가동 실패: {ex.Message}");
            }
        }

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
                string pbFilePath = Path.Combine(dir.FullName, "saved_model.pb");

                if (File.Exists(pbFilePath))
                {
                    string modelName = dir.Name;

                    // ⭐ 기본값 설정 (종류도 이제 변수로 관리합니다)
                    string dataset = "알 수 없음";
                    string modelType = "기본 주행 (Linear)"; // 기본값은 Linear
                    string isTransfer = "-";
                    string memo = "저장되어 있던 모델";

                    string metaFilePath = Path.Combine(dir.FullName, "meta.txt");
                    if (File.Exists(metaFilePath))
                    {
                        try
                        {
                            string[] lines = File.ReadAllLines(metaFilePath);

                            // 옛날에 저장된 3줄짜리 영수증 파일일 경우
                            if (lines.Length == 3)
                            {
                                dataset = lines[0];
                                isTransfer = lines[1];
                                memo = lines[2];
                            }
                            // ⭐ 새롭게 저장될 4줄짜리 영수증 파일일 경우 (종류 포함)
                            else if (lines.Length >= 4)
                            {
                                dataset = lines[0];
                                modelType = lines[1]; // 2번째 줄에서 모델 종류를 쏙 읽어옵니다!
                                isTransfer = lines[2];
                                memo = lines[3];
                            }
                        }
                        catch { }
                    }

                    ListViewItem item = new ListViewItem(modelName);
                    item.SubItems.Add(modelName);
                    item.SubItems.Add(modelType); // ⬅️ 하드코딩 대신 파일에서 읽어온 진짜 종류 적용!
                    item.SubItems.Add(dataset);
                    item.SubItems.Add(dir.CreationTime.ToString("yy-MM-dd HH:mm"));
                    item.SubItems.Add(isTransfer);
                    item.SubItems.Add(memo);

                    item.ToolTipText = memo;
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
                    string originalType = item.SubItems[2].Text;

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
    }
}
