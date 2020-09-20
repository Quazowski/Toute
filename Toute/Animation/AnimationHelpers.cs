using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Toute
{
    public static class AnimationHelpers
    {
        public static Storyboard AddSlideFromLeft(this Storyboard storyboard, int width, int seconds = 2, float decelrationRatio = 0.9f, bool keepMargin = true)
        {
            var animation = new ThicknessAnimation
            {
                From = new Thickness(-width, 0, keepMargin ? width : 0, 0),
                To = new Thickness(0),
                Duration = TimeSpan.FromSeconds(seconds),
                DecelerationRatio = decelrationRatio
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        public static Storyboard AddSlideToLeft(this Storyboard storyboard, int width, int seconds = 2, float decelrationRatio = 0.9f, bool keepMargin = true)
        {
            var animation = new ThicknessAnimation
            {
                From = new Thickness(0),
                To = new Thickness(-width, 0, keepMargin ? width : 0, 0),
                Duration = TimeSpan.FromSeconds(seconds),
                DecelerationRatio = decelrationRatio
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        public static Storyboard AddFadeIn(this Storyboard storyboard, int seconds = 2, float decelrationRatio = 0.9f)
        {
            var animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(seconds),
                DecelerationRatio = decelrationRatio
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        public static Storyboard AddFadeOut(this Storyboard storyboard, int seconds = 2, float decelrationRatio = 0.9f)
        {
            var animation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(seconds),
                DecelerationRatio = decelrationRatio
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            storyboard.Children.Add(animation);

            return storyboard;
        }
    }
}
