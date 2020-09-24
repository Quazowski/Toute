using System.Windows;

namespace Toute
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            //Prepares Components for MainWindow
            InitializeComponent();

            //Sets DataContext of MainWindow to WindowViewModel
            DataContext = new WindowViewModel(this);
        }
    }
}
