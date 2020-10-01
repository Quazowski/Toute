using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Documents;
using System.Windows.Input;
using Toute.Core;
using Toute.Core.DataModels;

namespace Toute
{
    /// <summary>
    /// A ViewModel for LoginPage
    /// </summary>
    public class LoginPageViewModel : BaseViewModel
    {
        #region Public properties

        /// <summary>
        /// Username of user
        /// </summary>
        public string Username { get; set; } = "Testuser";

        #endregion

        #region Commands
        /// <summary>
        /// Command that handle login
        /// </summary>
        public ICommand LoginCommand { get; set; }
        /// <summary>
        /// Command that handle going to register page
        /// </summary>
        public ICommand GoToRegister { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginPageViewModel()
        {
            //Command that handle login
            LoginCommand = new ParametrizedRelayCommand((parameter) => Login(parameter));

            //Command that handle going to register page
            GoToRegister = new RelayCommand(GoToRegisterPage);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Method that handle login
        /// </summary>
        private void GoToRegisterPage()
        {
            //Go to register page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.RegisterPage);
        }

        /// <summary>
        /// Method that handle going to register page
        /// </summary>
        /// <param name="parameter"></param>
        public async void Login(object parameter)
        {
            //NOTE: It is only for testing, should be replaced with properly register
            //Password should not be hold in variables

            var response = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.ApiLogin,
                new LoginCredentialsApiModel
                {
                    Username = Username,
                    Password = /*(parameter as IHavePassword).SecureString.Unsecure()*/ "Mypassword1!"
                });

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var responseContext = response.Content.ReadAsStringAsync().Result;

                var Context = JsonConvert.DeserializeObject<ApiResponse<LoginResponseApiModel>>(responseContext);

                IoC.Get<ApplicationViewModel>().ApplicationUser = new ApplicationUserModel
                {
                    Id = Context.TResponse.Id,
                    Username = Context.TResponse.Username,
                    Email = Context.TResponse.Email,
                    Friends = Context.TResponse.Friends

            };
                if (IoC.Get<ApplicationViewModel>().ApplicationUser.Friends != null)
                {
                    foreach (var user in IoC.Get<ApplicationViewModel>().ApplicationUser.Friends)
                    {
                        IoC.Get<ApplicationViewModel>().Friends.Add(new ChatUserModel
                        {
                            Name = user.Name,
                            Status = user.Status
                        });

                    }
                }
                
                IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GamesPage);
            }
            else
            {
                //TODO: Show the password or login were wrong
            }

           

        }

        #endregion
    }
}
