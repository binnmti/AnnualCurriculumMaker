namespace Model;

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