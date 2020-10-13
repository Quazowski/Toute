using System;
using System.Globalization;

namespace Toute
{
    /// <summary>
    /// Converter that convert <see cref="DateTime"/> value to
    /// user friendly date time
    /// </summary>
    public class DateTimeConverter : BaseValueConverter<DateTimeConverter>
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/>
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Converted DateTime value</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if sender is not DateTime return
            if (!(value is DateTime datetime))
                return null;

            //If it is the same day...
            if(datetime.ToUniversalTime().Day == DateTime.UtcNow.Day)
                //Show DateTime like "15:21"
                return datetime.ToShortTimeString();

            //Otherwise show universal date of time
            return datetime.ToUniversalTime();
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>return null</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
