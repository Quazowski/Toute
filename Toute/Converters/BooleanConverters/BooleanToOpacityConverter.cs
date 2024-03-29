﻿using System;
using System.Globalization;

namespace Toute
{
    /// <summary>
    /// Converter that convert boolean value, to Double value (opacity)
    /// </summary>
    public class BooleanToOpacityConverter : BaseValueConverter<BooleanToOpacityConverter>
    {
        /// <summary>
        /// Converts a value of boolean type to Double value
        /// </summary>
        /// <param name="value">Double value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Converted value as HorizontalAlignment</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return 0.8;
            else
                return 1;

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
            return null;
        }
    }

    /// <summary>
    /// Converter that convert boolean value, to Double value (opacity)
    /// </summary>
    public class BooleanToFullOpacityConverter : BaseValueConverter<BooleanToOpacityConverter>
    {
        /// <summary>
        /// Converts a value of boolean type to Double value
        /// </summary>
        /// <param name="value">Double value</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Converted value as HorizontalAlignment</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return 0;
            else
                return 1;

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
            return null;
        }
    }
}
