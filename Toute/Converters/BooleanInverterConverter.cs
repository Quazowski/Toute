using System;
using System.Globalization;

namespace Toute
{
    class BooleanInverterConverter : BaseValueConverter<BooleanInverterConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
