using System.Collections.Generic;

namespace Toute.Core
{
    /// <summary>
    /// Request to refresh friend list
    /// </summary>
    public class RefreshFriendsRequest
    {
        /// <summary>
        /// Actual list of friends, contains all 
        /// friend IDs
        /// </summary>
        public List<string> FriendsId { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RefreshFriendsRequest()
        {
            //Create new list of strings
            FriendsId = new List<string>();
        }
    }
}
