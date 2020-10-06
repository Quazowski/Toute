using System.Collections.Generic;
using Toute.Core.DataModels;

namespace Toute.Core
{
    public class FriendResponse
    {
        /// <summary>
        /// Id of friend user
        /// </summary>
        public string FriendId { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image of the user in byte[]
        /// </summary>
        public byte[] BytesImage { get; set; }

        /// <summary>
        /// Is user selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Status of friendship e.g Pending
        /// </summary>
        public StatusOfFriendship Status { get; set; }

        /// <summary>
        /// List of all messages with a user
        /// </summary>
        public List<MessageResponse> Messages { get; set; } = new List<MessageResponse>();
    }
}
