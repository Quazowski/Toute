namespace Toute.Core
{
    public class ApiUserRoutes
    {
        public const string Login = "api/login";

        public const string Register = "api/register";

        public const string ChangeUsername = "api/ChangeUsername";

        public const string ChangeEmail = "api/ChangeEmail";

        public const string ChangePassword = "api/ChangePassword";

        public const string ChangeImage = "api/ChangeImage";

        public const string RestartPassword = "api/RestartPassword";

        public const string SetNewPassword = "api/SetNewPassword";

        public const string RefreshToken = "api/RefreshToken";

        public const string ConfirmEmail = "api/ConfirmEmail";
    }

    public class ApiFriendRoutes
    {
        public const string GetFriends = "api/Chat/GetFriends";

        public const string GetFriendImage = "api/GetFriendImage";

        public const string SendFriendRequest = "api/Chat/SendFriendRequest";

        public const string AddFriend = "api/Chat/AddFriend";

        public const string RejectFriendRequest = "api/Chat/RejectFriendRequest";

        public const string DeleteFriend = "api/Chat/DeleteFriend";

        public const string BlockFriend = "api/Chat/BlockFriend";

        public const string UnblockFriend = "api/Chat/UnblockFriend";
    }

    public class ApiMessageRoutes
    {
        public const string SendMessage = "api/Chat/SendMessage";

        public const string SendImage = "api/Chat/SendImage";

        public const string GetMessages = "api/Chat/GetMessage/{PageNumber?}/{MessagesNumber?}";
    }
}
