namespace DataManager.UserControls
{
    public partial class PilotArenaUI : UserControl
    {
        public PilotArenaUI()
        {
            InitializeComponent();
        }

        private void btnModelAdd_Click(object sender, EventArgs e)
        {
            if (flpModule.Controls.Count >= 2) return;

            var module = new ModelTestModule();
            flpModule.Controls.Add(module);
        }
    }
}