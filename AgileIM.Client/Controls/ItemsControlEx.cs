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
        /// <summary>
        /// 缺省页图片
        /// </summary>
        public ImageSource DefaultPageSource
        {
            get => (ImageSource)GetValue(DefaultPageSourceProperty);
            set => SetValue(DefaultPageSourceProperty, value);
        }

        public static readonly DependencyProperty HintContentProperty = DependencyProperty.Register(
            "HintContent", typeof(object), typeof(ItemsControlEx), new PropertyMetadata(default(object)));
        /// <summary>
        /// 缺省页文字
        /// </summary>
        public object HintContent
        {
            get => (object)GetValue(HintContentProperty);
            set => SetValue(HintContentProperty, value);
        }
    }
}
