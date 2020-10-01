using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Toute.Core;
using Toute.Core.DataModels;

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
        public bool BlockListOpen { get; set; }
        public bool RequestListOpen { get; set; }
        public string NameOfFriendToAdd { get; set; }
        public string CurrentNameOfFriendManaging { get; set; }
        public bool IsFriendSettingsOpen { get; set; }
        public bool IsFriendRequest { get; set; }
        public FriendUserModel Friend { get; set; }
        public ApplicationUserModel ApplicationUser { get; set; }

        /// <summary>
        /// If user is logged, store Friends in a list
        /// </summary>
        public ObservableCollection<ChatUserModel> Friends { get; set; }

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
        public ICommand OpenFriendSettings { get; set; }  
        
        /// <summary>
        /// Command that handle going to OptionsPage
        /// </summary>
        public ICommand SendFriendRequest { get; set; }
        public ICommand AcceptFriendRequest { get; set; }
        public ICommand DeclineFriendRequest { get; set; }
        public ICommand DeleteFriend { get; set; }
        public ICommand BlockFriend { get; set; }

        public ICommand BlockListCommand { get; set; }
        public ICommand RequestListCommand { get; set; }

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


            Friends = new ObservableCollection<ChatUserModel>();

            //Command that handle going to Chat page of specific user
            GoToUserCommand = new ParametrizedRelayCommand((id) => GoToUser(id));

            //Command that handle going to GamesPage
            GamesCommand = new RelayCommand(GoToGamesPage);

            //Command that handle going to OptionsPage
            SettingsCommand = new RelayCommand(GoToSettingsPage);

            OpenFriendSettings = new ParametrizedRelayCommand((id) => OpenSettingsFriend(id));

            SendFriendRequest = new RelayCommand(SendRequest);

            AcceptFriendRequest = new ParametrizedRelayCommand((username) => AcceptFriend(username));
            DeclineFriendRequest = new ParametrizedRelayCommand((username) => DeclineFriend(username));

            DeleteFriend = new RelayCommand(DeleteFriendFromFriendship);
            BlockFriend = new RelayCommand(BlockFriendMethod);

            RequestListCommand = new RelayCommand(RequestListStatus);
            BlockListCommand = new RelayCommand(BlockListStatus);
        }

        private void BlockListStatus()
        {
            BlockListOpen ^= true;
        }

        private void RequestListStatus()
        {
            RequestListOpen ^= true;
        }

        private async void BlockFriendMethod()
        {
            if (ApplicationUser == null || string.IsNullOrEmpty(CurrentNameOfFriendManaging))
                return;

            var userToBlock = new AddFriendModel
            {
                UserId = ApplicationUser.Id,
                FriendUsername = CurrentNameOfFriendManaging
            };

            var result = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.BlockFriend, userToBlock);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var httpContext = result.Content.ReadAsStringAsync().Result;

                var deserializedResult = JsonConvert.DeserializeObject<ApiResponse<ChatUserDataModel>>(httpContext);

                if (deserializedResult.IsSucessfull)
                {
                    Friends.FirstOrDefault(x => x.Name == CurrentNameOfFriendManaging).Status = StatusOfFriendship.Blocked;

                }
                else
                {
                    var newPopup = new DialogPopup();
                    newPopup.MainMessage.Text = deserializedResult.ErrorMessage;
                    newPopup.ShowDialog();
                }
            }
        }

        private async void DeleteFriendFromFriendship()
        {
            if (ApplicationUser == null || string.IsNullOrEmpty(CurrentNameOfFriendManaging))
                return;

            var userToDelete = new AddFriendModel
            {
                UserId = ApplicationUser.Id,
                FriendUsername = CurrentNameOfFriendManaging
            };

            var result = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.DeleteFriend, userToDelete);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var httpContext = result.Content.ReadAsStringAsync().Result;

                var deserializedResult = JsonConvert.DeserializeObject<ApiResponse<ChatUserDataModel>>(httpContext);

                if (deserializedResult.IsSucessfull)
                {
                    Friends.Remove(Friends.FirstOrDefault(x => x.Name == CurrentNameOfFriendManaging));
                }
                else
                {
                    var newPopup = new DialogPopup();
                    newPopup.MainMessage.Text = deserializedResult.ErrorMessage;
                    newPopup.ShowDialog();
                }

            }
        }

        private void OpenSettingsFriend(object name)
        {
            CurrentNameOfFriendManaging = name.ToString();

            IsFriendSettingsOpen ^= true;
        }

        private async void AcceptFriend(object test)
        {
            if (ApplicationUser == null || string.IsNullOrEmpty(test.ToString()))
                return;

            var userToAdd = new AddFriendModel
            {
                UserId = ApplicationUser.Id,
                FriendUsername = test.ToString()
            };

            var result = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.AddFriend, userToAdd);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var httpContext = result.Content.ReadAsStringAsync().Result;

                var friendToAdd = JsonConvert.DeserializeObject<ApiResponse<ChatUserDataModel>>(httpContext);

                if(friendToAdd.IsSucessfull)
                {
                    Friends.FirstOrDefault(x => x.Name == friendToAdd.TResponse.Name).Status = StatusOfFriendship.Accepted;
                }
                else
                {
                    var newPopup = new DialogPopup();
                    newPopup.MainMessage.Text = friendToAdd.ErrorMessage;
                    newPopup.ShowDialog();
                }

            }
        }

        private async void DeclineFriend(object username)
        {
            if (ApplicationUser == null || string.IsNullOrEmpty(username.ToString()))
                return;

            var userToReject = new AddFriendModel
            {
                UserId = ApplicationUser.Id,
                FriendUsername = username.ToString()
            };

            var result = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.RejectFriendRequest, userToReject);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var httpContext = result.Content.ReadAsStringAsync().Result;

                var deserializedResult = JsonConvert.DeserializeObject<ApiResponse<ChatUserDataModel>>(httpContext);

                if (deserializedResult.IsSucessfull)
                {
                    Friends.Remove(Friends.FirstOrDefault(x => x.Name == username.ToString()));

                }
                else
                {
                    var newPopup = new DialogPopup();
                    newPopup.MainMessage.Text = deserializedResult.ErrorMessage;
                    newPopup.ShowDialog();
                }

            }
        }

        private async void SendRequest()
        {

            if (ApplicationUser == null || string.IsNullOrEmpty(NameOfFriendToAdd))
                return;

            var userToAdd = new SendFriendRequestModel
            {
                UserId = ApplicationUser.Id,
                FriendUsername = NameOfFriendToAdd
            };

            var result = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.SendFriendRequest, userToAdd);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var httpContext = result.Content.ReadAsStringAsync().Result;

                var deserializedResult = JsonConvert.DeserializeObject<ApiResponse<ChatUserDataModel>>(httpContext);

                if (deserializedResult.IsSucessfull)
                {
                    var newPopup = new DialogPopup();
                    newPopup.MainMessage.Text = "Friend request sent";
                    newPopup.ShowDialog();
                }
                else
                {
                    var newPopup = new DialogPopup();
                    newPopup.MainMessage.Text = deserializedResult.ErrorMessage;
                    newPopup.ShowDialog();
                }
            }
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
        private async void GoToUser(object friend)
        {
            //Finds user of given if
            var chatUser = Friends.FirstOrDefault(x => x.Name == friend.ToString());

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

                var response = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.GetMessages, new GetMessages
                {
                    UserId = ApplicationUser.Id,
                    FriendUsername = chatUser.Name
                });

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    IoC.Get<ApplicationViewModel>().Friend = new FriendUserModel
                    {
                        Username = chatUser.Name
                    };

                    var content = await response.Content.ReadAsStringAsync();
                    var messages = JsonConvert.DeserializeObject<ApiResponse<ChatUserDataModel>>(content);

                    if(messages.IsSucessfull)
                    {
                        var mess = new ObservableCollection<MessageBoxModel>();
                        foreach (var message in messages.TResponse.Messages)
                        {
                            mess.Add(new MessageBoxModel
                            {
                                Message = message.Message,
                                SentByMe = message.SentByMe
                            });
                        }
                        chatUser.Messages = mess;
                    }          
                }

                //Go to chat page with specific user of given id
                IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.ContactPage, chatUser);
            }
        }

        #endregion
    }
}
