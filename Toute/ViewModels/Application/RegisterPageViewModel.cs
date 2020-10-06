using System.Net;
using System.Windows.Input;
using Toute.Core;
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
            //If password and confirm new password is the same...
            if ((parameter as RegisterPage).MyPassword.SecurePassword.Unsecure() == (parameter as RegisterPage).MyConfirmPassword.SecurePassword.Unsecure())
            {
                //Send request to the server
                var response = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.ApiRegister,
                                new RegisterCredentialsApiModel
                                {
                                    Username = Username,
                                    Email = Email,
                                    Password = (parameter as RegisterPage).MyPassword.SecurePassword.Unsecure()
                                });


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read server response as ApiResponse<RegisterCredentialsApiModel>
                    var context = response.DeseralizeHttpResponse<ApiResponse<RegisterCredentialsApiModel>>();

                    //If register went successfully
                    if (context.IsSucessfull)
                    {   
                        //Go to login page
                        IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.LoginPage);
                    }
                    //Otherwise...
                    else
                    {
                        //Show error message
                        PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                    }
                }
                else
                {
                    //Display error
                    PopupExtensions.NewPopupWithMessage("Unknown error occurred");
                }
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
