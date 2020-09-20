using System.Windows;
using System.Windows.Media.Animation;

namespace Toute
{
    public static class AnimationExtensions
    {
        /// <summary>
        /// Animation that will start animation from left to the right
        /// to the right and fade in at the same moment
        /// </summary>
        /// <param name="storyboard">The storyboard on which u want animation</param>
        /// <param name="element">Element that should be animated</param>
        /// <returns></returns>
        public static Storyboard AddSlideAndFadeInFromLeftAnimation(this Storyboard storyboard, FrameworkElement element)
        {
            //Add slide from left to right animation
            storyboard.AddSlideFromLeft((int)element.ActualWidth);

            //Add Fade in Animation
            storyboard.AddFadeIn();

            //Start animating the animation
            storyboard.Begin(element);

            //return the Storyboard
            return storyboard;
        }

        /// <summary>
        /// Animation will start hiding element to the left
        /// and at the same moment fading out him
        /// </summary>
        /// <param name="storyboard">The storyboard on which u want animation</param>
        /// <param name="element">Element that should be animated</param>
        /// <returns></returns>
        public static Storyboard AddSlideAndFadeOutToLeftAnimation(this Storyboard storyboard, FrameworkElement element)
        {

            //Add slide to left
            storyboard.AddSlideToLeft((int)element.ActualWidth);

            //Add Fade out Animation
            storyboard.AddFadeOut();

            //Start animating the animation
            storyboard.Begin(element);

            //return the Storyboard
            return storyboard;
        }
    }
}
