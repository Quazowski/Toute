using System.IO;

namespace Toute
{
    /// <summary>
    /// Extensions for Path/Directions
    /// </summary>
    public static class DirectoryExtensions
    {
        /// <summary>
        /// Get image from Images folder, of given name
        /// </summary>
        /// <param name="relativePathFromImagesDiretory">Path to the image, that starts after Images folder</param>
        /// <returns>full Path to the image</returns>
        public static string GetPathToImageFromImages(string relativePathFromImagesDiretory)
        {
            //Returns full Path to the image
            return Path.Combine(Path.GetFullPath(@"..\..\..\"), "Images", relativePathFromImagesDiretory);
        }
    }
}
