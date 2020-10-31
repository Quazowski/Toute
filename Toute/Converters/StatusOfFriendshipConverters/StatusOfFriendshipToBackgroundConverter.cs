using System;
using System.Globalization;
using Toute.Core;

namespace Toute
{
    /// <summary>
    /// Converter that convert <see cref="StatusOfFriendship"/> value, to color value
    /// </summary>
    public class StatusOfFriendshipToBackgroundConverter : BaseValueConverter<StatusOfFriendshipToBackgroundConverter>
    {
        /// <summary>
        /// Converts a value of <see cref="StatusOfFriendship"/> type to color value
        /// </summary>
        /// <param name="value">StatusOfFriendship value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Color hash value e.g "#000000"</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((StatusOfFriendship)value) switch
            {
                StatusOfFriendship.Accepted => "#aaaaaa",
                StatusOfFriendship.Pending => "#cabe1c",
                StatusOfFriendship.Blocked => "#c32926",
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
    /// Converter that convert <see cref="StatusOfFriendship"/> to color value
    /// </summary>
    public class StatusOfFriendshipToMouseEnterBackgroundConverter : BaseValueConverter<StatusOfFriendshipToMouseEnterBackgroundConverter>
    {
        /// <summary>
        /// Converter that convert <see cref="StatusOfFriendship"/> to color value
        /// </summary>
        /// <param name="value">StatusOfFriendship value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Color hash value e.g "#000000"</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((StatusOfFriendship)value) switch
            {
                StatusOfFriendship.Accepted => "#999999",
                StatusOfFriendship.Pending => "#d8cc22",
                StatusOfFriendship.Blocked => "#d92430",
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
        /// <returns>Always null.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Converter that convert <see cref="StatusOfFriendship"/> value, to color value
    /// </summary>
    public class StatusOfFriendshipToMouseLeaveBackgroundConverter : BaseValueConverter<StatusOfFriendshipToMouseLeaveBackgroundConverter>
    {
        /// <summary>
        /// Converts a value of <see cref="StatusOfFriendship"/> type to color value
        /// </summary>
        /// <param name="value">StatusOfFriendship value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Color hash value e.g "#000000"</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((StatusOfFriendship)value) switch
            {
                StatusOfFriendship.Accepted => "#aaaaaa",
                StatusOfFriendship.Pending => "#cabe1c",
                StatusOfFriendship.Blocked => "#c32926",
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
