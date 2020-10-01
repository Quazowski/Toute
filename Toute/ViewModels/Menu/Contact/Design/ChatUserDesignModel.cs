namespace Toute
{
    /// <summary>
    /// Design model for <see cref="ChatUserModel"/>
    /// to display in <see cref="UserChatControl"/>
    /// </summary>
    public class ChatUserDesignModel : ChatUserModel
    {
        /// <summary>
        /// Makes a static instance of this class
        /// </summary>
        public static ChatUserDesignModel Instance => new ChatUserDesignModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatUserDesignModel()
        {
            //Set name to...
            Name = "Design time name";
        }
    }
}
