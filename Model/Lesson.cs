namespace Model;

public class Lesson
{
    public Lesson(string name, string quarter, string week, string year, string period)
    {
        Name = name;
        Quarter = quarter;
        Week = week;
        Year = year;
        Period = period;
    }

    public string Name { get; } = "";
    public string Quarter { get; } = "";
    public string Week { get; } = "";
    public string Year { get; } = "";
    public string Period { get; } = "";
}