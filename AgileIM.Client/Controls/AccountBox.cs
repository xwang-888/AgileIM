using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AgileIM.Client.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ComboBoxItem))]
    public class AccountBox: Selector
    {
        public static readonly DependencyProperty AccountProperty = DependencyProperty.Register(
            "Account", typeof(string), typeof(AccountBox), new PropertyMetadata(default(string)));

        public string Account
        {
            get => (string)GetValue(AccountProperty);
            set => SetValue(AccountProperty, value);
        }
    }
}
