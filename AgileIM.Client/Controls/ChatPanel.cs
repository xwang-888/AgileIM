using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using AgileIM.Client.Models;
using AgileIM.Shared.Models.Users.Dto;

namespace AgileIM.Client.Controls
{
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    public class ChatPanel : ItemsControl
    {
        public ChatPanel()
        {
            ((INotifyCollectionChanged)Items).CollectionChanged += (sen, e) =>
            {
                _scrollViewer?.ScrollToEnd();
            };
        }


        private const string PartScrollViewer = "PART_ScrollViewer";
        private ScrollViewer _scrollViewer;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(PartScrollViewer) is ScrollViewer scrollViewer)
                _scrollViewer = scrollViewer;
        }

        public static readonly DependencyProperty SelfImageProperty = DependencyProperty.Register(
            "SelfImage", typeof(Image), typeof(ChatPanel), new PropertyMetadata(default(Image)));
        public static readonly DependencyProperty OtherProperty = DependencyProperty.Register(
            "OtherImage", typeof(Image), typeof(ChatPanel), new PropertyMetadata(default(Image)));
        public static readonly DependencyProperty OtherUserInfoProperty = DependencyProperty.Register(
            "OtherUserInfo", typeof(UserInfoDto), typeof(ChatPanel), new PropertyMetadata(default(UserInfoDto)));
        public static readonly DependencyProperty UpdateUserNoteCommandProperty = DependencyProperty.Register(
            "UpdateUserNoteCommand", typeof(ICommand), typeof(ChatPanel), new PropertyMetadata(default(ICommand)));
        public static readonly DependencyProperty ResendCommandProperty = DependencyProperty.Register(
            "ResendCommand", typeof(ICommand), typeof(ChatPanel), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// 重新发送消息
        /// </summary>
        public ICommand ResendCommand
        {
            get => (ICommand)GetValue(ResendCommandProperty);
            set => SetValue(ResendCommandProperty, value);
        }
        /// <summary>
        /// 修改好友备注命令
        /// </summary>
        public ICommand UpdateUserNoteCommand
        {
            get => (ICommand)GetValue(UpdateUserNoteCommandProperty);
            set => SetValue(UpdateUserNoteCommandProperty, value);
        }
        /// <summary>
        /// 自己的头像
        /// </summary>
        public Image SelfImage
        {
            get => (Image)GetValue(SelfImageProperty);
            set => SetValue(SelfImageProperty, value);
        }
        /// <summary>
        /// 对方
        /// </summary>
        public UserInfoDto OtherUserInfo
        {
            get => (UserInfoDto)GetValue(OtherUserInfoProperty);
            set => SetValue(OtherUserInfoProperty, value);
        }
    }
}
