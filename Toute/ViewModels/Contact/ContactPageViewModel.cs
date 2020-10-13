﻿using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Toute.Core;
using Toute.Core.Routes;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// View Model for Contact Page
    /// </summary>
    public class ContactPageViewModel : BaseViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public members

        /// <summary>
        /// List of messages, to display
        /// </summary>
        public ObservableCollection<MessageModel> Messages { get; set; }

        /// <summary>
        /// Current user that is selected
        /// </summary>
        public FriendModel CurrentChatUser { get; set; }

        /// <summary>
        /// Status of <see cref="SendMessageAsync(object)"/>
        /// </summary>
        public bool SendMessageIsRunning { get; set; }

        /// <summary>
        /// Status of <see cref="RefreshMessagesWithTheUserAsync(string, int)"/>
        /// </summary>
        public bool RefreshMessagesWithTheUserIsRunning { get; set; }

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
            _logger.Info("Start setting up ContactPageViewModel");
            //Create a list
            Messages = new ObservableCollection<MessageModel>();

            //Create command
            SendMessageCommand = new ParametrizedRelayCommand(async (message) => await SendMessageAsync(message));

            _logger.Info("Done setting up base constructor of ContactPageViewModel");
        }

        /// <summary>
        /// A constructor, that accept a <see cref="BaseViewModel"/>
        /// as a parameter.
        /// </summary>
        /// <param name="viewModel">ViewModel, should be <see cref="FriendModel"/></param>
        public ContactPageViewModel(BaseViewModel viewModel) : this()
        {
            //Convert viewModel to ChatUser ViewModel
            CurrentChatUser = (viewModel as FriendModel);

            //Set messages as list
            Messages = CurrentChatUser.Messages;

            ///Sets refresh time
            var refreshTime = 1;

            _logger.Info("Start refreshing messages with the friend");
            //Make call to API for new messages, every x seconds.
            ViewModelApplication.ApplicationUser.RefreshMessages = new Timer(async(e) =>
            {
                await RefreshMessagesWithTheUserAsync(CurrentChatUser.FriendId, refreshTime);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(refreshTime));

            _logger.Info("Done setting up ContactPageViewModel");
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Method that send message
        /// </summary>
        /// <param name="Textbox">TextBox with values</param>
        private async Task SendMessageAsync(object Textbox)
        {
            await RunCommandAsync(() => SendMessageIsRunning, async () =>
            {

                //Convert a TextBox
                if (!(Textbox is TextBox textBox))
                    return;
                    

                //if text of TextBox is null or empty return
                if (!(string.IsNullOrEmpty(textBox.Text)))
                {
                    _logger.Debug("Starts to send message");

                    //Send request to API
                    var result = await HttpExtensions.HandleHttpRequestAsync(MessageRoutes.SendMessage, new SendMessageRequest
                    {
                        FriendId = ViewModelApplication.CurrentFriendId,
                        Message = textBox.Text,
                        DateOfSend = DateTime.UtcNow
                    });

                    //If response is successful
                    if(result)
                    {
                        //Add message to Message list
                        ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == ViewModelApplication.CurrentFriendId).Messages.Add(new MessageModel
                        {
                            SentByMe = true,
                            Message = textBox.Text,
                            DateOfSent = DateTime.Now
                        });

                        //Clear and Focus TextBox
                        textBox.Text = "";
                        textBox.Focus();
                        _logger.Debug("Message successfully sent");
                        
                    }
                    else
                    {
                        _logger.Debug("Message not sent. Problem occurred when sending a message.");
                    }
                }
            });


        }

        /// <summary>
        /// Method that is fired every x seconds
        /// Gets all new messages if API returns any...
        /// </summary>
        /// <param name="FriendId">ID of friend that user are currently messaging with</param>
        /// <param name="lastRefresh">Seconds from last refresh</param>
        private async Task RefreshMessagesWithTheUserAsync(string FriendId, int lastRefresh)
        {
            _logger.Debug("Try to refresh messages");
            //Get current DateTime, and subtract lastRefresh seconds, to get when last refresh was 
            var LastRefresh = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(lastRefresh));

            //Make a request to API
            var context = await HttpExtensions.HandleHttpRequestOfTResponseAsync<List<MessageDataModel>>(MessageRoutes.GetMessages, new GetMessagesRequest
            {
                FriendId = FriendId.ToString(),
                LastRefreshDateTime = LastRefresh

            });

            //If there is successful response
            if (context?.Count > 0)
            {
                _logger.Debug($"Found {context.Count} new messages, adding them to the list of messages");
                //For every message...
                foreach (var message in context)
                {
                    //Add new message to message list
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (!message.SentByMe)
                            ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == FriendId.ToString()).Messages.Add(new MessageModel
                            {
                                Message = message.Message,
                                SentByMe = message.SentByMe,
                                DateOfSent = TimeZoneInfo.ConvertTimeFromUtc(message.DateOfSent, TimeZoneInfo.Local),
                                FriendsImage = ViewModelApplication.Friends.FirstOrDefault(x => x.FriendId == FriendId).Image
                            });
                    });

                }
            }
            _logger.Debug("Did not find any new messages");
        }
        #endregion
    }
}
