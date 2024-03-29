﻿using System;
using System.Globalization;
using System.Windows;

namespace Toute
{
    /// <summary>
    /// Converter that convert boolean value, to HorizontalAlignment value
    /// </summary>
    public class BooleanToHorizontalAligmentConverter : BaseValueConverter<BooleanToHorizontalAligmentConverter>
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
                return HorizontalAlignment.Right;
            //Otherwise...
            else
                //Returns
                return HorizontalAlignment.Left;

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
