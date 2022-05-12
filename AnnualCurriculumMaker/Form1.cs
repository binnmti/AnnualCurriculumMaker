using Model;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly List<string> Weeks = new() { "月", "火", "水", "木", "金", "土" };
        private readonly List<string> Quarters = new() { "1Q", "2Q", "3Q", "4Q" };
        private readonly List<string> Years = new() { "1年", "2年", "3年", "4年" };
        private readonly List<string> Periods = new() { "1限", "2限", "3限", "4限", "5限", "6限" };

        private string FileName = "";
        private string JsonString = "";
        private string CellBeginText = "";

        private Curriculum Curriculum { get; set; }
        private CurriculumCell CopyCell { get; set; }

        public Form1()
        {
            InitializeComponent();

            var weekTitles = new List<string>();
            var quarterTitles = new List<string>();
            var yearTitles = new List<string>();
            var periodTitles = new List<string>();

            foreach (var quarter in Quarters)
            {
                foreach (var week in Weeks)
                {
                    var column = new DataGridViewColumn
                    {
                        HeaderText = $"{quarter}:{week}",
                        CellTemplate = new DataGridViewTextBoxCell()
                    };
                    dataGridView1.Columns.Add(column);

                    weekTitles.Add(week);
                    quarterTitles.Add(quarter);
                }
            }
            dataGridView1.RowCount = Weeks.Count * Quarters.Count;
            int row = 0;
            foreach (var year in Years)
            {
                foreach (var period in Periods)
                {
                    dataGridView1.Rows[row].HeaderCell.Value = $"{year}:{period}";

                    yearTitles.Add(year);
                    periodTitles.Add(period);
                    row++;
                }
            }
            dataGridView1.RowHeadersWidth = 150;

            Curriculum = new Curriculum(dataGridView1.ColumnCount, dataGridView1.RowCount, weekTitles, quarterTitles, yearTitles, periodTitles);
        }

        private void UpdateListView()
        {
            var teachers = Curriculum.ToTeachers();
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (var teacher in teachers)
            {
                var item = listView1.Items.Add(teacher.Name);
                item.SubItems.Add(teacher.Frame);
                item.SubItems.Add(teacher.Lesson);
            }
            listView1.EndUpdate();
            Text = GetTitle();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //セルの編集中は Enabledをfalseにしないとイベントに取られて編集テキストをコピペ出来ない。
            CutToolStripMenuItem.Enabled = false;
            CopyToolStripMenuItem.Enabled = false;
            PasteToolStripMenuItem.Enabled = false;
            DeleteToolStripMenuItem.Enabled = false;

            CellBeginText = dataGridView1[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "";
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var value = dataGridView1[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "";
            if (CellBeginText == value) return;

            if (!Curriculum.TryParse(e.ColumnIndex, e.RowIndex, value, out var cell))
            {
                MessageBox.Show("重複しています！");
                return;
            }
            Curriculum[e.ColumnIndex, e.RowIndex] = cell;
            UpdateListView();

            CutToolStripMenuItem.Enabled = true;
            CopyToolStripMenuItem.Enabled = true;
            PasteToolStripMenuItem.Enabled = true;
            DeleteToolStripMenuItem.Enabled = true;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.ExecutablePath;
            openFileDialog1.ShowDialog();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                saveFileDialog1.ShowDialog();
            }
            else
            {
                SaveFile(FileName);
            }
        }

        private void NameSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(FileName))
            {
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(FileName);
                saveFileDialog1.FileName = Path.GetFileName(FileName);
            }
            saveFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoadFile(openFileDialog1.FileName);
        }

        private void SaveFile(string fileName)
        {
            File.WriteAllText(fileName, Curriculum.ToJson(true));

            FileName = fileName;
            JsonString = Curriculum.ToJson(false);
            Text = GetTitle();
        }

        private void LoadFile(string fileName)
        {
            Curriculum = CurriculumConvert.ToCurriculum(File.ReadAllText(fileName));
            for (int i = 0; i < Curriculum.Rows; i++)
            {
                for (int j = 0; j < Curriculum.Cols; j++)
                {
                    dataGridView1[i, j].Value = Curriculum[i, j].Value;
                }
            }
            UpdateListView();

            FileName = fileName;
            JsonString = Curriculum.ToJson(false);
            Text = GetTitle();
        }

        private string GetTitle()
        {
            var title = $"年間カリキュラムメーカー";
            if (!string.IsNullOrEmpty(FileName)) title += $" - {Path.GetFileName(FileName)}";
            if (JsonString != Curriculum.ToJson(false)) title += "*";
            return title;
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFile(saveFileDialog1.FileName);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (JsonString == Curriculum.ToJson(false)) return;

            var msg = MessageBox.Show($"{Path.GetFileName(FileName)} は変更されています。閉じる前に保存しますか？", "", MessageBoxButtons.YesNoCancel);
            if (msg == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (msg == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    saveFileDialog1.ShowDialog();
                }
                else
                {
                    SaveFile(FileName);
                }
            }
        }

        private void SetSelectCellText(string text)
        {
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (!Curriculum.TryParse(cell.ColumnIndex, cell.RowIndex, text, out var curriculumCell))
                {
                    MessageBox.Show("重複しています！");
                    return;
                }
                Curriculum[cell.ColumnIndex, cell.RowIndex] = curriculumCell;
                cell.Value = text;
            }
            UpdateListView();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 1) MessageBox.Show("複数選択ではコピー出来ません");
            var cell = Curriculum[dataGridView1.SelectedCells[0].ColumnIndex, dataGridView1.SelectedCells[0].RowIndex];
            if (string.IsNullOrEmpty(cell?.Value)) return;

            CopyCell = cell;
            Clipboard.SetText(cell.Value);
            SetSelectCellText("");
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 1) MessageBox.Show("複数選択ではコピー出来ません");
            var cell = Curriculum[dataGridView1.SelectedCells[0].ColumnIndex, dataGridView1.SelectedCells[0].RowIndex];
            if (string.IsNullOrEmpty(cell?.Value)) return;

            CopyCell = cell;
            Clipboard.SetText(cell.Value);
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Clipboard.GetText();
            if (!string.IsNullOrEmpty(s)) SetSelectCellText(s);
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectCellText("");
        }
    }
}