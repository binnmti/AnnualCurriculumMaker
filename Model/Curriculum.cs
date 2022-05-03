namespace Model;

public class Curriculum
{
    public int Rows { get; }
    public int Cols { get; }
    public List<CurriculumCell> Cells { get; } = new List<CurriculumCell>();

    public CurriculumCell this[int row, int col]
    {
        set { Cells[row * Cols + col] = value; }
        get { return Cells[row * Cols + col]; }
    }

    public Curriculum(int row, int col)
    {
        Rows = row;
        Cols = col;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Cells.Add(new CurriculumCell(new Lesson(), new List<string>()));
            }
        }
    }
}

public class CurriculumCell
{
    public Lesson Lesson { get; set; }
    public List<string> Teachers { get; set; } = new List<string>();
    public CurriculumCell(Lesson lesson, List<string> teachers)
    {
        Lesson = lesson;
        Teachers = teachers;
    }
}

public static class CheckCurriculumCell
{
    public static CurriculumCell ConvertCell(string cell, string quarter, string week, string year, string period)
    {
        string lesson;
        List<string> teachers = new();

        //,でつないでいる
        if (cell.Contains(','))
        {
            var words = cell.Split(',');
            lesson = words[0];
            teachers = words.Skip(1).ToList();
        }
        else
        {
            lesson = cell;
        }
        return new CurriculumCell(new Lesson(lesson, quarter, week, year, period), teachers);
    }
}