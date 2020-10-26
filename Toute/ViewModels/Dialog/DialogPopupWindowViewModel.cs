using NLog;
using System.Windows;

namespace Toute
{
    /// <summary>
    /// ViewModel for AddGameWindow
    /// </summary>
    public class DialogPopupWindowViewModel : WindowViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor that pass window as parameter
        /// </summary>
        /// <param name="window">AddGameWindow</param>
        public DialogPopupWindowViewModel(Window window) : base(window)
        {
            _logger.Info("Start setting up DialogPopupWindowViewModel");

            //Change DropShadowBorderPadding to 10
            DropShadowBorderPadding = new Thickness(10);

            //Change HeaderFontSize to 18, to make capitation height smaller
            HeaderFontSize = 18;

            //Change a CloseCommand to close AddGameWindow
            CloseCommand = new RelayCommand(() => ClosePopup(window));

            _logger.Info("Done setting up DialogPopupWindowViewModel");
        }

        #endregion

        #region Helper Methods

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
