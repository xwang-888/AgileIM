using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AgileIM.Client.Controls;
using AgileIM.Client.Models;
using AgileIM.Client.Views;
using AgileIM.IM.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace AgileIM.Client.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            MenuItems = new ObservableCollection<MenuItemModel>()
            {
                new("聊天", PackIconKind.Forum,  new ChatView()),
                new("联系人", PackIconKind.AccountMultiple, new MailListView())
            };
            SelectedMenuItem = MenuItems.First();
        }
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
        #endregion

    }
}
