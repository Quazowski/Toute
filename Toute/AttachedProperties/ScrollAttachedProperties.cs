using System.Windows;
using System.Windows.Controls;

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
            if((bool)e.NewValue)
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
        /// Range, on which scroll of ScrollViewer will move to
        /// the bottom if scroll will changed.
        /// </summary>
        private readonly int offset = 70;

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

                //Fires, when scroll of scrollViewer is changed...
                scrollViewer.ScrollChanged += (sender, e) =>
                {
                    //If height of scrollBar is greater than position of scroll, and scroll position plus offset is greater than height of scroll
                    //i.e if scroll is on the bottom of scroll range, and e.g new message comes, scroll on bottom
                    if ((scrollViewer.VerticalOffset < scrollViewer.ScrollableHeight) && (scrollViewer.ScrollableHeight < scrollViewer.VerticalOffset + offset))
                    {
                        //Scrolls to the very bottom
                        scrollViewer.ScrollToBottom();
                    }
                    //If Height of scrollBar and position of scroll plus scroll value changed,
                    //i.e scroll is on the very bottom, and user want to scroll up...
                    if ((scrollViewer.ScrollableHeight == (scrollViewer.VerticalOffset + (-e.VerticalChange))))
                    {
                        //Break auto scrolling to bottom, and scroll a bit more to up
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset - 1);
                    }
                };
            }
        }
    }
}
