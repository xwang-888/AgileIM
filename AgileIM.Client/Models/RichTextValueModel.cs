using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.Models.Enum;

namespace AgileIM.Client.Models
{
    public class ChatMsgModel
    {
        /// <summary>
        /// 聊天消息类型
        /// </summary>
        public ChatMsgType ChatMsgType { get; set; }
        /// <summary>
        /// 具体值
        /// </summary>
        public object Content { get; set; }
    }
}
