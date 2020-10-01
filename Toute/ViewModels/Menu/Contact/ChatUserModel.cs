using System.Collections.ObjectModel;
using Toute.Core.DataModels;

namespace Toute
{
    /// <summary>
    /// A Model for Chat user. Is used in friend list
    /// </summary>
    public class ChatUserModel : BaseViewModel
    {
        /// <summary>
        /// Id of user
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image of the user
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Is user selected
        /// </summary>
        public bool IsSelected { get; set; }
        public StatusOfFriendship Status { get; set; }

        /// <summary>
        /// Depending on <see cref="IsSelected"/> value, set a background to...
        /// </summary>
        public string IsSelectedBackground => IsSelected ? "#aaaaaa" : "#0000";

        /// <summary>
        /// List of all messages with a user
        /// </summary>
        public ObservableCollection<MessageBoxModel> Messages { get; set; }
    }
}
