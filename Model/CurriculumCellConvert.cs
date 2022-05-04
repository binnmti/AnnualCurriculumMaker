namespace Model;

public static class CurriculumCellConvert
{
    public static bool TryParse(this Curriculum curriculum, int col, string cell, string quarter, string week, string year, string period, out CurriculumCell curriculumCell)
    {
        string lesson;
        List<string> teachers = new();

        //,でつないでいる
        if (cell.Contains(','))
        {
            var words = cell.Split(',');
            lesson = words[0];
            teachers = words.Skip(1).ToList();

            curriculumCell = new CurriculumCell(new Lesson(lesson, quarter, week, year, period), teachers);
            foreach (var teacher in teachers)
            {
                if (curriculum.Any(teacher, col)) return false;
            }
            return true;
        }
        else
        {
            lesson = cell;
            curriculumCell = new CurriculumCell(new Lesson(lesson, quarter, week, year, period), teachers);
            return true;
        }
    }

    private static bool Any(this Curriculum curriculum, string teacherName, int col)
    {
        if (col >= curriculum.Cols) throw new ArgumentException();

        for (int row = 0; row < curriculum.Rows; row++)
        {
            var cell = curriculum[col, row];
            if (cell.Teachers.Any(x => x.Trim() == teacherName.Trim())) return true;
        }
        return false;
    }
}