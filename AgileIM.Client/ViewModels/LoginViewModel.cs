using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Client.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AgileIM.Client.ViewModels
{
    public class LoginViewModel : ObservableObject
    {

        public LoginViewModel()
        {
            LoginUserInfos = new();
            for (int i = 0; i < 5; i++)
            {
                LoginUserInfos.Add(new UserInfoDto { Account = $"YW16_{i}", Nick = $"飞翔的企鹅{i}" });
            }

            SelectedUserInfo = LoginUserInfos.FirstOrDefault();
        }

        private ObservableCollection<UserInfoDto> _loginUserInfos;

        public ObservableCollection<UserInfoDto> LoginUserInfos
        {
            get => _loginUserInfos;
            set => _loginUserInfos = value;
        }

        private UserInfoDto _selectedUserInfo;

        public UserInfoDto SelectedUserInfo
        {
            get => _selectedUserInfo;
            set => SetProperty(ref _selectedUserInfo, value);
        }


    }
}
