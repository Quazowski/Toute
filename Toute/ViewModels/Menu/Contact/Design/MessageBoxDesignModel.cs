namespace Toute
{
    /// <summary>
    /// Design model for <see cref="MessageBoxModel"/>
    /// </summary>
    public class MessageBoxDesignModel : MessageBoxModel
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
