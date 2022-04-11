using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AgileIM.Client.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace AgileIM.Client.ViewModels
{
    public class MailListViewModel : ObservableObject
    {

        public MailListViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                UserInfoList.Add(new UserInfoDto()
                {
                    Account = $"WxWX{i}{i + 1}{i + 2}",
                    Gender = 1,
                    Nick = $"自然醒{i}",
                    Address = "陕西 西安",
                    UserNote = i % 2 == 0 ? $"用户{i}" : null,
                    Signature = "熠熠生辉_"

                });
                NewFriendList.Add(new NewFriendDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    Nick = i % 2 == 0 ? $"飞翔的企鹅{i}" : $"自然醒{i}",
                    Message = "能通过一下验证吗？ 我有急事找你!"
                });
            }
        }

        private ObservableCollection<UserInfoDto> _userInfoList = new();
        private ObservableCollection<NewFriendDto> _newFriendList = new();
        private UserInfoDto _selectedUserInfo;
        private bool _IsNewFriend;

        public ObservableCollection<UserInfoDto> UserInfoList
        {
            get => _userInfoList;
            set => SetProperty(ref _userInfoList, value);
        }

        public UserInfoDto SelectedUserInfo
        {
            get => _selectedUserInfo;
            set
            {
                SetProperty(ref _selectedUserInfo, value);
                IsNewFriend = false;
            }
        }

        public ObservableCollection<NewFriendDto> NewFriendList
        {
            get => _newFriendList;
            set => SetProperty(ref _newFriendList, value);
        }


        public bool IsNewFriend
        {
            get => _IsNewFriend;
            set => SetProperty(ref _IsNewFriend, value);
        }


        public ICommand NewFriendCommand => new AsyncRelayCommand(NewFriend);


        private Task NewFriend()
        {

            SelectedUserInfo = null;
            IsNewFriend = true;
            return Task.CompletedTask;
        }


    }
}
