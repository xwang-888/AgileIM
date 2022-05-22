using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

using AgileIM.Client.Models;

namespace AgileIM.Client.Controls
{
    public class RichTextBoxEx : RichTextBox
    {

        public RichTextBoxEx()
        {
            KeyDown += RichTextBoxEx_KeyDown;
            DataObject.AddPastingHandler(this, PastingEvent);
            var enterParagraphBreak = new CommandBinding { Command = EditingCommands.EnterParagraphBreak };
            enterParagraphBreak.CanExecute += EnterParagraphBreak_CanExecute;
            CommandBindings.Add(enterParagraphBreak);
        }
        /// <summary>
        /// 分段符将请求插入当前位置达到或超过当前所选内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterParagraphBreak_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is null)
            {
                if (e.Command == EditingCommands.EnterParagraphBreak)
                {

                }
            }
        }
        /// <summary>
        /// 粘贴事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void PastingEvent(object sender, DataObjectPastingEventArgs args)
        {

        }
        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RichTextBoxEx_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                EditingCommands.EnterParagraphBreak.Execute(1, this);
            }
        }

        public static readonly DependencyProperty ChatMsgModelsProperty = DependencyProperty.Register(
            "ChatMsgModels", typeof(List<ChatMsgModel>), typeof(RichTextBoxEx), new PropertyMetadata(default(List<ChatMsgModel>)));
        /// <summary>
        /// 消息集合
        /// </summary>
        public List<ChatMsgModel> ChatMsgModels
        {
            get => (List<ChatMsgModel>)GetValue(ChatMsgModelsProperty);
            set => SetValue(ChatMsgModelsProperty, value);
        }



    }
}
