using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

using AgileIM.Client.Models;
using AgileIM.Shared.Models.ClientModels.Message.Dto;
using AgileIM.Shared.Models.Users.Dto;

namespace AgileIM.Client.Converters
{
    public class UnreadMessageCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count switch
                {
                    0 => new { Visibility = Visibility.Collapsed, Count = "0" },
                    > 99 => new { Visibility = Visibility.Visible, Count = "99+" },
                    _ => new { Visibility = Visibility.Visible, Count = count }
                };
            }

            return new { Visibility = Visibility.Collapsed, Count = "0" };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
