namespace AnnualCurriculumMaker;

public class Teacher
{
    public string Name { get; } = "";
    public List<Lesson> Lessons { get; } = new List<Lesson>();
    public Teacher(string name, List<Lesson> lessons)
    {
        Name = name;
        Lessons = lessons;
    }

    public string Lesson
    {
        get
        {
            var text = Lessons.GroupBy(x => x.Name).Select(g => g.Count() > 1
            ? $"[{g.First().WeekTitle}:{g.First().QuarterTitle}:{g.First().YearTitle}:{g.First().PeriodTitle}-{g.Last().PeriodTitle}]"
            : $"[{g.First().WeekTitle}:{g.First().QuarterTitle}:{g.First().YearTitle}:{g.First().PeriodTitle}]");
            return string.Join(',', text);
        }
    }

    public string Frame
    {
        get
        {
            var text = new List<string>() { "1Q", "2Q", "3Q", "4Q" }.Select(q => $"{q}:{Lessons.Count(x => x.QuarterTitle == q):D2}");
            return $"{string.Join(',', text)},合計:{Lessons.Count:D2}";
        }
    }
}
