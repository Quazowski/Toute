namespace Toute
{
    /// <summary>
    /// Model for sending a message
    /// </summary>
    public class MessageBoxModel : BaseViewModel
    {
        /// <summary>
        /// Value of the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Is message sent by user
        /// </summary>
        public bool SentByMe { get; set; }

        /// <summary>
        /// Set Background color depends on <see cref="SentByMe"/>
        /// </summary>
        public string BackgroundColorOfMessage => SentByMe ? "#8f9ae9" : "#ffffff";

        /// <summary>
        /// Set Foreground color depends on <see cref="SentByMe"/>
        /// </summary>
        public string ForegroundColorOfMessage => SentByMe ? "#ffffff" : "#000000";
        
    }
}
