using System.Text.Json.Serialization;

namespace Model;

public class Curriculum
{
    public int Rows { get; }
    public int Cols { get; }
    public List<string> ColTitles { get; }
    public List<string> RowTitles { get; }
    public List<CurriculumCell> Cells { get; } = new List<CurriculumCell>();

    public Curriculum(int cols, int rows, List<string> colTitles, List<string> rowTitles) : this(cols, rows, colTitles, rowTitles, GetCells(cols, rows, colTitles, rowTitles))
    {
    }

    [JsonConstructor]
    public Curriculum(int cols, int rows, List<string> colTitles, List<string> rowTitles, List<CurriculumCell> cells)
    {
        Cols = cols;
        Rows = rows;
        ColTitles = colTitles;
        RowTitles = rowTitles;
        Cells = cells;
    }

    public CurriculumCell this[int row, int col]
    {
        set { Cells[row * Cols + col] = value; }
        get { return Cells[row * Cols + col]; }
    }

    public string GetColTitle(int col) => ColTitles[col];
    public string GetRowTitle(int row) => RowTitles[row];

    private static List<CurriculumCell> GetCells(int cols, int rows, List<string> colTitles, List<string> rowTitles)
    {
        List<CurriculumCell> cells = new();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                cells.Add(new CurriculumCell(new Lesson("", colTitles[j], rowTitles[i]), new List<string>()));
            }
        }
        return cells;
    }
}