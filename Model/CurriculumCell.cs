using System.Drawing;

namespace Model;

public class CurriculumCell
{
    public Lesson Lesson { get; }
    public List<string> Teachers { get; } = new List<string>();

    //Colorはデシアライズできない。
    private Color backColor = Color.White;
    public int TextColorValue
    {
        get => backColor.ToArgb();
        private set => backColor = Color.FromArgb(value);
    }

    public void SetColor(Color color) => TextColorValue = color.ToArgb();

    public Color GetBackColor() => backColor;

    public CurriculumCell(Lesson lesson, List<string> teachers, int textColorValue)
    {
        Lesson = lesson;
        Teachers = teachers;
        TextColorValue = textColorValue;
    }

    public string Value
    {
        get
        {
            if (Lesson.Name == "") return "";
            if (Teachers.Count == 0) return Lesson.Name;
            return $"{Lesson.Name}\n{string.Join(',', Teachers)}";
        }
    }
}

public static class CurriculumCellConvert
{
    //TODO:Titleはコピらない。ただちょっと筋が悪い気がする。。。
    public static CurriculumCell Copy(this CurriculumCell src, CurriculumCell dst)
    {
        var lesson = new Lesson(dst.Lesson.Name, src.Lesson.WeekTitle, src.Lesson.QuarterTitle, src.Lesson.YearTitle, src.Lesson.PeriodTitle);
        return new CurriculumCell(lesson, dst.Teachers, dst.TextColorValue);
    }
}
