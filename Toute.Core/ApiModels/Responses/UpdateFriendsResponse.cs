using System.Collections.Generic;

namespace Toute.Core
{
    /// <summary>
    /// Request to update friend
    /// </summary>
    public class UpdateFriendsResponse
    {
        /// <summary>
        /// New friends, that will be added to friend list
        /// </summary>
        public List<FriendResponse> FriendsToAdd { get; set; }

        /// <summary>
        /// Friends, who will be removed from friend list
        /// </summary>
        public List<string> FriendsToRemove { get; set; }
    }
}
