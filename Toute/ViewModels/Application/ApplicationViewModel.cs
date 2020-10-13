using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ApplicationViewModel> _logger;

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
        public ApplicationViewModel(ILogger<ApplicationViewModel> _logger)
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

            InformationsAndErrors = new ObservableCollection<InfoControlViewModel>();

            DeleteInfoAndErrors = new Timer((e) =>
            {
                RefreshList(InformationsAndErrors);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            //Create new List of friends
            Friends = new ObservableCollection<FriendModel>();
            logger = _logger;
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
        public async Task Logout()
        {
            //Remove all method that are periodically fired
            TimerExtensions.RemoveRepetingMethodsFromApplicationUser();

            //Set ApplicationUser to null
            ViewModelApplication.ApplicationUser = null;

            //Clear friends list
            ViewModelApplication.Friends = new ObservableCollection<FriendModel>();

            await SqliteDb.RemoveLoginCredentialsAsync();

            //Go to login page
            ViewModelApplication.GoToPage(ApplicationPage.LoginPage);
        }

        /// <summary>
        /// Method that delete not needed informations
        /// or errors from list
        /// </summary>
        /// <param name="informationsAndErrors">List to remove unneeded items</param>
        private void RefreshList(ObservableCollection<InfoControlViewModel> informationsAndErrors)
        {
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
        }
        #endregion
    }
}
