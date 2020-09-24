using System.Diagnostics;

namespace Toute
{
    public static class ApplicationPageHelper
    {
        public static BasePage GoToBasePage(ApplicationPage page)
        {
            switch(page)
            {
                case ApplicationPage.LoginPage:
                    return new LoginPage(new LoginPageViewModel());
                case ApplicationPage.RegisterPage:
                    return new RegisterPage(new RegisterViewModel());
                default:
                    Debugger.Break();
                    return null;
            }

        }
    }
}
