using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SideMenuControl()
        {
            //Prepares Components for MainWindow
            InitializeComponent();

            DataContext = new SideMenuViewModel();
        }
    }
}
