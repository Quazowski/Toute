using NLog;
using System;
using System.Windows;
using System.Windows.Input;

namespace Toute
{
    public class NewNameDialogViewModel : WindowViewModel
    {
        #region Private members

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public members

        public string NewName { get; set; }

        #endregion

        #region Public commands

        public ICommand CancelSetNewNameCommand { get; set; }
        public ICommand SetNewNameCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor that pass window as parameter
        /// </summary>
        /// <param name="window">AddGameWindow</param>
        public NewNameDialogViewModel(Window window) : base(window)
        {
            _logger.Info("Start setting up DialogPopupWindowViewModel");

            //Change DropShadowBorderPadding to 10
            DropShadowBorderPadding = new Thickness(10);

            //Change HeaderFontSize to 18, to make capitation height smaller
            HeaderFontSize = 18;

            //Change a CloseCommand to close AddGameWindow
            CloseCommand = new RelayCommand(() => ClosePopup(window));
            CancelSetNewNameCommand = new RelayCommand(() => CancelSetNewName(window));
            SetNewNameCommand = new RelayCommand(() => ClosePopup(window));

            _logger.Info("Done setting up DialogPopupWindowViewModel");
        }

        private void CancelSetNewName(Window window)
        {
            NewName = "";

            ClosePopup(window);
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
