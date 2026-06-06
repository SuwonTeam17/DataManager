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

        public ModelDrivingUI()
        {
            InitializeComponent();
            RegisterServiceEvents();
        }

        // 프로젝트 상위 루트 경로 및 mycar 폴더 정의
        private static string DonkeyRoot =>
            System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    Application.StartupPath,  // ...\DataManager\bin\Debug
                    @"..\..\..\.."            // → ...\Donkey
                )
            );

        private string MycarDir => System.IO.Path.Combine(DonkeyRoot, @"mycar");

        // MycarDir를 기반으로 models 폴더 경로를 직접 생성
        private string MycarModelsDir => System.IO.Path.Combine(MycarDir, @"models");

        private void RegisterServiceEvents()
        {
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

        // [공용 UI 롤백 엔진] 첫 로드 및 초기화 시 동일한 버튼 활성 상태 규격을 보장합니다.
        private void ApplyInitialUIState()
        {
            if (cboMapList != null && cboMapList.Items.Count > 0)
                cboMapList.SelectedIndex = 0;

            // 버튼 텍스트 및 기본 제어 상태 설정
            btnConnect.Text = "서버 연결";
            btnDrive.Text = "▶ 주행 시작";
            btnDrive.BackColor = Color.FromArgb(72, 175, 120); // 기본 초록색 복원
            btnDrive.ForeColor = Color.White;

            lblLoadedModel.Text = "선택된 모델 가중치 파일 없음";

            // 요구사항 인터페이스 순서 잠금 ([모델 가져오기] 버튼 활성화)
            btnLoadModel.Enabled = true;
            btnStartSim.Enabled = false;
            btnConnect.Enabled = false;
            btnDrive.Enabled = false;

            // 카메라 프리뷰 픽처박스 잔상 청소
            if (picCamera.Image != null)
            {
                var oldImg = picCamera.Image;
                picCamera.Image = null;
                oldImg.Dispose();
            }
        }

        // 1단계: 모델 로드 (선택 후 재선택 차단 기능 반영)
        private void btnLoadModel_Click(object sender, EventArgs e)
        {
            string defaultModelsPath = MycarModelsDir;

            if (!Directory.Exists(defaultModelsPath))
            {
                Directory.CreateDirectory(defaultModelsPath);
            }

            using (CustomFolderBrowser cfb = new CustomFolderBrowser(defaultModelsPath, "테스트할 주행 인공지능 모델 선택"))
            {
                cfb.AllowFileSelection = true; // 파일 선택 모드 활성화

                if (cfb.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = cfb.SelectedPath;

                    if (string.IsNullOrEmpty(selectedFilePath) || !File.Exists(selectedFilePath))
                    {
                        ReportLog("경고", "유효한 모델 가중치 파일이 선택되지 않았습니다.");
                        return;
                    }

                    // 파일 확장자 추출 및 .tflite 체크 검사 (대소문자 무시)
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

                        // 서비스 내부의 가중치 경로 셋팅 및 meta.txt 자동 파싱 수행
                        _drivingService.LoadOrChangeModel(selectedFilePath);

                        lblLoadedModel.Text = $"로드됨: {fileName}";

                        // [핵심 해결책] 모델 선택 성공 시, 초기화 전까지 재선택할 수 없도록 버튼을 잠급니다.
                        btnLoadModel.Enabled = false;

                        // 다음 스텝 진행용 버튼 활성화
                        btnStartSim.Enabled = true;

                        ReportLog("성공", $"모델 가중치 연동 완료: {fileName}");
                        ReportLog("메타분석", $"[meta.txt 자동 인식 결과] -> 현재 모델은 '{_drivingService.DetectedModelType}' 모드입니다.");
                    }
                    catch (Exception ex)
                    {
                        ReportLog("에러", $"모델 파일 설정 또는 메타 파일 분석 실패: {ex.Message}");
                    }
                }
            }
        }

        // 2단계: 시뮬레이터 및 서버 시작
        private async void btnStartSim_Click(object sender, EventArgs e)
        {
            string envName = cboMapList.Text.Trim();
            if (string.IsNullOrEmpty(envName)) envName = "generated_track";

            try
            {
                _drivingService.StartSimulatorAndServer(envName);
                ReportLog("알림", $"인프라 및 코어 서버 구동 명령 송출 완료. (맵: {envName})");

                btnStartSim.Enabled = false;
                await Task.Delay(7000);
                btnConnect.Enabled = true;

                ReportLog("알림", "7초 대기 완료. 서버 창에 에러가 없다면 [연결] 버튼을 눌러주세요.");
            }
            catch (Exception ex)
            {
                ReportLog("에러", $"시뮬레이터/코어 서버 시작 실패: {ex.Message}");
                btnStartSim.Enabled = true;
            }
        }

        // 3단계: 소켓 링크 결합
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                await _drivingService.ConnectNetworkAsync();

                btnConnect.Text = "연결됨";
                btnConnect.Enabled = false;
                btnDrive.Enabled = true;

                ReportLog("알림", "통신 채널이 활성화되었습니다. [AI 주행 시작] 버튼으로 자율주행 테스트가 가능합니다.");
            }
            catch (Exception ex)
            {
                ReportLog("에러", $"웹소켓 서버 연결 실패: {ex.Message}");
                btnConnect.Enabled = true;
            }
        }

        // 4단계: 주행 작동 토글
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

        // 초기화 버튼 클릭 시 프로세스 트리 전면 강제 종료 및 UI 원상 복구 보정
        private void btnInit_Click(object sender, EventArgs e)
        {
            try
            {
                _drivingService.ResetService();

                // [안전 롤백] ApplyInitialUIState 내부에서 btnLoadModel.Enabled = true를 시켜주므로 다시 열립니다.
                ApplyInitialUIState();

                ReportLog("초기화", "모든 인프라 프로세스(Cmd 창 포함)를 무조건 완전 종료하고 가중치 선택 해제 및 인터페이스를 초기화 상태로 롤백했습니다.");
            }
            catch (Exception ex)
            {
                ReportLog("에러", $"시스템 초기화 진행 도중 오류가 발생했습니다: {ex.Message}");
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