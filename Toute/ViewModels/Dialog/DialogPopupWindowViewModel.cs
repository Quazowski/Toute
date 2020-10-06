using System;
using System.Windows;

namespace Toute
{
    /// <summary>
    /// ViewModel for AddGameWindow
    /// </summary>
    public class DialogPopupWindowViewModel : WindowViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor that pass window as parameter
        /// </summary>
        /// <param name="window">AddGameWindow</param>
        public DialogPopupWindowViewModel(DialogPopup window) : base(window)
        {
            //Change DropShadowBorderPadding to 10
            DropShadowBorderPadding = new Thickness(10);

            //Change HeaderFontSize to 18, to make capitation height smaller
            HeaderFontSize = 18;

            //Change a CloseCommand to close AddGameWindow
            CloseCommand = new RelayCommand(() => ClosePopup(window));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Method that Close Window
        /// </summary>
        /// <param name="window">AddGameWindow</param>
        private void ClosePopup(DialogPopup window)
        {
            //Close window
            window.Close();
        }

        #endregion

    }
}
