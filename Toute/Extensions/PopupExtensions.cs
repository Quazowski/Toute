using System.Windows;
using static Toute.DI;

namespace Toute.Extensions
{
    /// <summary>
    /// Extension for Popup
    /// </summary>
    public static class PopupExtensions
    {
        /// <summary>
        /// Open new DialogPopup with message
        /// </summary>
        /// <param name="Message">Message to display</param>
        public static void NewPopupWithMessage(string Message)
        {
            //Creates new DialogPopup
            var newPopup = new DialogPopup();

            //Sets message of dialog popup...
            newPopup.MainMessage.Text = Message;

            //Show DialogPopup with message
            newPopup.ShowDialog();
        }

        /// <summary>
        /// Popup that should contain information message for a user
        /// appears for short period of time
        /// </summary>
        /// <param name="informationMessage"></param>
        public static void NewInfoPopup(string informationMessage)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                ViewModelApplication.InformationsAndErrors.Add(new InfoControlViewModel
                {
                    Message = informationMessage,
                    IsError = false
                });
            });
        }

        /// <summary>
        /// Popup that should contain error information message for a user
        /// appears for short period of time
        /// </summary>
        public static void NewErrorPopup(string informationMessage)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                ViewModelApplication.InformationsAndErrors.Add(new InfoControlViewModel
                {
                    Message = informationMessage,
                    IsError = true
                });
            });
        }
    }
}
