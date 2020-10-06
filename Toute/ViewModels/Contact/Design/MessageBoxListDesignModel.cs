using System.Collections.ObjectModel;

namespace Toute
{
    /// <summary>
    /// Design model for <see cref="ContactPageViewModel"/>
    /// to display design time messages in <see cref="ContactPage"/>
    /// </summary>
    public class MessageBoxListDesignModel : ContactPageViewModel
    {
        /// <summary>
        /// Makes a static instance of this class
        /// </summary>
        public static MessageBoxListDesignModel Instance => new MessageBoxListDesignModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageBoxListDesignModel()
        {
            //Set messages to...
            Messages = new ObservableCollection<MessageModel>
            {
                new MessageModel
                {
                    Message = "Design message for a test",
                    SentByMe = true
                },
                new MessageModel
                {
                    Message = "Design message for a test, just a bit longer to see if wrapping works, and everything else is on place.",
                    SentByMe = true
                },
                new MessageModel
                {
                    Message = "Design message for a test Just standard",
                    SentByMe = false
                },
                new MessageModel
                {
                    Message = "Short mess",
                    SentByMe = true,
                },
                new MessageModel
                {
                    Message = "Design message for a test Just standard",
                    SentByMe = false
                }
            };
        }
    }
}
