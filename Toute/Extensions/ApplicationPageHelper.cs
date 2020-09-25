using System.Diagnostics;

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
        /// <returns>New page</returns>
        public static BasePage GoToBasePage(ApplicationPage page)
        {
            //Switch between ApplicationPage
            switch (page)
            {
                //If it is LoginPage
                case ApplicationPage.LoginPage:
                    //return new LoginPage of LoginPageViewModel
                    return new LoginPage(new LoginPageViewModel());
                //If it is RegisterPage

                case ApplicationPage.RegisterPage:
                    //return new LoginPage of RegisterPageViewModel
                    return new RegisterPage(new RegisterPageViewModel());

                //If it is GamesPage
                case ApplicationPage.GamesPage:
                    //return new GamesPage of Game PageViewModel
                    return new GamesPage(new GamesPageViewModel());

                //If it is ContactPage
                case ApplicationPage.ContactPage:
                    //return new ContactPage of ContactPageViewModel
                    return new ContactPage(new ContactPageViewModel());

                //If it is SettingsPage
                case ApplicationPage.SettingsPage:
                    //return new SettingsPage of SettingsPageViewModel
                    return new SettingsPage(new SettingsPageViewModel());

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
