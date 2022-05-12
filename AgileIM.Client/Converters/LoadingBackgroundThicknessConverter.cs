using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AgileIM.Client.Converters
{
    public class LoadingBackgroundThicknessConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var actualWidth = (value as double?).GetValueOrDefault();
            if (actualWidth == 0)
                return 0;
            return Math.Ceiling(actualWidth / 15);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
