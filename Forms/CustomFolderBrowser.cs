using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DataManager
{
    public class CustomFolderBrowser : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string SelectedPath { get; private set; } = string.Empty;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool AllowFileSelection { get; set; } = false;

        private readonly string _rootPath;
        private string _currentPath;

        private TextBox _txtPath;
        private Button _btnUp;
        private Button _btnBrowse;
        private ListView _listView;
        private Label _lblSelected;
        private Button _btnOk;
        private Button _btnCancel;

        public CustomFolderBrowser(string rootPath, string title = "폴더 선택")
        {
            _rootPath = rootPath;
            _currentPath = rootPath;
            Text = title;
            InitUI();
        }

        private void InitUI()
        {
            // ── 폼 기본 설정 ──────────────────────────────────────
            Width = 680;
            Height = 520;
            MinimumSize = new Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("맑은 고딕", 9.5F);

            // ── 상단: 위로 버튼 + 경로 레이블 ────────────────────
            var pnlTop = new Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 40;
            pnlTop.BackColor = Color.FromArgb(40, 50, 70);
            pnlTop.Padding = new Padding(4);

            _btnUp = new Button();
            _btnUp.Text = "▲ 위로";
            _btnUp.Dock = DockStyle.Left;
            _btnUp.Width = 80;
            _btnUp.BackColor = Color.FromArgb(80, 95, 120);
            _btnUp.ForeColor = Color.White;
            _btnUp.FlatStyle = FlatStyle.Flat;
            _btnUp.FlatAppearance.BorderSize = 0;
            _btnUp.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            _btnUp.Cursor = Cursors.Hand;
            _btnUp.Click += (s, e) => GoUp();

            _txtPath = new TextBox();
            _txtPath.Dock = DockStyle.Fill;
            _txtPath.ForeColor = Color.FromArgb(200, 215, 235);
            _txtPath.BackColor = Color.FromArgb(55, 68, 90);
            _txtPath.Font = new Font("맑은 고딕", 9F);
            _txtPath.BorderStyle = BorderStyle.None;
            _txtPath.Margin = new Padding(8, 0, 0, 0);
            _txtPath.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string typed = _txtPath.Text.Trim();
                    if (Directory.Exists(typed))
                        Navigate(typed);
                    else
                    {
                        _txtPath.ForeColor = Color.FromArgb(255, 120, 100);
                        _txtPath.Text = typed; // keep wrong input visible
                    }
                    e.SuppressKeyPress = true;
                }
            };
            // 포커스를 잃으면 현재 경로로 복원
            _txtPath.Leave += (s, e) =>
            {
                _txtPath.Text = _currentPath;
                _txtPath.ForeColor = Color.FromArgb(200, 215, 235);
            };
            // 포커스 받으면 색상 정상화
            _txtPath.Enter += (s, e) =>
            {
                _txtPath.ForeColor = Color.FromArgb(200, 215, 235);
            };

            pnlTop.Controls.Add(_txtPath);
            pnlTop.Controls.Add(_btnUp);

            // 파일탐색기 버튼 (텍스트박스 오른쪽 끝)
            _btnBrowse = new Button();
            _btnBrowse.Dock = DockStyle.Right;
            _btnBrowse.Width = 36;
            _btnBrowse.BackColor = Color.FromArgb(60, 75, 100);
            _btnBrowse.ForeColor = Color.White;
            _btnBrowse.FlatStyle = FlatStyle.Flat;
            _btnBrowse.FlatAppearance.BorderSize = 0;
            _btnBrowse.Cursor = Cursors.Hand;
            _btnBrowse.Font = new Font("Segoe UI Emoji", 13F);
            _btnBrowse.Text = "📂";
            _btnBrowse.TextAlign = ContentAlignment.MiddleCenter;
            _btnBrowse.Padding = new Padding(0);
            _btnBrowse.TabStop = false;
            _btnBrowse.Click += (s, e) =>
            {
                using (var dlg = new FolderBrowserDialog())
                {
                    dlg.Description = "폴더를 선택하세요";
                    dlg.ShowNewFolderButton = true;
                    if (Directory.Exists(_currentPath))
                        dlg.SelectedPath = _currentPath;

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                        Navigate(dlg.SelectedPath);
                }
            };
            pnlTop.Controls.Add(_btnBrowse);

            // ── 하단: 선택 표시 + 확인/취소 버튼 ────────────────
            var pnlBottom = new Panel();
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 50;
            pnlBottom.BackColor = Color.FromArgb(250, 251, 253);
            pnlBottom.Padding = new Padding(8, 8, 8, 8);

            _btnCancel = new Button();
            _btnCancel.Text = "취소";
            _btnCancel.Dock = DockStyle.Right;
            _btnCancel.Width = 90;
            _btnCancel.BackColor = Color.FromArgb(200, 70, 70);
            _btnCancel.ForeColor = Color.White;
            _btnCancel.FlatStyle = FlatStyle.Flat;
            _btnCancel.FlatAppearance.BorderSize = 0;
            _btnCancel.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            _btnCancel.Cursor = Cursors.Hand;
            _btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            _btnOk = new Button();
            _btnOk.Text = "선택";
            _btnOk.Dock = DockStyle.Right;
            _btnOk.Width = 90;
            _btnOk.BackColor = Color.FromArgb(67, 130, 220);
            _btnOk.ForeColor = Color.White;
            _btnOk.FlatStyle = FlatStyle.Flat;
            _btnOk.FlatAppearance.BorderSize = 0;
            _btnOk.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
            _btnOk.Cursor = Cursors.Hand;
            _btnOk.Enabled = false;
            _btnOk.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            _lblSelected = new Label();
            _lblSelected.Dock = DockStyle.Fill;
            _lblSelected.ForeColor = Color.FromArgb(120, 130, 150);
            _lblSelected.Font = new Font("맑은 고딕", 9F);
            _lblSelected.TextAlign = ContentAlignment.MiddleLeft;
            _lblSelected.Text = "선택된 항목 없음";

            pnlBottom.Controls.Add(_lblSelected);
            pnlBottom.Controls.Add(_btnOk);
            pnlBottom.Controls.Add(_btnCancel);

            // ── 중앙: ListView ────────────────────────────────────
            _listView = new ListView();
            _listView.Dock = DockStyle.Fill;
            _listView.View = View.Details;
            _listView.FullRowSelect = true;
            _listView.GridLines = false;
            _listView.BorderStyle = BorderStyle.None;
            _listView.BackColor = Color.White;
            _listView.ForeColor = Color.FromArgb(35, 45, 65);
            _listView.Font = new Font("맑은 고딕", 10F);
            _listView.HeaderStyle = ColumnHeaderStyle.None;
            _listView.MultiSelect = false;

            var col = new ColumnHeader();
            col.Text = "이름";
            col.Width = 600;
            _listView.Columns.Add(col);

            _listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            _listView.DoubleClick += ListView_DoubleClick;

            // ── 폼에 추가 (Top/Bottom 먼저, Fill 마지막) ─────────
            Controls.Add(_listView);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);

            // 폼 크기 바뀔 때 컬럼 너비 자동 맞춤
            Resize += (s, e) =>
            {
                if (_listView.Columns.Count > 0)
                    _listView.Columns[0].Width = _listView.ClientSize.Width - 4;
            };

            // 초기 목록 로드
            Navigate(_rootPath);
        }

        private void Navigate(string path)
        {
            if (!Directory.Exists(path)) return;
            _currentPath = path;

            // 경로 표시
            _txtPath.Text = _currentPath;
            _txtPath.ForeColor = Color.FromArgb(200, 215, 235);

            // 위로 버튼 (드라이브 루트에서만 비활성화)
            DirectoryInfo upCheck = Directory.GetParent(_currentPath);
            _btnUp.Enabled = upCheck != null;

            // 선택 초기화
            SelectedPath = string.Empty;
            _lblSelected.Text = "선택된 항목 없음";
            _lblSelected.ForeColor = Color.FromArgb(120, 130, 150);
            _btnOk.Enabled = false;

            _listView.Items.Clear();

            // 폴더 추가
            try
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    var item = new ListViewItem("  📁  " + Path.GetFileName(dir));
                    item.Tag = dir;
                    item.ForeColor = Color.FromArgb(35, 45, 65);
                    _listView.Items.Add(item);
                }
            }
            catch { }

            // 파일 추가 (옵션)
            if (AllowFileSelection)
            {
                try
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        var item = new ListViewItem("  📄  " + Path.GetFileName(file));
                        item.Tag = file;
                        item.ForeColor = Color.FromArgb(80, 90, 110);
                        _listView.Items.Add(item);
                    }
                }
                catch { }
            }

            if (_listView.Items.Count == 0)
            {
                var empty = new ListViewItem("  (비어 있음)");
                empty.ForeColor = Color.FromArgb(160, 170, 185);
                empty.Tag = null;
                _listView.Items.Add(empty);
            }

            // 컬럼 너비 맞춤
            if (_listView.Columns.Count > 0)
                _listView.Columns[0].Width = _listView.ClientSize.Width - 4;
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_listView.SelectedItems.Count == 0) return;
            var item = _listView.SelectedItems[0];
            if (item.Tag == null) return;

            SelectedPath = (string)item.Tag;
            _lblSelected.Text = Path.GetFileName(SelectedPath);
            _lblSelected.ForeColor = Color.FromArgb(35, 45, 65);
            _btnOk.Enabled = true;
        }

        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            if (_listView.SelectedItems.Count == 0) return;
            var item = _listView.SelectedItems[0];
            if (item.Tag == null) return;

            string path = (string)item.Tag;

            if (Directory.Exists(path))
            {
                Navigate(path);
            }
            else if (File.Exists(path))
            {
                SelectedPath = path;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void GoUp()
        {
            DirectoryInfo parent = Directory.GetParent(_currentPath);
            if (parent != null) Navigate(parent.FullName);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) { DialogResult = DialogResult.Cancel; Close(); return true; }
            if (keyData == Keys.Enter && _btnOk.Enabled) { DialogResult = DialogResult.OK; Close(); return true; }
            if (keyData == Keys.Back) { GoUp(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    public static class AppPaths
    {
        private static string _projectRoot;
        private static string _solutionRoot;

        public static string ProjectRoot
        {
            get
            {
                if (_projectRoot != null) return _projectRoot;
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                while (true)
                {
                    if (Path.GetFileName(dir).Equals("DataManager", StringComparison.OrdinalIgnoreCase))
                    {
                        _projectRoot = dir;
                        return _projectRoot;
                    }
                    DirectoryInfo p = Directory.GetParent(dir);
                    if (p == null) break;
                    dir = p.FullName;
                }
                // fallback
                dir = AppDomain.CurrentDomain.BaseDirectory;
                for (int i = 0; i < 2; i++)
                {
                    DirectoryInfo p = Directory.GetParent(dir);
                    if (p == null) break;
                    dir = p.FullName;
                }
                _projectRoot = dir;
                return _projectRoot;
            }
        }

        public static string SolutionRoot
        {
            get
            {
                if (_solutionRoot != null) return _solutionRoot;
                _solutionRoot = Directory.GetParent(ProjectRoot)?.FullName ?? ProjectRoot;
                return _solutionRoot;
            }
        }

        public static string EditedData => Path.Combine(SolutionRoot, "mycar", "data", "EditedData");
        public static string MycarData => Path.Combine(SolutionRoot, "mycar", "data");
        public static string MycarModels => Path.Combine(SolutionRoot, "mycar", "models");
    }
}