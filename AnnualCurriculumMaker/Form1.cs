using Model;
using System.Text;

namespace WinFormsApp2;

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
    private Curriculum CopyCurriculum { get; set; }

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
                    CellTemplate = new DataGridViewTextBoxCell(),
                };
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
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
        CutToolStripMenuItem.Enabled = true;
        CopyToolStripMenuItem.Enabled = true;
        PasteToolStripMenuItem.Enabled = true;
        DeleteToolStripMenuItem.Enabled = true;

        var value = dataGridView1[e.ColumnIndex, e.RowIndex].Value?.ToString() ?? "";
        if (CellBeginText == value) return;

        if (!Curriculum.TryParse(e.ColumnIndex, e.RowIndex, value, out var cell))
        {
            MessageBox.Show("重複しています！");
        }
        Curriculum[e.ColumnIndex, e.RowIndex] = cell;
        dataGridView1[e.ColumnIndex, e.RowIndex].Value = Curriculum[e.ColumnIndex, e.RowIndex].Value;
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
        if (Path.GetExtension(fileName) == ".csv")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            File.WriteAllText(fileName, Curriculum.ToCsv(), Encoding.GetEncoding("Shift_JIS"));
        }
        else
        {
            File.WriteAllText(fileName, Curriculum.ToJson(true));
        }

        FileName = fileName;
        JsonString = Curriculum.ToJson(false);
        Text = GetTitle();
    }

    private static void SetDataGridView(int col, int row, DataGridView dataGrid, Curriculum curriculum)
    {
        dataGrid[col, row].Value = curriculum[col, row].Value;
        //TODO:Color.EmptyはColorがデシリアライズに対応していないので使えない。このやり方で良いか微妙
        var bg = curriculum[col, row].GetBackColor();
        if (bg != Color.FromArgb(0))
        {
            dataGrid[col, row].Style.BackColor = curriculum[col, row].GetBackColor();
        }
    }

    private void LoadFile(string fileName)
    {
        //TODO:csvロードも出来そう。
        Curriculum = CurriculumConvert.ToCurriculum(File.ReadAllText(fileName));
        for (int row = 0; row < Curriculum.Rows; row++)
        {
            for (int col = 0; col < Curriculum.Cols; col++)
            {
                SetDataGridView(col, row, dataGridView1, Curriculum);
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

    private Curriculum GetCopyCurriculum()
    {
        int minRow = dataGridView1.RowCount;
        int maxRow = 0;
        int minCol = dataGridView1.ColumnCount;
        int maxCol = 0;
        for (int row = 0; row < dataGridView1.RowCount; row++)
        {
            for (int col = 0; col < dataGridView1.ColumnCount; col++)
            {
                if (dataGridView1[col, row].Selected)
                {
                    minRow = Math.Min(minRow, row);
                    maxRow = Math.Max(maxRow, row);
                    minCol = Math.Min(minCol, col);
                    maxCol = Math.Max(maxCol, col);
                }
            }
        }
        var copyCurriculum = new Curriculum(maxCol - minCol + 1, maxRow - minRow + 1);
        for (int row = 0; row < copyCurriculum.Rows; row++)
        {
            for (int col = 0; col < copyCurriculum.Cols; col++)
            {
                copyCurriculum[col, row] = Curriculum[col + minCol, row + minRow];
            }
        }
        return copyCurriculum;
    }

    private void CutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CopyCurriculum = GetCopyCurriculum();
        foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
        {
            Curriculum.TryParse(cell.ColumnIndex, cell.RowIndex, "", out var curriculumCell);
            Curriculum[cell.ColumnIndex, cell.RowIndex] = curriculumCell;
            Curriculum[cell.ColumnIndex, cell.RowIndex].SetColor(Color.White);
            cell.Style.BackColor = Color.White;
            cell.Value = "";
        }
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CopyCurriculum = GetCopyCurriculum();
    }

    private void Paste(Curriculum copyCurriculum)
    {
        for (int row = 0; row < copyCurriculum.Rows; row++)
        {
            for (int col = 0; col < copyCurriculum.Cols; col++)
            {
                var colIndex = Math.Min(dataGridView1.SelectedCells[0].ColumnIndex + col, Curriculum.Cols);
                var rowIndex = Math.Min(dataGridView1.SelectedCells[0].RowIndex + row, Curriculum.Rows);
                Curriculum[colIndex, rowIndex] = Curriculum[colIndex, rowIndex].Copy(copyCurriculum[col, row]);

                //TODO:重複警告は保存時とか終了時に再度したいかな
                if (Curriculum.IsExist(colIndex, rowIndex))
                {
                    MessageBox.Show($"{Curriculum[colIndex, rowIndex].Value}が重複しています！");
                }
                SetDataGridView(colIndex, rowIndex, dataGridView1, Curriculum);
            }
        }
        UpdateListView();
    }

    private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedCells.Count > 1)
        {
            MessageBox.Show("複数選択している状態ではペースト出来ません");
            return;
        }
        Paste(CopyCurriculum);
    }

    private void RowColPasetToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedCells.Count > 1)
        {
            MessageBox.Show("複数選択している状態ではペースト出来ません");
            return;
        }
        Paste(CopyCurriculum.ReplaceMatrix());
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

    private void BackColorToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var cd = new ColorDialog
        {
            Color = Curriculum[dataGridView1.SelectedCells[0].ColumnIndex, dataGridView1.SelectedCells[0].RowIndex].GetBackColor()
        };
        if (cd.ShowDialog() != DialogResult.OK) return;

        foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
        {
            cell.Style.BackColor = cd.Color;
            Curriculum[cell.ColumnIndex, cell.RowIndex].SetColor(cd.Color);
        }
    }
}
