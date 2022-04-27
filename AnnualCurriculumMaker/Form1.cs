namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private  List<string> Weeks = new List<string>() { "��", "��", "��", "��", "��", "�y" };
        public Form1()
        {
            InitializeComponent();
            for (int q = 0; q < 4; q++)
            {
                foreach(var week in Weeks)
                {
                    dataGridView1.Columns.Add($"", $"{q + 1}Q:{week}");
                }
            }
            dataGridView1.RowCount = 4 * 6;

            int row = 0;
            for (int year = 0; year < 4; year++)
            {
                for (int t = 0; t < 6; t++)
                {
                    dataGridView1.Rows[row].HeaderCell.Value = $"{year + 1}�N:{t + 1}��";
                    row++;
                }
            }
        }
    }
}