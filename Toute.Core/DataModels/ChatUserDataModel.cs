using System.Collections.Generic;
using Toute.Core.DataModels;

namespace Toute
{
    /// <summary>
    /// A Model for Chat user. Is used in friend list
    /// </summary>
    public class ChatUserDataModel
    {
        /// <summary>
        /// Id of user
        /// </summary>
        
        public virtual string Id { get; set; }
        public virtual string FriendId { get; set; }

        /// <summary>
        /// List of all messages with a user
        /// </summary>
        public virtual ICollection<MessageBoxDataModel> Messages { get; set; }
        public ChatUserDataModel()
        {
            Messages = new List<MessageBoxDataModel>();
        }

        public virtual StatusOfFriendship Status { get; set; }
    }
}
