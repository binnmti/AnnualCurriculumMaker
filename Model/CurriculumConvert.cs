namespace Model;

public static class CurriculumConvert
{
    public static bool Any(this Curriculum curriculum, string teacherName, int col)
    {
        if (col >= curriculum.Cols) throw new ArgumentException();

        for (int row = 0; row < curriculum.Rows; row++)
        {
            var cell = curriculum[row, col];
            if(cell.Teachers.Any(x => x == teacherName)) return true;
        }
        return false;
    }

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
}