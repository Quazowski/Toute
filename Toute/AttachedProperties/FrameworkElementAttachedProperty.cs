using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Toute
{
    /// <summary>
    /// Base animation attached property that help handle animations
    ///     NOTE: Before using <see cref="BaseAnimationAttachedProperty{T}"/>
    ///           make sure u are locking the button etc. that calls this property
    ///           on time duration of animation to prevent flickering of animation
    /// </summary>
    /// <typeparam name="T">Inheriting class</typeparam>
    public abstract class BaseAnimationAttachedProperty<T> : BaseAttachedProperty<T, bool>
        where T : BaseAttachedProperty<T, bool>, new()
    {

        bool IsFirstLoad = false;

        /// <summary>
        /// Fires when the value of attached property is changed
        /// </summary>
        /// <param name="sender">Framework element to animate</param>
        /// <param name="e">Value</param>
        public override async void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        //Sender must be framework element, and new value have to be different that old value
        if (!(sender is FrameworkElement) || ((bool)e.OldValue == (bool)e.NewValue))
            //otherwise return
            return;

            if (IsFirstLoad == false)
            {
                IsFirstLoad = true;

                await DoAnimation((FrameworkElement)sender, (bool)e.NewValue, true);
            }
            else
            {
                //Run a animation
                await DoAnimation((FrameworkElement)sender, (bool)e.NewValue, false);
            }
        }

        /// <summary>
        /// Virtual method that should be overridden, and used on own purpose
        /// </summary>
        /// <param name="element">Element to animate</param>
        /// <param name="vanish">True if element should fade out and move out of the screen</param>
        /// <returns></returns>
        protected virtual async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad) { await Task.Delay(1); }
    }

    /// <summary>
    /// Attached property that are responsible for handling left slide and fade status animations
    /// </summary>
    public class LeftFadeSlideAttachedProperty : BaseAnimationAttachedProperty<LeftFadeSlideAttachedProperty>
    {
        /// <summary>
        /// Left slide animation that fade in or out depends on vanish value
        /// </summary>
        /// <param name="element">Element to animate</param>
        /// <param name="vanish">True if element should fade out and move out of the screen to left</param>
        /// <returns></returns>
        protected override async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad)
        {
            //Fires a slide animation with Fade effect
            await element.AddSlideAndFadeAnimation(PageAnimation.LeftSlides, vanish, keepMargin: false, isFirstLoad:isFirstLoad);
        }
    }

    /// <summary>
    /// Attached property that are responsible for handling top slide and fade status animations
    /// </summary>
    public class TopFadeSlideAttachedProperty : BaseAnimationAttachedProperty<TopFadeSlideAttachedProperty>
    {
        /// <summary>
        /// Top slide animation that fade in or out depends on vanish value
        /// </summary>
        /// <param name="element">Element to animate</param>
        /// <param name="vanish">True if element should fade out and move out of the screen to top</param>
        /// <returns></returns>
        protected override async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad)
        {
            //Fires a slide animation with Fade effect
            await element.AddSlideAndFadeAnimation(PageAnimation.TopSlides, vanish, keepMargin: false, isFirstLoad: isFirstLoad);
        }
    }

    /// <summary>
    /// Attached property that are responsible for handling right slide and fade status animations
    /// </summary>
    public class RightFadeSlideAttachedProperty : BaseAnimationAttachedProperty<RightFadeSlideAttachedProperty>
    {
        /// <summary>
        /// Right slide animation that fade in or out depends on vanish value
        /// </summary>
        /// <param name="element">Element to animate</param>
        /// <param name="vanish">True if element should fade out and move out of the screen to the right</param>
        /// <returns></returns>
        protected override async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad)
        {
            //Fires a slide animation with Fade effect
            await element.AddSlideAndFadeAnimation(PageAnimation.RightSlides, vanish, keepMargin: false, isFirstLoad: isFirstLoad);
        }
    }

    /// <summary>
    /// Attached property that are responsible for handling bottom slide and fade status animations
    /// </summary>
    public class BottomFadeSlideAttachedProperty : BaseAnimationAttachedProperty<BottomFadeSlideAttachedProperty>
    {
        /// <summary>
        /// Left slide animation that fade in or out depends on vanish value
        /// </summary>
        /// <param name="element">Element to animate</param>
        /// <param name="vanish">True if element should fade out and move out of the screen to the bottom</param>
        /// <returns></returns>
        protected override async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad)
        {
            //Fires a slide animation with Fade effect
            await element.AddSlideAndFadeAnimation(PageAnimation.BottomSlides, vanish, keepMargin: false, isFirstLoad: isFirstLoad);
        }
    }
}
