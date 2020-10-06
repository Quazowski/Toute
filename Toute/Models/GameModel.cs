using System.IO;

namespace Toute
{
    /// <summary>
    /// Model of file that is used in <see cref="GamesPage"/>
    /// for running games etc.
    /// </summary>
    public class GameModel : BaseViewModel
    {
        /// <summary>
        /// File name
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Path to the image of a file
        /// </summary>
        public string PathToImage { get; set; }

        /// <summary>
        /// Full path to the image of a file
        /// </summary>
        public string FullPathToImage => string.IsNullOrEmpty(PathToImage) ? null : Path.GetFullPath(PathToImage);

        /// <summary>
        /// Full path to the file
        /// </summary>
        public string PathToFile { get; set; }

        /// <summary>
        /// Path to file
        /// </summary>
        public string PathToGame { get; set; }

        /// <summary>
        /// Is settings popup open
        /// </summary>
        public bool PopupOpen { get; set; }
    }
}
