using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
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
            LoginCommand = new ParametrizedRelayCommand((parameter) => Login(parameter));

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
        public async void Login(object parameter)
        {
            //If there is any username is given...
            if(string.IsNullOrEmpty(Username))
            {
                PopupExtensions.NewPopupWithMessage("Provide username first");
            }

            //Send request with credentials to server, to login
            var response = await WebRequests.PostAsync(UserRoutes.Login,
                new LoginRequest
                {
                    Username = Username,
                    Password = "Mypassword1!" ?? (parameter as IHavePassword).SecureString.Unsecure()
                });

            //If server respond OK...
            if(response.StatusCode == HttpStatusCode.OK)
            {
                //Read context as ApiResponse<LoginResponseApiModel>
                var context = response.DeseralizeHttpResponse<ApiResponse<LoginResponse>>();

                //if ApiResponse is successful...
                if(context.IsSuccessful)
                {
                    var Friends = new ObservableCollection<FriendModel>();

                    foreach (var friend in context.TResponse.Friends)
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
                        Id = context.TResponse.Id,
                        Username = context.TResponse.Username,
                        Email = context.TResponse.Email,
                        Friends = Friends,
                        Image = context.TResponse.Image,
                        JWTToken = context.TResponse.JWTToken
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
                    IoC.Get<ApplicationViewModel>().ApplicationUser.RefreshFriends = new Timer((e) =>
                    {
                        RefreshFriends(IoC.Get<ApplicationViewModel>().Friends);
                    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

                    //Go to GamesPage page
                    IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.GamesPage);
                }
                //Otherwise
                else
                {
                    //Display error with error message
                    PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                }

            }
            //Otherwise...
            else
            {
                //Display error
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");
            }
        }

        /// <summary>
        /// Method that is fired every x second, by timer.
        /// It used to refresh friend list
        /// </summary>
        /// <param name="friends">Actual friend IDs of user</param>
        private async void RefreshFriends(ObservableCollection<FriendModel> friends)
        {
            //Make a request
            var listOfFriendsId = new RefreshFriendsRequest();

            //add all friend IDs to the list
            foreach (var friend in friends)
            {
                listOfFriendsId.FriendsId.Add(friend.FriendId);
            }

            //Make a request to the server with friend IDs
            var response = await WebRequests.PostAsync(FriendRoutes.GetFriends,
                listOfFriendsId,
                IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

            //If server respond OK...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Deseralize content as ApiResponse<UpdateFriends>
                var context = response.DeseralizeHttpResponse<ApiResponse<UpdateFriendsResponse>>();

                //If response is successful and there is any TRespond...
                if (context.IsSuccessful && context.TResponse != null)
                {
                    //If there are any friends to add...
                    if(!(context.TResponse.FriendsToAdd == null || context.TResponse.FriendsToAdd.Count == 0))
                    {
                        //for every friends...
                        foreach (var friend in context.TResponse.FriendsToAdd)
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
                    if (!(context.TResponse.FriendsToRemove == null || context.TResponse.FriendsToRemove.Count == 0))
                    {
                        //For every friend to remove...
                        foreach (var friend in context.TResponse.FriendsToRemove)
                        {
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
            }
            //Else, if statusCode have no content...
            else if(response.StatusCode == HttpStatusCode.NoContent)
            {
                //Do nothing
            }
            //Otherwise...
            else
            {
                //TODO: Delete this, and let logger write a error
                PopupExtensions.NewPopupWithMessage("Unknown error occurred");
            }
        }


        #endregion
    }
}
