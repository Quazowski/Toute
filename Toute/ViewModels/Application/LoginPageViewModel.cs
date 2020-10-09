using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Toute.Core;
using Toute.Core.Routes;
using Toute.Extensions;

namespace Toute
{
    /// <summary>
    /// A ViewModel for LoginPage
    /// </summary>
    public class LoginPageViewModel : BaseViewModel
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
        public LoginPageViewModel()
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
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.RegisterPage);
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

                    //Set current application user to...
                    IoC.Get<ApplicationViewModel>().ApplicationUser = new ApplicationUserModel
                    {
                        Id = context.Id,
                        Username = context.Username,
                        Email = context.Email,
                        Friends = Friends,
                        Image = context.Image,
                        JWTToken = context.JWTToken
                    };

                    //foreach friend in Friends of user...
                    foreach (var friend in IoC.Get<ApplicationViewModel>().ApplicationUser.Friends)
                    {
                        IoC.Get<ApplicationViewModel>().Friends.Add(new FriendModel
                        {
                            FriendId = friend.FriendId,
                            Name = friend.Name,
                            Status = friend.Status,
                            BytesImage = friend.BytesImage
                        });

                    }

                    //Start refreshing list of friends every x seconds.
                    IoC.Get<ApplicationViewModel>().ApplicationUser.RefreshFriends = new Timer(async (e) =>
                    {
                        await RefreshFriendsAsync(IoC.Get<ApplicationViewModel>().Friends);
                    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

                    //Go to GamesPage page
                    IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GamesPage);
                }

            });
        }

        /// <summary>
        /// Method that is fired every x second, by timer.
        /// It used to refresh friend list
        /// </summary>
        /// <param name="friends">Actual friend IDs of user</param>
        private async Task RefreshFriendsAsync(ObservableCollection<FriendModel> friends)
        {

            //Make a request
            var listOfFriendsId = new RefreshFriendsRequest();

            //add all friend IDs to the list
            foreach (var friend in friends)
            {
                listOfFriendsId.FriendsId.Add(friend.FriendId);
            }

            //Make a request to the server with friend IDs
            var TContext = await HttpExtensions.HandleHttpRequestOfTResponseAsync<UpdateFriendsResponse>(FriendRoutes.GetFriends, listOfFriendsId);


            //If there are any friends to add...
            if (TContext?.FriendsToAdd.Count > 0)
            {
                //for every friends...
                foreach (var friend in TContext.FriendsToAdd)
                {
                    //add him to Friends of ApplicatioUser
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.Add(new FriendModel
                    {
                        FriendId = friend.FriendId,
                        Name = friend.Name,
                        Status = friend.Status,
                    });

                    //Add to friends of Application
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        IoC.Get<ApplicationViewModel>().Friends.Add(new FriendModel
                        {
                            FriendId = friend.FriendId,
                            BytesImage = friend.BytesImage,
                            Name = friend.Name,
                            Status = friend.Status,
                        });
                    });
                }
            }

            //If there are any friends to remove...
            if (TContext?.FriendsToRemove.Count > 0)
            {
                //For every friend to remove...
                foreach (var friend in TContext.FriendsToRemove)
                {
                    //If user are on page with given friend...
                    if (IoC.Get<ApplicationViewModel>().CurrentViewModel is FriendModel friendModel)
                    {
                        if (friendModel.FriendId == IoC.Get<ApplicationViewModel>().CurrentFriendId)
                        {
                            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GamesPage);
                        }
                    }

                    //remove form ApplicationUser friends
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.Remove(IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == friend));

                    //Remove from friends of Application
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        IoC.Get<ApplicationViewModel>().Friends.Remove(IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == friend));
                    });
                }
            }
        }
        #endregion
    }
}
