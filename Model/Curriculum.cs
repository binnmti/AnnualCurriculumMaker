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

    public Curriculum(int cols, int rows, List<string> colNames, List<string> rowNames)
    {
        Cols = cols;
        Rows = rows;
        ColNames = colNames;
        RowNames = rowNames;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Cells.Add(new CurriculumCell(new Lesson("", colNames[j], rowNames[i]), new List<string>()));
            }
        }
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
}




//public class Rootobject
//{
//    public int Rows { get; set; }
//    public int Cols { get; set; }
//    public Cell[] Cells { get; set; }
//}

//public class Cell
//{
//    public Lesson Lesson { get; set; }
//    public string[] Teachers { get; set; }
//}

//public class Lesson
//{
//    public string Name { get; set; }
//    public string ColName { get; set; }
//    public string RowName { get; set; }
//}
