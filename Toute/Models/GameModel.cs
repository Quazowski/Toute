using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Toute.Core;

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
        /// Full path to the files
        /// </summary>
        public List<StringDataModel> Paths { get; set; }

        /// <summary>
        /// Marks if property is selected
        /// </summary>
        public bool IsSelected { get; set; }

        public string FirstPath => Paths.FirstOrDefault().Value;

        public GameModel()
        {
            Paths = new List<StringDataModel>();
        }
    }
}
