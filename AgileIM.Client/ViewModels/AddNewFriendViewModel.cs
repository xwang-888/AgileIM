using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Agile.Client.Service.Services;
using AgileIM.Client.Controls;
using AgileIM.Client.Models;
using AgileIM.Client.Properties;
using AgileIM.Client.Views;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.ClientModels.Message.Dto;
using AgileIM.Shared.Models.ClientModels.Message.Entity;
using AgileIM.Shared.Models.Enum;
using AgileIM.Shared.Models.Users.Dto;
using AgileIM.Shared.Models.Users.Entity;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace AgileIM.Client.ViewModels
{
    public class AddNewFriendViewModel : ObservableObject
    {
        public AddNewFriendViewModel(IUserService userService, IImService imService)
        {
            _userService = userService;
            _imService = imService;
        }

        #region Service
        private readonly IUserService _userService;
        private readonly IImService _imService;
        #endregion

        #region Property

        private string _searchParamText;
        private List<UserInfoDto> _searchUserInfoList;
        private string _verificationMsg;

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchParamText
        {
            get => _searchParamText;
            set => SetProperty(ref _searchParamText, value);
        }
        /// <summary>
        /// 搜索用户列表
        /// </summary>
        public List<UserInfoDto> SearchUserInfoList
        {
            get => _searchUserInfoList;
            set => SetProperty(ref _searchUserInfoList, value);
        }

        public UserInfoDto SelectedUserInfoDto { get; set; }

        public string VerificationMsg
        {
            get => _verificationMsg;
            set => SetProperty(ref _verificationMsg, value);
        }

        #endregion

        #region Command
        public ICommand SearchCommand => new AsyncRelayCommand(SearchAsync);
        public ICommand ShowAddFriendCommand => new RelayCommand<UserInfoDto>(ShowAddFriend);
        public ICommand AddFriendCommand => new AsyncRelayCommand(AddFriendAsync);
        #endregion

        #region Methodes
        /// <summary>
        /// 搜索
        /// </summary>
        private async Task SearchAsync()
        {
            if (string.IsNullOrEmpty(SearchParamText?.Trim()))
            {
                MessageTip.Warning("请输入搜索条件！");
                return;
            }

            var result = await _userService.QueryFriends(SearchParamText);
            if (result is not null && result.Code.Equals(200))
                SearchUserInfoList = new List<UserInfoDto>(result.Data);
            else
                SearchUserInfoList = new List<UserInfoDto>();
        }
        /// <summary>
        /// 打开输入验证消息窗口
        /// </summary>
        /// <param name="userInfoDto"></param>
        public void ShowAddFriend(UserInfoDto userInfoDto)
        {
            // TODO 判断用户是否已经是好友
            if (userInfoDto.Id.Equals(Settings.Default.LoginUser.Id))
            {
                MessageTip.Warning("不能添加自己！");
                return;
            }
            SelectedUserInfoDto = userInfoDto;
            VerificationMsg = $"我是{Settings.Default.LoginUser.Nick}";
            var win = new FriendVerificationWindow() { DataContext = this, Owner = Application.Current.MainWindow };
            win.ShowDialog();

        }
        /// <summary>
        /// 添加好友
        /// </summary>
        private async Task AddFriendAsync()
        {
            var msg = new Messages()
            {
                FromId = Settings.Default.LoginUser.Id,
                TargetId = SelectedUserInfoDto.Id,
                Content = VerificationMsg
            };
            var result = await _imService.SendMessage(new SendMessageModel(MsgCategory.FriendVerification, msg));
            if (result?.Data?.Equals(200) is true)
            {
                // TODO 发送消息成功
            }
            else
            {
                // TODO 发送消息失败
            }

        }
        #endregion



    }
}
