using System.Collections.Generic;

namespace Toute.Core
{
    /// <summary>
    /// Response to login to application
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Image of the user
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// List of friends
        /// </summary>
        public List<FriendResponse> Friends { get; set; }

        /// <summary>
        /// Authorization Token
        /// </summary>

        public TokenResponse Token { get; set; }
    }
}
