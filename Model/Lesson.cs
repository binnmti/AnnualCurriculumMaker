namespace Model;

public class Lesson
{
    public Lesson()
    { }

    public Lesson(string name, string quarter, string week, string year, string period)
    {
        Name = name;
        Quarter = quarter;
        Week = week;
        Year = year;
        Period = period;
    }

    public string Name { get; private set; } = "";
    public string Quarter { get; private set; } = "";
    public string Week { get; private set; } = "";
    public string Year { get; private set; } = "";
    public string Period { get; private set; } = "";
}


