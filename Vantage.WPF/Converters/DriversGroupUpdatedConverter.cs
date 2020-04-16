using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using Vantage.Common.Models;
using Vantage.WPF.Models;

namespace Vantage.WPF.Converters
{
    public class DriversGroupUpdatedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return null;

            UpdateDriversGroup updateDriversGroup = new UpdateDriversGroup();
            updateDriversGroup.Driver = values[0] as Driver;
            updateDriversGroup.Group = values[1] as Group;

            return (object)updateDriversGroup;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
