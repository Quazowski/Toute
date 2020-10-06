using System;

namespace Toute.Core
{
    public class MessageResponse
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
        /// Date when message was sent (UTC)
        /// </summary>
        public DateTime DateOfSent { get; set; }
    }
}
