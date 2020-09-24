using System.Windows;
using System.Windows.Controls;

namespace Toute
{
    /// <summary>
    /// Attached Property, that if set on true, will hide
    /// Navigation bar, and clear history of navigation
    /// </summary>
    public class NavigationHistoryAttachedProperty : BaseAttachedProperty<NavigationHistoryAttachedProperty, bool>
    {
        /// <summary>
        /// Override base <see cref="OnValueChanged(DependencyObject, DependencyPropertyChangedEventArgs)"/>
        /// </summary>
        /// <param name="d">Element on which history will be clear
        /// And it have to be Frame</param>
        /// <param name="e">Value</param>
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //If element is not a frame...
            if (!(d is Frame frame))
                return;

            //If value is set to true...
            if ((bool)e.NewValue)
            {
                //Hide navigation bar
                frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

                //Clear history of navigation
                frame.Navigated += (ss, ee) => ((Frame)ss).NavigationService.RemoveBackEntry();
            }
        }
    }
}
