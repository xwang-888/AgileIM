using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Client.Models;
using AgileIM.IM.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AgileIM.Client.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            var messages = new List<MessageDto>();
            for (int i = 0; i < 12; i++)
                messages.Add(new MessageDto() { IsSelf = i % 2 == 0, Text = $"消息发送{i}！！！" });
            ChatUserList.Add(new UserInfoDto { Id = Guid.NewGuid().ToString(), Account = "xwang1234", Nick = "自然醒12", Gender = 1, Messages = new ObservableCollection<MessageDto>(messages) });

            ChatUserList.Add(new UserInfoDto { Id = Guid.NewGuid().ToString(), Account = "flay1234", Nick = "飞翔的企鹅", Gender = 1, Messages = new ObservableCollection<MessageDto>(messages) });
        }
        private ObservableCollection<UserInfoDto> _chatUserList = new();

        public ObservableCollection<UserInfoDto> ChatUserList
        {
            get => _chatUserList;
            set => SetProperty(ref _chatUserList, value);
        }


    }
}
