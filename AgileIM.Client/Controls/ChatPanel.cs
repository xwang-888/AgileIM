using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AgileIM.Client.Controls
{
    public class ChatPanel : ItemsControl
    {
        public static readonly DependencyProperty SelfImageProperty = DependencyProperty.Register(
            "SelfImage", typeof(Image), typeof(ChatPanel), new PropertyMetadata(default(Image)));
        public static readonly DependencyProperty OtherProperty = DependencyProperty.Register(
            "OtherImage", typeof(Image), typeof(ChatPanel), new PropertyMetadata(default(Image)));
        public static readonly DependencyProperty OtherNickProperty = DependencyProperty.Register(
            "OtherNick", typeof(string), typeof(ChatPanel), new PropertyMetadata(default(string)));

        /// <summary>
        /// 自己的头像
        /// </summary>
        public Image SelfImage
        {
            get => (Image)GetValue(SelfImageProperty);
            set => SetValue(SelfImageProperty, value);
        }
        /// <summary>
        /// 对方头像
        /// </summary>
        public Image OtherImage
        {
            get => (Image)GetValue(OtherProperty);
            set => SetValue(OtherProperty, value);
        }
        /// <summary>
        /// 对方昵称
        /// </summary>
        public string OtherNick
        {
            get => (string)GetValue(OtherNickProperty);
            set => SetValue(OtherNickProperty, value);
        }

    }
}
