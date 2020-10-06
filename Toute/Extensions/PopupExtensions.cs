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
    }
}
