namespace Model;

public static class CurriculumCellConvert
{
    public static CurriculumCell Convert(string cell, string quarter, string week, string year, string period)
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