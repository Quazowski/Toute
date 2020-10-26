using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Toute.Core;
using Toute.Core.Routes;
using Toute.Extensions;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// A ViewModel for LoginPage
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public properties

        /// <summary>
        /// Username of user
        /// </summary>
        public string Username { get; set; } = "wardls";

        /// <summary>
        /// Status of <see cref="LoginAsync(object)"/>
        /// </summary>
        public bool LoginIsRunning { get; set; }

        /// <summary>
        /// Status of <see cref="RefreshFriendsAsync(ObservableCollection{FriendModel})"/>
        /// method.
        /// </summary>
        public bool RefreshFriendIsRunning { get; set; }

        /// <summary>
        /// Indicate if <see cref="LoginAsGuestAsync"/> is running
        /// </summary>
        public bool LoginAsGuestIsRunning { get; set; }

        #endregion

        #region Commands
        /// <summary>
        /// Command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }
        /// <summary>
        /// Command to redirect user to register page
        /// </summary>
        public ICommand GoToRegisterCommand { get; set; }

        /// <summary>
        /// Command to redirect user to restart password page
        /// </summary>
        public ICommand GoToRestartPasswordCommand { get; set; }

        /// <summary>
        /// Command to login as guest to application
        /// </summary>
        public ICommand LoginAsGuestCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            _logger.Info("Start setting up LoginViewModel");

            //Create commands
            LoginCommand = new ParametrizedRelayCommand(async (parameter) => await LoginAsync(parameter));
            GoToRegisterCommand = new RelayCommand(GoToRegisterPage);
            GoToRestartPasswordCommand = new RelayCommand(GoToRestartPassword);
            LoginAsGuestCommand = new RelayCommand(async() => await LoginAsGuestAsync());

            _logger.Info("Done setting up LoginViewModel");
        }
        #endregion

        #region Private Command methods


        private async Task LoginAsGuestAsync()
        {
            await RunCommandAsync(() => LoginAsGuestIsRunning, async () =>
            {
                //Set popup, to give user basic information
                //PopupExtensions.NewPopupWithMessage("By registering in this application, " +
                //                "u can get access to messaging with friend. Guest version need no net for work.");

                //Get games from DB for not logged user
                var items = await SqliteDb.GetGames("");

                //For every file, that were added.... 
                foreach (var file in items)
                {
                    //Add to Item list a GameModel
                    ViewModelGame.Items.Add(new GameModel
                    {
                        Title = file.Title,
                        FileId = file.Id,
                        Paths = file.Paths,
                        BytesImage = file.Image
                    });
                }

                //Set items and move user to games page.
                ViewModelGame.FilteredItems = ViewModelGame.Items;

                ViewModelApplication.GoToPage(ApplicationPage.GamesPage);
            });

        }


        /// <summary>
        /// Method to redirect user to the RestartPasswordPage
        /// </summary>
        private void GoToRestartPassword()
        {
            ViewModelApplication.GoToPage(ApplicationPage.RestartPasswordPage);
        }

        /// <summary>
        /// Method redirect user to register page
        /// </summary>
        private void GoToRegisterPage()
        {
            ViewModelApplication.GoToPage(ApplicationPage.RegisterPage);
        }

        /// <summary>
        /// Method that handle going to register page
        /// </summary>
        /// <param name="parameter"></param>
        private async Task LoginAsync(object parameter)
        {
            await RunCommandAsync(() => LoginIsRunning, async () =>
            {
                _logger.Info("User is logging to application");

                //If there is any username is given...
                if (string.IsNullOrEmpty(Username))
                {
                    PopupExtensions.NewInfoPopup("Provide username first");
                }

                //Send request with credentials to server, to login
                var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<LoginResponse>(UserRoutes.Login,
                        new LoginRequest
                        {
                            Username = Username,
                            Password = (parameter as IHavePassword).SecureString.Unsecure()
                        });

                //If there is any context back
                if (context != null)
                {
                    //Make a list of friends
                    var Friends = new ObservableCollection<FriendModel>();

                    _logger.Debug("Saving user credentials to LocalDB");

                    await SqliteDb.SaveLoginCredentialsAsync(new LoginCredentialsDataModel
                    {
                        Id = context.Id,
                        Username = context.Username,
                        Email = context.Email,
                        Friends = new List<FriendDataModel>(),
                        Image = context.Image,
                        Token = context.Token.Token,
                        RefreshToken = context.Token.RefreshToken
                    });

                    _logger.Debug("Done saving user credentials to LocalDB");

                    _logger.Debug($"Setting up Application user of ID: {context.Id}");
                    //Set current application user to...
                    ViewModelApplication.ApplicationUser = new ApplicationUserModel
                    {
                        Id = context.Id,
                        Username = context.Username,
                        Email = context.Email,
                        Friends = Friends,
                        Image = context.Image,
                        Token = context.Token.Token
                    };

                    //foreach friend in Friends of user...
                    foreach (var friend in ViewModelApplication.ApplicationUser.Friends)
                    {
                        ViewModelApplication.Friends.Add(new FriendModel
                        {
                            FriendId = friend.FriendId,
                            Name = friend.Name,
                            Status = friend.Status,
                            BytesImage = friend.BytesImage
                        });

                    }
                    _logger.Debug($"Application user of is set ID: {context.Id}");

                    _logger.Debug("Trying to get user files from LocalDB");
                    //Loads all files that were added
                    var items = SqliteDb.GetGames(ViewModelApplication.ApplicationUser?.Id).Result;

                    _logger.Debug("Got user files from LocalDB");

                    //For every file, that were added.... 
                    foreach (var file in items)
                    {
                        //Add to Item list a GameModel
                        ViewModelGame.Items.Add(new GameModel
                        {
                            Title = file.Title,
                            FileId = file.Id,
                            Paths = file.Paths,
                            BytesImage = file.Image
                        });
                    }
                    ViewModelGame.FilteredItems = ViewModelGame.Items;

                    _logger.Info("Started to refreshing friend list");
                    //Start refreshing list of friends every x seconds.
                    ViewModelApplication.ApplicationUser.RefreshFriends = new Timer(async (e) =>
                    {
                        await ViewModelSideMenu.RefreshFriendsAsync(ViewModelApplication.Friends);
                    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

                    //Go to GamesPage page
                    ViewModelApplication.GoToPage(ApplicationPage.GamesPage);

                    _logger.Info("User is successfully logged to application");
                }

            });
        }


        #endregion
    }
}
