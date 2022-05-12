using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AgileIM.Shared.Models.ClientModels.Message.Dto
{
    public class MessageDto : ObservableObject
    {


        private bool _isRead;
        private bool _isSending;
        private bool _isError;

        public string Id { get; set; }
        /// <summary>
        /// 是否为自己发送的消息
        /// </summary>
        public bool IsSelf { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否阅读
        /// </summary>
        public bool IsRead
        {
            get => _isRead;
            set => SetProperty(ref _isRead, value);
        }
        /// <summary>
        /// 是否正在发送中
        /// </summary>
        public bool IsSending
        {
            get => _isSending;
            set => SetProperty(ref _isSending, value);
        }
        /// <summary>
        /// 是否发送失败
        /// </summary>
        public bool IsError
        {
            get => _isError;
            set => SetProperty(ref _isError, value);
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; } = DateTime.Now;
    }


}
