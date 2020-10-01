using System;
using System.Windows.Input;
using Toute.Core;

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
            RegisterCommand = new ParametrizedRelayCommand((parameter) => Register(parameter));

            //Command that handle going to login page
            GoToLogin = new RelayCommand(GoToLoginPage);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Method that handle register
        /// </summary>
        /// <param name="parameter"></param>
        private async void Register(object parameter)
        {
            //NOTE: It is only for testing, should be replaced with properly register
            //Password should not be hold in variables
            if((parameter as IHaveDoublePassword).FirstSecureString.Unsecure() == (parameter as IHaveDoublePassword).SecondSecureString.Unsecure())
            {
                var response = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.ApiRegister,
                                new RegisterCredentialsApiModel
                                {
                                    Username = Username,
                                    Email = Email,
                                    Password = (parameter as IHaveDoublePassword).FirstSecureString.Unsecure()
                                });
            }
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
