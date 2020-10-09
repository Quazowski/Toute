﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using Toute.Core;
using Toute.Core.DataModels;
using Toute.Core.Routes;
using Toute.Extensions;

namespace Toute
{
    /// <summary>
    /// A view model for SideMenu where are
    /// handled friend managing
    /// </summary>
    public class SideMenuViewModel : BaseViewModel
    {
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

        public bool SendFriendRequestIsRunning { get; set;}
        public bool AcceptFriendIsRunning { get; set;}
        public bool DeclineFriendIsRunning { get; set;}
        public bool DeleteFriendIsRunning { get; set;}
        public bool UnblockFriendIsRunning { get; set;}
        public bool GoToUserIsRunning { get; set;}
        public bool OpenFriendSettingsIsRunning { get; set;}
        public bool RequestListIsRunning { get; set;}
        public bool BlocFriendIsRunning { get; set;}
        public bool GamesIsRunning { get; set;}
        public bool SettingsIsRunning { get; set;}

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
            //Create commands
            SendFriendRequestCommand = new RelayCommand(async() => await SendFriendRequestAsync());
            AcceptFriendRequestCommand = new ParametrizedRelayCommand(async (FriendId) => await AcceptFriendRequestAsync(FriendId));
            DeclineFriendRequestCommand = new ParametrizedRelayCommand(async (FriendId) => await DeclineFriendRequestAsync(FriendId));
            DeleteFriendCommand = new RelayCommand(async() => await DeleteFriendAsync());
            BlockFriendCommand = new RelayCommand(async() => await BlockFriend());
            UnblockFriendCommand = new ParametrizedRelayCommand(async (FriendId) => await UnblockFriend(FriendId));
            GoToUserCommand = new ParametrizedRelayCommand(async (FriendId) => await GoToUser(FriendId));
            OpenFriendSettingsCommand = new ParametrizedRelayCommand((FriendId) => OpenFriendSettings(FriendId));
            RequestListCommand = new RelayCommand(RequestListChangeStatus);
            BlockListCommand = new RelayCommand(BlockListStatusChangeStatus);
            GamesCommand = new RelayCommand(GoToGamesPage);
            SettingsCommand = new RelayCommand(GoToSettingsPage);

        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Method that will process sending friend request
        /// to the web server
        /// </summary>
        private async Task SendFriendRequestAsync()
        {
            await RunCommandAsync(() => SendFriendRequestIsRunning, async () => 
            {
                //If user is not logged, or TextBox is empty...
                if (string.IsNullOrEmpty(NameOfFriendToAdd))
                    return;

                //Sets values of friend request
                var userToAdd = new SendFriendRequest
                {
                    FriendUsername = NameOfFriendToAdd
                };

                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.SendFriendRequest, userToAdd);
                if (result)
                    PopupExtensions.NewInfoPopup("Friend request sent");
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
                //Make a request Model
                var userToAdd = new AddFriendRequest
                {
                    FriendId = FriendId.ToString()
                };

                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.AddFriend, userToAdd);

                if(result)
                {
                    //Set value of StatusOfFiendship in ApplicationUser friends, to accepted with given friend
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Status = StatusOfFriendship.Accepted;
                    //Set value of StatusOfFiendship in Friends list, to accepted with given friend
                    IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Status = StatusOfFriendship.Accepted;
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
                //Make a request Model
                var userToReject = new AddFriendRequest
                {
                    FriendId = FriendId.ToString()
                };

                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.RejectFriendRequest, userToReject);

                if (result)
                {
                    //Remove friend from friend list
                    IoC.Get<ApplicationViewModel>().Friends.Remove(IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()));
                    //Remove friend from friends in ApllicationUser
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.Remove(IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()));
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
                //Make a request Model
                var userToDelete = new AddFriendRequest
                {
                    FriendId = CurrentIdOfManagedFriend
                };

                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.DeleteFriend, userToDelete);

                if (result)
                {
                    //If user are on page with given friend...
                    if (IoC.Get<ApplicationViewModel>().CurrentViewModel is FriendModel friend)
                    {
                        if (friend.FriendId == CurrentIdOfManagedFriend)
                            //Go to GamesPage
                            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GamesPage);

                    }

                    //Remove friend from friend list
                    IoC.Get<ApplicationViewModel>().Friends.Remove(IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == CurrentIdOfManagedFriend));
                    //Remove friend form ApplicationUser friends
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.Remove(IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == CurrentIdOfManagedFriend));
                    //Clear CurrentIdOfManagedFriend 
                    CurrentIdOfManagedFriend = "";
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
                //Close friend settings popup
                IsFriendSettingsOpen = false;

                //Make a request Model
                var userToBlock = new AddFriendRequest
                {
                    FriendId = CurrentIdOfManagedFriend
                };

                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.BlockFriend, userToBlock);

                if (result)
                {
                    //If user are on page with given friend...
                    if (IoC.Get<ApplicationViewModel>().CurrentViewModel is FriendModel friend)
                    {
                        if (friend.FriendId == IoC.Get<ApplicationViewModel>().CurrentFriendId)
                            //Go to GamesPage
                            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GamesPage);

                    }

                    IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == CurrentIdOfManagedFriend).Status = StatusOfFriendship.Blocked;
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == CurrentIdOfManagedFriend).Status = StatusOfFriendship.Blocked;
                    CurrentIdOfManagedFriend = "";
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
                //Make a request Model
                var friendToUnblock = new UnblockFriendRequest
                {
                    FriendId = FriendId.ToString()
                };

                var result = await HttpExtensions.HandleHttpRequestAsync(FriendRoutes.UnblockFriend, friendToUnblock);

                if (result)
                {
                    //Change StatusOfFriendship with friend to accepted in ApplicationUser 
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == friendToUnblock.FriendId).Status = StatusOfFriendship.Accepted;

                    //Change StatusOfFriendship in friends to accepted
                    IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == friendToUnblock.FriendId).Status = StatusOfFriendship.Accepted;
                }
            });
        }

        /// <summary>
        /// Method that handle going to chat 
        /// with given friend
        /// </summary>
        /// <param name="FriendId">ID of friend</param>
        private async Task GoToUser(object FriendId)
        {
            await RunCommandAsync(() => GoToUserIsRunning, async () => 
            {
                //Remove previous refreshing
                TimerExtensions.RemoveRepetingMessagesFromApplicationUser();

                //Finds user of given if
                var chatUser = IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString());

                //Create new ObservableCollection with messages
                chatUser.Messages = new ObservableCollection<MessageModel>();

                //If StatusOfFriendship is not blocked or pending...
                if (!(chatUser.Status == StatusOfFriendship.Blocked || chatUser.Status == StatusOfFriendship.Pending))
                {
                    //If any user is selected
                    if (!(string.IsNullOrEmpty(IoC.Get<ApplicationViewModel>().CurrentFriendId)))
                    {
                        //Find if he exists
                        if(IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == IoC.Get<ApplicationViewModel>().CurrentFriendId) != null)
                        {
                            //Deselect him
                            IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == IoC.Get<ApplicationViewModel>().CurrentFriendId).IsSelected = false;
                        }
                    }

                    //Select new user
                    chatUser.IsSelected = true;

                    //Set actual friend
                    IoC.Get<ApplicationViewModel>().CurrentFriendId = FriendId.ToString();

                    //Send request to the API
                    var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<List<MessageDataModel>>(MessageRoutes.GetMessages, new GetMessagesRequest
                    {
                        FriendId = FriendId.ToString()
                    });

                    //If there is any messages...
                    if(context != null)
                    {
                        //For each message...
                        foreach (var message in context)
                        {
                            //add message to Message List
                            chatUser.Messages.Add(new MessageModel
                            {
                                Message = message.Message,
                                SentByMe = message.SentByMe,
                                DateOfSent = TimeZoneInfo.ConvertTimeFromUtc(message.DateOfSent, TimeZoneInfo.Local),
                                FriendsImage = IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Image
                            });
                        }
                        
                        //Set chatUser messages to new ObservableCollection and order them by date
                        chatUser.Messages = new ObservableCollection<MessageModel>(chatUser.Messages.OrderBy(x => x.DateOfSent));
                    }

                    //Go to chat page with specific user of given id
                    IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.ContactPage, chatUser);
                }
            });

        }

        /// <summary>
        /// Method that handle open friend settings
        /// </summary>
        /// <param name="FriendId">Friend ID that is clicked</param>
        private void OpenFriendSettings(object FriendId)
        {
            //Set CurrentIdOfManagedFriend
            CurrentIdOfManagedFriend = FriendId.ToString();

            //If StatusOfFriendship is accepted with this friend,
            //i.e open settings only for accepted friend
            if (IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Status == StatusOfFriendship.Accepted)
            {
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
