using Microsoft.Win32;
using NLog;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Toute.Extensions;

namespace Toute
{
    /// <summary>
    /// Extensions for <see cref="System.Windows.Media.Imaging"/> namespace, and
    /// <see cref="System.Drawing"/> namespace.
    /// </summary>
    public static class ImageExtension
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Extension that convert <see cref="Image"/> to
        /// byte[].
        /// </summary>
        /// <param name="image">Image to convert</param>
        /// <returns>image as byte[]</returns>
        public static byte[] ImageToBytes(this Image image)
        {
            _logger.Debug("Converting from System.Drawing.Image to byte[]");
            //using a MemoryStream...
            using var ms = new MemoryStream();
            //convert image to RawFormat
            image.Save(ms, image.RawFormat);

            //and return as byte[]
            return ms.ToArray();
        }

        /// <summary>
        /// Extension that convert byte[] to <see cref="Image"/>
        /// </summary>
        /// <param name="imageInBytes">image in byte[]</param>
        /// <returns>Image as <see cref="Image"/></returns>
        public static Image BytesToImage(this byte[] imageInBytes)
        {
            _logger.Debug("Converting from byte[] to System.Drawing.Image");
            //using a MemoryStream
            using var ms = new MemoryStream(imageInBytes);
            //convert from bytes to Image
            var image = Image.FromStream(ms);

            //and return image
            return image;
        }

        /// <summary>
        /// Extension that convert from byte[] 
        /// to <see cref="BitmapImage"/>
        /// </summary>
        /// <param name="imageInBytes">Image in byte[] format</param>
        /// <returns><see cref="BitmapImage"/></returns>
        public static BitmapImage BytesToBitMapImage(this byte[] imageInBytes)
        {
            _logger.Debug("Converting from byte[] to System.Windows.Media.Imaging.BitmapImage");

            //using a MemoryStream...
            using var ms = new MemoryStream(imageInBytes);

            //create new BitMapImage
            var image = new BitmapImage();

            //Convert from byte[] to BitMapImage
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();

            //Returns BitMapImage
            return image;
        }

        /// <summary>
        /// Extension that convert from <see cref="Image"/>
        /// to <see cref="BitmapImage"/>
        /// </summary>
        /// <param name="image">Image as <see cref="Image"/></param>
        /// <returns><see cref="BitmapImage"/></returns>
        public static BitmapImage ImageToBitMapImage(this Image image)
        {
            _logger.Debug("Converting from System.Drawing.Image to System.Windows.Media.Imaging.BitmapImage");

            //using MemoryStream...
            using var ms = new MemoryStream();

            //Convert to RawFormat
            image.Save(ms, image.RawFormat);

            //Convert to byte[]
            var bytes = ms.ToArray();

            //using MemoryStream...
            using var memoryStream = new MemoryStream(bytes);

            //Create new BitMapImage
            var bitMapImage = new BitmapImage();

            //Convert from byte[] to BitmapImage
            bitMapImage.BeginInit();
            bitMapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitMapImage.StreamSource = memoryStream;
            bitMapImage.EndInit();

            //returns BitMapImage
            return bitMapImage;
        }

        /// <summary>
        /// Extension that convert <see cref="Icon"/>
        /// to byte[]
        /// </summary>
        /// <param name="icon">Icon to convert</param>
        /// <returns></returns>
        public static byte[] IconToBytes(this Icon icon)
        {
            _logger.Debug("Converting from System.Drawing.Icon to byte[]");

            //using a memory stream, convert to array
            using MemoryStream ms = new MemoryStream();
            icon.Save(ms);
            return ms.ToArray();
        }

        public static byte[] GetImageFromPCinBytes()
        {
            _logger.Debug("Attempt to change image");
            //Create new OpenFileDialog
            var Dialog = new OpenFileDialog();

            //Open new dialog, await to choose a file, or close window
            Dialog.ShowDialog();

            //Set pathToImage to the given path...
            var pathToImage = Dialog.FileName.ToUpper();

            //If path is not empty or null...
            if ((!(string.IsNullOrEmpty(pathToImage))))
            {
                if (!pathToImage.EndsWith(".JPG") && !pathToImage.EndsWith(".JPE") && !pathToImage.EndsWith(".BMP") && !pathToImage.EndsWith(".GIF") && !pathToImage.EndsWith(".PNG"))
                {
                    PopupExtensions.NewInfoPopup("Wrong format of photo. Acceptable extensions: .JPG, .JPE, .BMP, .GIF, .PNG");
                    return null;
                }

                try
                {
                    //Take a image from PC
                    Image imageFromPC = Image.FromFile(pathToImage);

                    //Set Image in model, as byte[] image
                    return imageFromPC.ImageToBytes();
                }
                catch (Exception ex)
                {
                    PopupExtensions.NewErrorPopup("Wrong format of photo. Acceptable extensions: .JPG, .JPE, .BMP, .GIF, .PNG");
                    _logger.Warn(ex, "User tried to upload wrong image format");
                    return null;
                }
            }
            else
            {
                _logger.Debug("Did not changed Image.No new image was selected.");
                return null;
            }
        }

    }
}
