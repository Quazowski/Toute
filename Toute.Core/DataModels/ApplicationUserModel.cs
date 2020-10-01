using System;
using System.Collections.Generic;
using System.Text;

namespace Toute.Core
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public virtual ICollection<ChatUserDataModel> Friends { get; set; }
        public ApplicationUserModel()
        {
            Friends = new List<ChatUserDataModel>();
        }
    }
}
