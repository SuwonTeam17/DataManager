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

            this.Load += (s, e) => this.Focus();

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

        // ── 버튼 이벤트 ──────────────────────────
        private void btnStartSim_Click(object sender, EventArgs e)
        {
            _processService.StartSimulator(SimPath);
            btnStartSim.Enabled = false;
        }

        private async void btnStartPython_Click(object sender, EventArgs e)
        {
            string tubPathFile = System.IO.Path.Combine(MycarDir, "current_tub_path.txt");
            if (System.IO.File.Exists(tubPathFile))
                System.IO.File.Delete(tubPathFile);

            _processService.StartPython(PythonExe, MycarDir);
            btnStartPython.Enabled = false;
            await Task.Delay(7000);
            btnConnect.Enabled = true;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            await _connectionService.ConnectAsync();
            btnConnect.Text = "연결됨";
            btnConnect.Enabled = false;
            _cameraService.StartStream("http://localhost:8887/video");
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
            btnKeyBoard.BackColor = Color.FromArgb(100, 150, 210);
            btnJoyStick.BackColor = Color.FromArgb(100, 150, 210);
            btnGamePad.BackColor = Color.FromArgb(100, 150, 210);
            active.BackColor = Color.FromArgb(67, 130, 220);
        }

        // ── [변경됨] CustomFolderBrowser 연동 폴더 로직 ──────────────────

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            // AppPaths.MycarData (mycar/data) 경로 자동 빌드 및 확인
            string baseDataPath = AppPaths.MycarData;
            if (!Directory.Exists(baseDataPath))
            {
                Directory.CreateDirectory(baseDataPath);
            }

            // CustomFolderBrowser를 열어 생성할 '부모 폴더' 위치 지정 유도
            using var browser = new CustomFolderBrowser(baseDataPath, "새 폴더를 생성할 위치 선택");
            if (browser.ShowDialog(this) != DialogResult.OK) return;

            // Microsoft.VisualBasic 내장 InputBox 활용하여 하위 폴더명 입력받기
            string folderName = Microsoft.VisualBasic.Interaction.InputBox(
                "생성할 새 폴더 이름을 입력하세요", "새 폴더 생성", "");

            if (string.IsNullOrWhiteSpace(folderName)) return;

            string newPath = Path.Combine(browser.SelectedPath, folderName);

            if (Directory.Exists(newPath))
            {
                MessageBox.Show("이미 존재하는 폴더 이름입니다.", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Directory.CreateDirectory(newPath);
            MessageBox.Show($"폴더가 생성되었습니다.\n{newPath}", "완료",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 생성한 폴더를 바로 현재 활성 경로로 자동 지정
            _activeDataDir = newPath;
            lblSelectedFolderRoute.Text = newPath;
            chkRecording.Enabled = true;
            UpdateDataPath(_activeDataDir);
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
            lblSelectedFolderRoute.Text = "";
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