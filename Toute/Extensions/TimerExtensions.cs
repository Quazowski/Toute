using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// Extensions for <see cref="System.Threading.Timer"/>
    /// </summary>
    public static class TimerExtensions
    {
        /// <summary>
        /// Extension that stop requesting API
        /// to refresh messages with friend
        /// </summary>
        public static void RemoveRepetingMessagesFromApplicationUser()
        {
            //if ApplicationUser is not null...
            if (ViewModelApplication.ApplicationUser != null)
            {
                //If timer that refresh messages is not null...
                if (ViewModelApplication.ApplicationUser.RefreshMessages != null)
                {
                    //Clear timer, to stop requesting API for new messages
                    ViewModelApplication.ApplicationUser.RefreshMessages.Dispose();
                }
            }
        }

        /// <summary>
        /// Extension that stop requesting API
        /// to refresh friends list
        /// </summary>
        public static void RemoveRepetingFriendsFromApplicationUser()
        {
            //if ApplicationUser is not null...
            if (ViewModelApplication.ApplicationUser != null)
            {
                //If timer that refresh friends is not null...
                if (ViewModelApplication.ApplicationUser.RefreshFriends != null)
                {
                    //Clear timer, to stop requesting API for new friends
                    ViewModelApplication.ApplicationUser.RefreshFriends.Dispose();
                }
            }
        }

        /// <summary>
        /// Extension that stop requesting API 
        /// to refresh messages with user and
        /// messages with friend
        /// </summary>
        public static void RemoveRepetingMethodsFromApplicationUser()
        {
            RemoveRepetingMessagesFromApplicationUser();
            RemoveRepetingFriendsFromApplicationUser();
        }
    }
}
