using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Toute.Core
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Timer RefreshMessages { get; set; }
        public Timer RefreshFriends { get; set; }

        public byte[] Image { get; set; }
        public virtual ICollection<ChatUserModel> Friends { get; set; }
        public ApplicationUserModel()
        {
            Friends = new List<ChatUserModel>();
        }
        public string JWTToken { get; set; }
    }
}
