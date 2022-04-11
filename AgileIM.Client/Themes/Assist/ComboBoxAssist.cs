using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgileIM.Client.Themes.Assist
{
    public class ComboBoxAssist
    {

        public static  readonly DependencyProperty DisplayContentProperty = DependencyProperty.RegisterAttached("DisplayContent",typeof(object),typeof(ComboBoxAssist),new FrameworkPropertyMetadata(default(object)));


        public static object GetDisplayContent(DependencyObject element) =>
            (object)element.GetValue(DisplayContentProperty);

        public static void SetDisplayContent(DependencyObject element, object value) =>
            element.SetValue(DisplayContentProperty, value);
    }
}
