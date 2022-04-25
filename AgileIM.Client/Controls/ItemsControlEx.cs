using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace AgileIM.Client.Controls
{
    public class ItemsControlEx : ItemsControl
    {
        public static readonly DependencyProperty DefaultPageSourceProperty = DependencyProperty.Register(
            "DefaultPageSource", typeof(ImageSource), typeof(ItemsControlEx), new PropertyMetadata(default(ImageSource)));

        public ImageSource DefaultPageSource
        {
            get => (ImageSource)GetValue(DefaultPageSourceProperty);
            set => SetValue(DefaultPageSourceProperty, value);
        }
    }
}
