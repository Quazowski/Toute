using System.Collections.ObjectModel;

namespace Toute
{
    /// <summary>
    /// A design Model for <see cref="SideMenuControl"/>
    /// to display friends
    /// </summary>
    public class ChatUserListDesignModel : ApplicationViewModel
    {
        /// <summary>
        /// Makes a static instance of this class
        /// </summary>
        public static ChatUserListDesignModel Instance => new ChatUserListDesignModel();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatUserListDesignModel()
        {
            //Set friends in design time to...
            Friends = new ObservableCollection<ChatUser>
            {
                new ChatUser
                {
                    Name = "Design"
                },
                new ChatUser
                {
                    Name = "Short Design"
                },
                new ChatUser
                {
                    Name = "Long name in design"
                },
                new ChatUser
                {
                    Name = "Second Design"
                },
            };
        }
    }
}
