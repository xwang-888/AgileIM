using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using AgileIM.Shared.Models.Enum;

namespace AgileIM.Shared.Models.ClientModels.Message.Entity
{
    [Table("Messages")]
    public class Messages : BaseEntity
    {
        /// <summary>
        /// 发送者Id
        /// </summary>
        [Required]
        public string FromId { get; set; }
        /// <summary>
        /// 接收者Id
        /// </summary>
        [Required]
        public string TargetId { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 消息状态,是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 消息类型，群聊or单聊
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 消息类别 文字，图片，文件等
        /// </summary>
        public ChatMsgType ChatMsgType { get; set; }

    }
}
