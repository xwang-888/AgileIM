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
        public MenuItemModel() { }

        public MenuItemModel(string name, PackIconKind icon, object content)
        {
            Name = name;
            Icon = icon;
            Content = content;
        }


        private bool _isChecked;

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
    }
}
