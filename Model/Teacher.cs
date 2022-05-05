namespace Model;

public class Teacher
{
    public string Name { get; } = "";
    public List<Lesson> Lessons { get; } = new List<Lesson>();
    public Teacher(string name, List<Lesson> lessons)
    {
        Name = name;
        Lessons = lessons;
    }
}
