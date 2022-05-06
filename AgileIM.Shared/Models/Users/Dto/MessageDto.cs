using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AgileIM.Shared.Models.Users.Dto
{
    public class MessageDto : ObservableObject
    {
        private bool _isRead;

        /// <summary>
        /// 是否为自己发送的消息
        /// </summary>
        public bool IsSelf { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否阅读
        /// </summary>
        public bool IsRead
        {
            get => _isRead;
            set => SetProperty(ref _isRead, value);
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = DateTime.Now;
    }
}
