using DataManager.Services.DataCollection.Controls;
using DataManager.Services.DataCollection.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO; // Directory, Path 사용을 위해 추가
using System.Threading.Tasks;
using System.Windows.Forms;
using DataManager; // CustomFolderBrowser 사용을 위해 추가

namespace DataManager.UserControls
{
    public partial class DataCollectionUI : UserControl
    {
        private readonly ProcessService _processService = new ProcessService();
        private readonly DonkeyConnectionService _connectionService = new DonkeyConnectionService();
        private readonly CameraService _cameraService = new CameraService();
        private readonly DriveInputService _driveInputService = new DriveInputService();

        private static string DonkeyRoot =>
            System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    Application.StartupPath,  // ...\DataManager\bin\Debug
                    @"..\..\..\.."            // → ...\Donkey
                )
            );

        private string _activeDataDir = "";

        private Panel _logPanel;
        private RichTextBox _logBox;

        private string SimPath => System.IO.Path.Combine(DonkeyRoot, @"DonkeySimWin\donkey_sim.exe");
        private string PythonExe => System.IO.Path.Combine(DonkeyRoot, @"env\Scripts\python.exe");
        private string MycarDir => System.IO.Path.Combine(DonkeyRoot, @"mycar");

        // 코드로 생성하는 컨트롤
        private VirtualJoystick _virtualJoystick;
        private DirectionBar _barAngle;
        private DirectionBar _barThrottle;

        public event Action<string, string, string> OnLogReported;

        public DataCollectionUI()
        {
            InitializeComponent();
            InitDirectionBars();
            InitJoystick();
            BuildLogPanel();

            // 서비스 이벤트 연결
            _connectionService.OnRawMessage += _cameraService.ProcessMessage;

            _cameraService.OnFrameReceived += frame =>
            {
                try
                {
                    if (!picCamera.IsHandleCreated || picCamera.IsDisposed) return;
                    picCamera.BeginInvoke(new Action(() =>
                    {
                        var old = picCamera.Image;
                        picCamera.Image = frame;
                        old?.Dispose();
                    }));
                }
                catch { }
            };

            _driveInputService.OnInputChanged += (angle, throttle) =>
            {
                // 1. cboThrottleMax에 선택된 % 문자열을 숫자로 파싱 (기본값: 100% -> 1.0f)
                float maxRatio = 1.0f;
                if (cboThrottleMax != null && cboThrottleMax.SelectedItem != null)
                {
                    string maxStr = cboThrottleMax.SelectedItem.ToString().Replace("%", "").Trim();
                    if (float.TryParse(maxStr, out float percent))
                    {
                        maxRatio = percent / 100f; // 예: 90 -> 0.9f
                    }
                }

                // 2. cboThrottleType 모드에 따른 스로틀(Throttle) 변형 계산
                float processedThrottle = throttle;

                if (cboThrottleType != null && cboThrottleType.SelectedItem != null)
                {
                    string typeMode = cboThrottleType.SelectedItem.ToString();

                    if (typeMode == "최댓값")
                    {
                        // 전진(+)일 때는 maxRatio를 넘지 못하게, 후진(-)일 때는 -maxRatio보다 낮아지지 않게 제한
                        if (processedThrottle > 0)
                            processedThrottle = Math.Min(processedThrottle, maxRatio);
                        else if (processedThrottle < 0)
                            processedThrottle = Math.Max(processedThrottle, -maxRatio);
                    }
                    else if (typeMode == "고정값")
                    {
                        // 가만히 정지 상태(0)가 아닐 때만 강제로 값 고정 처리
                        // 입력 장치의 미세한 쏠림 노이즈(데드존)를 감안하여 0.01f 기준으로 판별합니다.
                        if (processedThrottle > 0.01f)
                        {
                            processedThrottle = maxRatio;
                        }
                        else if (processedThrottle < -0.01f)
                        {
                            processedThrottle = -maxRatio; // 후진 고정
                        }
                        else
                        {
                            processedThrottle = 0f; // 완전 중립일 땐 0 유지
                        }
                    }
                }

                // 원본 throttle 대신 가공된 processedThrottle을 서버로 전송합니다!
                _connectionService.SendControl(angle, processedThrottle, chkRecording.Checked);

                // UI 게이지 바에도 가공된 결과 값을 투영합니다.
                if (_barAngle != null) _barAngle.Value = angle;
                if (_barThrottle != null) _barThrottle.Value = processedThrottle;
            };
            _driveInputService.Start();

            this.Load += (s, e) =>
            {
                // 1. DropDownList 상태인 콤보박스의 첫 번째 항목("generated_track")을 초기 선택 (빈 칸 해결)
                if (cboMapList != null && cboMapList.Items.Count > 0)
                {
                    cboMapList.SelectedIndex = 0;
                }
                if (cboThrottleType != null && cboThrottleType.Items.Count > 0)
                {
                    cboThrottleType.SelectedIndex = 0;
                }

                // 2. 콤보박스에 포커스가 잡혀 파랗게 변하는 현상 방지 (파란색 강조 해결)
                // 처음 눌러야 하는 시뮬레이터 시작 버튼으로 포커스를 가로채 옵니다.
                if (btnStartSim != null)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        btnStartSim.Focus();
                    }));
                }
                else
                {
                    this.Focus(); // btnStartSim이 없을 경우의 방어 코드
                }
            };

            this.HandleDestroyed += (s, e) =>
            {
                _cameraService.StopStream();
                _driveInputService.Stop();
                _connectionService.Disconnect();
                _processService.StopAll();
            };
        }

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }

        // ── 로그 패널 동적 생성 ───────────────────────────────────────────────
        private void BuildLogPanel()
        {
            // 1. 이미 컨트롤이 존재한다면 삭제 (중복 생성 방지)
            if (_logPanel != null) return;

            // 2. 로그 패널 생성 (하단 고정)
            _logPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 110, // 높이 조정
                Visible = false
            };

            _logBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.LimeGreen,
                Font = new Font("Consolas", 8.5f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            _logPanel.Controls.Add(_logBox);

            // 4. pnlCamera에 로그 패널 추가
            pnlCamera.Controls.Add(_logPanel);
        }

        // 로그 한 줄을 RichTextBox에 추가 (색상 구분)
        private void AppendPythonLog(string line)
        {
            if (_logBox == null || _logBox.IsDisposed) return;

            if (_logBox.InvokeRequired)
            {
                _logBox.BeginInvoke(new Action(() => AppendPythonLog(line)));
                return;
            }

            // 키워드에 따라 글자 색 구분
            Color color;
            if (line.Contains("ERROR") || line.Contains("Error") || line.Contains("Traceback"))
                color = Color.FromArgb(255, 100, 100);  // 빨강
            else if (line.Contains("WARNING") || line.Contains("Warning"))
                color = Color.FromArgb(255, 200, 80);   // 노랑
            else if (line.Contains("You can now") || line.Contains("Starting vehicle"))
                color = Color.FromArgb(100, 230, 130);  // 밝은 초록 (준비 완료)
            else
                color = Color.FromArgb(180, 220, 180);  // 기본 연두

            _logBox.SelectionStart = _logBox.TextLength;
            _logBox.SelectionLength = 0;
            _logBox.SelectionColor = color;
            _logBox.AppendText(line + "\n");

            // 항상 최신 줄로 스크롤
            _logBox.ScrollToCaret();
        }
        // ─────────────────────────────────────────────────────────────────────

        // ── DirectionBar 초기화 ──────────────────
        private void InitDirectionBars()
        {
            _barAngle = new DirectionBar
            {
                Label = "각도",
                IsVertical = false,
                Location = new Point(0, 20),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(0, 0, 0)
            };
            _barThrottle = new DirectionBar
            {
                Label = "속도",
                IsVertical = false,
                Location = new Point(0, 20),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(0, 0, 0)
            };
            lblAngle.Controls.Add(_barAngle);
            lblThrottle.Controls.Add(_barThrottle);
        }

        // ── 조이스틱 초기화 ──────────────────────
        private void InitJoystick()
        {
            pnlControlJoystick.BackColor = Color.FromArgb(30, 30, 30);
            _virtualJoystick = new VirtualJoystick { Visible = true };
            _virtualJoystick.OnJoystickMoved += _driveInputService.SetJoystick;
            pnlControlJoystick.Controls.Add(_virtualJoystick);
            pnlControlJoystick.Visible = false;
            FitJoystickToPanel();
            pnlControlJoystick.Resize += (s, e) => FitJoystickToPanel();
        }

        private void FitJoystickToPanel()
        {
            if (_virtualJoystick == null) return;
            int side = Math.Min(pnlControlJoystick.Width, pnlControlJoystick.Height);
            int x = (pnlControlJoystick.Width - side) / 2;
            int y = (pnlControlJoystick.Height - side) / 2;
            pnlControlJoystick.Invalidate();
            _virtualJoystick.Location = new Point(x, y);
            _virtualJoystick.Size = new Size(side, side);
            _virtualJoystick.Invalidate();
            pnlControlJoystick.Update();
        }

        // 키 입력 전달만 확인
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_SYSKEYDOWN = 0x0104;

            bool isKeyDown =
                msg.Msg == WM_KEYDOWN ||
                msg.Msg == WM_SYSKEYDOWN;

            if (!isKeyDown)
                return base.ProcessCmdKey(ref msg, keyData);

            if (_driveInputService.HandleKeyboardKey(keyData))
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnStartSim_Click(object sender, EventArgs e)
        {
            string envName = cboMapList.Text.Trim();
            _logBox.Clear();
            _logPanel.Visible = true;

            // 시뮬레이터 실행
            _processService.StartSimulator(SimPath);

            // Python 서버 시작 — 준비 완료 감지 시 자동으로 소켓 연결까지 수행
            _processService.StartPython(PythonExe, MycarDir, envName, async (line) =>
            {
                this.Invoke(new Action(() => AppendPythonLog(line)));

                if (line.Contains("http://localhost:8887") || line.Contains("Starting vehicle"))
                {
                    // 서버 준비 완료 → 소켓 + 카메라 연결 자동 실행
                    await _connectionService.ConnectAsync();
                    _cameraService.StartStream("http://localhost:8887/video");

                    this.BeginInvoke(new Action(() =>
                    {
                        ReportLog("성공", "서버 준비 완료! 소켓 및 카메라 연결이 자동으로 완료되었습니다.");
                    }));
                }
            });

            btnStartSim.Enabled = false;
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 켜져 있는 시뮬레이터 및 파이썬 백그라운드 프로세스가 있다면 모두 종료
                _processService.StopAll();

                // 2. 카메라 뷰어(picCamera) 이미지 자원 해제 및 초기화
                if (picCamera != null)
                {
                    // 메모리 누수 방지를 위해 기존에 떠 있던 이미지가 있다면 Dispose 처리
                    if (picCamera.Image != null)
                    {
                        picCamera.Image.Dispose();
                    }
                    picCamera.Image = null; // 화면을 빈 상태(회색 또는 흰색 바탕)로 리셋
                    picCamera.Refresh();    // UI 강제 갱신
                }

                // 3. 버튼 활성화 상태 및 UI 컨트롤 초기화
                btnStartSim.Enabled = true;       // 시뮬레이터/파이썬 통합 실행 버튼 다시 활성화

                chkRecording.Checked = false;

                // 로그 출력
                ReportLog("알림", "사용자 요청에 의해 시뮬레이터 및 파이썬 엔진 프로세스 초기화 완료");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"초기화 진행 중 오류가 발생했습니다:\n{ex.Message}", "에러",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReportLog("오류", $"초기화 실패: {ex.Message}");
            }

            if (_logPanel != null)
            {
                _logPanel.Visible = false;
                _logBox.Clear();
            }
        }

        private void btnKeyBoard_Click(object sender, EventArgs e)
        {
            _driveInputService.SetMode(DriveInputService.InputMode.Keyboard);
            pnlControlJoystick.Visible = false;
            HighlightButton(btnKeyBoard);
            this.Focus();
        }

        private void btnJoyStick_Click(object sender, EventArgs e)
        {
            _driveInputService.SetMode(DriveInputService.InputMode.Joystick);
            pnlControlJoystick.Visible = true;
            HighlightButton(btnJoyStick);
        }

        private void btnGamePad_Click(object sender, EventArgs e)
        {
            _driveInputService.SetMode(DriveInputService.InputMode.Gamepad);
            pnlControlJoystick.Visible = false;
            HighlightButton(btnGamePad);
        }

        private void HighlightButton(Button active)
        {
            btnKeyBoard.BackColor = Color.FromArgb(140, 185, 245);
            btnJoyStick.BackColor = Color.FromArgb(140, 185, 245);
            btnGamePad.BackColor = Color.FromArgb(140, 185, 245);
            active.BackColor = Color.FromArgb(40, 95, 175);
        }

        // ── [변경됨] CustomFolderBrowser 연동 폴더 로직 ──────────────────

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            // 1. 기준이 되는 기본 데이터 폴더 경로 지정 (mycar/data)
            string baseDataPath = AppPaths.MycarData;

            if (!Directory.Exists(baseDataPath))
            {
                Directory.CreateDirectory(baseDataPath);
            }

            // 2. Microsoft.VisualBasic 내장 InputBox 활용하여 새 폴더명 직접 입력받기
            string folderName = Microsoft.VisualBasic.Interaction.InputBox(
                "mycar/data 폴더 내부에 생성할 새 폴더 이름을 입력하세요.", "새 주행 데이터 폴더 생성", "");

            // 취소 버튼을 누르거나 공백으로 두었을 때 예외 처리
            if (string.IsNullOrWhiteSpace(folderName)) return;

            // 금지된 특수문자나 폴더명 필터링 (윈도우 파일 시스템 기준 방어 코드)
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (folderName.Contains(c))
                {
                    MessageBox.Show("폴더 이름에 특수문자(\\, /, :, *, ?, \", <, >, |)를 포함할 수 없습니다.",
                        "이름 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // 3. 최종 경로 조합 (mycar/data/입력한폴더명)
            string newPath = Path.Combine(baseDataPath, folderName);

            // 중복 검사
            if (Directory.Exists(newPath))
            {
                MessageBox.Show("이미 존재하는 폴더 이름입니다.\n다른 이름을 입력해 주세요.", "중복 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 4. 실제 폴더 생성 진행
                Directory.CreateDirectory(newPath);

                MessageBox.Show($"새 주행 데이터 저장 폴더가 생성되었습니다!\n\n경로: {newPath}", "생성 완료",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 생성된 폴더를 현재 활성 저장 경로로 자동 지정 및 UI 연동
                _activeDataDir = newPath;
                lblSelectedFolderRoute.Text = newPath;
                chkRecording.Enabled = true;
                UpdateDataPath(_activeDataDir);

                // 로그 기록
                ReportLog("알림", $"새 데이터 저장 폴더 생성 및 자동 지정 완료: {folderName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"폴더 생성 중 시스템 오류가 발생했습니다:\n{ex.Message}", "에러",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReportLog("오류", $"새 폴더 생성 실패: {ex.Message}");
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            string baseDataPath = AppPaths.MycarData;
            if (!Directory.Exists(baseDataPath))
            {
                Directory.CreateDirectory(baseDataPath);
            }

            // CustomFolderBrowser를 기본 데이터 폴더 경로로 설정하여 띄움
            using var browser = new CustomFolderBrowser(baseDataPath, "저장 경로 선택");
            if (browser.ShowDialog(this) != DialogResult.OK) return;

            _activeDataDir = browser.SelectedPath;
            lblSelectedFolderRoute.Text = browser.SelectedPath;
            chkRecording.Enabled = true;
            UpdateDataPath(_activeDataDir);
        }

        private void btnDelFolder_Click(object sender, EventArgs e)
        {
            string baseDataPath = AppPaths.MycarData;
            if (!Directory.Exists(baseDataPath)) return;

            using var browser = new CustomFolderBrowser(baseDataPath, "삭제할 폴더 선택");
            if (browser.ShowDialog(this) != DialogResult.OK) return;

            string selectedPath = browser.SelectedPath;

            // 사용자가 실수로 mycar/data/EditedData 폴더 자체를 선택하고 삭제하려 하는 것을 사전에 강력히 차단합니다.
            string forbiddenEditedDataPath = Path.GetFullPath(AppPaths.EditedData).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string normalizedSelected = Path.GetFullPath(selectedPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (string.Equals(normalizedSelected, forbiddenEditedDataPath, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("데이터 가공 전용 폴더(EditedData) 자체는 데이터 수집 화면에서 삭제할 수 없습니다.\n해당 데이터 관리는 TubManager 탭을 이용해 주세요.",
                    "보안 경고", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                // 메인 로그 박스에도 경고 기록 남기기
                ReportLog("경고", "DataCollection 탭에서 EditedData 최상위 폴더 삭제 시도가 차단되었습니다.");
                return;
            }

            // 💡 수집 화면에서도 똑같이 'ReportLog'를 함께 넘겨줍니다.
            bool isDeleted = CustomFolderBrowser.SafeDeleteDirectoryImmediate(
                selectedPath,
                baseDataPath,
                "mycar/data",
                ReportLog
            );

            if (isDeleted && _activeDataDir == selectedPath)
            {
                _activeDataDir = "";
                lblSelectedFolderRoute.Text = "";
                chkRecording.Checked = false;
                chkRecording.Enabled = false;
            }
        }

        private void btnUnSelectFolder_Click(object sender, EventArgs e)
        {
            _activeDataDir = "";
            lblSelectedFolderRoute.Text = "(선택된 폴더 경로)";
            chkRecording.Checked = false;
            chkRecording.Enabled = false;
        }

        private void UpdateDataPath(string selectedPath)
        {
            string tubPathFile = System.IO.Path.Combine(MycarDir, "current_tub_path.txt");
            System.IO.File.WriteAllText(tubPathFile, selectedPath,
                new System.Text.UTF8Encoding(false));
        }

        private void chkRecording_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecording.Checked && string.IsNullOrEmpty(_activeDataDir))
            {
                MessageBox.Show("먼저 새 폴더를 만들거나 기존 폴더를 선택해주세요.",
                    "저장 경로 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chkRecording.Checked = false;
            }
        }
    }
}