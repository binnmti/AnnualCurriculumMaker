using System.Text.Json.Serialization;

namespace Model;

public class Curriculum
{
    public int Rows { get; }
    public int Cols { get; }
    public List<string> ColNames { get; }
    public List<string> RowNames { get; }
    public List<CurriculumCell> Cells { get; } = new List<CurriculumCell>();

    public CurriculumCell this[int row, int col]
    {
        set { Cells[row * Cols + col] = value; }
        get { return Cells[row * Cols + col]; }
    }

    public string GetColName(int col) => ColNames[col];
    public string GetRowName(int row) => RowNames[row];

    public Curriculum(int cols, int rows, List<string> colNames, List<string> rowNames) : this(cols, rows, colNames, rowNames, GetCells(cols, rows, colNames, rowNames))
    {
    }

    [JsonConstructor]
    public Curriculum(int cols, int rows, List<string> colNames, List<string> rowNames, List<CurriculumCell> cells)
    {
        Cols = cols;
        Rows = rows;
        ColNames = colNames;
        RowNames = rowNames;
        Cells = cells;
    }

    private static List<CurriculumCell> GetCells(int cols, int rows, List<string> colNames, List<string> rowNames)
    {
        List<CurriculumCell> cells = new();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                cells.Add(new CurriculumCell(new Lesson("", colNames[j], rowNames[i]), new List<string>()));
            }
        }
        return cells;
    }
}