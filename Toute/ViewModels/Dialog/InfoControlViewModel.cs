using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Toute
{
    /// <summary>
    /// ViewModel of <see cref="InfoControl"/>
    /// </summary>
    public class InfoControlViewModel : BaseViewModel
    {
        /// <summary>
        /// Determines if there is any error
        /// </summary>
        public bool IsError { get; set; } = false;

        /// <summary>
        /// Message of the info or error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Time of creation
        /// </summary>
        public DateTime DateOfCreation { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Is marked as to delete
        /// </summary>
        public bool ToDelete => DateOfCreation < DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(20));

        /// <summary>
        /// Image that is displayed, depending on <see cref="IsError"/>
        /// </summary>
        public BitmapImage ImageToDisplay => IsError ? Image.FromFile(DirectoryExtensions.GetPathToImageFromImages("cancelIcon.png")).ImageToBitMapImage() :
            Image.FromFile(DirectoryExtensions.GetPathToImageFromImages("info.png")).ImageToBitMapImage();
    }
}
