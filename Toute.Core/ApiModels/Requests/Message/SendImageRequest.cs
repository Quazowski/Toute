using System;
using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to send a message
    /// </summary>
    public class SendImageRequest : RelationshipModel
    {
        /// <summary>
        /// Value of the message
        /// </summary>
        [Required(ErrorMessage = "Image can not be empty")]
        public byte[] Image{ get; set; }

        /// <summary>
        /// Date when message was sent
        /// </summary>
        [Required(ErrorMessage = "You have to provide date!")]
        [DataType(DataType.Date, ErrorMessage = "Time must be of Date type!")]
        public DateTime DateOfSend { get; set; }

    }
}
