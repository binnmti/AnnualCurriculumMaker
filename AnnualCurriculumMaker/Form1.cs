using Model;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private string FileName = "";
        private string JsonString = "";


        private Curriculum Curriculum { get; set; }

        private  List<string> Weeks = new List<string>() { "åé", "âŒ", "êÖ", "ñÿ", "ã‡", "ìy" };
        public Form1()
        {
            InitializeComponent();
            for (int q = 0; q < 4; q++)
            {
                foreach(var week in Weeks)
                {
                    dataGridView1.Columns.Add($"", $"{q + 1}Q:{week}");
                }
            }
            dataGridView1.RowCount = 4 * 6;

            int row = 0;
            for (int year = 0; year < 4; year++)
            {
                for (int t = 0; t < 6; t++)
                {
                    dataGridView1.Rows[row].HeaderCell.Value = $"{year + 1}îN:{t + 1}å¿";
                    row++;
                }
            }

            var colNames = dataGridView1.Columns.Cast<DataGridViewColumn>().Select(x => x.HeaderText).ToList();
            var rowNames = dataGridView1.Rows.Cast<DataGridViewRow>().Select(x => x.HeaderCell.Value.ToString() ?? "").ToList();
            Curriculum = new Curriculum(dataGridView1.ColumnCount, dataGridView1.RowCount, colNames, rowNames);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var value = e.FormattedValue.ToString() ?? "";

            var colName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            var rowName = dataGridView1.Rows[e.RowIndex].HeaderCell.Value.ToString() ?? "";
            if (!Curriculum.TryParse(e.ColumnIndex, e.RowIndex, value, colName, rowName, out var cell))
            {
                MessageBox.Show("èdï°ÇµÇƒÇ¢Ç‹Ç∑ÅI");
                dataGridView1.CancelEdit();
                e.Cancel = false;
                return;
            }
            Curriculum[e.ColumnIndex, e.RowIndex] = cell;

            var teacher = Curriculum.ToTeacher();
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach(var t in teacher)
            {
                var item = listView1.Items.Add(t.Key);
                item.SubItems.Add(string.Join(',', t.Value.Select(x => $"{x.Name}:[{x.ColName}][{x.RowName}]")));
            }
            listView1.EndUpdate();
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
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveFile(saveFileDialog1.FileName);
        }
    }
}