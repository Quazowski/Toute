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
        /// Model that hold actual ID, of friend that is managed
        /// </summary>
        public string CurrentFriendId { get; set; }
        
        /// <summary>
        /// If user is logged, store Friends in a list
        /// </summary>
        public ObservableCollection<FriendModel> Friends { get; set; }

        /// <summary>
        /// Information about user, that are already logged to application
        /// </summary>
        public ApplicationUserModel ApplicationUser { get; set; }
        
        /// <summary>
        /// Current viewModel of application
        /// </summary>
        public BaseViewModel CurrentViewModel { get; set; }

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
            Friends = new ObservableCollection<FriendModel>();
          
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
        /// Method that logout user from Application
        /// </summary>
        public void Logout()
        {
            //Remove all method that are periodically fired
            TimerExtensions.RemoveRepetingMethodsFromApplicationUser();

            //Set ApplicationUser to null
            IoC.Get<ApplicationViewModel>().ApplicationUser = null;

            //Clear friends list
            IoC.Get<ApplicationViewModel>().Friends = new ObservableCollection<FriendModel>();

            //Go to login page
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.LoginPage);
        }

        #endregion
    }
}
