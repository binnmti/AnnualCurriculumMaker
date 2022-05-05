namespace Model;

public class Lesson
{
    public Lesson(string name, string colTitle, string rowTitle)
    {
        Name = name;
        ColTitle = colTitle;
        RowTitle = rowTitle;
    }

    public string Name { get; } = "";
    public string ColTitle { get; } = "";
    public string RowTitle { get; } = "";
}