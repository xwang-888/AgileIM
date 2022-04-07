using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AgileIM.Client.Themes.Assist
{
    public static class ControlAssist
    {

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ControlAssist), new PropertyMetadata(new CornerRadius(4)));
        public static CornerRadius GetCornerRadius(DependencyObject element) => (CornerRadius)element.GetValue(CornerRadiusProperty);
        public static void SetCornerRadius(DependencyObject element, CornerRadius value) =>
            element.SetValue(CornerRadiusProperty, value);


        public static readonly DependencyProperty HintProperty = DependencyProperty.RegisterAttached("Hint", typeof(string), typeof(ControlAssist), new PropertyMetadata(default(string)));
        public static string GetHint(DependencyObject element) => (string)element.GetValue(HintProperty);
        public static void SetHint(DependencyObject element, string value) => element.SetValue(HintProperty, value);
    }
}
