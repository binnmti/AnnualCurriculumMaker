using AnnualCurriculumMaker;

namespace AnnualCurriculumMakerForWinForm;

internal static class AnnualCurriculumMakerControlExtention
{
    internal static void Update(this ListView listView, Curriculum curriculum)
    {
        var teachers = curriculum.ToTeachers();
        listView.BeginUpdate();
        listView.Items.Clear();
        foreach (var teacher in teachers)
        {
            var item = listView.Items.Add(teacher.Name);
            item.SubItems.Add(teacher.Frame);
            item.SubItems.Add(teacher.Lesson);
        }
        listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        listView.EndUpdate();
    }

    internal static Curriculum SetDataGridViewAndToCurriculum(this DataGridView dataGridView)
    {
        var weeks = new List<string>() { "月", "火", "水", "木", "金", "土" };
        var quarters = new List<string>() { "1Q", "2Q", "3Q", "4Q" };
        var years = new List<string>() { "1年", "2年", "3年", "4年" };
        var periods = new List<string>() { "1限", "2限", "3限", "4限", "5限", "6限" };
        var weekTitles = new List<string>();
        var quarterTitles = new List<string>();
        var yearTitles = new List<string>();
        var periodTitles = new List<string>();

        foreach (var quarter in quarters)
        {
            foreach (var week in weeks)
            {
                var column = new DataGridViewColumn
                {
                    HeaderText = $"{quarter}:{week}",
                    CellTemplate = new DataGridViewTextBoxCell(),
                };
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView.Columns.Add(column);
                weekTitles.Add(week);
                quarterTitles.Add(quarter);
            }
        }
        dataGridView.RowCount = weeks.Count * quarters.Count;
        int row = 0;
        foreach (var year in years)
        {
            foreach (var period in periods)
            {
                dataGridView.Rows[row].HeaderCell.Value = $"{year}:{period}";
                yearTitles.Add(year);
                periodTitles.Add(period);
                row++;
            }
        }
        dataGridView.RowHeadersWidth = 150;
        return new Curriculum(dataGridView.ColumnCount, dataGridView.RowCount, weekTitles, quarterTitles, yearTitles, periodTitles);
    }

    internal static void LoadCsv(this DataGridView dataGridView, Curriculum curriculum, string csv)
    {
        var colTitles = csv[..csv.IndexOf('\n')].Split(',');
        var cols = colTitles.Length - 1;
        int row = 0;
        foreach (var line in csv.Split(',').Chunk(cols))
        {
            var col = 0;
            foreach (var cell in line)
            {
                if (row != 0 && col != 0)
                {
                    dataGridView.Edit(col - 1, row - 1, cell.Replace("\"", ""), curriculum);
                }
                col++;
            }
            row++;
        }
    }

    internal static void Load(this DataGridView dataGridView, Curriculum curriculum)
    {
        for (int row = 0; row < curriculum.Rows; row++)
        {
            for (int col = 0; col < curriculum.Cols; col++)
            {
                dataGridView[col, row].SetDataGridViewCell(curriculum[col, row]);
            }
        }
    }

    internal static void Cut(this DataGridView dataGridView, Curriculum curriculum)
    {
        foreach (DataGridViewCell cell in dataGridView.SelectedCells)
        {
            curriculum.TryParse(cell.ColumnIndex, cell.RowIndex, "", out var curriculumCell);
            curriculum[cell.ColumnIndex, cell.RowIndex] = curriculumCell;
            cell.Value = "";

            curriculum[cell.ColumnIndex, cell.RowIndex].SetColor(Color.White);
            cell.Style.BackColor = Color.White;
        }
    }

    internal static Curriculum GetCopyCurriculum(this DataGridView dataGridView, Curriculum curriculum)
    {
        //選択した位置から最小になる矩形を作る
        int minRow = dataGridView.RowCount;
        int maxRow = 0;
        int minCol = dataGridView.ColumnCount;
        int maxCol = 0;
        for (int row = 0; row < dataGridView.RowCount; row++)
        {
            for (int col = 0; col < dataGridView.ColumnCount; col++)
            {
                if (dataGridView[col, row].Selected)
                {
                    minRow = Math.Min(minRow, row);
                    maxRow = Math.Max(maxRow, row);
                    minCol = Math.Min(minCol, col);
                    maxCol = Math.Max(maxCol, col);
                }
            }
        }
        //矩形のサイズのCurriculumを作る
        var copyCurriculum = new Curriculum(maxCol - minCol + 1, maxRow - minRow + 1);
        for (int row = 0; row < copyCurriculum.Rows; row++)
        {
            for (int col = 0; col < copyCurriculum.Cols; col++)
            {
                copyCurriculum[col, row] = curriculum[col + minCol, row + minRow];
            }
        }
        return copyCurriculum;
    }

    internal static void Paste(this DataGridView dataGridView, Curriculum curriculum, Curriculum copyCurriculum)
    {
        static CurriculumCell GetCell(CurriculumCell cell, CurriculumCell copyCell)
        {
            //タイトルだけそのまま使い、それ以外はコピーを使う
            var lesson = new Lesson(copyCell.Lesson.Name, cell.Lesson.WeekTitle, cell.Lesson.QuarterTitle, cell.Lesson.YearTitle, cell.Lesson.PeriodTitle);
            return new CurriculumCell(lesson, copyCell.Teachers, copyCell.TextColorValue);
        }

        for (int row = 0; row < copyCurriculum.Rows; row++)
        {
            for (int col = 0; col < copyCurriculum.Cols; col++)
            {
                var colIndex = Math.Min(dataGridView.SelectedCells[0].ColumnIndex + col, curriculum.Cols - 1);
                var rowIndex = Math.Min(dataGridView.SelectedCells[0].RowIndex + row, curriculum.Rows - 1);
                var cell = GetCell(curriculum[colIndex, rowIndex], copyCurriculum[col, row]);
                if (cell.Teachers.Any(t => curriculum.IsExist(t, colIndex, rowIndex)))
                {
                    MessageBox.Show($"{cell.Value}が重複しています！");
                }
                curriculum[colIndex, rowIndex] = cell;
                dataGridView[colIndex, rowIndex].SetDataGridViewCell(cell);
            }
        }
    }

    internal static void Edit(this DataGridView dataGridView, int col, int row, string value, Curriculum curriculum)
    {
        if (!curriculum.TryParse(col, row, value, out var cell))
        {
            MessageBox.Show($"{cell.Value}が重複しています！");
        }
        curriculum[col, row] = cell;
        dataGridView[col, row].SetDataGridViewCell(cell);
    }

    internal static void SetColor(this DataGridView dataGridView, Curriculum curriculum, Color color)
    {
        foreach (DataGridViewCell cell in dataGridView.SelectedCells)
        {
            curriculum[cell.ColumnIndex, cell.RowIndex].SetColor(color);
            cell.Style.BackColor = color;
        }
    }

    internal static void SelectName(this DataGridView dataGridView, Curriculum curriculum, string name)
    {
        dataGridView.ClearSelection();
        for (int row = 0; row < dataGridView.RowCount; row++)
        {
            for (int col = 0; col < dataGridView.ColumnCount; col++)
            {
                if (curriculum[col, row].Teachers.Any(t => t == name))
                {
                    dataGridView[col, row].Selected = true;
                }
            }
        }
    }

    private static void SetDataGridViewCell(this DataGridViewCell dataGridViewCell, CurriculumCell curriculumCell)
    {
        dataGridViewCell.Value = curriculumCell.Value;
        //TODO:Color.EmptyはColorがデシリアライズに対応していないので使えない。このやり方で良いか微妙
        var bg = curriculumCell.GetBackColor();
        if (bg != Color.FromArgb(0))
        {
            dataGridViewCell.Style.BackColor = bg;
        }
    }
}
