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
    /// A ViewModel for RegisterPage
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {
        #region Private Members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public properties

        /// <summary>
        /// Username of user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email of user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Status of <see cref="RegisterAsync(object)"/>
        /// </summary>
        public bool RegisterIsRunning { get; set; }

        #endregion

        #region Commands
        /// <summary>
        /// Command that handle register
        /// </summary>
        public ICommand RegisterCommand { get; set; }
        /// <summary>
        /// Command that handle going to login page
        /// </summary>
        public ICommand GoToLogin { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisterViewModel()
        {
            _logger.Info("Start setting up RegisterViewModel");

            //Command that handle register
            RegisterCommand = new ParametrizedRelayCommand(async (parameter) => await RegisterAsync(parameter));

            //Command that handle going to login page
            GoToLogin = new RelayCommand(GoToLoginPage);

            _logger.Info("Done setting up RegisterViewModel");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Method that handle register
        /// </summary>
        /// <param name="parameter"></param>
        private async Task RegisterAsync(object parameter)
        {
            await RunCommandAsync(() => RegisterIsRunning, async () =>
            {
                _logger.Info("User tries to register...");

                //Checks if user password length is greater than 4 characters...
                if ((parameter as RegisterPage).MyPassword.SecurePassword.Unsecure().Length < 4)
                {
                    _logger.Info("User provided too short password. Aborting registration");
                    PopupExtensions.NewErrorPopup("Password must have at least 5 letters");
                    return;
                }
                //If password and confirm new password is the same...
                if ((parameter as RegisterPage).MyPassword.SecurePassword.Unsecure() == (parameter as RegisterPage).MyConfirmPassword.SecurePassword.Unsecure())
                {
                    //Make a request to register, with the credentials
                    var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<RegisterRequest>(UserRoutes.Register,
                                    new RegisterRequest
                                    {
                                        Username = Username,
                                        Email = Email,
                                        Password = (parameter as RegisterPage).MyPassword.SecurePassword.Unsecure()
                                    });

                    //If there is content back
                    if (context != null)
                    {
                        _logger.Info("User successfully registered, moving to login page...");
                        //Go to login page
                        ViewModelApplication.GoToPage(ApplicationPage.LoginPage);
                    }
                }
                else
                {
                    _logger.Info("User provided different password, and confirm password. Aborting registration");
                    PopupExtensions.NewErrorPopup("Password, and confirm password must match!");
                }
            });

        }



        /// <summary>
        /// Method that handle going to login page
        /// </summary>
        private void GoToLoginPage()
        {
            //Go to login page
            ViewModelApplication.GoToPage(ApplicationPage.LoginPage);
        }

        #endregion

    }
}
