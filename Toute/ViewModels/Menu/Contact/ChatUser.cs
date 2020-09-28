using System.Collections.ObjectModel;

namespace Toute
{
    /// <summary>
    /// A Model for Chat user. Is used in friend list
    /// </summary>
    public class ChatUser : BaseViewModel
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
        /// Path to the image of the user
        /// </summary>
        public string PathToImage { get; set; }

        /// <summary>
        /// Is user selected
        /// </summary>
        public bool IsSelected { get; set; }

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
