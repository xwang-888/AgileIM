﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;

using AgileIM.Shared.Models.ClientModels.Message.Dto;
using AgileIM.Shared.Models.ClientModels.Message.Entity;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AgileIM.Shared.Models.Users.Dto
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
        private string? _userNote;
        public string? UserNote
        {
            get => _userNote;
            set => SetProperty(ref _userNote, value);
        }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string? Signature { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }
        private MessageDto? _lastMessage;
        private ObservableCollection<MessageDto> _messages;
        private bool _isUnreadMessage = true;
        private int _unreadMsgCount;

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
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnreadMsgCount
        {
            get => _unreadMsgCount;
            set => SetProperty(ref _unreadMsgCount, value);
        }

        private void Messages_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            LastMessage = Messages.LastOrDefault();
            UnreadMsgCount = Messages.Count(a => !a.IsRead);
        }
    }
}
