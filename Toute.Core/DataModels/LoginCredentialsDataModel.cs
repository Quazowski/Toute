using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Toute.Core
{
    public class LoginCredentialsDataModel
    {
        /// <summary>
        /// Unique Id of the user
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Username of the user
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Image of the user
        /// </summary>
        public virtual byte[] Image { get; set; }

        /// <summary>
        /// List of all friends, that user have
        /// </summary>
        public virtual ICollection<FriendDataModel> Friends { get; set; }

        /// <summary>
        /// Authorization token
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// Refresh authorization token
        /// </summary>
        public virtual string RefreshToken { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginCredentialsDataModel()
        {
            Friends = new List<FriendDataModel>();
        }
    }
}
