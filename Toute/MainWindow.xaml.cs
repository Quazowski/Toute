using Microsoft.Extensions.Options;
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

        /// <summary>
        /// Fired when window is deactivated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            (DataContext as WindowViewModel).OverlayVisible = true;
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            (DataContext as WindowViewModel).OverlayVisible = false;
        }
    }
}
