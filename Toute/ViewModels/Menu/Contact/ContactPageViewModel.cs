using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Toute
{
    /// <summary>
    /// View Model for Contact Page
    /// </summary>
    public class ContactPageViewModel : BaseViewModel
    {
        #region Public members

        /// <summary>
        /// Make a Messages List, to display for user
        /// </summary>
        public ObservableCollection<MessageBoxModel> Messages { get; set; }

        /// <summary>
        /// Current user that is selected
        /// </summary>
        public ChatUser CurrentChatUser { get; set; }

        #endregion

        #region Public commands

        /// <summary>
        /// Command that is fired, when user send message
        /// </summary>
        public ICommand SendMessageCommand { get; set; }

        #endregion

        #region Constructor

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

        #endregion

        #region Helpers



        #endregion

        /// <summary>
        /// Helper for <see cref="SendMessageCommand"/>
        /// that send message
        /// </summary>
        /// <param name="Textbox">TextBox with values</param>
        private void SendMessage(object Textbox)
        {
            //Convert a TextBox
            var textBox = (Textbox as TextBox);

            //if text of TextBox is null or Empty return
            if (!(string.IsNullOrEmpty(textBox.Text)))
            {
                //Add message to the User Message List
                CurrentChatUser.Messages.Add(new MessageBoxModel
                {
                    SentByMe = true,
                    Message = textBox.Text
                });

                //Clear TextBox for a next message
                textBox.Text = "";
            }

        }

        /// <summary>
        /// A constructor, that accept a <see cref="BaseViewModel"/>
        /// as a parameter.
        /// </summary>
        /// <param name="viewModel">ViewModel, should be <see cref="ChatUser"/></param>
        public ContactPageViewModel(BaseViewModel viewModel) : this()
        {
            //Convert viewModel to ChatUser ViewModel
            CurrentChatUser = (viewModel as ChatUser);

            //If list is not created...
            if (CurrentChatUser.Messages == null)
                //create a list
                CurrentChatUser.Messages = new ObservableCollection<MessageBoxModel>();

            //Set messages as list
            Messages = CurrentChatUser.Messages;

            //TODO: Get a ID from a user, and retrieve all messages from DB with this user
        }


    }
}
