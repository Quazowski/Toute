using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Toute
{
    /// <summary>
    /// Extensions for <see cref="System.Windows.Media.Imaging"/> namespace, and
    /// <see cref="System.Drawing"/> namespace.
    /// </summary>
    public static class ImageExtension
    {
        /// <summary>
        /// Extension that convert <see cref="Image"/> to
        /// byte[].
        /// </summary>
        /// <param name="image">Image to convert</param>
        /// <returns>image as byte[]</returns>
        public static byte[] ImageToBytes(this Image image)
        {
            //using a MemoryStream...
            using var ms = new MemoryStream();
            //convert image to RawFormat
             image.Save(ms, image.RawFormat);

            //and return as byte[]
            return ms.ToArray();
        }

        /// <summary>
        /// Extension that convert byte[] to <see cref="System.Drawing.Image"/>
        /// </summary>
        /// <param name="imageInBytes">image in byte[]</param>
        /// <returns>Image as <see cref="System.Drawing.Image"/></returns>
        public static Image BytesToImage(this byte[] imageInBytes)
        {
            //using a MemoryStream
            using var ms = new MemoryStream(imageInBytes);
             //convert from bytes to Image
             var image = Image.FromStream(ms);

            //and return image
            return image;
        }

        /// <summary>
        /// Extension that convert from byte[] 
        /// to <see cref="System.Windows.Media.Imaging.BitmapImage"/>
        /// </summary>
        /// <param name="imageInBytes">Image in byte[] format</param>
        /// <returns><see cref="System.Windows.Media.Imaging.BitmapImage"/></returns>
        public static BitmapImage BytesToBitMapImage(this byte[] imageInBytes)
        {
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
        /// Extension that convert from <see cref="System.Drawing.Image"/>
        /// to <see cref="System.Windows.Media.Imaging.BitmapImage"/>
        /// </summary>
        /// <param name="image">Image as <see cref="System.Drawing.Image"/></param>
        /// <returns><see cref="System.Windows.Media.Imaging.BitmapImage"/></returns>
        public static BitmapImage ImageToBitMapImage(this Image image)
        {
            //using MemoryStream...
            using var ms = new MemoryStream();

            //Convert to RawFormat
            image.Save(ms, image.RawFormat);

            //Convert to byte[]
            var bytes =  ms.ToArray();

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


    }
}
