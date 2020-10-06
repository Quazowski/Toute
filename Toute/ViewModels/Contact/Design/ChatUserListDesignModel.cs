﻿using System.Collections.ObjectModel;

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
            Friends = new ObservableCollection<FriendModel>
            {
                new FriendModel
                {
                    Name = "Design",
                    Status = Core.DataModels.StatusOfFriendship.Accepted,
                    IsSelected = true
                },
                new FriendModel
                {
                    Name = "Short Design",
                    Status = Core.DataModels.StatusOfFriendship.Blocked
                },
                new FriendModel
                {
                    Name = "Long name in design",
                    Status = Core.DataModels.StatusOfFriendship.Pending
                },
                new FriendModel
                {
                    Name = "Second Design",
                    Status = Core.DataModels.StatusOfFriendship.Accepted
                },
            };
        }
    }
}