using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Vantage.WPF.Converters
{
    public class ScoreToPassedStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                throw new ArgumentNullException("Score is null");

            if (!(value is int))
                throw new ArgumentException("Score is not in integer type");

            int score = int.Parse(value.ToString());

            return score >= Config.MinimumPassScore ? "Yes" : "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
