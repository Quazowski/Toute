using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Toute.Core;
using Toute.Core.Routes;
using Toute.Extensions;

namespace Toute
{
    /// <summary>
    /// A ViewModel for RegisterPage
    /// </summary>
    public class RegisterPageViewModel : BaseViewModel
    {
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
        public RegisterPageViewModel()
        {
            //Command that handle register
            RegisterCommand = new ParametrizedRelayCommand(async(parameter) => await RegisterAsync(parameter));

            //Command that handle going to login page
            GoToLogin = new RelayCommand(GoToLoginPage);
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
                if(string.IsNullOrEmpty(Username)|| string.IsNullOrEmpty(Email))
                {
                    PopupExtensions.NewInfoPopup("Provide all values");
                    return;
                }

                if((parameter as RegisterPage).MyPassword.SecurePassword.Unsecure().Length < 4)
                {
                    PopupExtensions.NewInfoPopup("Password must have at least 5 letters");
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
                        //Go to login page
                        IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.LoginPage);
                    }
                }
                else
                {
                    PopupExtensions.NewInfoPopup("Password, and confirm password must match!");
                }
            });

        }



        /// <summary>
        /// Method that handle going to login page
        /// </summary>
        private void GoToLoginPage()
        {
            //Go to login page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.LoginPage);
        }

        #endregion

    }
}
