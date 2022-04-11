using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AgileIM.Client.Models
{
    /// <summary>
    /// 新朋友验证模型
    /// </summary>
    public class NewFriendDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public Image Photo { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// 验证消息
        /// </summary>
        public string Message { get; set; }
    }
}
