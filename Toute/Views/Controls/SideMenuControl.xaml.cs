using System.ComponentModel;
using System.Windows.Controls;
using static Toute.DI;
using static Toute.Core.CoreDI;
using Toute.Core;

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
