using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Client.Models;
using AgileIM.IM.Models;
using AgileIM.Shared.Models.Users.Dto;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace AgileIM.Client.ChangedMessage;

public class LoginMessage : ValueChangedMessage<bool>
{
    public LoginMessage(bool isOk) : base(isOk) { }
}
public class LoginSuccessMessage : ValueChangedMessage<UserInfoDto>
{
    public LoginSuccessMessage(UserInfoDto userInfo) : base(userInfo) { }
}