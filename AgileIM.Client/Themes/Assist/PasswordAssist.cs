using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.IdentityModel.Tokens;

namespace AgileIM.Client.Themes.Assist
{
    public class PasswordAssist
    {

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordAssist), new PropertyMetadata(default(string), OnPasswordChanged));



        public static string GetPassword(DependencyObject element) => (string)element.GetValue(PasswordProperty);

        public static void SetPassword(DependencyObject element, string value) => element.SetValue(PasswordProperty, value);

        private static void OnPasswordChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            if (o is not PasswordBox passwordBox) return;

            if (args.NewValue is null) passwordBox.Password = null;
            if (args.NewValue is not string value) return;
            if (!value.Equals(passwordBox.Password))
                passwordBox.Password = value;
        }

        public static readonly DependencyProperty IsShowPwdBtnVisibleProperty = DependencyProperty.RegisterAttached("IsShowPwdBtnVisible", typeof(bool), typeof(PasswordAssist), new PropertyMetadata(true));

        public static bool GetIsShowPwdBtnVisible(DependencyObject element) => (bool)element.GetValue(IsShowPwdBtnVisibleProperty);

        public static void SetIsShowPwdBtnVisible(DependencyObject element, bool value) =>
            element.SetValue(IsShowPwdBtnVisibleProperty, value);



        internal static readonly DependencyProperty PasswordHookProperty = DependencyProperty.RegisterAttached("PasswordHook", typeof(bool), typeof(PasswordAssist), new PropertyMetadata(OnPasswordHookChanged));

        internal static bool GetPasswordHook(DependencyObject element) => (bool)element.GetValue(PasswordHookProperty);

        internal static void SetPasswordHook(DependencyObject element, bool value) => element.SetValue(PasswordHookProperty, value);


        private static void OnPasswordHookChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            if (o is not PasswordBox passwordBox) return;
            if (!passwordBox.Password.IsNullOrEmpty())
                SetPassword(passwordBox, passwordBox.Password);
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;


        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is not PasswordBox passwordBox) return;
            SetPassword(passwordBox, passwordBox.Password);

        }
    }
}
