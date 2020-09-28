using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        /// If user is logged, store Friends in a list
        /// </summary>
        public ObservableCollection<ChatUser> Friends { get; set; }

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
        /// Command that changes page, or page and viewModel and
        /// goes to chat page of given viewModel
        /// </summary>
        public ICommand GoToUserCommand { get; set; }

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

            //If user logged
            if(true == true)
            {
                //Set users
                //TODO: Take a friends form DB
                Friends = new ObservableCollection<ChatUser>
                {
                    new ChatUser { Name = "test", Id = "aa" },
                    new ChatUser { Name = "test1", Id = "bb" },
                    new ChatUser { Name = "test2", Id = "cc" }
                };
            }

            //Command that handle going to Chat page of specific user
            GoToUserCommand = new ParametrizedRelayCommand((id) => GoToUser(id));

            //Command that handle going to GamesPage
            GamesCommand = new RelayCommand(GoToGamesPage);

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
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {

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

        /// <summary>
        /// Methods that handle going to a chat page with
        /// specific user
        /// </summary>
        /// <param name="id">Id of the user</param>
        private void GoToUser(object id)
        {
            //Finds user of given if
            var chatUser = Friends.FirstOrDefault(x => x.Id == id.ToString());

            //If user exists...
            if(chatUser != null)
            {
                //For each friend in list...
                foreach (var user in Friends)
                {
                    //Make a user unselected
                    user.IsSelected = false;
                }

                //Select new user
                chatUser.IsSelected = true;

                //Go to chat page with specific user of given id
                IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.ContactPage, chatUser);
            }
        }

        #endregion
    }
}
