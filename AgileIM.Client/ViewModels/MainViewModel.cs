using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AgileIM.Client.Models;
using AgileIM.IM.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace AgileIM.Client.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            var messages = new List<MessageDto>();
            var messages1 = new List<MessageDto>();
            for (int i = 0; i < 12; i++)
            {
                messages.Add(new MessageDto() { IsSelf = i % 2 == 0, Text = $"消息发送{i}！！！" });
                messages1.Add(new MessageDto() { IsSelf = i % 2 == 0, Text = $"飞翔的企鹅测试消息发送是否成功{i}！！！" });
            }
            ChatUserList.Add(new UserInfoDto { Id = Guid.NewGuid().ToString(), Account = "xwang1234", Nick = "自然醒12", Gender = 1, Messages = new ObservableCollection<MessageDto>(messages) });

            ChatUserList.Add(new UserInfoDto { Id = Guid.NewGuid().ToString(), Account = "flay1234", Nick = "飞翔的企鹅", Gender = 1, Messages = new ObservableCollection<MessageDto>(messages1) });
        }
        private ObservableCollection<UserInfoDto> _chatUserList = new();

        public ObservableCollection<UserInfoDto> ChatUserList
        {
            get => _chatUserList;
            set => SetProperty(ref _chatUserList, value);
        }

        private UserInfoDto _selectedUserInfo;
        private bool _sendTextIsFocus;

        /// <summary>
        /// 当前选中的user
        /// </summary>
        public UserInfoDto SelectedUserInfo
        {
            get => _selectedUserInfo;
            set
            {
                SetProperty(ref _selectedUserInfo, value);
                SendTextIsFocus = false;
                SendTextIsFocus = true;
                _selectedUserInfo.IsUnreadMessage = false;

            }
        }
        /// <summary>
        /// 发送消息文本框焦点
        /// </summary>
        public bool SendTextIsFocus
        {
            get => _sendTextIsFocus;
            set => SetProperty(ref _sendTextIsFocus, value);
        }


        public ICommand SendMessageCommand => new AsyncRelayCommand(SendMessage);



        private async Task SendMessage()
        {
            foreach (var messageDto in _selectedUserInfo.Messages)
                messageDto.IsRead = true;

        }
    }
}
