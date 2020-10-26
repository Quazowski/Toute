using System.Diagnostics;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// ApplicationPageHelper, that will be used every time
    /// if page of application will changed
    /// </summary>
    public static class ApplicationPageHelper
    {
        /// <summary>
        /// Extensions that will be used to change
        /// page of application
        /// </summary>
        /// <param name="page">Page on which frame of application should be changed</param>
        /// <param name="viewModel">ViewModel to pass to next page, can be null</param>
        /// <returns>New page</returns>
        public static BasePage GoToBasePage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            //Switch between ApplicationPage
            switch (page)
            {
                //If it is LoginPage
                case ApplicationPage.LoginPage:
                    //return new LoginPage of LoginPageViewModel
                    return new LoginPage(new LoginViewModel());

                //If it is RegisterPage
                case ApplicationPage.RegisterPage:
                    //return new LoginPage of RegisterPageViewModel
                    return new RegisterPage(new RegisterViewModel());

                //If it is RestartPasswordPage
                case ApplicationPage.RestartPasswordPage:
                    //return new RestartPasswordPage of RestartPasswordViewModel
                    return new RestartPasswordPage(new RestartPasswordViewModel());

                //If it is GamesPage
                case ApplicationPage.GamesPage:
                    //return static GamesPage of static GamesPageViewModel
                    return new GamesPage(ViewModelGame);

                //If it is ContactPage
                case ApplicationPage.ContactPage:
                    //return new ContactPage of ContactPageViewModel, and sends given ViewModel
                    return new ContactPage(new ContactPageViewModel(viewModel));

                //If it is SettingsPage
                case ApplicationPage.SettingsPage:
                    //return new SettingsPage of SettingsPageViewModel
                    return new SettingsPage(new SettingsViewModel());

                //If none of this
                default:
                    //Break to give developer info
                    Debugger.Break();
                    //and return null
                    return null;
            }
        }
    }
}
