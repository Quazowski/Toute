using System;
using System.Globalization;
using System.Windows;

namespace Toute
{
    /// <summary>
    /// Converter that convert boolean value to margin value. Used in <see cref="ContactPage"/>
    /// to set a margin for a message depending on who sent message.
    /// </summary>
    public class BooleanToMarginConverter : BaseValueConverter<BooleanToMarginConverter>
    {
        /// <summary>
        /// Converts a value of boolean type to new Thickness value
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Converted value as Thickness</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return new Thickness(150, 5, 20, 5);
            else
                return new Thickness(20, 5, 150, 5);

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
