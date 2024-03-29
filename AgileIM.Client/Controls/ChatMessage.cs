﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AgileIM.Client.Controls
{
    [TemplatePart(Name = TextBlockTemplateName, Type = typeof(TextBlock))]
    [TemplatePart(Name = RichTextBoxTemplateName, Type = typeof(RichTextBox))]
    public class ChatMessage : Control, ICommandSource
    {
        static ChatMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatMessage), new FrameworkPropertyMetadata(typeof(ChatMessage)));
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ChatMessage), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty SendTimeProperty = DependencyProperty.Register(
            "SendTime", typeof(DateTime), typeof(ChatMessage), new PropertyMetadata(default(DateTime)));
        public static readonly DependencyProperty PhotoProperty = DependencyProperty.Register(
            "Photo", typeof(Image), typeof(ChatMessage), new PropertyMetadata(default(Image)));
        public static readonly DependencyProperty IsSelfProperty = DependencyProperty.Register(
            "IsSelf", typeof(bool), typeof(ChatMessage), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty IsSendingProperty = DependencyProperty.Register(
            "IsSending", typeof(bool), typeof(ChatMessage), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(
            "IsError", typeof(bool), typeof(ChatMessage), new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(ChatMessage), new PropertyMetadata(default(ICommand)));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(ChatMessage), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
            "CommandTarget", typeof(IInputElement), typeof(ChatMessage), new PropertyMetadata(default(IInputElement)));
        private const string RichTextBoxTemplateName = "PART_RichTextBox";
        private const string TextBlockTemplateName = "PART_TextBlock";

        private RichTextBox _richTextBox;
        private TextBlock _textBlock;
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime
        {
            get => (DateTime)GetValue(SendTimeProperty);
            set => SetValue(SendTimeProperty, value);
        }
        /// <summary>
        /// 头像
        /// </summary>
        public Image Photo
        {
            get => (Image)GetValue(PhotoProperty);
            set => SetValue(PhotoProperty, value);
        }
        /// <summary>
        /// 是否为自己
        /// </summary>
        public bool IsSelf
        {
            get => (bool)GetValue(IsSelfProperty);
            set => SetValue(IsSelfProperty, value);
        }
        /// <summary>
        /// 是否发送中，显示loading
        /// </summary>
        public bool IsSending
        {
            get => (bool)GetValue(IsSendingProperty);
            set => SetValue(IsSendingProperty, value);
        }
        /// <summary>
        /// 是否发送成功
        /// </summary>
        public bool IsError
        {
            get => (bool)GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }
        /// <summary>
        /// 重新发送
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }
        public override void OnApplyTemplate()
        {
            _textBlock = GetTemplateChild(TextBlockTemplateName) as TextBlock;
            _richTextBox = GetTemplateChild(RichTextBoxTemplateName) as RichTextBox;
            base.OnApplyTemplate();
        }


    }
}
