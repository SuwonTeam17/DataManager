using System.Diagnostics;
using System.Threading.Tasks;

namespace DataManager.Services.DataCollection.Services
{
    public class ProcessService
    {
        private Process _simProcess;
        private Process _pythonProcess;
        public void StartSimulator(string simPath)
        {
            _simProcess = Process.Start(new ProcessStartInfo
            {
                FileName = simPath,
                WorkingDirectory = System.IO.Path.GetDirectoryName(simPath)
            });
        }
        public void StartPython(string pythonExe, string workingDir, string envName, Action<string> onLogReceived)
        {
            var psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = $"manage.py drive --env_name={envName.Trim()}",
                WorkingDirectory = workingDir,
                UseShellExecute = false,
                CreateNoWindow = true, // CMD 창 숨김
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            _pythonProcess = new Process { StartInfo = psi, EnableRaisingEvents = true };
            _pythonProcess.OutputDataReceived += (s, e) => { if (e.Data != null) onLogReceived?.Invoke(e.Data); };
            _pythonProcess.ErrorDataReceived += (s, e) => { if (e.Data != null) onLogReceived?.Invoke(e.Data); };

            _pythonProcess.Start();
            _pythonProcess.BeginOutputReadLine();
            _pythonProcess.BeginErrorReadLine();
        }
        public void StopAll()
        {
            _simProcess?.Kill();
            _pythonProcess?.Kill();
        }
    }
}
