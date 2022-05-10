using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using AgileIM.Client.Common;
using AgileIM.Client.Controls;
using AgileIM.Shared.Models.Users.Dto;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace AgileIM.Client.ViewModels
{
    public class CreateChatViewModel : ObservableObject
    {
        public CreateChatViewModel()
        {
            var mailListVm = ServiceProvider.Get<MailListViewModel>();
            SourceUserInfoList = new List<UserInfoDto>(mailListVm.UserInfoList);
            BindUserInfoList = new ObservableCollection<UserInfoDto>(SourceUserInfoList);
        }

        #region Property
        private ObservableCollection<UserInfoDto> _bindUserInfoList;
        private List<UserInfoDto> _sourceUserInfoList;

        private string? _searchParamText;

        public ObservableCollection<UserInfoDto> BindUserInfoList
        {
            get => _bindUserInfoList;
            set => SetProperty(ref _bindUserInfoList, value);
        }

        public List<UserInfoDto> SourceUserInfoList
        {
            get => _sourceUserInfoList;
            set => SetProperty(ref _sourceUserInfoList, value);
        }

        public string? SearchParamText
        {
            get => _searchParamText;
            set
            {
                SetProperty(ref _searchParamText, value);
                SearchUsers();
            }
        }
        #endregion

        #region Command
        public ICommand CreateChatCommand => new AsyncRelayCommand<IEnumerable<UserInfoDto>>(CreateChat);
        #endregion

        #region Methods
        /// <summary>
        /// 创建聊天
        /// </summary>
        /// <param name="userInfos"></param>
        /// <returns></returns>
        public Task CreateChat(IEnumerable<UserInfoDto>? userInfos)
        {
            if (userInfos is null) return Task.CompletedTask;
            foreach (var userInfoDto in userInfos)
            {
                WeakReferenceMessenger.Default.Send(userInfoDto, "ChatViewModel");
            }
            WeakReferenceMessenger.Default.Send("", "OpenChatPage");
            return Task.CompletedTask;
        }
        /// <summary>
        /// 搜索用户
        /// </summary>
        public void SearchUsers()
        {
            if (SearchParamText is null || string.IsNullOrEmpty(SearchParamText?.Trim()))
            {
                BindUserInfoList = new ObservableCollection<UserInfoDto>(SourceUserInfoList);
                return;
            }

            var param = SearchParamText.Trim();
            var result = SourceUserInfoList.Where(a =>
                 (a.UserNote?.Contains(param, StringComparison.OrdinalIgnoreCase) is true) || a.Nick.Contains(param, StringComparison.OrdinalIgnoreCase) || a.Account.Contains(param, StringComparison.OrdinalIgnoreCase));

            BindUserInfoList = new ObservableCollection<UserInfoDto>(result);
        }
        #endregion
    }
}
