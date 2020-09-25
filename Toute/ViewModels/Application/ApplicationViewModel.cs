using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Toute
{
    /// <summary>
    /// A ViewModel that is most important
    /// Will handle all changes on application
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private properties

        /// <summary>
        /// Current application page in frame
        /// </summary>
        private BasePage currentPage;

        #endregion

        #region Public properties

        /// <summary>
        /// Determines a visibility of side menu
        /// </summary>
        public bool SideMenuHidden { get; set; }

        /// <summary>
        /// Current Application Page of enum value
        /// </summary>
        public ApplicationPage CurrentApplicationPage { get; set; }

        /// <summary>
        /// Current application page in frame
        /// </summary>
        public BasePage CurrentPage
        {
            get => currentPage;
            set
            {
                if (CurrentPage == value)
                    return;

                currentPage = value;
            }
        }

        #endregion

        #region Public Commands

        //Command that handle going to GamesPage
        public ICommand GamesCommand { get; set; }
        //Command that handle going to ContactPage
        public ICommand ContactCommand { get; set; }
        //Command that handle going to OptionsPage
        public ICommand SettingsCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for <see cref="ApplicationViewModel"/>
        /// </summary>
        public ApplicationViewModel()
        {
            //If application is in designer mode...
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                //Set here DesignPage value
                ApplicationPage DesignPage = ApplicationPage.GamesPage;

                //if DesignPage is LoginPage or RegisterPage...
                if ((DesignPage == ApplicationPage.LoginPage) || (DesignPage == ApplicationPage.RegisterPage))
                {
                    //Hide side menu
                    SideMenuHidden = true;
                }
                //Otherwise...
                else
                {
                    //Show Side Menu
                    SideMenuHidden = false;
                }

                //Set current page to DesignPage value
                CurrentPage = ApplicationPageHelper.GoToBasePage(DesignPage);                
            }

            //Command that handle going to GamesPage
            GamesCommand = new RelayCommand(GoToGamesPage);

            //Command that handle going to ContactPage
            ContactCommand = new RelayCommand(GoToContactPage);

            //Command that handle going to OptionsPage
            SettingsCommand = new RelayCommand(GoToSettingsPage);
        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Method that have to be used every time, when page of 
        /// main frame in application should frame
        /// </summary>
        /// <param name="page">Page which to go</param>
        public void GoToPage(ApplicationPage page)
        {

            //If it is the same page return
            if (CurrentApplicationPage == page)
                return;

            //If page is LoginPage or RegisterPage...
            if ((page == ApplicationPage.LoginPage) || (page == ApplicationPage.RegisterPage))
            {
                //Hide side menu
                SideMenuHidden = true;
            }
            //Otherwise...
            else
            {
                // Show side menu
                SideMenuHidden = false;
            }

            //Sets CurrentApplicationPage to given page
            CurrentApplicationPage = page;

            //Sets frame to given page
            CurrentPage = ApplicationPageHelper.GoToBasePage(CurrentApplicationPage);
        }

        /// <summary>
        /// Method that handle going to GamesPage
        /// </summary>
        private void GoToGamesPage()
        {
            //Go to GamesPage
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GamesPage);
        }
        /// <summary>
        /// Method that handle going to ContactPage
        /// </summary>
        private void GoToContactPage()
        {
            //Go to ContactPage
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.ContactPage);
        }
        /// <summary>
        /// Method that handle going to OptionsPage
        /// </summary>
        private void GoToSettingsPage()
        {
            //Go to SettingsPage
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.SettingsPage);
        }

        #endregion
    }
}
