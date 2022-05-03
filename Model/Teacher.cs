namespace Model;

public static class TeacherConvert
{
    public static Dictionary<string, List<Lesson>> ToTeacher(this Curriculum Curriculum)
    {
        Dictionary<string, List<Lesson>> teacherList = new();
        foreach (var cell in Curriculum.Cells)
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

    //
    //internal static bool Check(this Curriculum Curriculum)
    //{

    //}

}