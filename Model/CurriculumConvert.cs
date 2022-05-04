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
}