using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AgileIM.Client.Controls
{
    public class TextBoxEx : TextBox
    {
        public static readonly DependencyProperty IsFocusProperty = DependencyProperty.Register(
            "IsFocus", typeof(bool), typeof(TextBoxEx), new PropertyMetadata(default(bool), (sender, args) =>
            {
                if (sender is not TextBoxEx tbx) return;
                if ((bool)args.NewValue)
                    tbx.Focus();
              
            }));

        /// <summary>
        /// 是否有焦点
        /// </summary>
        public bool IsFocus
        {
            get => (bool)GetValue(IsFocusProperty);
            set => SetValue(IsFocusProperty, value);
        }
    }
}
