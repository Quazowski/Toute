using System;
using System.Globalization;

namespace Toute
{
    /// <summary>
    /// Converter that revert value of boolean value
    /// </summary>
    public class BooleanInverterConverter : BaseValueConverter<BooleanInverterConverter>
    {
        /// <summary>
        /// Converts a boolean value
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Converted value</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //If value is true...
            if ((bool)value)
            {
                //Returns false
                return false;
            }
            //Otherwise, if value is false...
            else
            {
                //Returns true;
                return true;
            }

        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>returns always true</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Retruns true
            return true;
        }
    }
}
