using System;
using System.Globalization;
using System.Windows;
using Toute.Core.DataModels;

namespace Toute
{
    /// <summary>
    /// Converter <see cref="StatusOfFriendship"/>boolean value, to Visibility
    /// </summary>
    public class StatusOfAcceptedToVisibilityConverter : BaseValueConverter<StatusOfAcceptedToVisibilityConverter>
    {
        /// <summary>
        /// Converts a value of <see cref="StatusOfFriendship"/> type to Visibility
        /// </summary>
        /// <param name="value">StatusOfFriendship value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Visibility value</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((StatusOfFriendship)value) switch
            {
                StatusOfFriendship.Accepted => Visibility.Visible,
                StatusOfFriendship.Pending => Visibility.Collapsed,
                StatusOfFriendship.Blocked => Visibility.Collapsed,
                _ => null,
            };
        }

        /// <summary>
        /// Converts back value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Always null/returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Converter <see cref="StatusOfFriendship"/>boolean value, to Visibility
    /// </summary>
    public class StatusOfPendingToVisibilityConverter : BaseValueConverter<StatusOfPendingToVisibilityConverter>
    {
        /// <summary>
        /// Converts a value of <see cref="StatusOfFriendship"/> type to Visibility value
        /// </summary>
        /// <param name="value">StatusOfFriendship value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Visibility value</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((StatusOfFriendship)value) switch
            {
                StatusOfFriendship.Accepted => Visibility.Collapsed,
                StatusOfFriendship.Pending => Visibility.Visible,
                StatusOfFriendship.Blocked => Visibility.Collapsed,
                _ => null,
            };
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Always null</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Converter <see cref="StatusOfFriendship"/> value, to Visibility
    /// </summary>
    public class StatusOfBlockToVisibilityConverter : BaseValueConverter<StatusOfBlockToVisibilityConverter>
    {
        /// <summary>
        /// Converts a value of <see cref="StatusOfFriendship"/> type to visibility value
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Visibility value</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((StatusOfFriendship)value) switch
            {
                StatusOfFriendship.Accepted => Visibility.Collapsed,
                StatusOfFriendship.Pending => Visibility.Collapsed,
                StatusOfFriendship.Blocked => Visibility.Visible,
                _ => null,
            };
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Always null</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
