using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Toute
{
    /// <summary>
    /// Helpers for storyboard animations
    /// </summary>
    public static class AnimationHelpers
    {

        #region Slide animation

        /// <summary>
        /// Adds animation to storyboard with direction to which should go
        /// and if should appear or hide
        /// </summary>
        /// <param name="storyboard">Storyboard to which add animation</param>
        /// <param name="direction">Direction to which animation should go</param>
        /// <param name="hide">True if animation should hide, false if should appear </param>
        /// <param name="width">Width of the element to move</param>
        /// <param name="seconds">Duration of animation</param>
        /// <param name="decelerationRatio">Deceleration of animation</param>
        /// <param name="keepMargin">True if margin should not be moved, false if whole content should be moved</param>
        public static Storyboard AddSlideAnimation(this Storyboard storyboard, PageAnimation direction, bool hide, int width, float seconds = 0.3f, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // Create a thickness animation
            var animation = new ThicknessAnimation();

            //if block if animation should appear or hide
            //if true animation will hide
            if (hide)
            {
                //switch block determining to which side animation should hide
                switch (direction)
                {
                    //Animation will hide to the left
                    case PageAnimation.LeftSlides:
                        //Add animation
                        animation = new ThicknessAnimation
                        {
                            From = new Thickness(0),
                            To = new Thickness(-width, 0, keepMargin ? width : 0, 0),
                            Duration = TimeSpan.FromSeconds(seconds),
                            DecelerationRatio = decelerationRatio
                        };
                        break;
                    //Animation will hide to the bottom
                    case PageAnimation.BottomSlides:
                        //Add animation
                        animation = new ThicknessAnimation
                        {
                            From = new Thickness(0),
                            To = new Thickness(0, width, 0, keepMargin ? -width : 0),
                            Duration = TimeSpan.FromSeconds(seconds),
                            DecelerationRatio = decelerationRatio
                        };
                        break;
                    //Animation will hide to the right
                    case PageAnimation.RightSlides:
                        //Add animation
                        animation = new ThicknessAnimation
                        {
                            From = new Thickness(0),
                            To = new Thickness(keepMargin ? width : 0, 0, -width, 0),
                            Duration = TimeSpan.FromSeconds(seconds),
                            DecelerationRatio = decelerationRatio
                        };
                        break;
                    //Animation will hide to the top
                    case PageAnimation.TopSlides:
                        //Add animation
                        animation = new ThicknessAnimation
                        {
                            From = new Thickness(0),
                            To = new Thickness(0, -width, 0, keepMargin ? width : 0),
                            Duration = TimeSpan.FromSeconds(seconds),
                            DecelerationRatio = decelerationRatio
                        };
                        break;
                    //Default if any direction was chosen
                    default:
                        break;
                }
            }
            //if false animation will appear on screen
            else
            {
                //switch block determining to which side animation should hide
                switch (direction)
                {
                    //Animation will appear from the left
                    case PageAnimation.LeftSlides:
                        //Add animation
                        animation = new ThicknessAnimation
                        {
                            From = new Thickness(-width, 0, keepMargin ? width : 0, 0),
                            To = new Thickness(0),
                            Duration = TimeSpan.FromSeconds(seconds),
                            DecelerationRatio = decelerationRatio
                        };
                        break;
                    //Animation will appear from the bottom
                    case PageAnimation.BottomSlides:
                        //Add animation
                        animation = new ThicknessAnimation
                        {
                            From = new Thickness(0, width, 0, keepMargin ? -width : 0),
                            To = new Thickness(0),
                            Duration = TimeSpan.FromSeconds(seconds),
                            DecelerationRatio = decelerationRatio
                        };
                        break;
                    //Animation will appear from the right
                    case PageAnimation.RightSlides:
                        //Add animation
                        animation = new ThicknessAnimation
                        {
                            From = new Thickness(keepMargin ? width : 0, 0, -width, 0),
                            To = new Thickness(0),
                            Duration = TimeSpan.FromSeconds(seconds),
                            DecelerationRatio = decelerationRatio
                        };
                        break;
                    //Animation will appear from the top
                    case PageAnimation.TopSlides:
                        //Add animation
                        animation = new ThicknessAnimation
                        {
                            From = new Thickness(0, -width, 0, keepMargin ? width : 0),
                            To = new Thickness(0),
                            Duration = TimeSpan.FromSeconds(seconds),
                            DecelerationRatio = decelerationRatio
                        };
                        break;
                    //Default if any direction was chosen
                    default:
                        break;
                }
            }

            //Adds animation to Storyboard Property
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            //Add animation to storyboard
            storyboard.Children.Add(animation);

            return storyboard;
        }

        #endregion

        #region Fade animations

        /// <summary>
        /// Add fade in or fade out animation to the storyboard
        /// </summary>
        /// <param name="storyboard">Storyboard to which animation should be added</param>
        /// <param name="Vanish">True if animation should Fade Out, false if animation should Fade In</param>
        /// <param name="seconds">Duration of fade effect</param>
        /// <param name="decelerationRatio">deceleration ratio of animation</param>
        /// <returns>returns storyboard</returns>
        public static Storyboard AddFadeAnimation(this Storyboard storyboard, bool Vanish, float seconds = 0.3f, float decelerationRatio = 0.9f)
        {
            //Create double animation
            DoubleAnimation animation;

            //if block, if animation should fade in or out
            //true if animation should fade out
            if (Vanish)
            {
                //Add animation
                animation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(seconds),
                    DecelerationRatio = decelerationRatio
                };
            }
            //false if animation should fade in
            else
            {
                //Add animation
                animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(seconds),
                    DecelerationRatio = decelerationRatio
                };
            }
            //Adds animation to Storyboard Property
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            //Add animation to storyboard
            storyboard.Children.Add(animation);

            //returns storyboard
            return storyboard;
        }

        #endregion

    }
}
