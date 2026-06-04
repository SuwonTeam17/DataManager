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
        public void StartPython(string pythonExe, string workingDir)
        {
            _pythonProcess = Process.Start(new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = "manage.py drive",
                WorkingDirectory = workingDir,
                UseShellExecute = false
            });
        }
        public void StopAll()
        {
            _simProcess?.Kill();
            _pythonProcess?.Kill();
        }
    }
}
