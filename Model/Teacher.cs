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
            var text = Lessons.GroupBy(x => x.QuarterTitle).Select(q => $"{q.Key}:{q.Count()}");
            return $"{string.Join(',', text)},合計:{Lessons.Count}";
        }
    }
}
