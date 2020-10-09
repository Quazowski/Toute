using System.ComponentModel.DataAnnotations;

namespace Toute.Core
{
    /// <summary>
    /// Request to send friend request
    /// </summary>
    public class SendFriendRequest
    {
        /// <summary>
        /// Username of the friend
        /// </summary>
        [Required(ErrorMessage = "You must provide a name!")]
        public string FriendUsername { get; set; }
    }
}
