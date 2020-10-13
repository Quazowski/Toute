using NLog;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Toute
{
    /// <summary>
    /// Animation Extensions for storyboard animation
    /// </summary>
    public static class AnimationExtensions
    {
        #region Private members

        /// <summary>
        /// Private logger for <see cref="AnimationExtensions"/>
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Extensions

        /// <summary>
        /// Add Slide and Fade animation to the given element
        /// </summary>
        /// <param name="element">The element on which animation will happen</param>
        /// <param name="direction">Direction of the slide animation</param>
        /// <param name="vanish">True if animation should slide to the given direction, and fade out.
        /// False if animation should slide from given direction and Fade In</param>
        /// <param name="isFirstLoad">If it is first animation, element Visibility will be collapsed</param>
        /// <param name="seconds">Duration of animation in seconds</param>
        /// <param name="decelerationRatio">Deceleration ratio of animation</param>
        /// <param name="keepMargin">True if margin should stay. False if whole content should move</param>
        public static async Task AddSlideAndFadeAnimation(this FrameworkElement element, PageAnimation direction,
            bool vanish, bool isFirstLoad = false, float seconds = 0.3f, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            var statusOfVanishAsString = vanish ? "hide" : "appear";
            _logger.Trace($"Firing Slide and fade animation of Direction: {direction}, and animation is about to {statusOfVanishAsString}");

            //Creates a storyboard
            Storyboard storyboard = new Storyboard();

            //Set width or height
            int widthOrHeight;

            //If the element ActualWidth is 0, for example on start of application
            //use element.Width property instead of element.ActualWith
            //Also if element should slide bottom/top manage his height,
            //instead only width
            if (direction == PageAnimation.BottomSlides || direction == PageAnimation.TopSlides)
            {
                widthOrHeight = (int)element.ActualHeight == 0 ? (int)element.Height : (int)element.ActualHeight;
            }
            else
            {
                widthOrHeight = (int)element.ActualWidth == 0 ? (int)element.Width : (int)element.ActualWidth;
            }

            //Adds slide animation to right. 
            storyboard.AddSlideAnimation(direction, vanish, widthOrHeight,
                seconds, decelerationRatio, keepMargin);

            //Adds Fade out Animation
            storyboard.AddFadeAnimation(vanish, seconds, decelerationRatio);

            //Sets the element Visibility to Visible before
            //doing the animation
            element.Visibility = Visibility.Visible;

            //Start animating the animation
            storyboard.Begin(element);

            //If it is first load, set element Visibility to collapsed
            //to prevent flickering on start of application
            if (isFirstLoad)
                element.Visibility = Visibility.Collapsed;

            //Wait for animation to happen before
            //doing anything
            await Task.Delay((int)(seconds * 1000));

            _logger.Trace($"Slide and fade animation of Direction: {direction}, and {statusOfVanishAsString} value is done");
        }

        /// <summary>
        /// Add Slide animation to the given element
        /// </summary>
        /// <param name="element">The element on which animation will happen</param>
        /// <param name="direction">Direction of the slide animation</param>
        /// <param name="vanish">True if animation should slide to the given direction.
        /// False if animation should slide from given direction.</param>
        /// <param name="isFirstLoad">If it is first animation, element Visibility will be collapsed</param>
        /// <param name="seconds">Duration of animation in seconds</param>
        /// <param name="decelerationRatio">Deceleration ratio of animation</param>
        /// <param name="keepMargin">True if margin should stay. False if whole content should move</param>
        public static async Task AddSlideAnimation(this FrameworkElement element, PageAnimation direction, bool vanish,
            bool isFirstLoad = false, float seconds = 0.3f, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            var statusOfVanishAsString = vanish ? "hide" : "appear";
            _logger.Trace($"Firing Slide of Direction: {direction}, and animation is about to {statusOfVanishAsString}");

            //Create a storyboard
            Storyboard storyboard = new Storyboard();

            //Set width or height
            int widthOrHeight;

            //If the element ActualWidth is 0, for example on start of application
            //use element.Width property instead of element.ActualWith
            //Also if element should slide bottom/top manage his height,
            //instead only width
            if (direction == PageAnimation.BottomSlides || direction == PageAnimation.TopSlides)
            {
                widthOrHeight = (int)element.ActualHeight == 0 ? (int)element.Height : (int)element.ActualHeight;
            }
            else
            {
                widthOrHeight = (int)element.ActualWidth == 0 ? (int)element.Width : (int)element.ActualWidth;
            }


            //Adds slide animation to right. 
            storyboard.AddSlideAnimation(direction, vanish, widthOrHeight,
                seconds, decelerationRatio, keepMargin);

            //Sets the element Visibility to Visible before
            //doing the animation
            element.Visibility = Visibility.Visible;

            //Start animating the animation
            storyboard.Begin(element);

            //If it is first load, set element Visibility to collapsed
            //to prevent flickering on start of application
            if (isFirstLoad)
                element.Visibility = Visibility.Collapsed;

            //Wait for animation to happen before
            //doing anything
            await Task.Delay((int)(seconds * 1000));

            _logger.Trace($"Slide animation of Direction: {direction}, and {statusOfVanishAsString} value is done");
        }

        /// <summary>
        /// Add Fade animation to the given element
        /// </summary>
        /// <param name="element">The element on which animation will happen</param>
        /// <param name="vanish">True if animation should fade out.
        /// False if animation should Fade In</param>
        /// <param name="isFirstLoad">If it is first animation, element Visibility will be collapsed</param>
        /// <param name="seconds">Duration of animation in seconds</param>
        /// <param name="decelerationRatio">Deceleration ratio of animation</param>
        public static async Task AddFadeAnimation(this FrameworkElement element, bool vanish, bool isFirstLoad = false, float seconds = 0.3f,
            float decelerationRatio = 0.9f)
        {
            var statusOfVanishAsString = vanish ? "hide" : "appear";
            _logger.Trace($"Firing fade animation, and is about to {statusOfVanishAsString}");

            //Creates a storyboard
            Storyboard storyboard = new Storyboard();

            //Adds Fade out Animation
            storyboard.AddFadeAnimation(vanish, seconds, decelerationRatio);

            //Sets the element Visibility to Visible before
            //doing the animation
            element.Visibility = Visibility.Visible;

            //Start animating the animation
            storyboard.Begin(element);

            //If it is first load, set element Visibility to collapsed
            //to prevent flickering on start of application
            if (isFirstLoad)
                element.Visibility = Visibility.Collapsed;

            //Wait for animation to happen before
            //doing anything
            await Task.Delay((int)(seconds * 1000));

            //If element is about to hide, set Visibility to Collapsed
            if (vanish)
                element.Visibility = Visibility.Collapsed;

            _logger.Trace($"Fade animation of value {statusOfVanishAsString} is done");
        }

        #endregion

    }
}
