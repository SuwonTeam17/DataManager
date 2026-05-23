

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

                    // (기존 코드) 4줄 영수증 기록
                    string curDataset = Path.GetFileName(tubPath);
                    string modelFolder = Path.Combine(mycarPath, "models", modelName);
                    string metaFilePath = Path.Combine(modelFolder, "meta.txt");

                    try { File.WriteAllLines(metaFilePath, new string[] { curDataset, displayType, curTransfer, curMemo }); }
                    catch { }

                    // ==========================================================
                    // ⭐ [영구 박제 로직] 방금 만든 스마트 엔진을 사용해서 에러 없이 병합!
                    string baseConfigPath = Path.Combine(mycarPath, "config.py");
                    string myConfigPath = Path.Combine(mycarPath, "myconfig.py");
                    string savedConfigPath = Path.Combine(modelFolder, "final_config.txt");

                    try
                    {
                        // 1. 원본 파일에서 주석 싹 빼고 깔끔하게 가져오기
                        var finalConfig = ParseConfigToDict(baseConfigPath);

                        // 2. 내가 덮어쓴 파일 가져와서 병합하기
                        var customConfig = ParseConfigToDict(myConfigPath);
                        foreach (var kvp in customConfig)
                        {
                            finalConfig[kvp.Key] = kvp.Value;
                        }

                        // 3. 병합된 결과를 텍스트 파일로 예쁘게 저장
                        List<string> saveLines = new List<string>();

                        // 항목 이름(A-Z) 순서대로 정렬해서 텍스트 파일에 박제하면 더 보기 좋습니다.
                        var sortedKeys = finalConfig.Keys.ToList();
                        sortedKeys.Sort();

                        foreach (var key in sortedKeys)
                        {
                            saveLines.Add($"{key} = {finalConfig[key]}");
                        }

                        File.WriteAllLines(savedConfigPath, saveLines);
                    }
                    catch { }
                    // ==========================================================

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 1. [UX 방어막] 리스트뷰에서 아무것도 선택하지 않고 버튼을 눌렀을 때 컷!
            if (lvwModel.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 모델을 목록에서 먼저 선택해주세요.", "선택된 모델 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                // 4. 실제 컴퓨터의 모델 폴더 경로 추적 (mycar\models\모델명)
                string currentPath = Application.StartupPath;
                while (!Directory.Exists(Path.Combine(currentPath, "env")))
                {
                    DirectoryInfo parentInfo = Directory.GetParent(currentPath);
                    if (parentInfo == null) break;
                    currentPath = parentInfo.FullName;
                }
                string modelFolderPath = Path.Combine(currentPath, "mycar", "models", modelName);

                // 5. 하드디스크에서 폴더 통째로 완전 삭제 (기존 코드)
                if (Directory.Exists(modelFolderPath))
                {
                    Directory.Delete(modelFolderPath, true);
                }

                // ==========================================================
                // ⭐ [추가할 부분] 폴더 옆에 숨어있는 보너스 파일(.tflite, .png)도 추적해서 암살!
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

                // 6. 화면의 리스트뷰(표)에서 해당 줄 지우기 (기존 코드)
                lvwModel.Items.Remove(selectedItem);

                // 7. 전이학습 선택 콤보박스 목록에서도 똑같이 지워줘서 동기화하기!
                if (cboSelectTransferModel.Items.Contains(modelName))
                {
                    cboSelectTransferModel.Items.Remove(modelName);
                }

                MessageBox.Show("모델과 실제 파일이 깨끗하게 삭제되었습니다.", "삭제 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // 파일이 다른 프로그램이나 가상환경 프로세스에 의해 잠겨있을 때 안전하게 예외 처리
                MessageBox.Show($"파일을 지우는 중 오류가 발생했습니다: {ex.Message}", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChgComment_Click(object sender, EventArgs e)
        {
            // 1. 리스트뷰에서 아무것도 선택하지 않았을 때 컷!
            if (lvwModel.SelectedItems.Count == 0)
            {
                MessageBox.Show("메모를 수정할 모델을 목록에서 먼저 선택해주세요.", "선택된 모델 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. 선택된 줄(모델) 정보 가져오기
            ListViewItem selectedItem = lvwModel.SelectedItems[0];
            string modelName = selectedItem.Text;

            // ⭐ 메모는 정상적으로 6번째 칸(index 6)에서 가져옵니다.
            string currentMemo = selectedItem.SubItems[6].Text;

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
                    string metaFilePath = Path.Combine(currentPath, "mycar", "models", modelName, "meta.txt");

                    if (File.Exists(metaFilePath))
                    {
                        string[] lines = File.ReadAllLines(metaFilePath);

                        // ⭐ 새 모델들은 무조건 4줄이므로, 군더더기 없이 바로 덮어씁니다!
                        if (lines.Length >= 4)
                        {
                            lines[3] = newMemo;
                            File.WriteAllLines(metaFilePath, lines);
                        }
                    }

                    // 화면의 리스트뷰 표 업데이트
                    selectedItem.SubItems[6].Text = newMemo;
                    selectedItem.ToolTipText = newMemo;

                    MessageBox.Show("메모가 성공적으로 수정되었습니다.", "수정 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"영수증 파일을 수정하는 중 오류가 발생했습니다: {ex.Message}", "수정 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("설정을 확인할 모델을 먼저 선택해주세요.", "선택된 모델 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("이 모델은 예전 방식이라 통합 설정 파일(final_config.txt)이 존재하지 않습니다.\n다시 훈련된 모델부터 적용됩니다.", "파일 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("그래프를 확인할 모델을 먼저 선택해주세요.", "선택된 모델 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // 모델 폴더 바깥에 있는 성적표 사진(모델명.png) 경로
            string pngPath = Path.Combine(currentPath, "mycar", "models", $"{modelName}.png");

            // 3. 사진 파일이 있으면 뷰어 띄우기
            if (File.Exists(pngPath))
            {
                ShowGraphViewer(modelName, pngPath);
            }
            else
            {
                // 훈련 중 에러가 났거나, 사용자가 폴더에서 사진만 실수로 지운 경우
                MessageBox.Show("이 모델의 훈련 그래프(png) 파일이 존재하지 않습니다.", "파일 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
