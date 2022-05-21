using System.Drawing;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Model;

public static class CurriculumConvert
{
    public static string ToCsv(this Curriculum curriculum)
    {
        var csv = new StringBuilder();

        csv.Append(',');
        for (int col = 0; col < curriculum.Cols; col++)
        {
            csv.Append($"\"{curriculum.GetQuarterTitle(col)}:{curriculum.GetWeekTitle(col)}\",");
        }
        csv.AppendLine();
        for (int row = 0; row < curriculum.Rows; row++)
        {
            csv.Append($"\"{curriculum.GetYearTitle(row)}:{curriculum.GetPeriodTitle(row)}\",");
            for (int col = 0; col < curriculum.Cols; col++)
            {
                if(curriculum[col, row].Value == "")
                {
                    csv.Append(',');
                }
                else
                {
                    csv.Append($"\"{curriculum[col, row].Value}\",");
                }
            }
            csv.AppendLine();
        }
        return csv.ToString();
    }

    public static string ToJson(this Curriculum curriculum, bool indent)
        => JsonSerializer.Serialize(curriculum, new JsonSerializerOptions { WriteIndented = indent, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });

    public static Curriculum ToCurriculum(string json)
        => JsonSerializer.Deserialize<Curriculum>(json) ?? new Curriculum(0, 0, new List<string>(), new List<string>(), new List<string>(), new List<string>());

    public static IEnumerable<Teacher> ToTeachers(this Curriculum curriculum)
    {
        Dictionary<string, List<Lesson>> teacherList = new();
        foreach (var cell in curriculum.Cells)
        {
            foreach (var teacher in cell.Teachers)
            {
                if (teacherList.ContainsKey(teacher))
                {
                    teacherList[teacher].Add(cell.Lesson);
                }
                else
                {
                    teacherList.Add(teacher, new List<Lesson>() { cell.Lesson });
                }
            }
        }
        return teacherList.Select(x => new Teacher(x.Key, x.Value));
    }

    public static bool TryParse(this Curriculum curriculum, int col, int row, string cell, out CurriculumCell curriculumCell)
    {
        string lesson;
        List<string> teachers = new();
        var weekTitle = curriculum.GetWeekTitle(col);
        var quarterTitle = curriculum.GetQuarterTitle(col);
        var yearTitle = curriculum.GetYearTitle(row);
        var periodTitle = curriculum.GetPeriodTitle(row);
        //,でつないでいる
        if (cell.Contains(','))
        {
            var words = cell.Split(',');
            lesson = words[0];
            teachers = words.Skip(1).ToList();
            curriculumCell = new CurriculumCell(new Lesson(lesson, weekTitle, quarterTitle, yearTitle, periodTitle), teachers, 0);
            return !teachers.Any(t => curriculum.IsExist(t, col, row));
        }
        else
        {
            lesson = cell;
            curriculumCell = new CurriculumCell(new Lesson(lesson, weekTitle, quarterTitle, yearTitle, periodTitle), teachers, 0);
            return true;
        }
    }

    public static bool IsExist(this Curriculum curriculum, int col, int row)
        => curriculum[col, row].Teachers.Any(t => curriculum.IsExist(t, col, row));

    private static bool IsExist(this Curriculum curriculum, string teacherName, int colIndex, int rowIndex)
    {
        if (colIndex >= curriculum.Cols) throw new ArgumentException();

        var yearTitle = curriculum.GetYearTitle(rowIndex);
        var periodTitle = curriculum.GetPeriodTitle(rowIndex);
        for (int row = 0; row < curriculum.Rows; row++)
        {
            if (row == rowIndex) continue;

            var cell = curriculum[colIndex, row];
            if (cell.Teachers.Any(x => x.Trim() == teacherName.Trim() && cell.Lesson.YearTitle != yearTitle && cell.Lesson.PeriodTitle == periodTitle)) return true;
        }
        return false;
    }
}