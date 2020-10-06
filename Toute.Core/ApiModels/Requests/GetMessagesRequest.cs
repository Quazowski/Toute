using System;

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
        public DateTime LastRefreshDateTime { get; set; }
    }
}
