using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Toute.Core;
using Toute.Extensions;

namespace Toute
{
    /// <summary>
    /// View Model for Contact Page
    /// </summary>
    public class ContactPageViewModel : BaseViewModel
    {
        #region Public members

        /// <summary>
        /// List of messages, to display
        /// </summary>
        public ObservableCollection<MessageBoxModel> Messages { get; set; }

        /// <summary>
        /// Current user that is selected
        /// </summary>
        public ChatUserModel CurrentChatUser { get; set; }

        #endregion

        #region Public commands

        /// <summary>
        /// Command that is fired, when user send message
        /// </summary>
        public ICommand SendMessageCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactPageViewModel()
        {
            //Create a list
            Messages = new ObservableCollection<MessageBoxModel>();

            //Create command
            SendMessageCommand = new ParametrizedRelayCommand((message) => SendMessage(message));
        }

        /// <summary>
        /// A constructor, that accept a <see cref="BaseViewModel"/>
        /// as a parameter.
        /// </summary>
        /// <param name="viewModel">ViewModel, should be <see cref="ChatUserModel"/></param>
        public ContactPageViewModel(BaseViewModel viewModel) : this()
        {
            //Convert viewModel to ChatUser ViewModel
            CurrentChatUser = (viewModel as ChatUserModel);

            //If list is not created...
            if (CurrentChatUser.Messages == null)
                //create a list
                CurrentChatUser.Messages = new ObservableCollection<MessageBoxModel>();

            //Set messages as list
            Messages = CurrentChatUser.Messages;

            ///Sets refresh time
            var refreshTime = 3;

            //Make call to API for new messages, every x seconds.
            IoC.Get<ApplicationViewModel>().ApplicationUser.RefreshMessages = new Timer((e) =>
            {
                GetMessagesWithGivenUser(CurrentChatUser.FriendId, refreshTime);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(refreshTime));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Method that send message
        /// </summary>
        /// <param name="Textbox">TextBox with values</param>
        private async void SendMessage(object Textbox)
        {
            //Convert a TextBox
            var textBox = (Textbox as TextBox);

            //if text of TextBox is null or empty return
            if (!(string.IsNullOrEmpty(textBox.Text)))
            {
                //Send request to API
                var response = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.SendMessage, new SendMessageModel
                {
                    FriendId = IoC.Get<ApplicationViewModel>().Friend.FriendId,
                    Message = textBox.Text,
                    DateOfSend = DateTime.UtcNow
                }, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

                //If status code of response is OK...
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Read context as ApiResponse
                    var context = response.DeseralizeHttpResponse<ApiResponse>();
                    
                    //If response is successful...
                    if (context.IsSucessfull)
                    {
                        //Add message to Message list
                        Messages.Add(new MessageBoxModel
                        {
                            SentByMe = true,
                            Message = textBox.Text,
                            DateOfSent = DateTime.UtcNow
                        });

                        //Clear and Focus TextBox
                        textBox.Text = "";
                        textBox.Focus();
                    }
                    //Otherwise...
                    else
                    {
                        //Show error message
                        PopupExtensions.NewPopupWithMessage(context.ErrorMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Method that is fired every x seconds
        /// Gets all new messages if API returns any...
        /// </summary>
        /// <param name="FriendId">ID of friend that user are currently messaging with</param>
        /// <param name="lastRefresh">Seconds from last refresh</param>
        private async void GetMessagesWithGivenUser(string FriendId, int lastRefresh)
        {
            //Get current DateTime, and subtract lastRefresh seconds, to get when last refresh was 
            var LastRefresh = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(lastRefresh));

            //Make a request to API
            var response = await WebRequests.PostAsync(ApiRoutes.BaseUrl + ApiRoutes.GetMessages, new GetMessages
            {
                FriendId = FriendId.ToString(),
                LastRefreshDateTime = LastRefresh

            }, IoC.Get<ApplicationViewModel>().ApplicationUser.JWTToken);

            //If response status code is OK...
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Read context as ApiResponse<ChatUserDataModel>
                var context = response.DeseralizeHttpResponse<ApiResponse<ChatUserDataModel>>();

                //If there is successful response
                if (context.IsSucessfull)
                {
                    //For every message...
                    foreach (var message in context.TResponse.Messages)
                    {
                        //Add new message to message list
                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            if (message.SentByMe == false)
                                Messages.Add(new MessageBoxModel
                                {
                                    Message = message.Message,
                                    SentByMe = message.SentByMe,
                                    DateOfSent = message.DateOfSent
                                });
                        });
                    }

                    //Orders all messages by DateOfSent
                    Messages = new ObservableCollection<MessageBoxModel>(Messages.OrderBy(x => x.DateOfSent));
                }
            }
        }

        #endregion
    }
}
