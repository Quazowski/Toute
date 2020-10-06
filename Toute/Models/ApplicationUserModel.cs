﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace Toute.Core
{
    /// <summary>
    /// Model of user
    /// </summary>
    public class ApplicationUserModel : BaseViewModel
    {
        /// <summary>
        /// Unique Id of the user
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Timer that periodically refresh messages,
        /// with the friend. Only when user is logged,
        /// and are on chat page with friend
        /// </summary>
        public Timer RefreshMessages { get; set; }

        /// <summary>
        /// Timer t hat periodically refresh friend list,
        /// only when user is logged
        /// </summary>
        public Timer RefreshFriends { get; set; }

        /// <summary>
        /// Image of the user
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// List of all friends, that user have
        /// </summary>
        public ObservableCollection<FriendModel> Friends { get; set; }

        /// <summary>
        /// Authorization token
        /// </summary>
        public string JWTToken { get; set; }
    }
}
