namespace Toute
{
    /// <summary>
    /// Design model for <see cref="MessageModel"/>
    /// </summary>
    public class MessageBoxDesignModel : MessageModel
    {
        /// <summary>
        /// Makes a static instance of this class
        /// </summary>
        public static MessageBoxDesignModel Instance => new MessageBoxDesignModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageBoxDesignModel()
        {
            //Set message to...
            Message = "Design time message";
        }
    }
}
