using System.Collections.Generic;

namespace Toute.Core
{
    public class LoginResponseApiModel
    {

        public string Id { get; set; }
        /// <summary>
        /// Username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }
        public byte[] Image { get; set; }

        public List<ChatUserModel> Friends { get; set; }
        public string JWTToken { get; set; }
    }
}
