using System.Diagnostics;
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
        /// <summary>
        /// Add Slide and fade animation at the same time
        /// </summary>
        /// <param name="storyboard">Storyboard to which animation should be added</param>
        /// <param name="element">Element on which animation should go on</param>
        /// <param name="direction">Direction of animation</param>
        /// <param name="vanish">True if animation should hide, false if should appear</param>
        /// <returns></returns>
        public static async Task AddSlideAndFadeAnimation(this FrameworkElement element, PageAnimation direction, bool vanish, float seconds = 0.3f, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            //Create a storyboard
            Storyboard storyboard = new Storyboard();

            //Add slide to right
            storyboard.AddSlideAnimation(direction, vanish, (int)element.ActualWidth, seconds, decelerationRatio, keepMargin);

            //Add Fade out Animation
            storyboard.AddFadeAnimation(vanish, seconds, decelerationRatio);

            //Start animating the animation
            storyboard.Begin(element);
            
            //return the Storyboard
            await Task.Delay((int)(seconds * 1000));
        }
    }
}
