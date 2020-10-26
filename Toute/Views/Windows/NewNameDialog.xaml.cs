using System.Windows;

namespace Toute
{
    /// <summary>
    /// Interaction logic for  AddGameWindow.xaml
    /// </summary>
    public partial class NewNameDialog : Window
    {
        /// <summary>
        /// Default constructor of AddGameWindow
        /// </summary>
        public NewNameDialog()
        {
            //Prepares components for window
            InitializeComponent();

            //Makes owner of window a MainWindow
            Owner = Application.Current.MainWindow;

            //Binds DataContext to AddGameWindowViewModel and pass window as parameter
            DataContext = new NewNameDialogViewModel(this);
        }
    }
}
