using System.Windows.Input;

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
        public string Username { get; set; }

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
        public void Login(object parameter)
        {
            //NOTE: It is only for testing, should be replaced with properly register
            //Password should not be hold in variables
            var password = (parameter as IHavePassword).SecureString.Unsecure();
        }

        #endregion
    }
}
