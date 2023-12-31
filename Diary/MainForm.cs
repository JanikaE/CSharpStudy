using System.Configuration;

namespace Diary
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 所有日记（文件名合法）
        /// </summary>
        public static readonly List<DiaryDto> DiarysAll = new();
        /// <summary>
        /// 所有非法文件名
        /// </summary>
        public static readonly List<string> DiaryInvalid = new();
        /// <summary>
        /// 文件路径
        /// </summary>
        public static readonly string? prePath = ConfigurationManager.AppSettings["DiaryPath"]?.ToString();

        public MainForm()
        {
            InitializeComponent();

            x = Width;
            y = Height;
            SetTag(this);

            RefreshList();
        }

        /// <summary>
        /// 刷新，重新加载所有内容
        /// </summary>
        private void RefreshList()
        {
            DiarysAll.Clear();
            if (prePath != null)
            {
                string[] files = Directory.GetFiles(prePath, "*.txt");
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    fileName = fileName[..fileName.LastIndexOf('.')];
                    try
                    {
                        string[] s = fileName.Split('_');
                        string dateString = s[0];
                        string[] s2 = dateString.Split(".");
                        DateTime date = new(int.Parse(s2[0]), int.Parse(s2[1]), int.Parse(s2[2]));
                        string topic = s[1];
                        DiaryDto dto = new(date, fileName, topic, file);
                        DiarysAll.Add(dto);
                    }
                    catch
                    {
                        // 非法文件名放到另一个列表
                        DiaryInvalid.Add(fileName);
                    }
                }
                ButtonInvalid.Text = $"非法文件名（{DiaryInvalid.Count}）";
                // 按照日期升序排序
                DiarysAll.Sort((a, b) =>
                {
                    return a.date.CompareTo(b.date);
                });
            }

            // 初始化下拉框选项
            ComboBoxYear.Items.Clear();
            ComboBoxYear.Items.Add("All");
            foreach (DiaryDto diary in DiarysAll)
            {
                int year = diary.date.Year;
                if (!ComboBoxYear.Items.Contains(year))
                {
                    ComboBoxYear.Items.Add(year);
                }
            }
            ComboBoxYear.SelectedIndex = 0;
            ComboBoxMonth.Items.Clear();
            ComboBoxMonth.Items.Add("All");
            ComboBoxMonth.SelectedIndex = 0;
            UpdateListBoxFile();

            RichTextBoxShow.Text = string.Empty;
        }

        #region 控件大小随窗体大小等比例缩放

        /// <summary>
        /// 定义当前窗体的宽度
        /// </summary>
        private readonly float x;
        /// <summary>
        /// 定义当前窗体的高度
        /// </summary>
        private readonly float y;

        private static void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    SetTag(con);
                }
            }
        }

        private static void SetControls(float newx, float newy, Control cons)
        {
            // 遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                // 获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string? tag = con.Tag.ToString();
                    if (string.IsNullOrEmpty(tag))
                        continue;
                    string[] mytag = tag.Split(new char[] { ';' });
                    // 根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(Convert.ToSingle(mytag[0]) * newx);
                    con.Height = Convert.ToInt32(Convert.ToSingle(mytag[1]) * newy);
                    con.Left = Convert.ToInt32(Convert.ToSingle(mytag[2]) * newx);
                    con.Top = Convert.ToInt32(Convert.ToSingle(mytag[3]) * newy);
                    // 字体大小
                    float currentSize = Convert.ToSingle(mytag[4]) * newy;
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        SetControls(newx, newy, con);
                    }
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            float newx = Width / x;
            float newy = Height / y;
            SetControls(newx, newy, this);
        }

        #endregion

        #region 查看

        private void ComboBoxYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBoxMonth.Items.Clear();
            ComboBoxMonth.Items.Add("All");
            if (ComboBoxYear.SelectedIndex != 0)
            {
                int year = (int)ComboBoxYear.SelectedItem;
                foreach (DiaryDto diary in DiarysAll)
                {
                    if (diary.date.Year == year)
                    {
                        int month = diary.date.Month;
                        if (!ComboBoxMonth.Items.Contains(month))
                        {
                            ComboBoxMonth.Items.Add(month);
                        }
                    }
                }
            }
            ComboBoxMonth.SelectedIndex = 0;
            UpdateListBoxFile();
        }

        private void ComboBoxMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateListBoxFile();
        }

        /// <summary>
        /// 按照筛选更新列表内容
        /// </summary>
        private void UpdateListBoxFile()
        {
            ListBoxFile.Items.Clear();
            if (prePath == null)
            {
                ListBoxFile.Items.Add("Path is null.");
            }
            else if (DiarysAll.Count == 0)
            {
                ListBoxFile.Items.Add("No diary exists.");
            }
            else
            {
                foreach (DiaryDto diary in DiarysAll)
                {
                    if ((ComboBoxYear.SelectedIndex == 0 || diary.date.Year == (int)ComboBoxYear.SelectedItem) &&
                        (ComboBoxMonth.SelectedIndex == 0 || diary.date.Month == (int)ComboBoxMonth.SelectedItem))
                    {
                        ListBoxFile.Items.Add(diary.name);
                    }
                }
            }
        }

        private void ListBoxFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? path = ListBoxFile.SelectedItem?.ToString();
            if (path == null || prePath == null)
                return;

            // 读取文本
            string text = File.ReadAllText(prePath + path + ".txt");
            RichTextBoxShow.Text = text;
        }

        private void ButtonInvalid_Click(object sender, EventArgs e)
        {
            InvalidForm form = new();
            form.ShowDialog();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        #endregion

        #region 编辑

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (ListBoxFile.SelectedItem == null)
            {
                MessageBox.Show("未选择文件");
                return;
            }
            EditModeOn();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            EditModeOff();
            // 重新读取文本
            string? fileName = ListBoxFile.SelectedItem.ToString();
            string text = File.ReadAllText(prePath + fileName + ".txt");
            RichTextBoxShow.Text = text;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            EditModeOff();
            string? fileName = ListBoxFile.SelectedItem.ToString();
            string path = prePath + fileName + ".txt";
            File.WriteAllText(path, RichTextBoxShow.Text);
        }

        private void EditModeOn()
        {
            RichTextBoxShow.ReadOnly = false;
            ButtonEdit.Visible = false;
            ButtonCancel.Visible = true;
            ButtonSave.Visible = true;
            ListBoxFile.Enabled = false;
            ComboBoxYear.Enabled = false;
            ComboBoxMonth.Enabled = false;
            ButtonRefresh.Enabled = false;
        }

        private void EditModeOff()
        {
            RichTextBoxShow.ReadOnly = true;
            ButtonEdit.Visible = true;
            ButtonCancel.Visible = false;
            ButtonSave.Visible = false;
            ListBoxFile.Enabled = true;
            ComboBoxYear.Enabled = true;
            ComboBoxMonth.Enabled = true;
            ButtonRefresh.Enabled = true;
        }

        #endregion

        #region 新增

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            DateTime date = DateTimePicker.Value;
            string topic = TextBoxTopic.Text;
            string fileName = date.ToString("yyyy.M.d") + "_" + topic;
            string path = prePath + fileName + ".txt";
            File.WriteAllText(path, RichTextBoxInput.Text);
        }

        #endregion
    }
}