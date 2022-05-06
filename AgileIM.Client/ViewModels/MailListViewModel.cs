﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Agile.Client.Service.Services;

using AgileIM.Client.Controls;
using AgileIM.Client.Models;
using AgileIM.Client.Views;
using AgileIM.Shared.Models.Friend.Dto;
using AgileIM.Shared.Models.Users.Dto;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace AgileIM.Client.ViewModels
{
    public class MailListViewModel : ObservableObject, IRecipient<IEnumerable<UserInfoDto>>
    {

        public MailListViewModel()
        {
            for (int i = 0; i < 20; i++)
            {
                NewFriendList.Add(new NewFriendDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    Nick = i % 2 == 0 ? $"飞翔的企鹅{i}" : $"自然醒{i}",
                    Message = "能通过一下验证吗？ 我有急事找你!"
                });
            }

            WeakReferenceMessenger.Default.Register(this, "MailListViewModel");
        }

        private readonly IUserService _userService;

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
        public ICommand AddNewFriendCommand => new AsyncRelayCommand(AddNewFriend);


        private Task NewFriend()
        {

            SelectedUserInfo = null;
            IsNewFriend = true;
            return Task.CompletedTask;
        }

        private async Task AddNewFriend()
        {
            DialogHostHelper.ShowDialog(new AddNewFriendView());
        }


        public void Receive(IEnumerable<UserInfoDto> message)
        {
            UserInfoList = new ObservableCollection<UserInfoDto>(message);
        }
    }
}
