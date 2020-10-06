using System;

namespace Toute
{
    /// <summary>
    /// Model for sending a message
    /// </summary>
    public class MessageBoxDataModel
    {
        /// <summary>
        /// Id of the message
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Value of the message
        /// </summary>
        public string Message { get; set; }
        public DateTime DateOfSent { get; set; }

        /// <summary>
        /// Is message sent by user
        /// </summary>
        public bool SentByMe { get; set; }      
    }
}
