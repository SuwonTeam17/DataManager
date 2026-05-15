using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataManager.UserControls
{
    public partial class ModelTestModule : UserControl
    {
        public ModelTestModule()
        {
            InitializeComponent();
        }
        private void btnDelModel_Click(object sender, EventArgs e)
        {
            var parent = this.Parent;
            parent?.Controls.Remove(this);
            this.Dispose();
        }
    }
}