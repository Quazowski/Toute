using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Toute
{
    public abstract class BaseAnimationAttachedProperty<T> : BaseAttachedProperty<T, bool> 
        where T : BaseAttachedProperty<T, bool>, new()
    {
        Dictionary<DependencyObject, bool>
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Storyboard sb = new Storyboard();

            DoAnimation(sb, (FrameworkElement)d, (bool)e.NewValue);
        }

        protected virtual void DoAnimation(Storyboard sb, FrameworkElement element, bool vanish) { }
    }
    public class LeftSlideAttachedProperty : BaseAnimationAttachedProperty<LeftSlideAttachedProperty>
    {
        protected override async void DoAnimation(Storyboard sb, FrameworkElement element, bool vanish)
        {
            await sb.AddSlideAndFadeAnimation(element, PageAnimation.LeftSlides, vanish);
        }
    }
}
