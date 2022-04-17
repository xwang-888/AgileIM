using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AgileIM.Client.Messages;
using AgileIM.Client.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace AgileIM.Client.ViewModels
{
    public class LoginViewModel : ObservableObject
    {

        public LoginViewModel()
        {
            LoginUserInfos = new();
            for (int i = 0; i < 5; i++)
            {
                LoginUserInfos.Add(new UserInfoDto { Account = $"YW16_{i}", Nick = $"飞翔的企鹅{i}", Password = "2112313aa" });
            }

            SelectedUserInfo = LoginUserInfos.FirstOrDefault();
        }

        private ObservableCollection<UserInfoDto> _loginUserInfos;

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


        public ICommand LoginCommand => new AsyncRelayCommand(Login);


        private Task Login()
        {

            var mainWindow = new MainWindow();
            mainWindow.Show();

            WeakReferenceMessenger.Default.Send(new LoginMessage(true));
            return Task.CompletedTask;
        }



    }
}
