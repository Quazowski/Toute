using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// A view model for SideMenu where are
    /// handled friend managing
    /// </summary>
    public class SideMenuViewModel : BaseViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public properties

        /// <summary>
        /// The name of friend, to which will be
        /// friend request sent
        /// </summary>
        public string NameOfFriendToAdd { get; set; }

        /// <summary>
        /// The name of friend that is currently managed.
        /// Sets when user right click on user control
        /// with friend
        /// </summary>
        public string CurrentIdOfManagedFriend { get; set; }

        /// <summary>
        /// Value that shows is friend block list
        /// open or close. If true, it is open.
        /// </summary>
        public bool BlockListOpen { get; set; }

        /// <summary>
        /// Value that shows is friend request list
        /// open or close. If true, it is open.
        /// </summary>
        public bool RequestListOpen { get; set; }

        /// <summary>
        /// Value that shows is friend settings menu 
        /// open or close. If true, it is open.
        /// </summary>
        public bool IsFriendSettingsOpen { get; set; }

        /// <summary>
        /// If <see cref="SendFriendRequestAsync"/> is running
        /// set this to true
        /// </summary>
        public bool SendFriendRequestIsRunning { get; set;}

        /// <summary>
        /// If <see cref="AcceptFriendRequestAsync(object)"/> is running
        /// set this to true
        /// </summary>
        public bool AcceptFriendIsRunning { get; set;}

        /// <summary>
        /// If <see cref="DeclineFriendRequestAsync(object)"/> is running
        /// set this to true
        /// </summary>
        public bool DeclineFriendIsRunning { get; set;}

        /// <summary>
        /// If <see cref="DeleteFriendAsync"/> is running
        /// set this to true
        /// </summary>
        public bool DeleteFriendIsRunning { get; set;}

        /// <summary>
        /// If <see cref="UnblockFriend(object)"/> is running
        /// set this to true
        /// </summary>
        public bool UnblockFriendIsRunning { get; set;}

        /// <summary>
        /// If <see cref="BlockFriend"/> is running
        /// set this to true
        /// </summary>
        public bool BlocFriendIsRunning { get; set;}

        /// <summary>
        /// If <see cref="GamesIsRunning"/> is running
        /// set this to true
        /// </summary>
        public bool GamesIsRunning { get; set;}

        /// <summary>
        /// If <see cref="SendFriendRequestAsync"/> is running
        /// set this to true
        /// </summary>
        public bool LoadMoreMessagesIsRunning { get; set;}

        /// <summary>
        /// Set the last page that is loaded
        /// with the friend we are currently chat
        /// </summary>
        public int LastPageLoaded { get; set; } = 1;

        /// <summary>
        /// Indicate if there is any more messages to load
        /// used in <see cref="ScrollToBottomOnValueChangedAttachedProperty"/>
        /// to prevent not needed loads
        /// </summary>
        public bool IsMoreMessages { get; set; } = true;

        #endregion

        #region Public commands

        /// <summary>
        /// Command that handle sending friend request
        /// </summary>
        public ICommand SendFriendRequestCommand { get; set; }

        /// <summaryhandle
        /// Command that handling accepting a friend request
        /// </summary>
        public ICommand AcceptFriendRequestCommand { get; set; }

        /// <summary>
        /// Command that handle declining friend request
        /// </summary>
        public ICommand DeclineFriendRequestCommand { get; set; }

        /// <summary>
        /// Command that handle deleting user from friend list
        /// </summary>
        public ICommand DeleteFriendCommand { get; set; }

        /// <summary>
        /// Command that handle block user
        /// </summary>
        public ICommand BlockFriendCommand { get; set; }

        /// <summary>
        /// Command that unblock friend
        /// </summary>
        public ICommand UnblockFriendCommand { get; set; }

        /// <summary>
        /// Command that open friend settings menu
        /// </summary>
        public ICommand OpenFriendSettingsCommand { get; set; }

        /// <summary>
        /// Command that changes page, or page and viewModel and
        /// goes to chat page of given viewModel
        /// in this case to chat with a friend
        /// </summary>
        public ICommand GoToUserCommand { get; set; }

        /// <summary>
        /// Command that will close or open block list
        /// </summary>
        public ICommand BlockListCommand { get; set; }

        /// <summary>
        /// Command that will close or open friend request list
        /// </summary>
        public ICommand RequestListCommand { get; set; }

        /// <summary>
        /// Command that handle going to GamesPage
        /// </summary>
        public ICommand GamesCommand { get; set; }

        /// <summary>
        /// Command that handle going to OptionsPage
        /// </summary>
        public ICommand SettingsCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SideMenuViewModel()
        {
            _logger.Info("Start setting up SideMenuViewModel");

            //Create commands
            SendFriendRequestCommand = new RelayCommand(async() => await SendFriendRequestAsync());
            AcceptFriendRequestCommand = new ParametrizedRelayCommand(async (FriendId) => await AcceptFriendRequestAsync(FriendId));
            DeclineFriendRequestCommand = new ParametrizedRelayCommand(async (FriendId) => await DeclineFriendRequestAsync(FriendId));
            DeleteFriendCommand = new RelayCommand(async() => await DeleteFriendAsync());
            BlockFriendCommand = new RelayCommand(async() => await BlockFriend());
            UnblockFriendCommand = new ParametrizedRelayCommand(async (FriendId) => await UnblockFriend(FriendId));
            GoToUserCommand = new ParametrizedRelayCommand((FriendId) =>  GoToUser(FriendId));
            OpenFriendSettingsCommand = new ParametrizedRelayCommand((FriendId) => OpenFriendSettings(FriendId));
            RequestListCommand = new RelayCommand(RequestListChangeStatus);
            BlockListCommand = new RelayCommand(BlockListStatusChangeStatus);
            GamesCommand = new RelayCommand(GoToGamesPage);
            SettingsCommand = new RelayCommand(GoToSettingsPage);

            _logger.Info("Done setting up SideMenuViewModel");
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Method that will process sending friend request
        /// to the API
        /// </summary>
        private async Task SendFriendRequestAsync()
        {
            await RunCommandAsync(() => SendFriendRequestIsRunning, async () => 
            {
                //If user is not logged, or TextBox is empty...
                if (string.IsNullOrEmpty(NameOfFriendToAdd))
                    return;
                _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is trying to send friend request to {NameOfFriendToAdd}");
                //Sets values of friend request
                var userToAdd = new SendFriendRequest
                {
                    FriendUsername = NameOfFriendToAdd
                };
                
                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.SendFriendRequest, userToAdd);

                //If response is successful
                if (result)
                {
                    NameOfFriendToAdd = "";
                    PopupExtensions.NewInfoPopup("Friend request sent");
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} sent friend request to {NameOfFriendToAdd}");
                }
                else
                {
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} failed to send friend request to {NameOfFriendToAdd}");
                }
                    
            });
        }

        /// <summary>
        /// Method that handle accept friend request
        /// </summary>
        /// <param name="FriendId">ID of friend to accept</param>
        private async Task AcceptFriendRequestAsync(object FriendId)
        {
            await RunCommandAsync(() => AcceptFriendIsRunning, async() => 
            {
                _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is trying to accept friend request with ID: {FriendId}");
                //Make a request Model
                var userToAdd = new AddFriendRequest
                {
                    FriendId = FriendId.ToString()
                };

                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.AddFriend, userToAdd);

                //If request succeeded
                if (result)
                {
                    //Set value of StatusOfFiendship in ApplicationUser friends, to accepted with given friend
                    ViewModelApplication.ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Status = StatusOfFriendship.Accepted;
                    //Set value of StatusOfFiendship in Friends list, to accepted with given friend
                    ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Status = StatusOfFriendship.Accepted;
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is accepted friend request with ID: {FriendId}");
                }
                else
                {
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is failed  to accept friend request of friend with ID: {FriendId}");
                }
            });

        }

        /// <summary>
        /// Method that handle declining a friend request
        /// </summary>
        /// <param name="FriendId">ID of friend to decline friend request</param>
        private async Task DeclineFriendRequestAsync(object FriendId)
        {
            await RunCommandAsync(() => DeclineFriendIsRunning, async () =>
            {
                _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is trying to decline friend request with ID: {FriendId}");
                //Make a request Model
                var userToReject = new AddFriendRequest
                {
                    FriendId = FriendId.ToString()
                };

                //Gets a result
                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.RejectFriendRequest, userToReject);

                //If request succeeded
                if (result)
                {
                    //Remove friend from friend list
                    ViewModelApplication.Friends.Remove(ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()));
                    //Remove friend from friends in ApllicationUser
                    ViewModelApplication.ApplicationUser.Friends.Remove(ViewModelApplication.ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()));
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is declined friend request of friend ID: {FriendId}");
                }
                else
                {
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is failed to decline friend request of friend with ID: {FriendId}");
                }
            });
        }

        /// <summary>
        /// Method that handle deleting friend
        /// </summary>
        /// <param name="FriendId">ID of friend to delete</param>
        private async Task DeleteFriendAsync()
        {
            await RunCommandAsync(() => DeleteFriendIsRunning, async () =>
            {
                _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is trying to delete friend with ID: {CurrentIdOfManagedFriend}");
                //Make a request Model
                var userToDelete = new AddFriendRequest
                {
                    FriendId = CurrentIdOfManagedFriend
                };

                //Gets a result
                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.DeleteFriend, userToDelete);

                //If request succeeded
                if (result)
                {
                    //If user are on page with given friend...
                    if (ViewModelApplication.CurrentViewModel is FriendModel friend)
                    {
                        if (friend.FriendId == CurrentIdOfManagedFriend)
                            //Go to GamesPage
                            ViewModelApplication.GoToPage(ApplicationPage.GamesPage);

                    }

                    //Remove friend from friend list
                    ViewModelApplication.Friends.Remove(ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == CurrentIdOfManagedFriend));
                    //Remove friend form ApplicationUser friends
                    ViewModelApplication.ApplicationUser.Friends.Remove(ViewModelApplication.ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == CurrentIdOfManagedFriend));
                    //Clear CurrentIdOfManagedFriend 
                    CurrentIdOfManagedFriend = "";
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} deleted friend with friend ID: {CurrentIdOfManagedFriend}");
                }
                else
                {
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is failed to delete friend request of friend with ID: {CurrentIdOfManagedFriend}");
                }
            });
        }

        /// <summary>
        /// Method that handle blocking friend
        /// </summary>
        private async Task BlockFriend()
        {
            await RunCommandAsync(() => BlocFriendIsRunning, async () =>
            {
                _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is trying to block friend with ID: {CurrentIdOfManagedFriend}");

                //Close friend settings popup
                IsFriendSettingsOpen = false;

                //Make a request Model
                var userToBlock = new AddFriendRequest
                {
                    FriendId = CurrentIdOfManagedFriend
                };

                //Gets a result
                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.BlockFriend, userToBlock);

                //If request succeeded
                if (result)
                {
                    //If user are on page with given friend...
                    if (ViewModelApplication.CurrentViewModel is FriendModel friend)
                    {
                        //If we are on the page with the friend...
                        if (friend.FriendId == ViewModelApplication.CurrentFriendId)
                            //Go to GamesPage
                            ViewModelApplication.GoToPage(ApplicationPage.GamesPage);

                    }

                    //Set friend in friend list to blocked
                    ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == CurrentIdOfManagedFriend).Status = StatusOfFriendship.Blocked;
                    //Set friend in application user to blocked
                    ViewModelApplication.ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == CurrentIdOfManagedFriend).Status = StatusOfFriendship.Blocked;
                    //Set we are not managing this friend
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} blocked friend with friend ID: {CurrentIdOfManagedFriend}");
                    CurrentIdOfManagedFriend = "";

                }
                else
                {
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is failed to block friend request of friend with ID: {CurrentIdOfManagedFriend}");
                }
            });
        }

        /// <summary>
        /// Method that handle unlocking friend
        /// </summary>
        /// <param name="FriendId"></param>
        private async Task UnblockFriend(object FriendId)
        {
            await RunCommandAsync(() => UnblockFriendIsRunning, async () =>
            {
                _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is trying to unblock friend with ID: {FriendId}");
                //Make a request Model
                var friendToUnblock = new UnblockFriendRequest
                {
                    FriendId = FriendId.ToString()
                };

                //Gets a result
                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.UnblockFriend, friendToUnblock);

                //If request succeeded
                if (result)
                {
                    //Change StatusOfFriendship with friend to accepted in ApplicationUser 
                    ViewModelApplication.ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == friendToUnblock.FriendId).Status = StatusOfFriendship.Accepted;

                    //Change StatusOfFriendship in friends to accepted
                    ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == friendToUnblock.FriendId).Status = StatusOfFriendship.Accepted;
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} unblocked friend with friend ID: {FriendId}");

                }
                else
                {
                    _logger.Debug($"User of ID {ViewModelApplication.ApplicationUser.Id} is failed to unblock friend request of friend with ID: {FriendId}");
                }
            });
        }

        /// <summary>
        /// Method that handle going to chat 
        /// with given friend
        /// </summary>
        /// <param name="FriendId">ID of friend</param>
        private void GoToUser(object FriendId)
        {
            if (ViewModelApplication.CurrentFriendId == FriendId.ToString())
                return;

            _logger.Debug($"User is trying to go chat with friend of ID: {FriendId}");

            //_logger.Info("Deleted refreshing messages");
            ////Remove previous refreshing
            //TimerExtensions.RemoveRepetingMessagesFromApplicationUser();

            //Finds user of given if
            var chatUser = ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString());

            if (chatUser == null)
            {
                _logger.Debug($"User is trying to go chat with friend of ID: {FriendId}, but user does not exist.");
                return;
            }
                

            //Create new ObservableCollection with messages
            chatUser.Messages = new ObservableCollection<MessageModel>();

            //If StatusOfFriendship is not blocked or pending...
            if (!(chatUser.Status == StatusOfFriendship.Blocked || chatUser.Status == StatusOfFriendship.Pending))
            {
                //If any user is selected
                if (!(string.IsNullOrEmpty(ViewModelApplication.CurrentFriendId)))
                {
                    //Find if he exists
                    if(ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == ViewModelApplication.CurrentFriendId) != null)
                    {
                        //De-select him
                        ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == ViewModelApplication.CurrentFriendId).IsSelected = false;
                    }
                }

                //Select new user
                chatUser.IsSelected = true;

                //Set actual friend
                ViewModelApplication.CurrentFriendId = FriendId.ToString();

                //Set chatUser messages to new ObservableCollection and order them by date
                ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Messages = new ObservableCollection<MessageModel>(chatUser.Messages.OrderBy(x => x.DateOfSent));
                //}
                IsMoreMessages = true;
                LastPageLoaded = 1;
                //Go to chat page with specific user of given id
                ViewModelApplication.GoToPage(ApplicationPage.ContactPage, chatUser);
            }
            else
            {
                _logger.Debug($"Failed to go to chat with friend of ID: {FriendId}. User is blocked, or status of friendship is pending");
            }

        }

        /// <summary>
        /// Method that handle open friend settings
        /// </summary>
        /// <param name="FriendId">Friend ID that is clicked</param>
        private void OpenFriendSettings(object FriendId)
        {
            _logger.Debug($"User is trying to open settings of friend ID: {FriendId}");

            //Set CurrentIdOfManagedFriend
            CurrentIdOfManagedFriend = FriendId.ToString();

            //Open settings only for accepted friend
            if (ViewModelApplication.ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Status == StatusOfFriendship.Accepted)
            {
                _logger.Debug($"User opened settings of friend ID: {FriendId}");
                IsFriendSettingsOpen = true;
            }
        }


        /// <summary>
        /// Method that toggle Visibility friend requests
        /// </summary>
        private void RequestListChangeStatus()
        {
            //Toggle
            RequestListOpen ^= true;
        }

        /// <summary>
        /// Method that toggle Visibility of blocked friends
        /// </summary>
        private void BlockListStatusChangeStatus()
        {
            //toggle
            BlockListOpen ^= true;
        }

        /// <summary>
        /// Method that handle going to GamesPage
        /// </summary>
        private void GoToGamesPage()
        {
            //Go to GamesPage
            ViewModelApplication.GoToPage(ApplicationPage.GamesPage);
        }

        /// <summary>
        /// Method that handle going to OptionsPage
        /// </summary>
        private void GoToSettingsPage()
        {
            //Go to SettingsPage
            ViewModelApplication.GoToPage(ApplicationPage.SettingsPage);
        }

        /// <summary>
        /// Method that load more messages from database with the friend
        /// </summary>
        /// <returns>More messages</returns>
        public async Task LoadMoreMessagesAsync()
        {
            await RunCommandAsync(() => LoadMoreMessagesIsRunning, async () =>
            {
                //Gets user id
                var FriendId = ViewModelApplication.CurrentFriendId;

                _logger.Debug($"Attempt to refresh messages with {FriendId}");

                //Make a request to API
                var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<List<MessageDataModel>>(MessageRoutes.GetMessages + $"/{LastPageLoaded}", new GetMessagesRequest
                {
                    FriendId = FriendId
                });

                //If there is successful response
                if (context?.Count > 0)
                {
                    _logger.Debug($"Found {context.Count} new messages. Now adding them into the message list");
                    //If number is less that 20 (twenty, because its default number of
                    //returning items from pagination)
                    if (context.Count < 20)
                        //Set there is no more messages
                        IsMoreMessages = false;

                    //Set last page loaded
                    LastPageLoaded++;

                    //Get friend
                    var friendUser = ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == FriendId);

                    //For every message...
                    foreach (var message in context)
                    {
                        friendUser.Messages.Insert(0, new MessageModel
                        {
                            Message = message.Message,
                            SentByMe = message.SentByMe,
                            DateOfSent = TimeZoneInfo.ConvertTimeFromUtc(message.DateOfSent, TimeZoneInfo.Local),
                            FriendsImage = ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == FriendId).Image
                        });
                    }
                }
                else
                {
                    _logger.Debug($"No new messages with friend of ID: {FriendId} were found");
                }
            });
        }

        /// <summary>
        /// Method that is fired every x second, by timer.
        /// It used to refresh friend list
        /// </summary>
        /// <param name="friends">Actual friend IDs of user</param>
        public async Task RefreshFriendsAsync(ObservableCollection<FriendModel> friends)
        {
            _logger.Debug("Attempt to refresh friend list");
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
                _logger.Debug($"Found {TContext.FriendsToAdd.Count} new friends to add");
                //for every friends...
                foreach (var friend in TContext.FriendsToAdd)
                {
                    //add him to Friends of ApplicatioUser
                    ViewModelApplication.ApplicationUser.Friends.Add(new FriendModel
                    {
                        FriendId = friend.FriendId,
                        Name = friend.Name,
                        Status = friend.Status,
                    });

                    //Add to friends of Application
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        ViewModelApplication.Friends.Add(new FriendModel
                        {
                            FriendId = friend.FriendId,
                            BytesImage = friend.BytesImage,
                            Name = friend.Name,
                            Status = friend.Status,
                        });
                    });
                    _logger.Debug($"Added Friend with ID: {friend.FriendId} to friend list");
                }
            }

            //If there are any friends to remove...
            if (TContext?.FriendsToRemove.Count > 0)
            {
                _logger.Debug($"Found {TContext.FriendsToRemove.Count} new friends to remove");
                //For every friend to remove...
                foreach (var friend in TContext.FriendsToRemove)
                {
                    //If user are on page with given friend...
                    if (ViewModelApplication.CurrentViewModel is FriendModel friendModel)
                    {
                        if (friendModel.FriendId == ViewModelApplication.CurrentFriendId)
                        {
                            ViewModelApplication.GoToPage(ApplicationPage.GamesPage);
                        }
                    }

                    //remove form ApplicationUser friends
                    ViewModelApplication.ApplicationUser.Friends.Remove(ViewModelApplication.ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == friend));

                    //Remove from friends of Application
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        ViewModelApplication.Friends.Remove(ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == friend));
                    });

                    _logger.Debug($"Remove Friend with ID: {ViewModelApplication.CurrentFriendId} from friend list");
                }
            }
            _logger.Debug("Done refreshing friend list");
        }
        #endregion

        #region Public helpers



        #endregion
    }
}
