namespace Toute.Core.DataModels
{
    /// <summary>
    /// All status than user can have with a friend
    /// </summary>
    public enum StatusOfFriendship
    {
        /// <summary>
        /// Friend request was sent, by other user, and user still did not accept it
        /// </summary>
        Pending = 1,

        /// <summary>
        /// User accepted friend request, and both users have themselves in friend list
        /// </summary>
        Accepted = 2,

        /// <summary>
        /// User has blocked friend
        /// </summary>
        Blocked = 3
    }
}
