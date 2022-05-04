namespace Model;

public class Curriculum
{
    public int Rows { get; }
    public int Cols { get; }
    public IList<CurriculumCell> Cells { get; } = new List<CurriculumCell>();

    public CurriculumCell this[int row, int col]
    {
        set { Cells[row * Cols + col] = value; }
        get { return Cells[row * Cols + col]; }
    }

    public Curriculum(int col, int row, List<string> colNames, List<string> rowNames)
    {
        Rows = row;
        Cols = col;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Cells.Add(new CurriculumCell(new Lesson("", colNames[j], rowNames[i]), new List<string>()));
            }
        }
    }
}