namespace AgileIM.IM.Models
{
    public class Message
    {
        /// <summary>
        /// 发送者ID
        /// </summary>
        public string FromId { get; set; }
        /// <summary>
        /// 发送者唯一ID 多端消息处理
        /// </summary>
        public string FromGuid { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public string SendTime { get; }
            = Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds).ToString();
        /// <summary>
        /// 接收者ID
        /// </summary>
        public string TargetId { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType MsgType { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 心跳
        /// </summary>
        Heartbeat = 0,
        /// <summary>
        /// 单聊
        /// </summary>
        One = 1,
        /// <summary>
        /// 群组
        /// </summary>
        Group = 2
    }
}
