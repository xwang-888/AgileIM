using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using AgileIM.Client.Models;
using AgileIM.Shared.Models.Users.Dto;

namespace AgileIM.Client.Converters
{
    public class UnreadMessageCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ObservableCollection<MessageDto> messageDto) return "";
            var count = messageDto.Count(a => !a.IsRead);
            return count switch
            {
                0 => "",
                > 99 => "99+",
                _ => count
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
