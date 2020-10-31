using System;
using System.Windows.Media.Imaging;

namespace Toute
{
    /// <summary>
    /// Model for sending a message
    /// </summary>
    public class MessageModel : BaseViewModel
    {
        /// <summary>
        /// Value of the message
        /// </summary>
        public string Message { get; set; }

        public bool IsImage { get; set; }

        /// <summary>
        /// Is message sent by user
        /// </summary>
        public bool SentByMe { get; set; }

        /// <summary>
        /// Date when message was sent (UTC)
        /// </summary>
        public DateTime DateOfSent { get; set; }

        public BitmapImage FriendsImage { get; set; }

        public BitmapImage ImageMessage => IsImage ? Convert.FromBase64String(Message).BytesToBitMapImage() : null;

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
