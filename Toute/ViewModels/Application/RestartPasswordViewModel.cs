using NLog;
using System.Threading.Tasks;
using System.Windows.Input;
using Toute.Core;
using Toute.Core.Routes;
using Toute.Extensions;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// ViewModel for <see cref="RestartPasswordPage"/>
    /// </summary>
    public class RestartPasswordViewModel : BaseViewModel
    {
        #region Private members

        /// <summary>
        /// Logger for <see cref="RestartPasswordViewModel"/>
        /// </summary>
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public members

        /// <summary>
        /// Username or Email which will be restarted
        /// </summary>
        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// If <see cref="RestartPassword"/> is running, this
        /// field is true, otherwise false
        /// </summary>
        public bool RestartPasswordIsRunning { get; set; }

        #endregion

        #region Public commands

        /// <summary>
        /// Command to restart password
        /// </summary>
        public ICommand RestartPasswordCommand { get; set; }

        /// <summary>
        /// Command to go to login page
        /// </summary>
        public ICommand GoToLoginPageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RestartPasswordViewModel()
        {
            //Create commands
            RestartPasswordCommand = new RelayCommand(async () => await RestartPassword());
            GoToLoginPageCommand = new RelayCommand(GoToLoginPage);
        }

        #endregion

        #region Private Command methods

        /// <summary>
        /// Method to redirect user to login page
        /// </summary>
        private void GoToLoginPage()
        {
            ViewModelApplication.GoToPage(ApplicationPage.LoginPage);
        }

        /// <summary>
        /// Method to restart password,
        /// and if it succeeded redirect user to the login page
        /// </summary>
        /// <returns>Task</returns>
        private async Task RestartPassword()
        {
            await RunCommandAsync(() => RestartPasswordIsRunning, async () =>
            {
                _logger.Info("User is logging to application");

                //If there is any username is given...
                if (string.IsNullOrEmpty(UsernameOrEmail))
                {
                    PopupExtensions.NewInfoPopup("Provide username first");
                }

                //Send request with credentials to server, to restart password
                var context = await HttpExtensions.HandleHttpRequestAsync(UserRoutes.RestartPassword,
                        new RestartPasswordRequest
                        {
                            UsernameOrEmail = UsernameOrEmail
                        });

                //If password is restarted successfully
                if (context)
                {
                    PopupExtensions.NewInfoPopup("Password restarted. Check you mailbox to set new password.");
                    _logger.Debug("Successfully restarted password");
                    ViewModelApplication.GoToPage(ApplicationPage.LoginPage);
                }

            });
        }

        #endregion

    }
}
