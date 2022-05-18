using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.Models.ClientModels.Message.Entity;
using AgileIM.Shared.Models.Enum;

namespace AgileIM.Shared.Models.ClientModels.Message.Dto
{
    public class SendMessageModel
    {

        public SendMessageModel(MsgCategory category, Messages msg)
        {
            Category = category;
            Messages = msg;
        }
        public MsgCategory Category { get; set; }
        public Messages Messages { get; set; }
    }
}
