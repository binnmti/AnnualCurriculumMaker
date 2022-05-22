using AnnualCurriculumMaker;
using System.Collections.ObjectModel;
using System.Windows;

namespace AnnualCurriculumMakerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            dataGrid.ItemsSource = new ObservableCollection<Curriculum>();
        }
    }
}
