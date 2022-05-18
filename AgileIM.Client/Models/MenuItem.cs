using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Client.Controls;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace AgileIM.Client.Models
{
    public class MenuItemModel : ObservableObject
    {
        private int _unreadCount;
        public MenuItemModel() { }

        public MenuItemModel(string name, PackIconKind icon, object content)
        {
            Name = name;
            Icon = icon;
            Content = content;
        }



        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public PackIconKind Icon { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnreadCount
        {
            get => _unreadCount;
            set => SetProperty(ref _unreadCount, value);
        }
    }
}
