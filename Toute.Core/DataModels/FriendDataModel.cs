using System.Collections.Generic;
using Toute.Core.DataModels;

namespace Toute
{
    /// <summary>
    /// A Model for Friend, that will be saved to DB
    /// </summary>
    public class FriendDataModel
    {
        /// <summary>
        /// Unique Id of friendship in DB
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Id of friend
        /// </summary>
        public virtual string FriendId { get; set; }

        /// <summary>
        /// List of all messages with a user
        /// </summary>
        public virtual ICollection<MessageDataModel> Messages { get; set; }

        /// <summary>
        /// Status of friendship with friend
        /// </summary>
        public virtual StatusOfFriendship Status { get; set; }

        public FriendDataModel()
        {
            Messages = new List<MessageDataModel>();
        }
    }
}
