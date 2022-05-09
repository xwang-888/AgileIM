using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AgileIM.Client.Controls
{
    public class NickTextBlock:Control
    {
        public static readonly DependencyProperty NickProperty = DependencyProperty.Register(
            "Nick", typeof(string), typeof(NickTextBlock), new PropertyMetadata(default(string)));
        /// <summary>
        /// 昵称
        /// </summary>

        public string Nick
        {
            get => (string)GetValue(NickProperty);
            set => SetValue(NickProperty, value);
        }

        public static readonly DependencyProperty UserNoteProperty = DependencyProperty.Register(
            "UserNote", typeof(string), typeof(NickTextBlock), new PropertyMetadata(default(string)));
        /// <summary>
        /// 备注
        /// </summary>

        public string UserNote
        {
            get => (string)GetValue(UserNoteProperty);
            set => SetValue(UserNoteProperty, value);
        }

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            "TextTrimming", typeof(TextTrimming), typeof(NickTextBlock), new PropertyMetadata(default(TextTrimming)));

        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }

    }
}
