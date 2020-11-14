using System;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Message can not be empty")]
        [MaxLength(402, ErrorMessage = "Message too long")]
        public string Message { get; set; }

        /// <summary>
        /// Date when message was sent
        /// </summary>
        [Required(ErrorMessage = "You have to provide date!")]
        [DataType(DataType.Date, ErrorMessage = "Time must be of Date type!")]
        public DateTime DateOfSend { get; set; }

        /// <summary>
        /// Determines if message is image
        /// </summary>
        public bool IsImage { get; set; }
    }
}
