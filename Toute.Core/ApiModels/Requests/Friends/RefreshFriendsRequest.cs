using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "List of friends must exists!")]
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
