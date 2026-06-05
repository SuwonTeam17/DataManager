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
        public void StartPython(string pythonExe, string workingDir, string envName)
        {
            string arguments = "manage.py drive";

            // 맵 이름이 정상적으로 선택되어 있다면 인자를 붙여줍니다.
            if (!string.IsNullOrEmpty(envName))
            {
                arguments += $" --env_name={envName.Trim()}";
            }

            _pythonProcess = Process.Start(new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = arguments,
                WorkingDirectory = workingDir,
                UseShellExecute = false,
                CreateNoWindow = false // 파이썬 창이 눈에 보이도록 설정하여 에러 추적을 돕습니다.
            });
        }
        public void StopAll()
        {
            _simProcess?.Kill();
            _pythonProcess?.Kill();
        }
    }
}
