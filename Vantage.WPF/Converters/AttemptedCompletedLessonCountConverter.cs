using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Vantage.Common.Models;

namespace Vantage.WPF.Converters
{
    public class AttemptedCompletedLessonCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "0/0";

            if (value is List<GroupedAttemptsByLesson>)
            {
                List<GroupedAttemptsByLesson> attempts = value as List<GroupedAttemptsByLesson>;
                return $"{attempts.Count(x => x.IsComplete)}/{attempts.Count}";
            }

            return "0/0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
