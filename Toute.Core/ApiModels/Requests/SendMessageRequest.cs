using System;

namespace Toute.Core
{
    /// <summary>
    /// Request to send a message
    /// </summary>
    public class SendMessageRequest : RelationshipModel
    {
        /// <summary>
        /// Value of the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Date when message was sent
        /// </summary>
        public DateTime DateOfSend { get; set; }

    }
}
