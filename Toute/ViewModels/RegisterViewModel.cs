using System.Windows.Input;

namespace Toute
{
    public class RegisterViewModel : BaseViewModel
    {
        public ICommand GoToLogin { get; set; }

        public RegisterViewModel()
        {
            GoToLogin = new RelayCommand(GoToLoginPage);
        }

        private void GoToLoginPage()
        {
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.LoginPage);
        }
    }
}
