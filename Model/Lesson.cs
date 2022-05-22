namespace AnnualCurriculumMaker;

public class Lesson
{
    public Lesson(string name, string weekTitle, string quarterTitle, string yearTitle, string periodTitle)
    {
        Name = name;
        WeekTitle = weekTitle;
        QuarterTitle = quarterTitle;
        YearTitle = yearTitle;
        PeriodTitle = periodTitle;
    }

    public string Name { get; } = "";
    public string WeekTitle { get; } = "";
    public string QuarterTitle { get; } = "";
    public string YearTitle { get; } = "";
    public string PeriodTitle { get; } = "";
}