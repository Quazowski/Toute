using System;
using System.Collections.Generic;
using System.Text;
using Toute.Core.DataModels;

namespace Toute.Core
{
    public class ChatUserModel
    {
        /// <summary>
        /// Friend id of user
        /// </summary>
        public string FriendId { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }
        public byte[] Image { get; set; }
        /// <summary>
        /// List of all messages with a user
        /// </summary>
        public List<MessageBoxDataModel> Messages { get; set; }
        public ChatUserModel()
        {
            Messages = new List<MessageBoxDataModel>();
        }

        public StatusOfFriendship Status { get; set; }
    }
}
