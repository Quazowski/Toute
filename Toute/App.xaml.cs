using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.Threading.Tasks;
using System.Windows;
using Toute.Core;
using static Toute.Core.CoreDI;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private members

        private readonly ILogger<App> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger">Default logger for application</param>
        public App(ILogger<App> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Application Setup

        /// <summary>
        /// Override <see cref="OnStartup(StartupEventArgs)"/> to implement
        /// own function etc. before application start
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            _logger.LogInformation("Application starting...");

            //Set base settings for app
            base.OnStartup(e);

            //Setup application
            await ApplicationSetup();

            //Create a new MainWindow
            Current.MainWindow = new MainWindow();

            //Show MainWindow
            Current.MainWindow.Show();
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Helper that setup basics for application
        /// </summary>
        /// <returns>Task</returns>
        private async Task ApplicationSetup()
        {
            //Configure IHostBuilder
            ConfigureDI().ConfigureServices((context, services) =>
            {
                Configuration = context.Configuration;
                Services = services;
                services.AddViewModels();
                services.AddSqliteDb();
            }).ConfigureLogging(options =>
            {
                options.SetMinimumLevel(LogLevel.Information);
                options.AddNLog("nlog.config");
            }).BuildDI();


            _logger.LogInformation("Configured IWebHost for application");


            await SqliteDb.EnsureDataStoreAsync();

            //If user credentials were saved...
            if (SqliteDb.HasCredentials())
            {
                //If user credentials exists, get all user credentials
                //and go to GamesPage
                await UserApplicationHelpers.LoginToApp(_logger);
                ViewModelApplication.GoToPage(ApplicationPage.GamesPage);
            }
            else
            {
                ViewModelApplication.GoToPage(ApplicationPage.LoginPage);
            }
        }

        #endregion

    }
}
