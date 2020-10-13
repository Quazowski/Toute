using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Toute.DI;

namespace Toute
{
    /// <summary>
    /// Attached property that when page load occurs, and
    /// there is scrollViewer, make this scrollViewer
    /// scroll to bottom after load
    /// </summary>
    public class ScrollToBottomOnLoadAttachedProperty : BaseAttachedProperty<ScrollToBottomOnLoadAttachedProperty, bool>
    {
        /// <summary>
        /// When value of attached property is changed...
        /// </summary>
        /// <param name="d">Object to manipulate with</param>
        /// <param name="e">Changed Value</param>
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //If new value is true...
            if ((bool)e.NewValue)
            {
                //If sent object is not ScrollViewer...
                if (!(d is ScrollViewer scroll))
                    //returns
                    return;

                //Fires, when scrollViewer with page is loaded...
                scroll.Loaded += (sender, e) =>
                {
                    //Scroll to bottom
                    scroll.ScrollToBottom();
                };
            }
        }
    }

    /// <summary>
    /// Attached property that changes value of VerticialOffset
    /// </summary>
    public class ScrollToBottomOnValueChangedAttachedProperty : BaseAttachedProperty<ScrollToBottomOnValueChangedAttachedProperty, bool>
    {
        /// <summary>
        /// When value of attached property is changed...
        /// </summary>
        /// <param name="d">Object to manipulate with</param>
        /// <param name="e">Changed Value</param>
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //If new value is true...
            if ((bool)e.NewValue)
            {
                //If object is not scrollViewer
                if (!(d is ScrollViewer scrollViewer))
                    //Returns
                    return;

                // Scroll content to bottom when context changes
                scrollViewer.DataContextChanged -= Control_DataContextChanged;
                scrollViewer.DataContextChanged += Control_DataContextChanged;

                // Scroll content to bottom when context changes
                scrollViewer.ScrollChanged -= Control_ScrollChanged;
                scrollViewer.ScrollChanged += Control_ScrollChanged;
            }
        }
        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Scroll to bottom
            (sender as ScrollViewer).ScrollToBottom();
        }

        private async void Control_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Get a scroll
            var scroll = sender as ScrollViewer;

            // If we are close enough to the bottom...
            if (scroll.ScrollableHeight - scroll.VerticalOffset < 20)
                // Scroll to the bottom
                scroll.ScrollToEnd();

            //If we are at the very top, and there is more messages...
            if ((scroll.ScrollableHeight == 0 || scroll.VerticalOffset == 0) && ViewModelSideMenu.IsMoreMessages)
            {
                //Scroll a bit to bottom
                scroll.ScrollToVerticalOffset(400);
                //if loading is not running...
                if (!ViewModelSideMenu.LoadMoreMessagesIsRunning)
                    //load more messages
                    await ViewModelSideMenu.LoadMoreMessagesAsync();
            }
        }
    }
}

