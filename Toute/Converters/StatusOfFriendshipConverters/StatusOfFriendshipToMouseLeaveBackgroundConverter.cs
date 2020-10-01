using System;
using System.Globalization;
using System.Windows;
using Toute.Core.DataModels;

namespace Toute
{
    /// <summary>
    /// Converter that convert boolean value, to HorizontalAlignment value
    /// </summary>
    public class StatusOfFriendshipToMouseLeaveBackgroundConverter : BaseValueConverter<StatusOfFriendshipToMouseLeaveBackgroundConverter>
    {
        /// <summary>
        /// Converts a value of boolean type to HorizontalAlignment value
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Converted value as HorizontalAlignment</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((StatusOfFriendship)value)
            {
                case StatusOfFriendship.Accepted:
                    return "#0000";
                case StatusOfFriendship.Pending:
                    return "#eee83b";
                case StatusOfFriendship.Blocked:
                    return "#ea3540";
                default:
                    return null;
            }

        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Converted value. If the method returns null, the valid null value is used.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
