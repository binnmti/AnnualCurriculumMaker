namespace Model;

public class Lesson
{
    public Lesson(string name, string colName, string rowName)
    {
        Name = name;
        ColName = colName;
        RowName = rowName;
    }

    public string Name { get; } = "";
    public string ColName { get; } = "";
    public string RowName { get; } = "";
}