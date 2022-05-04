namespace Model;

public static class CurriculumConvert
{
    public static Dictionary<string, List<Lesson>> ToTeacher(this Curriculum curriculum)
    {
        Dictionary<string, List<Lesson>> teacherList = new();
        foreach (var cell in curriculum.Cells)
        {
            foreach (var teacher in cell.Teachers)
            {
                if (teacherList.ContainsKey(teacher))
                {
                    teacherList[teacher].Add(cell.Lesson);
                }
                else
                {
                    teacherList.Add(teacher, new List<Lesson>() { cell.Lesson });
                }
            }
        }
        return teacherList;
    }

    public static bool TryParse(this Curriculum curriculum, int col, int row, string cell, string colName, string rowName, out CurriculumCell curriculumCell)
    {
        string lesson;
        List<string> teachers = new();

        //,でつないでいる
        if (cell.Contains(','))
        {
            var words = cell.Split(',');
            lesson = words[0];
            teachers = words.Skip(1).ToList();

            curriculumCell = new CurriculumCell(new Lesson(lesson, colName, rowName), teachers);
            foreach (var teacher in teachers)
            {
                if (curriculum.Any(teacher, col, row)) return false;
            }
            return true;
        }
        else
        {
            lesson = cell;
            curriculumCell = new CurriculumCell(new Lesson(lesson, colName, rowName), teachers);
            return true;
        }
    }

    private static bool Any(this Curriculum curriculum, string teacherName, int colIndex, int rowIndex)
    {
        if (colIndex >= curriculum.Cols) throw new ArgumentException();

        for (int row = 0; row < curriculum.Rows; row++)
        {
            if (row == rowIndex) continue;

            var cell = curriculum[colIndex, row];
            if (cell.Teachers.Any(x => x.Trim() == teacherName.Trim())) return true;
        }
        return false;
    }
}