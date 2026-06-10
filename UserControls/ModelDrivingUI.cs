using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataManager.Services.ModelDriving.Service;

namespace DataManager.UserControls
{
    public partial class ModelDrivingUI : UserControl
    {
        private readonly ModelDrivingService _drivingService = new ModelDrivingService();

        public event Action<string, string, string> OnLogReported;

        // ── 로그 패널 컨트롤 (Designer 대신 코드로 생성) ──────────────────────
        private Panel _logPanel;
        private RichTextBox _logBox;
        private bool _logVisible = false;
        // ─────────────────────────────────────────────────────────────────────

        public ModelDrivingUI()
        {
            InitializeComponent();
            BuildLogPanel();       // 로그 패널 동적 생성
            RegisterServiceEvents();
        }

        // ── 경로 헬퍼 ────────────────────────────────────────────────────────
        private static string DonkeyRoot =>
            System.IO.Path.GetFullPath(
                System.IO.Path.Combine(Application.StartupPath, @"..\..\..\..")
            );

        private string MycarDir => System.IO.Path.Combine(DonkeyRoot, @"mycar");
        private string MycarModelsDir => System.IO.Path.Combine(MycarDir, @"models");
        // ─────────────────────────────────────────────────────────────────────

        // ── 로그 패널 동적 생성 ───────────────────────────────────────────────
        private void BuildLogPanel()
        {
            // 2) 로그 박스를 감싸는 패널
            _logPanel = new Panel
            {
                BackColor = Color.FromArgb(18, 18, 28),
                Dock = DockStyle.Bottom,
                Height = 110,
                Visible = false,
                Padding = new Padding(4),
            };

            // 3) RichTextBox (읽기 전용, 스크롤 자동)
            _logBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(18, 18, 28),
                ForeColor = Color.FromArgb(180, 220, 180),
                Font = new Font("Consolas", 8.5f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                WordWrap = false,
            };

            _logPanel.Controls.Add(_logBox);

            // 4) 토글 버튼을 UserControl에, 패널을 Bottom에 붙임
            //    (InitializeComponent 이후에 추가해야 z-order 안전)
            this.Controls.Add(_logPanel);
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

        private void RegisterServiceEvents()
        {
            // 카메라 프레임
            _drivingService.OnFrameReceived += frame =>
            {
                try
                {
                    if (!picCamera.IsHandleCreated || picCamera.IsDisposed) return;
                    picCamera.BeginInvoke(new Action(() =>
                    {
                        var oldImage = picCamera.Image;
                        picCamera.Image = frame;
                        oldImage?.Dispose();
                    }));
                }
                catch { }
            };

            // Python 로그 수신 → RichTextBox에 출력
            _drivingService.OnPythonLog += line =>
            {
                AppendPythonLog(line);
            };

            // 서버 준비 완료 → 소켓 연결 자동 수행 후 btnDrive 활성화
            _drivingService.OnServerReady += async () =>
            {
                if (!IsHandleCreated || IsDisposed) return;

                try
                {
                    await _drivingService.ConnectNetworkAsync();

                    this.BeginInvoke(new Action(() =>
                    {
                        if (IsDisposed) return;
                        btnDrive.Enabled = true;
                        ReportLog("성공", "✅ 서버 준비 완료! 소켓 및 카메라 연결이 자동으로 완료되었습니다. [AI 주행 시작] 버튼을 누르세요.");

                        // 로그 패널이 닫혀 있었다면 자동으로 열어줌
                        if (!_logVisible)
                        {
                            _logVisible = true;
                            _logPanel.Visible = true;
                        }
                    }));
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        if (IsDisposed) return;
                        ReportLog("에러", $"소켓 자동 연결 실패: {ex.Message}");
                        btnLoadModel.Enabled = true;
                    }));
                }
            };

            this.Load += (s, e) =>
            {
                ApplyInitialUIState();
                ReportLog("알림", "자율주행 테스트를 위해 먼저 [모델 가져오기]를 눌러 모델 파일을 선택해주세요.");
            };

            this.HandleDestroyed += (s, e) =>
            {
                _drivingService.DisconnectAll();
                ReportLog("알림", "ModelDriving 서비스를 종료하고 리소스를 해제했습니다.");
            };
        }

        // ── UI 상태 초기화 ────────────────────────────────────────────────────
        private void ApplyInitialUIState()
        {
            if (cboMapList != null && cboMapList.Items.Count > 0)
                cboMapList.SelectedIndex = 0;

            btnDrive.Text = "▶ 주행 시작";
            btnDrive.BackColor = Color.FromArgb(72, 175, 120);
            btnDrive.ForeColor = Color.White;
            lblLoadedModel.Text = "tflite 파일을 선택해주세요";

            btnLoadModel.Enabled = true;
            btnDrive.Enabled = false;

            // 로그 박스 초기화
            _logBox?.Clear();
            if (_logVisible)
            {
                _logVisible = false;
                _logPanel.Visible = false;
            }

            if (picCamera.Image != null)
            {
                var oldImg = picCamera.Image;
                picCamera.Image = null;
                oldImg.Dispose();
            }
        }
        // ─────────────────────────────────────────────────────────────────────

        // 통합 버튼: 모델 가져오기 → 시뮬레이터 시작 → 서버 준비 감지 → 소켓 연결 자동 수행
        private void btnLoadModel_Click(object sender, EventArgs e)
        {
            string defaultModelsPath = MycarModelsDir;
            if (!Directory.Exists(defaultModelsPath))
                Directory.CreateDirectory(defaultModelsPath);

            using (CustomFolderBrowser cfb = new CustomFolderBrowser(defaultModelsPath, "테스트할 주행 인공지능 모델 선택"))
            {
                cfb.AllowFileSelection = true;

                if (cfb.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = cfb.SelectedPath;

                    if (string.IsNullOrEmpty(selectedFilePath) || !File.Exists(selectedFilePath))
                    {
                        ReportLog("경고", "유효한 모델 가중치 파일이 선택되지 않았습니다.");
                        return;
                    }

                    string extension = Path.GetExtension(selectedFilePath);
                    if (!extension.Equals(".tflite", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("자율주행 테스트는 .tflite 확장자를 가진 모델 파일만 선택할 수 있습니다.",
                            "확장자 불일치", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ReportLog("경고", $"선택 실패: .tflite 파일이 아닙니다. (선택된 확장자: {extension})");
                        return;
                    }

                    try
                    {
                        string fileName = Path.GetFileName(selectedFilePath);
                        _drivingService.LoadOrChangeModel(selectedFilePath);

                        if (_drivingService.DetectedModelType.Contains("3D"))
                        {
                            ReportLog("경고", "3D 모델은 현재 시뮬레이터 환경에서 실행이 지원되지 않습니다. 다른 모델을 선택해주세요.");
                            lblLoadedModel.Text = $"⚠ 지원 불가: {fileName}";
                            btnLoadModel.Enabled = true;
                            return;
                        }

                        lblLoadedModel.Text = $"로드됨: {fileName}";
                        btnLoadModel.Enabled = false;

                        ReportLog("성공", $"모델 가중치 연동 완료: {fileName}");
                        ReportLog("메타분석", $"[meta.txt 자동 인식 결과] -> 현재 모델은 '{_drivingService.DetectedModelType}' 모드입니다.");
                    }
                    catch (Exception ex)
                    {
                        ReportLog("에러", $"모델 파일 설정 또는 메타 파일 분석 실패: {ex.Message}");
                        return;
                    }

                    // 모델 로드 성공 → 시뮬레이터 + Python 서버 자동 시작
                    string envName = cboMapList.Text.Trim();
                    if (string.IsNullOrEmpty(envName)) envName = "generated_track";

                    try
                    {
                        _logBox?.Clear();

                        _drivingService.StartSimulatorAndServer(envName);

                        // 로그 패널 자동 오픈
                        _logVisible = true;
                        _logPanel.Visible = true;

                        ReportLog("알림", $"시뮬레이터 및 Python 서버 실행 중... (맵: {envName})");
                        ReportLog("알림", "서버 준비 완료가 감지되면 소켓 연결이 자동으로 수행되고 [AI 주행 시작] 버튼이 활성화됩니다.");
                    }
                    catch (Exception ex)
                    {
                        ReportLog("에러", $"시뮬레이터/서버 시작 실패: {ex.Message}");
                        btnLoadModel.Enabled = true;
                    }
                }
            }
        }

        // 4단계: 주행 토글
        private void btnDrive_Click(object sender, EventArgs e)
        {
            try
            {
                _drivingService.ToggleAiDriving();

                if (_drivingService.IsAiDriving)
                {
                    btnDrive.Text = "■ 주행 중지";
                    btnDrive.BackColor = Color.FromArgb(210, 70, 70);
                    btnDrive.ForeColor = Color.White;
                    ReportLog("알림", "AI 자율 주행 상태: FullAuto 모드 가동 시작");
                }
                else
                {
                    btnDrive.Text = "▶ 주행 시작";
                    btnDrive.BackColor = Color.FromArgb(72, 175, 120);
                    btnDrive.ForeColor = Color.White;
                    ReportLog("알림", "AI 자율 주행 상태: 중지 (User 모드 복귀)");
                }
            }
            catch (Exception ex)
            {
                ReportLog("경고", ex.Message);
            }
        }

        // 초기화
        private void btnInit_Click(object sender, EventArgs e)
        {
            try
            {
                _drivingService.ResetService();
                ApplyInitialUIState();
                ReportLog("초기화", "모든 인프라 프로세스를 완전 종료하고 인터페이스를 초기화했습니다.");
            }
            catch (Exception ex)
            {
                ReportLog("에러", $"시스템 초기화 도중 오류: {ex.Message}");
            }
        }

        private void cboMapList_SelectedIndexChanged(object sender, EventArgs e) { }

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }
    }
}