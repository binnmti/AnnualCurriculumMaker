namespace Model;

public class CurriculumCell
{
    public Lesson Lesson { get; }
    public List<string> Teachers { get; } = new List<string>();
    public CurriculumCell(Lesson lesson, List<string> teachers)
    {
        Lesson = lesson;
        Teachers = teachers;
    }

    public string Value
    {
        get
        {
            if (Lesson.Name == "") return "";
            if (Teachers.Count == 0) return Lesson.Name;
            return $"{Lesson.Name},{string.Join(',', Teachers)}";
        }
    } 

}