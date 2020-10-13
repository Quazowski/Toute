using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        #region Public properties

        /// <summary>
        /// Username of user
        /// </summary>
        public string Username { get; set; } = "Testuser";

        /// <summary>
        /// Status of <see cref="LoginAsync(object)"/>
        /// </summary>
        public bool LoginIsRunning { get; set; }

        /// <summary>
        /// Status of <see cref="RefreshFriendsAsync(ObservableCollection{FriendModel})"/>
        /// method.
        /// </summary>
        public bool RefreshFriendIsRunning { get; set; }

        #endregion

        #region Commands
        /// <summary>
        /// Command that handle login
        /// </summary>
        public ICommand LoginCommand { get; set; }
        /// <summary>
        /// Command that handle going to register page
        /// </summary>
        public ICommand GoToRegister { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            //Command that handle login
            LoginCommand = new ParametrizedRelayCommand(async (parameter) => await LoginAsync(parameter));

            //Command that handle going to register page
            GoToRegister = new RelayCommand(GoToRegisterPage);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Method that handle login
        /// </summary>
        private void GoToRegisterPage()
        {
            //Go to register page
            ViewModelApplication.GoToPage(ApplicationPage.RegisterPage);
        }

        /// <summary>
        /// Method that handle going to register page
        /// </summary>
        /// <param name="parameter"></param>
        public async Task LoginAsync(object parameter)
        {
            await RunCommandAsync(() => LoginIsRunning, async () =>
            {
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
                            Password = "Mypassword1!" ?? (parameter as IHavePassword).SecureString.Unsecure()
                        });

                //If there is any context back
                if(context != null)
                {
                    //Make a list of friends
                    var Friends = new ObservableCollection<FriendModel>();

                    foreach (var friend in context.Friends)
                    {
                        var messages = new ObservableCollection<MessageModel>();
                        foreach (var message in friend.Messages)
                        {
                            messages.Add(new MessageModel
                            {
                                Message = message.Message,
                                DateOfSent = message.DateOfSent,
                                SentByMe = message.SentByMe
                            });
                        }
                        Friends.Add(new FriendModel
                        {
                            FriendId = friend.FriendId,
                            BytesImage = friend.BytesImage,
                            Name = friend.Name,
                            Status = friend.Status,
                            Messages = messages
                        });
                    }

                    ICollection<FriendDataModel> DBFriends = new List<FriendDataModel>();
                    foreach (var friendResponse in context.Friends)
                    {
                        DBFriends.Add(new FriendDataModel
                        {
                            Id =  Guid.NewGuid().ToString(),
                            FriendId = friendResponse.FriendId,
                            Messages = new List<MessageDataModel>(),
                            Status = friendResponse.Status
                        });
                    }

                    await SqliteDb.SaveLoginCredentialsAsync(new LoginCredentialsDataModel
                    {
                        Id = context.Id,
                        Username = context.Username,
                        Email = context.Email,
                        Friends = DBFriends,
                        Image = context.Image,
                        JWTToken = context.JWTToken
                    });

                    //Set current application user to...
                    ViewModelApplication.ApplicationUser = new ApplicationUserModel
                    {
                        Id = context.Id,
                        Username = context.Username,
                        Email = context.Email,
                        Friends = Friends,
                        Image = context.Image,
                        JWTToken = context.JWTToken
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

                    //Start refreshing list of friends every x seconds.
                    ViewModelApplication.ApplicationUser.RefreshFriends = new Timer(async (e) =>
                    {
                        await ViewModelSideMenu.RefreshFriendsAsync(ViewModelApplication.Friends);
                    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

                    //Go to GamesPage page
                    ViewModelApplication.GoToPage(ApplicationPage.GamesPage);
                }

            });
        }


        #endregion
    }
}
