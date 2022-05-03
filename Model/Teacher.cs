namespace Model;

internal class Teacher
{
    public string Name { get; private set; } = "";
    public List<string> Lessons { get; private set; } = new List<string>();
}



internal static class TeacherConvert
{
    //internal static Teacher ToTeacher(this Curriculum Curriculum)
    //{
    //    var teachers = Curriculum.Cell.GroupBy(x => x.Teachers);
    //}

    //
    //internal static bool Check(this Curriculum Curriculum)
    //{

    //}

}