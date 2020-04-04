using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Vantage.WPF.Controls.Switch.Converters
{
    public class MarginDecreaserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double thumbSize = (double)value;
            double decreaseAmount = 4;
            if (parameter != null)
                decreaseAmount = double.Parse(parameter.ToString());

            return thumbSize - decreaseAmount;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 4;
        }
    }
}
