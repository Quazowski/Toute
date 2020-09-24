using System.Windows;
using System.Windows.Controls;

namespace Toute
{
    public class NavigationHistoryAttachedProperty : BaseAttachedProperty<NavigationHistoryAttachedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Frame frame))
                return;

            if ((bool)e.NewValue)
            {
                frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

                frame.Navigated += (ss, ee) => ((Frame)ss).NavigationService.RemoveBackEntry();
            }
        }
    }
}
