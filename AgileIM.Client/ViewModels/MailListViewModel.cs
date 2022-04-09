using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Client.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

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
                    UserNote = i % 2 == 0 ? $"用户{i}" : "测试",
                    Signature = "熠熠生辉_"

                });
            }
        }

        private ObservableCollection<UserInfoDto> _userInfoList = new();
        private UserInfoDto _selectedUserInfo;

        public ObservableCollection<UserInfoDto> UserInfoList
        {
            get => _userInfoList;
            set => SetProperty(ref _userInfoList, value);
        }

        public UserInfoDto SelectedUserInfo
        {
            get => _selectedUserInfo;
            set => SetProperty(ref _selectedUserInfo, value);
        }


    }
}
