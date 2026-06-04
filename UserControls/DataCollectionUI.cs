using DataManager.Services.DataCollection.Controls;
using DataManager.Services.DataCollection.Services;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                @"..\..\..\.."               // → ...\Donkey
            )
        );

        private string _activeDataDir = ""; // 활성 저장 경로

        private string SimPath => System.IO.Path.Combine(DonkeyRoot, @"DonkeySimWin\donkey_sim.exe");
        private string PythonExe => System.IO.Path.Combine(DonkeyRoot, @"env\Scripts\python.exe");
        private string MycarDir => System.IO.Path.Combine(DonkeyRoot, @"mycar");
        // 코드로 생성하는 컨트롤
        private VirtualJoystick _virtualJoystick;
        private DirectionBar _barAngle;
        private DirectionBar _barThrottle;

        public DataCollectionUI()
        {
            InitializeComponent();
            InitDirectionBars();
            InitJoystick();
            // 서비스 이벤트 연결
            _connectionService.OnRawMessage += _cameraService.ProcessMessage;
            // 표시용 — 고품질 보간으로 렌더링
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
                _connectionService.SendControl(angle, throttle, chkRecording.Checked);
                if (_barAngle != null) _barAngle.Value = angle;
                if (_barThrottle != null) _barThrottle.Value = throttle;
            };
            _driveInputService.Start();
            // UserControl이 로드될 때 키 입력 받기
            this.Load += (s, e) => this.Focus();

            this.HandleDestroyed += (s, e) =>
            {
                _cameraService.StopStream();
                _driveInputService.Stop();
                _connectionService.Disconnect();
                _processService.StopAll();
            };
        }
        // ── DirectionBar 초기화 ──────────────────
        private void InitDirectionBars()
        {
            // barAngle (가로) — 원하는 위치/크기로 조정
            _barAngle = new DirectionBar
            {
                Label = "Angle",
                IsVertical = false,
                Location = new Point(0, 20),   // 원하는 위치
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(0, 0, 0)
            };
            // barThrottle (세로) — 원하는 위치/크기로 조정
            _barThrottle = new DirectionBar
            {
                Label = "Throttle",
                IsVertical = false,
                Location = new Point(0, 20),  // 원하는 위치
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(0, 0, 0)
            };
            // 원하는 패널에 추가 (예: pnlController)
            lblAngle.Controls.Add(_barAngle);
            lblThrottle.Controls.Add(_barThrottle);
        }
        // ── 조이스틱 초기화 ──────────────────────
        private void InitJoystick()
        {
            // 패널 배경색 명시 (잔상 방지)
            pnlControlJoystick.BackColor = Color.FromArgb(30, 30, 30);

            _virtualJoystick = new VirtualJoystick
            {
                Visible = true
            };
            _virtualJoystick.OnJoystickMoved += _driveInputService.SetJoystick;
            pnlControlJoystick.Controls.Add(_virtualJoystick);
            pnlControlJoystick.Visible = false;
            // 초기 크기 적용
            FitJoystickToPanel();
            // 패널 크기 바뀔 때마다 자동 조정
            pnlControlJoystick.Resize += (s, e) => FitJoystickToPanel();
        }
        private void FitJoystickToPanel()
        {
            if (_virtualJoystick == null) return;
            int side = Math.Min(pnlControlJoystick.Width, pnlControlJoystick.Height);
            int x = (pnlControlJoystick.Width - side) / 2;
            int y = (pnlControlJoystick.Height - side) / 2;
            // 이전 잔상 지우기 — 먼저 패널 전체 초기화
            pnlControlJoystick.Invalidate();
            _virtualJoystick.Location = new Point(x, y);
            _virtualJoystick.Size = new Size(side, side);
            // 새 크기로 다시 그리기
            _virtualJoystick.Invalidate();
            pnlControlJoystick.Update();  // 즉시 반영
        }
        // ── 키보드 입력 (UserControl용) ──────────
        // KeyPreview 대신 ProcessCmdKey 사용
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Space:
                    _driveInputService.KeyDown(keyData);
                    return true; // 키 이벤트 소비 (상위 전파 차단)
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            _driveInputService.KeyUp(e.KeyCode);
            base.OnKeyUp(e);
        }
        // ── 버튼 이벤트 ──────────────────────────
        private void btnStartSim_Click(object sender, EventArgs e)
        {
            _processService.StartSimulator(SimPath);
            btnStartSim.Enabled = false;
        }
        private async void btnStartPython_Click(object sender, EventArgs e)
        {
            // Python 시작 전 이전 경로 파일 삭제 → Temp로 시작
            string tubPathFile = System.IO.Path.Combine(MycarDir, "current_tub_path.txt");
            if (System.IO.File.Exists(tubPathFile))
                System.IO.File.Delete(tubPathFile);

            _processService.StartPython(PythonExe, MycarDir);
            btnStartPython.Enabled = false;
            await Task.Delay(7000);
            btnConnect.Enabled = true;
        }
        // Connection 연결 후 카메라 스트림 시작
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            await _connectionService.ConnectAsync();
            btnConnect.Text = "연결됨";
            btnConnect.Enabled = false;
            // 연결 후 카메라 스트림 시작
            _cameraService.StartStream("http://localhost:8887/video");
        }
        private void btnKeyBoard_Click(object sender, EventArgs e)
        {
            _driveInputService.SetMode(DriveInputService.InputMode.Keyboard);
            pnlControlJoystick.Visible = false;  // 패널 숨김
            HighlightButton(btnKeyBoard);
            this.Focus();
        }
        private void btnJoyStick_Click(object sender, EventArgs e)
        {
            _driveInputService.SetMode(DriveInputService.InputMode.Joystick);
            pnlControlJoystick.Visible = true;   // 패널 표시 → 안에 조이스틱 같이 보임
            HighlightButton(btnJoyStick);
        }
        private void btnGamePad_Click(object sender, EventArgs e)
        {
            _driveInputService.SetMode(DriveInputService.InputMode.Gamepad);
            pnlControlJoystick.Visible = false;  // 패널 숨김
            HighlightButton(btnGamePad);
        }
        private void HighlightButton(Button active)
        {
            btnKeyBoard.BackColor = SystemColors.Control;
            btnJoyStick.BackColor = SystemColors.Control;
            btnGamePad.BackColor = SystemColors.Control;
            active.BackColor = Color.DodgerBlue;
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            string folderName = Microsoft.VisualBasic.Interaction.InputBox(
                "새 폴더 이름을 입력하세요", "새 폴더 생성", "");

            if (string.IsNullOrWhiteSpace(folderName)) return;

            string newPath = System.IO.Path.Combine(MycarDir, "data", folderName);

            if (System.IO.Directory.Exists(newPath))
            {
                MessageBox.Show("이미 존재하는 폴더 이름입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            System.IO.Directory.CreateDirectory(newPath);
            MessageBox.Show($"폴더가 생성되었습니다.\n{newPath}", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                InitialDirectory = System.IO.Path.Combine(MycarDir, "data"),
                Description = "저장할 폴더를 선택하세요"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            _activeDataDir = dialog.SelectedPath;
            lblSelectedFolderRoute.Text = dialog.SelectedPath;
            chkRecording.Enabled = true;

            UpdateDataPath(_activeDataDir);  // Python에 경로 전달
        }

        private void btnDelFolder_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                InitialDirectory = System.IO.Path.Combine(MycarDir, "data"),
                Description = "삭제할 폴더를 선택하세요"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            string selectedPath = dialog.SelectedPath;

            var result = MessageBox.Show($"정말 삭제하시겠습니까?\n{selectedPath}", "폴더 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            System.IO.Directory.Delete(selectedPath, true);

            if (_activeDataDir == selectedPath)
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
            System.IO.File.WriteAllText(tubPathFile, selectedPath, new System.Text.UTF8Encoding(false));
        }

        private void chkRecording_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecording.Checked && string.IsNullOrEmpty(_activeDataDir))
            {
                MessageBox.Show("먼저 새 폴더를 만들거나 기존 폴더를 선택해주세요.", "저장 경로 없음", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chkRecording.Checked = false;
                return;
            }
        }
    }
}