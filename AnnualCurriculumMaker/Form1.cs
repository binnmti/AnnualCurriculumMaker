using Model;
using System.Text;

namespace AnnualCurriculumMaker;

public partial class Form1 : Form
{
    private string FileName = "";
    private string JsonString = "";
    private string CellBeginText = "";
    private Curriculum Curriculum { get; set; }
    private Curriculum CopyCurriculum { get; set; }

    public Form1()
    {
        InitializeComponent();

        PasteToolStripMenuItem.Enabled = false;
        MatrixReplacePasteToolStripMenuItem.Enabled = false;
        Curriculum = dataGridView1.SetDataGridViewAndToCurriculum();
        CopyCurriculum = Curriculum;
    }

    private string GetTitle()
    {
        var title = $"年間カリキュラムメーカー";
        if (!string.IsNullOrEmpty(FileName)) title += $" - {Path.GetFileName(FileName)}";
        if (JsonString != Curriculum.ToJson(false)) title += "*";
        return title;
    }

    private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
    {
        openFileDialog1.InitialDirectory = Application.ExecutablePath;
        openFileDialog1.ShowDialog();
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

    private void LoadFile(string fileName)
    {
        //TODO:csvロードも出来そう。
        Curriculum = CurriculumConvert.ToCurriculum(File.ReadAllText(fileName));
        dataGridView1.Load(Curriculum);
        listView1.Update(Curriculum);
        FileName = fileName;
        JsonString = Curriculum.ToJson(false);
        Text = GetTitle();
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

    private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
    {
        SaveFile(saveFileDialog1.FileName);
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

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Application.Exit();
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
        //,の時は自動改行
        if (value.Contains(',')) value = $"{value[..value.IndexOf(',')]}\n{value[(value.IndexOf(',') + 1)..]}";

        dataGridView1.Edit(e.ColumnIndex, e.RowIndex, value, Curriculum);
        listView1.Update(Curriculum);
    }

    private void CutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CopyCurriculum = dataGridView1.GetCopyCurriculum(Curriculum);
        dataGridView1.Cut(Curriculum);
        listView1.Update(Curriculum);
        PasteToolStripMenuItem.Enabled = true;
        MatrixReplacePasteToolStripMenuItem.Enabled = true;
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        CopyCurriculum = dataGridView1.GetCopyCurriculum(Curriculum);
        PasteToolStripMenuItem.Enabled = true;
        MatrixReplacePasteToolStripMenuItem.Enabled = true;
    }

    private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedCells.Count > 1)
        {
            MessageBox.Show("複数選択している状態ではペースト出来ません");
            return;
        }
        dataGridView1.Paste(Curriculum, CopyCurriculum);
        listView1.Update(Curriculum);
    }

    private void MatrixReplacePasetToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (dataGridView1.SelectedCells.Count > 1)
        {
            MessageBox.Show("複数選択している状態ではペースト出来ません");
            return;
        }
        dataGridView1.Paste(Curriculum, CopyCurriculum.ReplaceMatrix());
        listView1.Update(Curriculum);
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        dataGridView1.Cut(Curriculum);
        listView1.Update(Curriculum);
    }

    private void BackColorToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var cd = new ColorDialog
        {
            Color = Curriculum[dataGridView1.SelectedCells[0].ColumnIndex, dataGridView1.SelectedCells[0].RowIndex].GetBackColor()
        };
        if (cd.ShowDialog() != DialogResult.OK) return;

        dataGridView1.SetColor(Curriculum, cd.Color);
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count == 0) return;

        dataGridView1.SelectName(Curriculum, listView1.SelectedItems[0].Text);
    }
}
