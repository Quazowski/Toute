using Microsoft.Win32;
using NLog;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Toute
{
    /// <summary>
    /// ViewModel for AddGameWindow
    /// </summary>
    public class AddMultipleFilesDialogViewModel : WindowViewModel
    {
        #region Private members

        /// <summary>
        /// private logger for <see cref="AddMultipleFilesDialogViewModel"/>
        /// </summary>
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public members

        /// <summary>
        /// Name for the file
        /// </summary>
        public string NameForLaunch { get; set; }

        /// <summary>
        /// Image that will be shown to the user. 
        /// It is converted <see cref="ImageInBytes"/>
        /// </summary>
        public BitmapImage ImageShow => ImageInBytes?.BytesToBitMapImage();

        /// <summary>
        /// Image in bytes can be null, but by default its red circle image
        /// </summary>
        public byte[] ImageInBytes { get; set; } = Image.FromFile(Path.GetFullPath(@"..\..\..\Images\LOGOToute.png"))?.ImageToBytes();

        /// <summary>
        /// Radio box that will indicate to get image from file
        /// </summary>
        public bool FromImageChecked { get; set; } = true;

        /// <summary>
        /// Radio box that will indicate to get image from icon of file
        /// </summary>
        public bool FromIconChecked { get; set; } = false;

        /// <summary>
        /// Indicate if <see cref="ChangeImageAsync"/> is running
        /// </summary>
        public bool ChangeImageIsRunning { get; set; }

        /// <summary>
        /// Final result of getting info about files
        /// </summary>
        public bool SavedSucesfully { get; set; }

        #endregion

        #region Public commands

        /// <summary>
        /// Command to select FromImage RadioBox
        /// </summary>
        public ICommand FromImageSelectedCommand { get; set; }

        /// <summary>
        /// Command to select FromIcon RadioBox
        /// </summary>
        public ICommand FromIconSelectedCommand { get; set; }

        /// <summary>
        /// Command to change image for a launch
        /// </summary>
        public ICommand ChangeImageCommand { get; set; }

        /// <summary>
        /// Command to save current informations
        /// </summary>
        public ICommand SaveMultiLaunchCommand { get; set; }

        /// <summary>
        /// Command to cancel saving a file
        /// </summary>
        public ICommand CancelMultiLaunchCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor that pass window as parameter
        /// </summary>
        /// <param name="window">AddGameWindow</param>
        public AddMultipleFilesDialogViewModel(Window window) : base(window)
        {
            _logger.Info("Start setting up AddMultipleFilesPopupViewModel");

            //Change DropShadowBorderPadding to 10
            DropShadowBorderPadding = new Thickness(10);

            //Change HeaderFontSize to 18, to make capitation height smaller
            HeaderFontSize = 18;

            //Create commands
            CloseCommand = new RelayCommand(() => ClosePopup(window));
            FromImageSelectedCommand = new RelayCommand(() => FromImageSelected());
            FromIconSelectedCommand = new RelayCommand(() => FromIconSelected());
            ChangeImageCommand = new RelayCommand(() => ChangeImageAsync());
            CancelMultiLaunchCommand = new RelayCommand(() => CancelMultiLaunch(window));
            SaveMultiLaunchCommand = new RelayCommand(() => SaveMultiLaunch(window));


            _logger.Info("Done setting up AddMultipleFilesPopupViewModel");

        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Method to save current informations about file
        /// </summary>
        /// <param name="window">Window that will be 
        /// closed at the end (this popup)</param>
        private void SaveMultiLaunch(Window window)
        {
            SavedSucesfully = true;
            ClosePopup(window);
        }

        /// <summary>
        /// Method to cancel saving current informations about file
        /// </summary>
        /// <param name="window">Window that will be 
        /// closed at the end (this popup)</param>
        private void CancelMultiLaunch(Window window)
        {
            SavedSucesfully = false;
            ClosePopup(window);
        }

        /// <summary>
        /// Method to change image of the file
        /// </summary>
        private void ChangeImageAsync()
        {


            if (FromImageChecked)
            {
                var imageInBytes = ImageExtension.GetImageFromPCinBytes();

                if (imageInBytes != null)
                {
                    ImageInBytes = imageInBytes;
                }
            }
            else
            {
                //Open dialog to select image
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();
                if(!string.IsNullOrEmpty(dialog.FileName))
                {
                    Icon icon = Icon.ExtractAssociatedIcon(dialog.FileName);
                    ImageInBytes = icon.IconToBytes();
                }
            }

        }

        /// <summary>
        /// Method to get option to get image from a icon
        /// </summary>
        private void FromIconSelected()
        {
            FromImageChecked = false;
            FromIconChecked = true;
        }

        /// <summary>
        /// Method to get option to get image from a image
        /// </summary>
        private void FromImageSelected()
        {
            FromImageChecked = true;
            FromIconChecked = false;
        }

        /// <summary>
        /// Method that Close Window
        /// </summary>
        /// <param name="window">AddGameWindow</param>
        private void ClosePopup(Window window)
        {
            _logger.Debug("Try to close popup window");

            //Close popup window
            window.Close();

            _logger.Debug("Closed popup window");
        }

        #endregion

    }
}
