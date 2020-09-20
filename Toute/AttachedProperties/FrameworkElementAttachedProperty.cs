using System.Windows;
using System.Windows.Media.Animation;

namespace Toute
{
    public class SlideFromLeftAttachedProperty : BaseAttachedProperty<SlideFromLeftAttachedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sb = new Storyboard();

            if((bool)e.NewValue == true)
            {
                sb.AddSlideAndFadeInFromLeftAnimation((FrameworkElement)d);

            }
            else
            {
                sb.AddSlideAndFadeOutToLeftAnimation((FrameworkElement)d);
            }

            
        }
    }
}
