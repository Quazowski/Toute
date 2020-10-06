using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
            SendFriendRequestCommand = new RelayCommand(SendFriendRequest);
            AcceptFriendRequestCommand = new ParametrizedRelayCommand((FriendId) => AcceptFriendRequest(FriendId));
            DeclineFriendRequestCommand = new ParametrizedRelayCommand((FriendId) => DeclineFriendRequest(FriendId));
            DeleteFriendCommand = new RelayCommand(DeleteFriend);
            BlockFriendCommand = new RelayCommand(BlockFriend);
            UnblockFriendCommand = new ParametrizedRelayCommand((FriendId) => UnblockFriend(FriendId));
            GoToUserCommand = new ParametrizedRelayCommand((FriendId) => GoToUser(FriendId));
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
        private async void SendFriendRequest()
        {
            //If user is not logged, or TextBox is empty...
            if (IoC.Get<ApplicationViewModel>().ApplicationUser == null || string.IsNullOrEmpty(NameOfFriendToAdd))
                return;

            //Sets values of friend request
            var userToAdd = new SendFriendRequest
            {
                FriendUsername = NameOfFriendToAdd
            };

            //Sends request to API
            var response = await WebRequests.PostAsync(FriendRoutes.SendFriendRequest, userToAdd, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

            //If response status code is OK...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Read context as ApiResponse<ChatUserDataModel>
                var context = response.DeseralizeHttpResponse<ApiResponse<FriendDataModel>>();

                //If ApiResponse is successful
                if (context.IsSuccessful)
                {
                    //Show that friend request is sent successfully
                    //TODO: Change it to nice looking animation
                    PopupExtensions.NewPopupWithMessage("Friend request sent");
                }
                //Otherwise...
                else
                {
                    //Display error message
                    PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// Method that handle accept friend request
        /// </summary>
        /// <param name="FriendId">ID of friend to accept</param>
        private async void AcceptFriendRequest(object FriendId)
        {
            //If ID is null...
            if (string.IsNullOrEmpty(FriendId.ToString()))
                //show error message
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");

            //Make a request Model
            var userToAdd = new AddFriendRequest
            {
                FriendId = FriendId.ToString()
            };

            //Send request to the API
            var response = await WebRequests.PostAsync(FriendRoutes.AddFriend, userToAdd, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

            //if response status code is OK...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Read context as ApiResponse<ChatUserDataModel>
                var context = response.DeseralizeHttpResponse<ApiResponse<FriendDataModel>>();

                //If ApiResponse is successful
                if (context.IsSuccessful)
                {
                    //Set value of StatusOfFiendship in ApplicationUser friends, to accepted with given friend
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Status = StatusOfFriendship.Accepted;
                    //Set value of StatusOfFiendship in Friends list, to accepted with given friend
                    IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Status = StatusOfFriendship.Accepted;
                }
                //Otherwise...
                else
                {
                    //Show error message
                    PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                }

            }
        }

        /// <summary>
        /// Method that handle declining a friend request
        /// </summary>
        /// <param name="FriendId">ID of friend to decline friend request</param>
        private async void DeclineFriendRequest(object FriendId)
        {
            //If there is not Friend id...
            if (string.IsNullOrEmpty(FriendId.ToString()))
                //display error message
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");

            //Make a request Model
            var userToReject = new AddFriendRequest
            {
                FriendId = FriendId.ToString()
            };

            //Send request to the API
            var response = await WebRequests.PostAsync(FriendRoutes.RejectFriendRequest, userToReject, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

            //if response status code is OK...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Read context as ApiResponse<ChatUserDataModel>
                var context = response.DeseralizeHttpResponse<ApiResponse<FriendDataModel>>();

                //If ApiResponse is successful
                if (context.IsSuccessful)
                {
                    //Remove friend from friend list
                    IoC.Get<ApplicationViewModel>().Friends.Remove(IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()));
                    //Remove friend from friends in ApllicationUser
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.Remove(IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()));

                }
                //Otherwise...
                else
                {
                    //Show error message
                    PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                }

            }
        }

        /// <summary>
        /// Method that handle deleting friend
        /// </summary>
        /// <param name="FriendId">ID of friend to delete</param>
        private async void DeleteFriend()
        {
            //If there is not Friend id...
            if (string.IsNullOrEmpty(CurrentIdOfManagedFriend))
                //display error message
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");

            //Close friend settings popup
            IsFriendSettingsOpen = false;

            //Make a request Model
            var userToDelete = new AddFriendRequest
            {
                FriendId = CurrentIdOfManagedFriend
            };

            //Send request to the API
            var response = await WebRequests.PostAsync(FriendRoutes.DeleteFriend, userToDelete, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

            //if response status code is OK...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Read context as ApiResponse<ChatUserDataModel>
                var context = response.DeseralizeHttpResponse<ApiResponse<FriendDataModel>>();

                //If ApiResponse is successful
                if (context.IsSuccessful)
                {
                    //If user are on page with given friend...
                    if(IoC.Get<ApplicationViewModel>().CurrentViewModel is FriendModel friend)
                    {
                        if(friend.FriendId == CurrentIdOfManagedFriend)     
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
                //Otherwise...
                else
                {
                    //Show error message
                    PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// Method that handle blocking friend
        /// </summary>
        private async void BlockFriend()
        {
            //If there is not Friend id...
            if (string.IsNullOrEmpty(CurrentIdOfManagedFriend))
                //display error message
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");

            //Close friend settings popup
            IsFriendSettingsOpen = false;

            //Make a request Model
            var userToBlock = new AddFriendRequest
            {
                FriendId = CurrentIdOfManagedFriend
            };

            //Send request to the API
            var response = await WebRequests.PostAsync(FriendRoutes.BlockFriend, userToBlock, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

            //if response status code is OK...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Read context as 
                var context = response.DeseralizeHttpResponse<ApiResponse<FriendDataModel>>();

                //If ApiResponse is successful
                if (context.IsSuccessful)
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
                //Otherwise...
                else
                {
                    //Show error message
                    PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                }
            }
            //If status code is Unauthorized...
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //Show error message...
                PopupExtensions.NewPopupWithMessage("Unauthorized error. Please login to continue...");

                //Redirect user to login page
                IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.LoginPage);
            }
            //If any other error occurred...
            else
            {
                //Display error
                PopupExtensions.NewPopupWithMessage("Unknown error happened, please try again later...");
            }
        }

        /// <summary>
        /// Method that handle unlocking friend
        /// </summary>
        /// <param name="FriendId"></param>
        private async void UnblockFriend(object FriendId)
        {
            //If there is not Friend id...

            if (string.IsNullOrEmpty(FriendId.ToString()))
                //display error message
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");

            //Make a request Model
            var friendToUnblock = new UnblockFriendRequest
            {
                FriendId = FriendId.ToString()
            };

            //Send request to the API
            var response = await WebRequests.PostAsync(FriendRoutes.UnblockFriend,
                friendToUnblock,
                IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

            //if response status code is OK...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Read context as ApiResponse<ChatUserDataModel>
                var context = response.DeseralizeHttpResponse<ApiResponse<FriendDataModel>>();

                //If ApiResponse is successful
                if (context.IsSuccessful)
                {
                    //Change StatusOfFriendship with friend to accepted in ApplicationUser 
                    IoC.Get<ApplicationViewModel>().ApplicationUser.Friends.FirstOrDefault(x => x.FriendId == friendToUnblock.FriendId).Status = StatusOfFriendship.Accepted;

                    //Change StatusOfFriendship in friends to accepted
                    IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == friendToUnblock.FriendId).Status = StatusOfFriendship.Accepted;
                }
                //Otherwise...
                else
                {
                    //Show error message
                    PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                }

            }
        }

        /// <summary>
        /// Method that handle going to chat 
        /// with given friend
        /// </summary>
        /// <param name="FriendId">ID of friend</param>
        private async void GoToUser(object FriendId)
        {
            //Remove previous refreshing
            TimerExtensions.RemoveRepetingMessagesFromApplicationUser();

            //If there is not Friend id...

            if (string.IsNullOrEmpty(FriendId.ToString()))
                //display error message
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");

            //Finds user of given if
            var chatUser = IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString());

            if(chatUser == null)
            {
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");              
            }
            else
            {
                //Create new ObservableCollection with messages
                chatUser.Messages = new ObservableCollection<MessageModel>();

                //If StatusOfFriendship is not blocked or pending...
                if (!(chatUser.Status == StatusOfFriendship.Blocked || chatUser.Status == StatusOfFriendship.Pending))
                {

                    if(!(string.IsNullOrEmpty(IoC.Get<ApplicationViewModel>().CurrentFriendId)))
                    {
                        IoC.Get<ApplicationViewModel>().Friends.FirstOrDefault(x => x.FriendId == IoC.Get<ApplicationViewModel>().CurrentFriendId).IsSelected = false;
                    }

                    //Select new user
                    chatUser.IsSelected = true;

                    //Send request to the API
                    var response = await WebRequests.PostAsync(MessageRoutes.GetMessages, new GetMessagesRequest
                    {
                        FriendId = FriendId.ToString()
                    }, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

                    //if response status code is OK...
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //Read context as ApiResponse<ChatUserDataModel>
                        var context = response.DeseralizeHttpResponse<ApiResponse<List<MessageDataModel>>>();

                        //If ApiResponse is successful
                        if (context.IsSuccessful)
                        {
                            //Set actual friend
                            IoC.Get<ApplicationViewModel>().CurrentFriendId = FriendId.ToString();

                            //For each message...
                            foreach (var message in context.TResponse)
                            {
                                //add message to Message List
                                chatUser.Messages.Add(new MessageModel
                                {
                                    Message = message.Message,
                                    SentByMe = message.SentByMe,
                                    DateOfSent = message.DateOfSent
                                });
                            }
                            //Set chatUser messages to new ObservableCollection and order them by date
                            chatUser.Messages = new ObservableCollection<MessageModel>(chatUser.Messages.OrderBy(x => x.DateOfSent));
                        }
                    }

                    //Go to chat page with specific user of given id
                    IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.ContactPage, chatUser);

                }
            }
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
