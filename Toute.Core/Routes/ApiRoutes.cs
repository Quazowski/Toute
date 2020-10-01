﻿namespace Toute.Core
{
    public class ApiRoutes
    {
        public const string BaseUrl = "https://localhost:5000/";

        public const string ApiRegister = "api/register";

        public const string ApiLogin = "api/login";

        public const string SendFriendRequest = "api/Chat/SendFriendRequest";

        public const string RejectFriendRequest = "api/Chat/RejectFriendRequest";

        public const string DeleteFriend = "api/Chat/DeleteFriend";

        public const string BlockFriend = "api/Chat/BlockFriend";

        public const string UnblockFriend = "api/Chat/UnblockFriend";

        public const string AddFriend = "api/Chat/AddFriend";

        public const string SendMessage = "api/Chat/SendMessage";

        public const string GetMessages = "api/Chat/GetMessage";

    }
}