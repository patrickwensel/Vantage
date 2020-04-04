using System;
using System.Globalization;
using System.Windows.Data;

namespace Vantage.WPF.Converters
{
    public class BoolToOnOffConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is bool))
                return "Off";

            bool onOff = (bool)value;
            return onOff ? "On" : "Off";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
