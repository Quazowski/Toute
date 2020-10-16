using NLog;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Toute.Core;
using static Toute.DI;

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
        private BasePage _currentPage;

        /// <summary>
        /// Logger to <see cref="ApplicationViewModel"/>
        /// </summary>
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
        public bool LogoutIsRunning { get; set; }

        public ObservableCollection<InfoControlViewModel> InformationsAndErrors { get; set; }

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
        /// Timer, that delete list of errors and infos
        /// every x seconds
        /// </summary>
        public Timer DeleteInfoAndErrors { get; set; }

        /// <summary>
        /// Current Application Page of enum value
        /// </summary>
        public ApplicationPage CurrentApplicationPage { get; set; }

        /// <summary>
        /// Current application page in frame
        /// </summary>
        public BasePage CurrentPage
        {
            get => _currentPage;
            set
            {
                if (CurrentPage == value)
                    return;

                _currentPage = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for <see cref="ApplicationViewModel"/>
        /// </summary>
        public ApplicationViewModel()
        {
            _logger.Info("Starts setting up ApplicationViewModel for a Application");

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
                else
                {
                    //Show Side Menu
                    SideMenuHidden = false;
                }

                //Set current page to DesignPage value
                CurrentPage = ApplicationPageHelper.GoToBasePage(DesignPage);                
            }

            //Create new list for infos and errors
            InformationsAndErrors = new ObservableCollection<InfoControlViewModel>();

            _logger.Info("Refreshing DeleteInfoAndErrors has started");
            //Makes timer to clear this list over time
            DeleteInfoAndErrors = new Timer((e) =>
            {
                RefreshList(InformationsAndErrors);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            //Create new List of friends
            Friends = new ObservableCollection<FriendModel>();

            _logger.Info("Done setting up ApplicationViewModel for a Application");
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
            _logger.Debug($"User attempt to go to {page} page");

            //Remove refreshing messages, only when user was on chat page 
            if (CurrentApplicationPage == ApplicationPage.ContactPage)
            {
                _logger.Info("Timer that refresh messages are removed");
                TimerExtensions.RemoveRepetingMessagesFromApplicationUser();
            }

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

            Application.Current.Dispatcher.Invoke(delegate
            {
                //Sets frame to given page
                CurrentPage = ApplicationPageHelper.GoToBasePage(CurrentApplicationPage, viewModel);
            });

            _logger.Debug($"User successfully went to {page} page");
        }

        /// <summary>
        /// Method that logout user from Application
        /// </summary>
        public async Task LogoutAsync()
        {
            await RunCommandAsync(() => LogoutIsRunning, async () =>
            {
                _logger.Info($"User is logging out");

                _logger.Debug("Try to stop refresh messages and friend list");
                //Remove all method that are periodically fired
                TimerExtensions.RemoveRepetingMethodsFromApplicationUser();
                _logger.Info("Stopped refreshing messages and friend list");


                //Set ApplicationUser to null
                ViewModelApplication.ApplicationUser = null;
                ViewModelSideMenu.NameOfFriendToAdd = "";
                //Clear friends list
                ViewModelApplication.Friends = new ObservableCollection<FriendModel>();

                ViewModelGame.Items = new ObservableCollection<GameModel>();

                _logger.Debug("Removing user credentials from LocalDB");
                await SqliteDb.RemoveLoginCredentialsAsync();
                _logger.Debug("Removed user credentials from LocalDB");

                //Go to login page
                ViewModelApplication.GoToPage(ApplicationPage.LoginPage);

                _logger.Info($"User logged out");
            });
        }

        /// <summary>
        /// Method that delete not needed informations
        /// or errors from list
        /// </summary>
        /// <param name="informationsAndErrors">List to remove unneeded items</param>
        private void RefreshList(ObservableCollection<InfoControlViewModel> informationsAndErrors)
        {
            _logger.Debug("RefreshList is called, start to clear not needed infos and errors...");
            //If there is any item...
            if (informationsAndErrors.Count > 0)
            {
                //Use Dispatcher to get update ObservableCollection
                Application.Current.Dispatcher.Invoke(delegate
                {
                    //For each info/error
                    foreach (var infoOrError in informationsAndErrors.ToList())
                    {
                        //If is mark as to delete...
                        if (infoOrError.ToDelete)
                        {
                            //Remove from list
                            informationsAndErrors.Remove(infoOrError);
                        }
                    }
                });
            }
            _logger.Debug("Done clearing  not needed infos and errors");
        }
        #endregion
    }
}
