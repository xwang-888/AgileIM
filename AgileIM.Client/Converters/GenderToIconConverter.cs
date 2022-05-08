using AgileIM.Client.Controls;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace AgileIM.Client.Converters
{
    internal class GenderToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gander = (int)value;
            if (gander is 1)
                return new { Kind = PackIconKind.AccountTie, Color = Brushes.DodgerBlue };
            else
                return new { Kind = PackIconKind.AccountTieWoman, Color = Brushes.HotPink };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
