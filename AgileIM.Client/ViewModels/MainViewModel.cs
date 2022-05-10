using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Agile.Client.Service.Services;

using AgileIM.Client.Controls;
using AgileIM.Client.Messages;
using AgileIM.Client.Models;
using AgileIM.Client.Views;
using AgileIM.IM.Models;
using AgileIM.Shared.Models.Users.Dto;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace AgileIM.Client.ViewModels
{
    public class MainViewModel : ObservableObject, IRecipient<UserInfoDto>, IRecipient<string>
    {
        public MainViewModel(IFriendService friendService)
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
        }

        #region Service
        private readonly IFriendService _friendService; 
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
                var list = await _friendService.GetFriendListByUserId(message.Id);
                if (list?.Data is not null)
                    WeakReferenceMessenger.Default.Send(list.Data, "MailListViewModel");
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
}
