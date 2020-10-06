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
            if (IoC.Get<ApplicationViewModel>().ApplicationUser != null)
            {
                //If timer that refresh messages is not null...
                if (IoC.Get<ApplicationViewModel>().ApplicationUser.RefreshMessages != null)
                {
                    //Clear timer, to stop requesting API for new messages
                    IoC.Get<ApplicationViewModel>().ApplicationUser.RefreshMessages.Dispose();
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
            if (IoC.Get<ApplicationViewModel>().ApplicationUser != null)
            {
                //If timer that refresh friends is not null...
                if (IoC.Get<ApplicationViewModel>().ApplicationUser.RefreshFriends != null)
                {
                    //Clear timer, to stop requesting API for new friends
                    IoC.Get<ApplicationViewModel>().ApplicationUser.RefreshFriends.Dispose();
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
