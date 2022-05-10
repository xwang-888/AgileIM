using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileIM.Shared.Models.ClientModels.ChatUser.Entity
{
    /// <summary>
    /// 聊天列表模型
    /// </summary>
    [Table("ChatUser")]
    public class ChatUser : BaseEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// 好友Id
        /// </summary>
        [Required]
        public string FriendId { get; set; }

    }
}
