using System.Collections.Generic;

namespace Toute.Core
{
    public class RefreshFriendsModel
    {
        public List<string> FriendsId { get; set; }

        public RefreshFriendsModel()
        {
            FriendsId = new List<string>();
        }
    }
}
