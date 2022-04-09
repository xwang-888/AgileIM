using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using AgileIM.IM.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AgileIM.Client.Models
{

    public class UserInfoDto : ObservableObject
    {


        /// <summary>
        /// 唯一ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 用户备注
        /// </summary>
        public string UserNote { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public Image Image { get; set; }

        private MessageDto? _lastMessage;
        private ObservableCollection<MessageDto> _messages = new();
        private bool _isUnreadMessage = true;

        /// <summary>
        /// 是否有未读消息
        /// </summary>
        public bool IsUnreadMessage
        {
            get => _isUnreadMessage;
            set => SetProperty(ref _isUnreadMessage, value);
        }
        /// <summary>
        /// 最后一条消息
        /// </summary>
        public MessageDto? LastMessage
        {
            get => _lastMessage;
            set => SetProperty(ref _lastMessage, value);
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        public ObservableCollection<MessageDto> Messages
        {
            get => _messages;
            set
            {
                SetProperty(ref _messages, value);
                _messages.CollectionChanged += Messages_CollectionChanged;
                LastMessage = value.LastOrDefault();
            }
        }

        private void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            LastMessage = Messages?.LastOrDefault();
        }
    }
}
