using System.Windows.Media.Imaging;

namespace Toute
{
    /// <summary>
    /// Model of file that is used in <see cref="GamesPage"/>
    /// for running games etc.
    /// </summary>
    public class GameModel : BaseViewModel
    {
        /// <summary>
        /// Unique ID of the file
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Image of BitmapImage format
        /// </summary>
        public BitmapImage BitmapImage => BytesImage.BytesToBitMapImage();

        /// <summary>
        /// Image of byte[] format
        /// </summary>
        public byte[] BytesImage { get; set; }

        /// <summary>
        /// Full path to the file
        /// </summary>
        public string Path { get; set; }
    }
}
