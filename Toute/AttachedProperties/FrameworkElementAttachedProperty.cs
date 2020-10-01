using System.Threading.Tasks;
using System.Windows;

namespace Toute
{
    #region Base Animation

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

            //If it is first load...
            if (IsFirstLoad == false)
            {
                //Sets first load to true
                IsFirstLoad = true;

                //Do animation with first load property set on true.
                //Element Visibility will be collapsed
                await DoAnimation((FrameworkElement)sender, (bool)e.NewValue, true);
            }
            //Otherwise...
            else
            {
                //Run a animation with first load property set on false.
                await DoAnimation((FrameworkElement)sender, (bool)e.NewValue, false);
            }

        }

        /// <summary>
        /// Abstract method that have to be overridden, and used on own purpose
        /// </summary>
        /// <param name="element">Element to animate</param>
        /// <param name="vanish">True if element should fade out and move out of the screen</param>
        protected abstract Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad);
    }

    #endregion

    #region Slide And Fade Animations

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
        protected override async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad)
        {
            //Fires a slide animation with Fade effect
            await element.AddSlideAndFadeAnimation(PageAnimation.LeftSlides, vanish, keepMargin: false, isFirstLoad: isFirstLoad);
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
        protected override async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad)
        {
            //Fires a slide animation with Fade effect
            await element.AddSlideAndFadeAnimation(PageAnimation.TopSlides, vanish, keepMargin: false, isFirstLoad: false);
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
        protected override async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad)
        {
            //Fires a slide animation with Fade effect
            await element.AddSlideAndFadeAnimation(PageAnimation.BottomSlides, vanish, keepMargin: false, isFirstLoad: isFirstLoad);
        }
    }

    #endregion

    #region Fade Animations

    /// <summary>
    /// Attached property that are responsible for handling fade in or out animation
    /// </summary>
    public class FadeAnimationAttachedProperty : BaseAnimationAttachedProperty<LeftFadeSlideAttachedProperty>
    {
        /// <summary>
        /// Animation that fade in or out depends on vanish value
        /// </summary>
        /// <param name="element">Element to animate</param>
        /// <param name="vanish">True if element should fade out, false if element should fade in</param>
        protected override async Task DoAnimation(FrameworkElement element, bool vanish, bool isFirstLoad)
        {
            //Fires a fade in or out animation
            await element.AddFadeAnimation(vanish);
        }
    }

    #endregion

}
