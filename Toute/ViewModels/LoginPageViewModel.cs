using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Toute
{
    public class LoginPageViewModel : BaseViewModel
    {
        public ICommand Testow { get; set; }
        public ICommand GoToRegister { get; set; }
        public LoginPageViewModel()
        {
            Testow = new ParamizedRelayCommand((parameter) => Test(parameter));
            GoToRegister = new ParamizedRelayCommand(GoToRegisterPage);

        }

        private void GoToRegisterPage(object obj)
        {
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.RegisterPage);
        }

        public void Test(object parameter)
        {
            var Test = (parameter as IHavePassword).SecureString.Unsecure();
        }
    }
}
