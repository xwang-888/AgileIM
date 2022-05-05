using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Agile.Client.Service.Api.Common;
using Agile.Client.Service.Services;

using AgileIM.Client.Messages;
using AgileIM.Client.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace AgileIM.Client.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        public LoginViewModel(IUserService userService)
        {
            _userService = userService;

            for (int i = 0; i < 3; i++)
            {
                LoginUserInfos.Add(new UserInfoDto { Account = $"YW16_{i}", Nick = $"飞翔的企鹅{i}", Password = "2112313aa" });
            }

            LoginUserInfos[0].Account = "Y12345678_";
            LoginUserInfos[0].Password = "admin123";

            SelectedUserInfo = LoginUserInfos.FirstOrDefault();
        }

        #region Service
        private readonly IUserService _userService;
        #endregion

        #region Property

        private ObservableCollection<UserInfoDto> _loginUserInfos = new();

        public ObservableCollection<UserInfoDto> LoginUserInfos
        {
            get => _loginUserInfos;
            set => SetProperty(ref _loginUserInfos, value);
        }

        private UserInfoDto _selectedUserInfo;

        public UserInfoDto SelectedUserInfo
        {
            get => _selectedUserInfo;
            set => SetProperty(ref _selectedUserInfo, value);
        }
        #endregion

        #region Command
        public ICommand LoginCommand => new AsyncRelayCommand(Login);

        public ICommand RemoveUserAccountCommand => new AsyncRelayCommand<UserInfoDto>(RemoveUserAccount);

        #endregion

        #region Methodes
        private async Task Login()
        {
            var user = await _userService.Login(SelectedUserInfo.Account, SelectedUserInfo.Password);
            if (user is not null && user.Code.Equals((int)StatusCode.Success))
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                WeakReferenceMessenger.Default.Send(new LoginMessage(true));
            }
        }

        private Task RemoveUserAccount(UserInfoDto userInfoDto)
        {
            LoginUserInfos.Remove(userInfoDto);

            SelectedUserInfo = LoginUserInfos.FirstOrDefault() ?? new UserInfoDto();

            return Task.CompletedTask;
        }
        #endregion
    }
}
