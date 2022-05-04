using Model;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {

        Curriculum Curriculum { get; }

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

            Curriculum = new Curriculum(dataGridView1.ColumnCount, dataGridView1.RowCount);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var value = e.FormattedValue.ToString() ?? "";
            if (!Curriculum.TryParse(e.ColumnIndex, value, "", "", "", "", out var cell))
            {
                MessageBox.Show("�d�����Ă��܂��I");
                dataGridView1.CancelEdit();
                e.Cancel = false;
                return;
            }
            Curriculum[e.ColumnIndex, e.RowIndex] = cell;

            var teacher = Curriculum.ToTeacher();
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach(var t in teacher)
            {
                var item = listView1.Items.Add(t.Key);
                item.SubItems.Add(string.Join(',', t.Value.Select(x => x.Name)));
            }
            listView1.EndUpdate();
        }
    }
}