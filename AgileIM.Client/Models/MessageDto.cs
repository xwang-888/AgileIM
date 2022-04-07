using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AgileIM.Client.Models
{
    public class MessageDto
    {
        /// <summary>
        /// 是否为自己发送的消息
        /// </summary>
        public bool IsSelf { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public Image Photo { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 是否阅读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = DateTime.Now;
    }
}
