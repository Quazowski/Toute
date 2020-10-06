using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Toute.Core;

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
        /// Model that hold actual info, of friend that is managed
        /// </summary>
        public FriendUserModel Friend { get; set; }
        
        /// <summary>
        /// If user is logged, store Friends in a list
        /// </summary>
        public ObservableCollection<ChatUserModel> Friends { get; set; }

        /// <summary>
        /// Information about user, that are already logged to application
        /// </summary>
        public ApplicationUserModel ApplicationUser { get; set; }
        
        /// <summary>
        /// Current viewModel of application
        /// </summary>
        public BaseViewModel CurrentViewModel { get; set; }

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

        /// <summary>
        /// Command that handle going to GamesPage
        /// </summary>
        public ICommand GamesCommand { get; set; }

        /// <summary>
        /// Current Application Page of enum value
        /// </summary>
        public ApplicationPage CurrentApplicationPage { get; set; }

        /// <summary>
        /// Command that handle going to OptionsPage
        /// </summary>
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

            //Create new List of friends
            Friends = new ObservableCollection<ChatUserModel>();

            //Create commands
            GamesCommand = new RelayCommand(GoToGamesPage);
            SettingsCommand = new RelayCommand(GoToSettingsPage);


        }

        #endregion

        #region Public Helpers

        /// <summary>
        /// Method that have to be used every time,
        /// when user changes page. Changes frame in <see cref="MainWindow"/>
        /// </summary>
        /// <param name="page">Page which to go</param>
        /// <param name="viewModel">If page is the same, but viewModel different,
        /// pass new viewModel here</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            TimerExtensions.RemoveRepetingMessagesFromApplicationUser();

            //If it is the same page and view model return
            if (CurrentApplicationPage == page && CurrentViewModel == viewModel)
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

            //Set currentViewModel to viewModel
            CurrentViewModel = viewModel;

            //Sets CurrentApplicationPage to given page
            CurrentApplicationPage = page;

            //Sets frame to given page
            CurrentPage = ApplicationPageHelper.GoToBasePage(CurrentApplicationPage, viewModel);

            
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
