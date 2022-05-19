using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Agile.Client.Service.Api.Common;
using Agile.Client.Service.Services;

using AgileIM.Client.ChangedMessage;
using AgileIM.Client.Common;
using AgileIM.Client.Controls;
using AgileIM.Client.Models;
using AgileIM.Client.Properties;
using AgileIM.Shared.EFCore.DbContexts;
using AgileIM.Shared.Models.Users.Dto;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace AgileIM.Client.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        public LoginViewModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;

            LoginUserInfos.Add(new UserInfoDto { Account = "Y12345678_", Nick = "Private_", Password = "admin123" });
            LoginUserInfos.Add(new UserInfoDto { Account = "X1987872", Nick = "Patton", Password = "admin123" });
            LoginUserInfos.Add(new UserInfoDto { Account = "WX1987921", Nick = "Venus", Password = "admin123" });

            SelectedUserInfo = LoginUserInfos.FirstOrDefault();
        }

        #region Service
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        #endregion

        #region Property

        private bool _isLoginExec = true;
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
        public ICommand LoginCommand => new AsyncRelayCommand(Login, () => _isLoginExec);

        public ICommand RemoveUserAccountCommand => new AsyncRelayCommand<UserInfoDto>(RemoveUserAccount);

        #endregion

        #region Methodes
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            _isLoginExec = false;
            var user = await _userService.Login(SelectedUserInfo.Account, SelectedUserInfo.Password);
            if (user is not null && user.Code.Equals((int)StatusCode.Success) && user.Data is not null)
            {

                var userInfo = _mapper.Map<LoginUserDto, UserInfoDto>(user.Data);
                Settings.Default.LoginUser = userInfo;
                var dbContext = ServiceProvider.Get<AgileImClientDbContext>();

                #region 判断对应用户文件夹是否存在
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Document", "Files",
                         userInfo.Account);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                #endregion
                #region 判断有无数据库
                dbContext.Database.SetConnectionString($"Data Source={Path.Combine(path, "ChatMsg.db")}");
                await dbContext.Database.EnsureCreatedAsync();
                #endregion
                var mainWindow = new MainWindow();
                mainWindow.Show();
                WeakReferenceMessenger.Default.Send(new LoginMessage(true));
                WeakReferenceMessenger.Default.Send(userInfo, "MainViewModel");
                MessageTip.Success("登录成功！");
            }
            else
            {
                MessageTip.Error(user?.Message ?? "连接服务器失败！");
            }

            _isLoginExec = true;
        }
        /// <summary>
        /// 登录下拉框删除账号信息
        /// </summary>
        /// <param name="userInfoDto"></param>
        /// <returns></returns>
        private Task RemoveUserAccount(UserInfoDto userInfoDto)
        {
            LoginUserInfos.Remove(userInfoDto);

            SelectedUserInfo = LoginUserInfos.FirstOrDefault() ?? new UserInfoDto();

            return Task.CompletedTask;
        }
        #endregion
    }
}
