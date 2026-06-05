using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataManager.UserControls
{
    public partial class ModelDrivingUI : UserControl
    {
        public event Action<string, string, string> OnLogReported;

        public ModelDrivingUI()
        {
            InitializeComponent();
        }

        private void ReportLog(string type, string message)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            OnLogReported?.Invoke(currentTime, type, message);
        }
    }
}
