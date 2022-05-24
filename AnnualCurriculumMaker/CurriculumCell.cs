using System.Drawing;

namespace AnnualCurriculumMaker;

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
            var teacher = "";
            for(int i = 0; i < Teachers.Count; i++)
            {
                if (i == Teachers.Count - 1)
                {
                    teacher += Teachers[i];
                }
                else if (i % 3 == 2)
                {
                    teacher += Teachers[i] + "\n";
                }
                else
                {
                    teacher += Teachers[i] + ",";
                }
            }
            return $"{Lesson.Name}\n{teacher}";
        }
    }
}
