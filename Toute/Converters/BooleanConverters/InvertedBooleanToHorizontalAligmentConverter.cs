﻿using System;
using System.Globalization;
using System.Windows;

namespace Toute
{
    /// <summary>
    /// Converter that convert boolean value, to HorizontalAlignment value
    /// it is reversed <see cref="BooleanToHorizontalAligmentConverter"/> converter
    /// </summary>
    public class InvertedBooleanToHorizontalAligmentConverter : BaseValueConverter<InvertedBooleanToHorizontalAligmentConverter>
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
            //If value is true...
            if ((bool)value)
                //Returns 
                return HorizontalAlignment.Left;
            //Otherwise...
            else
                //Returns
                return HorizontalAlignment.Right;

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
