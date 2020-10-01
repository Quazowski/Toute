using System.IO;
using System.Windows;

namespace Toute
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Override <see cref="OnStartup(StartupEventArgs)"/> to implement
        /// own function etc. before application start
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //Start application as normal...
            base.OnStartup(e);

            //Set up IoC for our application
            IoC.Setup();

            //On start go to LoginPage
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.LoginPage);

            //Create a new MainWindow
            Current.MainWindow = new MainWindow();

            //Show MainWindow
            Current.MainWindow.Show();
        }
    }
}
