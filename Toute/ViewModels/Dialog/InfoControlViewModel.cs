using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Toute
{
    public class InfoControlViewModel : BaseViewModel
    {     
        public bool IsError { get; set; } = false;
        public string Message { get; set; }

        public DateTime DateOfCreation { get; set; } = DateTime.UtcNow;

        public bool ToDelete => DateOfCreation < DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(20));

        public BitmapImage ImageToDisplay => IsError ? Image.FromFile(DirectoryExtensions.GetPathToImageFromImages("cancelIcon.png")).ImageToBitMapImage() : 
            Image.FromFile(DirectoryExtensions.GetPathToImageFromImages("info.png")).ImageToBitMapImage();
    }
}
