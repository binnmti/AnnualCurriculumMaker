using System.Text.Json.Serialization;

namespace AnnualCurriculumMaker;

public class Curriculum
{
    public int Cols { get; }
    public int Rows { get; }
    public List<string> WeekTitles { get; }
    public List<string> QuarterTitles { get; }
    public List<string> YearTitles { get; }
    public List<string> PeriodTitles { get; }
    public List<CurriculumCell> Cells { get; } = new List<CurriculumCell>();

    public Curriculum(int cols, int rows)
        : this(cols, rows, new List<string>(), new List<string>(), new List<string>(), new List<string>(), GetCells(cols, rows, new List<string>(), new List<string>(), new List<string>(), new List<string>()))
    {
    }

    public Curriculum(int cols, int rows, List<string> weekTitles, List<string> quarterTitles, List<string> yearTitles, List<string> periodTitles)
        : this(cols, rows, weekTitles, quarterTitles, yearTitles, periodTitles, GetCells(cols, rows, weekTitles, quarterTitles, yearTitles, periodTitles))
    {
    }

    [JsonConstructor]
    public Curriculum(int cols, int rows, List<string> weekTitles, List<string> quarterTitles, List<string> yearTitles, List<string> periodTitles, List<CurriculumCell> cells)
    {
        Cols = cols;
        Rows = rows;
        WeekTitles = weekTitles;
        QuarterTitles = quarterTitles;
        YearTitles = yearTitles;
        PeriodTitles = periodTitles;
        Cells = cells;
    }

    public CurriculumCell this[int col, int row]
    {
        set { Cells[row * Cols + col] = value; }
        get { return Cells[row * Cols + col]; }
    }

    public string GetQuarterTitle(int col) => QuarterTitles[col];
    public string GetWeekTitle(int col) => WeekTitles[col];
    public string GetYearTitle(int row) => YearTitles[row];
    public string GetPeriodTitle(int row) => PeriodTitles[row];


    private static List<CurriculumCell> GetCells(int cols, int rows, List<string> weekTitles, List<string> quarterTitles, List<string> yearTitles, List<string> periodTitles)
    {
        List<CurriculumCell> cells = new();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var weekTitle = j < weekTitles?.Count ? weekTitles[j] : "";
                var quarterTitle = j < quarterTitles?.Count ? quarterTitles[j] : "";
                var yearTitle = j < yearTitles?.Count ? yearTitles[i] : "";
                var periodTitle = j < periodTitles?.Count ? periodTitles[i] : "";
                cells.Add(new CurriculumCell(new Lesson("", weekTitle, quarterTitle, yearTitle, periodTitle), new List<string>(), 0));
            }
        }
        return cells;
    }
}