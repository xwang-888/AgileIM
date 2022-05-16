using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Agile.Client.Service.Services;
using Agile.Client.Service.Services.Impl;

using AgileIM.Client.Controls;
using AgileIM.Client.Models;
using AgileIM.Client.Properties;
using AgileIM.Client.Views;
using AgileIM.IM.Models;
using AgileIM.Shared.Models.Users.Dto;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

using static AgileIM.Client.ViewModels.Extensions;

namespace AgileIM.Client.ViewModels
{
    public class MainViewModel : ObservableObject, IRecipient<UserInfoDto>, IRecipient<string>
    {
        public MainViewModel(IFriendService friendService, IChatUserService chatUserService, IMessagesService messagesService)
        {
            _friendService = friendService;
            MenuItems = new ObservableCollection<MenuItemModel>()
            {
                new("聊天", PackIconKind.Forum,  new ChatView()),
                new("联系人", PackIconKind.AccountMultiple, new MailListView())
            };
            SelectedMenuItem = MenuItems.First();
            WeakReferenceMessenger.Default.Register<UserInfoDto, string>(this, "MainViewModel");
            WeakReferenceMessenger.Default.Register<string, string>(this, "OpenChatPage");
            _chatUserService = chatUserService;
            _messagesService = messagesService;

        }

        #region Service
        private readonly IFriendService _friendService;
        private readonly IMessagesService _messagesService;
        private readonly IChatUserService _chatUserService;

        #endregion

        #region Property
        private MenuItemModel _selectedMenuItem;
        private ObservableCollection<MenuItemModel> _menuItems;

        public MenuItemModel SelectedMenuItem
        {
            get => _selectedMenuItem;
            set => SetProperty(ref _selectedMenuItem, value);
        }
        public ObservableCollection<MenuItemModel> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }
        private UserInfoDto _user;

        public UserInfoDto User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        #endregion

        #region Recipient
        public void Receive(UserInfoDto message)
        {
            User = message;
            Task.Run(async () =>
            {
                var chatUsers = await _chatUserService.GetChatUsersByUserId(message.Id);
                var list = await _friendService.GetFriendListByUserId(message.Id);
                if (list?.Data is not null)
                {
                    var friendIds = chatUsers?.Select(a => a.FriendId);
                    if (friendIds is not null)
                    {
                        var userInfos = list.Data.Join(friendIds, u => u.Id, cu => cu, (c, cu) => c);
                        var userInfoDtos = userInfos as UserInfoDto[] ?? userInfos.ToArray();
                        var mailList = list.Data.ExceptBy(userInfoDtos.Select(a => a.Id), a => a.Id).ToList();

                        if (userInfoDtos.Any())
                        {
                            var msgUserInfos = await _messagesService.GetChatUsersMessages(message.Id, userInfoDtos);
                            if (msgUserInfos is not null)
                            {
                                mailList.AddRange(msgUserInfos);
                                WeakReferenceMessenger.Default.Send(msgUserInfos, "SetChatUserList");
                                WeakReferenceMessenger.Default.Send(mailList, "MailListViewModel");
                                return;
                            }
                        }
                    }
                    WeakReferenceMessenger.Default.Send(list.Data, "MailListViewModel");
                }
            });
        }

        public void Receive(string message)
        {
            SelectedMenuItem = MenuItems.First();
        }
        #endregion

        ~MainViewModel()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }

    public static class Extensions
    {

        public sealed class DynamicEqualityComparer<T> : IEqualityComparer<T> where T : class
        {
            public DynamicEqualityComparer(Func<T?, T?, bool> func)
            {
                _func = func;
            }
            private readonly Func<T?, T?, bool> _func;

            public bool Equals(T? x, T? y)
            {
                return _func(x, y);
            }

            public int GetHashCode(T obj) => 0;
        }


    }
}
