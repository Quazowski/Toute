using System;
using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to get messages with the friend
    /// that is requested periodically
    /// </summary>
    public class GetMessagesRequest : RelationshipModel
    {
        /// <summary>
        /// Date when last updated occurred
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Invalid type for a date")]
        public DateTime LastRefreshDateTime { get; set; }
    }
}
