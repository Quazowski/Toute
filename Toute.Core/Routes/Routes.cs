using System;
using System.Collections.Generic;
using System.Text;

namespace Toute.Core.Routes
{
    public class BaseRoute
    {
        protected const string BaseUrl = "https://localhost:5000/";
    }

    public class UserRoutes : BaseRoute
    {
        public const string Login = BaseUrl + "api/login";

        public const string Register = BaseUrl + "api/register";

        public const string ChangeUsername = BaseUrl + "api/ChangeUsername";

        public const string ChangeEmail = BaseUrl + "api/ChangeEmail";

        public const string ChangePassword = BaseUrl + "api/ChangePassword";

        public const string ChangeImage = BaseUrl + "api/ChangeImage";

        public const string RestartPassword = BaseUrl + "api/RestartPassword";

        public const string RefreshToken = BaseUrl + "api/RefreshToken";

        public const string ConfirmEmail = BaseUrl + "api/ConfirmEmail/{token}/{userId}";
    }

    public class FriendRoutes : BaseRoute
    {
        public const string GetFriends = BaseUrl + "api/Chat/GetFriends";

        public const string GetFriendImage = BaseUrl + "api/GetFriendImage";

        public const string SendFriendRequest = BaseUrl + "api/Chat/SendFriendRequest";

        public const string AddFriend = BaseUrl + "api/Chat/AddFriend";

        public const string RejectFriendRequest = BaseUrl + "api/Chat/RejectFriendRequest";

        public const string DeleteFriend = BaseUrl + "api/Chat/DeleteFriend";

        public const string BlockFriend = BaseUrl + "api/Chat/BlockFriend";

        public const string UnblockFriend = BaseUrl + "api/Chat/UnblockFriend";
    }

    public class MessageRoutes : BaseRoute
    {
        public const string SendMessage = BaseUrl + "api/Chat/SendMessage";

        public const string GetMessages = BaseUrl + "api/Chat/GetMessage";
    }
}
