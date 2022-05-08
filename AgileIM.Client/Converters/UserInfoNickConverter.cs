using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using AgileIM.Client.Models;
using AgileIM.Shared.Models.Users.Dto;
using Microsoft.IdentityModel.Tokens;

namespace AgileIM.Client.Converters
{
    public class UserInfoNickConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not UserInfoDto userInfoDto) return null;

            return string.IsNullOrEmpty(userInfoDto.UserNote) ? userInfoDto.Nick : userInfoDto.UserNote;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
