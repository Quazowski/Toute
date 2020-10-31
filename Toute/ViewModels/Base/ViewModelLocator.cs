using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// Locator that help find ViewModels in xaml, and keep single instances of them.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Static ViewModel of ApplicationViewModel
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel = ViewModelApplication;

        /// <summary>
        /// Static ViewModel of GamesPageViewModel
        /// </summary>
        public static GamesViewModel GamesPageViewModel = ViewModelGame;

        /// <summary>
        /// Static ViewModel of SideMenuViewModel
        /// </summary>
        public static SideMenuViewModel SideMenuViewModel = ViewModelSideMenu;
    }
}
