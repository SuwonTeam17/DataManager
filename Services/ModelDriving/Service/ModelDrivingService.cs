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

        private static string DonkeyRoot =>
            Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\.."));

        private string SimPath => Path.Combine(DonkeyRoot, @"DonkeySimWin\donkey_sim.exe");
        private string PythonExe => Path.Combine(DonkeyRoot, @"env\Scripts\python.exe");
        private string MycarDir => Path.Combine(DonkeyRoot, @"mycar");

        public bool IsConnected => _connectionService.IsConnected;
        public string LoadedModelPath => _loadedModelPath;
        public bool IsAiDriving => _isAiDriving;

        private System.Diagnostics.Process _debugPythonProcess;

        public string DetectedModelType { get; private set; } = "지정되지 않음";
        private string _donkeyTypeArgument = "tflite_linear";
        private bool _isStereoCamera = false;

        public ModelDrivingService()
        {
            _connectionService.OnRawMessage += _cameraService.ProcessMessage;
            _cameraService.OnFrameReceived += frame =>
            {
                OnFrameReceived?.Invoke(frame);
            };
        }

        public void StartSimulatorAndServer(string envName)
        {
            _processService.StartSimulator(SimPath);

            string envArg = string.IsNullOrEmpty(envName) ? "generated_track" : envName.Trim();
            string cmdArguments = $"/k \"\"{PythonExe}\" manage.py drive --env_name={envArg}";

            if (!string.IsNullOrEmpty(_loadedModelPath))
            {
                cmdArguments += $" --model=\"{_loadedModelPath}\"";
                cmdArguments += $" --type={_donkeyTypeArgument}";

                if (_isStereoCamera)
                {
                    cmdArguments += " --camera=stereo";
                }
            }
            cmdArguments += "\"";

            _debugPythonProcess = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = cmdArguments,
                WorkingDirectory = MycarDir,
                UseShellExecute = true,
                CreateNoWindow = false
            });
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
            {
                throw new InvalidOperationException("서버가 연결되어 있지 않습니다.");
            }

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
                    {
                        clientInstance.Send(jsonPayload);
                    }
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

            // [핵심 해결책] cmd.exe /k 로 버티고 있는 프로세스를 하위 트리(/t)까지 강제(/f)로 날려 창을 무조건 닫습니다.
            try
            {
                if (_debugPythonProcess != null && !_debugPythonProcess.HasExited)
                {
                    string killArgs = $"/f /t /pid {_debugPythonProcess.Id}";
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "taskkill",
                        Arguments = killArgs,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }).WaitForExit();
                }
            }
            catch { }

            _isAiDriving = false;
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