using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace AgileIM.Client.Messages
{
    public class LoginMessage : ValueChangedMessage<bool>
    {
        public LoginMessage(bool isOk) : base(isOk) { }
    }
}
