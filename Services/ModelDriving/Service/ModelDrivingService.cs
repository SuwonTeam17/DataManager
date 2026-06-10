using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataManager.Services.DataCollection.Services;

namespace DataManager.Services.ModelDriving.Service
{
    public class ModelDrivingService
    {
        private readonly ProcessService _processService = new ProcessService();
        private readonly DonkeyConnectionService _connectionService = new DonkeyConnectionService();
        private readonly CameraService _cameraService = new CameraService();

        private string _loadedModelPath = "";
        private bool _isAiDriving = false;

        public event Action<Bitmap> OnFrameReceived;

        // Python 프로세스 로그를 UI로 전달하는 이벤트
        public event Action<string> OnPythonLog;

        // Python 서버가 준비 완료되었음을 알리는 이벤트
        public event Action OnServerReady;

        private static string DonkeyRoot =>
            Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\.."));

        private string SimPath => Path.Combine(DonkeyRoot, @"DonkeySimWin\donkey_sim.exe");
        private string PythonExe => Path.Combine(DonkeyRoot, @"env\Scripts\python.exe");
        private string MycarDir => Path.Combine(DonkeyRoot, @"mycar");

        public bool IsConnected => _connectionService.IsConnected;
        public string LoadedModelPath => _loadedModelPath;
        public bool IsAiDriving => _isAiDriving;

        private System.Diagnostics.Process _pythonProcess;

        public string DetectedModelType { get; private set; } = "지정되지 않음";
        private string _donkeyTypeArgument = "tflite_linear";
        private bool _isStereoCamera = false;

        // 서버 준비 완료 판단 키워드 목록
        // 아래 문자열 중 하나라도 로그에 등장하면 서버가 준비된 것으로 간주합니다.
        private static readonly string[] _serverReadyKeywords = new[]
        {
            "You can now go to http://localhost:8887",
            "You can now move your controller",
            "Starting vehicle at",
            "Recording Change = False",
        };

        private bool _serverReadyFired = false;

        public ModelDrivingService()
        {
            _connectionService.OnRawMessage += _cameraService.ProcessMessage;
            _cameraService.OnFrameReceived += frame => OnFrameReceived?.Invoke(frame);
        }

        public void StartSimulatorAndServer(string envName)
        {
            _serverReadyFired = false;

            _processService.StartSimulator(SimPath);

            string envArg = string.IsNullOrEmpty(envName) ? "generated_track" : envName.Trim();

            // Python 실행 인자 구성
            string pythonArgs = $"manage.py drive --env_name={envArg}";

            if (!string.IsNullOrEmpty(_loadedModelPath))
            {
                pythonArgs += $" --model=\"{_loadedModelPath}\"";
                pythonArgs += $" --type={_donkeyTypeArgument}";

                if (_isStereoCamera)
                    pythonArgs += " --camera=stereo";
            }

            // cmd 창 없이 Python 직접 실행 — stdout/stderr 모두 리다이렉트
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = PythonExe,
                Arguments = pythonArgs,
                WorkingDirectory = MycarDir,
                UseShellExecute = false,
                CreateNoWindow = true,          // cmd 창 숨김
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            _pythonProcess = new System.Diagnostics.Process { StartInfo = psi, EnableRaisingEvents = true };

            // stdout 비동기 수신
            _pythonProcess.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null) HandlePythonLog(e.Data);
            };

            // stderr 비동기 수신 (donkeycar는 INFO 로그를 stderr로 출력하기도 함)
            _pythonProcess.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null) HandlePythonLog(e.Data);
            };

            _pythonProcess.Start();
            _pythonProcess.BeginOutputReadLine();
            _pythonProcess.BeginErrorReadLine();
        }

        // 로그 한 줄을 처리: UI 이벤트 발행 + 서버 준비 완료 감지
        private void HandlePythonLog(string line)
        {
            OnPythonLog?.Invoke(line);

            if (_serverReadyFired) return;

            foreach (var keyword in _serverReadyKeywords)
            {
                if (line.Contains(keyword))
                {
                    _serverReadyFired = true;
                    OnServerReady?.Invoke();
                    break;
                }
            }
        }

        public async Task ConnectNetworkAsync()
        {
            await _connectionService.ConnectAsync();
            _cameraService.StartStream("http://localhost:8887/video");
        }

        public void LoadOrChangeModel(string modelPath)
        {
            if (string.IsNullOrEmpty(modelPath))
                throw new ArgumentException("유효하지 않은 모델 경로입니다.");

            _loadedModelPath = modelPath;
            _donkeyTypeArgument = "tflite_linear";
            DetectedModelType = "TFLite (Linear 방식 - 메타파일 없음)";
            _isStereoCamera = false;

            try
            {
                string modelFolder = Path.GetDirectoryName(modelPath);
                string metaPath = Path.Combine(modelFolder, "meta.txt");

                if (File.Exists(metaPath))
                {
                    string[] lines = File.ReadAllLines(metaPath);
                    if (lines.Length > 1)
                    {
                        string modeInfo = lines[1].Trim();

                        if (modeInfo.Contains("Linear") || modeInfo.Contains("기본"))
                        {
                            _donkeyTypeArgument = "tflite_linear";
                            DetectedModelType = "기본 주행 (Linear)";
                        }
                        else if (modeInfo.Contains("Categorical") || modeInfo.Contains("분류"))
                        {
                            _donkeyTypeArgument = "tflite_categorical";
                            DetectedModelType = "분류형 주행 (Categorical)";
                        }
                        else if (modeInfo.Contains("Inferred") || modeInfo.Contains("추론"))
                        {
                            _donkeyTypeArgument = "tflite_inferred";
                            DetectedModelType = "추론형 주행 (Inferred)";
                        }
                    }
                }
            }
            catch
            {
                _donkeyTypeArgument = "tflite_linear";
                DetectedModelType = "TFLite (Linear 방식 - 오류 fallback)";
            }
        }

        public void ToggleAiDriving()
        {
            if (!IsConnected)
                throw new InvalidOperationException("서버가 연결되어 있지 않습니다.");

            _isAiDriving = !_isAiDriving;
            SendDriveCommandToWebsocket();
        }

        private void SendDriveCommandToWebsocket()
        {
            if (!IsConnected) return;

            string modeString = _isAiDriving ? "local" : "user";
            float targetAngle = 0f;
            float targetThrottle = _isAiDriving ? 0.15f : 0f;

            var packet = new { angle = targetAngle, throttle = targetThrottle, drive_mode = modeString, recording = false };
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(packet);

            try
            {
                var clientField = typeof(DonkeyConnectionService).GetField("_client",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (clientField != null)
                {
                    var clientInstance = clientField.GetValue(_connectionService) as Websocket.Client.WebsocketClient;
                    if (clientInstance != null && clientInstance.IsStarted)
                        clientInstance.Send(jsonPayload);
                }
            }
            catch
            {
                _connectionService.SendControl(targetAngle, targetThrottle, false);
            }
        }

        public void DisconnectAll()
        {
            _cameraService.StopStream();
            _connectionService.Disconnect();
            _processService.StopAll();

            // Python 프로세스 강제 종료 (하위 트리 포함)
            try
            {
                if (_pythonProcess != null && !_pythonProcess.HasExited)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "taskkill",
                        Arguments = $"/f /t /pid {_pythonProcess.Id}",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    })?.WaitForExit();
                }
            }
            catch { }

            _pythonProcess = null;
            _isAiDriving = false;
            _serverReadyFired = false;
        }

        public void ResetService()
        {
            DisconnectAll();

            _loadedModelPath = "";
            DetectedModelType = "지정되지 않음";
            _donkeyTypeArgument = "tflite_linear";
            _isStereoCamera = false;
        }
    }
}
