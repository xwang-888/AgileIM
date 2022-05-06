using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class MainViewModel : ObservableObject, IRecipient<UserInfoDto>
    {
        public MainViewModel(IUserService userService)
        {
            _userService = userService;
            MenuItems = new ObservableCollection<MenuItemModel>()
            {
                new("聊天", PackIconKind.Forum,  new ChatView()),
                new("联系人", PackIconKind.AccountMultiple, new MailListView())
            };
            SelectedMenuItem = MenuItems.First();
            WeakReferenceMessenger.Default.Register(this, "MainViewModel");
        }

        private readonly IUserService _userService;
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

        public async void Receive(UserInfoDto message)
        {
            User = message;

            var list = await _userService.GetFriendListByUserId(message.Id);
            if (list?.Data is not null)
                WeakReferenceMessenger.Default.Send(list.Data, "MailListViewModel");
        }

        ~MainViewModel()
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}
