using System.Collections.Generic;

namespace Toute.Core
{
    public class UpdateFriends
    {
        public List<ChatUserModel> friendsToAdd { get; set; }
        public List<string> friendsToRemove { get; set; }
    }
}
