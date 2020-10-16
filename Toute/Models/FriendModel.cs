using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Toute.Core;

namespace Toute
{
    /// <summary>
    /// A Model for Chat user. Is used in friend list
    /// </summary>
    public class FriendModel : BaseViewModel
    {
        /// <summary>
        /// Id of friend user
        /// </summary>
        public string FriendId { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Image of the user in byte[]
        /// </summary>
        public byte[] BytesImage { get; set; }

        /// <summary>
        /// If <see cref="BytesImage"/> is not null,
        /// Get image as BitmapImage converting <see cref="BytesImage"/> 
        /// using extensions from <see cref="DirectoryExtensions"/>
        /// and <see cref="ImageExtension"/>
        /// </summary>
        public BitmapImage Image => BytesImage?.BytesToBitMapImage() ?? System.Drawing.Image.FromFile(DirectoryExtensions.GetPathToImageFromImages("user.png")).ImageToBitMapImage();

        /// <summary>
        /// Is user selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Status of friendship e.g Pending
        /// </summary>
        public StatusOfFriendship Status { get; set; }

        /// <summary>
        /// Depends on <see cref="IsSelected"/> value, set a background to...
        /// </summary>
        public string IsSelectedBackground => IsSelected ? "#aaaaaa" : "#0000";

        /// <summary>
        /// List of all messages with a user
        /// </summary>
        public ObservableCollection<MessageModel> Messages { get; set; }
    }
}
