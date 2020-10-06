using System;

namespace Toute.Core
{
    public class SendMessageModel : RelationshipModel
    {
        /// <summary>
        /// Value of the message
        /// </summary>
        public string Message { get; set; }
        public DateTime DateOfSend { get; set; }

    }
}
