using Model;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private string FileName = "";
        private string JsonString = "";

        private Curriculum Curriculum { get; set; }

        private  List<string> Weeks = new List<string>() { "月", "火", "水", "木", "金", "土" };
        public Form1()
        {
            InitializeComponent();
            for (int q = 0; q < 4; q++)
            {
                foreach(var week in Weeks)
                {
                    var column = new DataGridViewColumn
                    {
                        HeaderText = $"{q + 1}Q:{week}",
                        CellTemplate = new DataGridViewTextBoxCell()
                    };
                    dataGridView1.Columns.Add(column);
                }
            }
            dataGridView1.RowCount = 4 * 6;

            int row = 0;
            for (int year = 0; year < 4; year++)
            {
                for (int t = 0; t < 6; t++)
                {
                    dataGridView1.Rows[row].HeaderCell.Value = $"{year + 1}年:{t + 1}限";
                    row++;
                }
            }
            dataGridView1.RowHeadersWidth = 150;

            var colNames = dataGridView1.Columns.Cast<DataGridViewColumn>().Select(x => x.HeaderText).ToList();
            var rowNames = dataGridView1.Rows.Cast<DataGridViewRow>().Select(x => x.HeaderCell.Value.ToString() ?? "").ToList();
            Curriculum = new Curriculum(dataGridView1.ColumnCount, dataGridView1.RowCount, colNames, rowNames);
        }

        private void UpdateListView()
        {
            var teacher = Curriculum.ToTeacher();
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (var t in teacher)
            {
                var item = listView1.Items.Add(t.Key);
                item.SubItems.Add(string.Join(',', t.Value.Select(x => $"{x.Name}:[{x.ColName}][{x.RowName}]")));
            }
            listView1.EndUpdate();
            Text = GetTitle();
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var value = e.FormattedValue.ToString() ?? "";

            if (!Curriculum.TryParse(e.ColumnIndex, e.RowIndex, value, out var cell))
            {
                MessageBox.Show("重複しています！");
                dataGridView1.CancelEdit();
                e.Cancel = false;
                return;
            }
            Curriculum[e.ColumnIndex, e.RowIndex] = cell;
            UpdateListView();
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
                    dataGridView1[j, i].Value = Curriculum[i,j].Value;
                }
            }

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

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                Curriculum.TryParse(cell.ColumnIndex, cell.RowIndex, "", out var curriculumCell);
                Curriculum[cell.ColumnIndex, cell.RowIndex] = curriculumCell;

                cell.Value = "";
            }
            UpdateListView();
        }
    }
}