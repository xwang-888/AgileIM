using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;

using AgileIM.Client.Common;
using AgileIM.Client.Models;
using Microsoft.Toolkit.Mvvm.Input;

namespace AgileIM.Client.Controls
{
    [TemplatePart(Name = SystemColorsName, Type = typeof(ItemsControl))]
    public class CustomWindow : Window
    {
        public CustomWindow()
        {

            var chrome = new WindowChrome()
            {
                GlassFrameThickness = new Thickness(1),
                ResizeBorderThickness = new Thickness(4)
            };
            WindowChrome.SetWindowChrome(this, chrome);

            BindingOperations.SetBinding(chrome, WindowChrome.CaptionHeightProperty,
                new Binding(CaptionHeightProperty.Name) { Source = this });
            BindSystemBindings();
            SwitchThemeCommand = new RelayCommand(() =>
            {
                AppColors.Instance.PrimaryThemes = AppColors.Instance.PrimaryThemes.Equals(PrimaryThemes.Dark) ? PrimaryThemes.Light : PrimaryThemes.Dark;
            });
            SwitchColorCommand = new RelayCommand<KeyValuePair<PrimaryColors, string>>(color =>
             {
                 AppColors.Instance.PrimaryColors = color.Key;
             });
        }

        private const string SystemColorsName = "PART_SystemColors";
        private ItemsControl SystemColors;
        private Dictionary<PrimaryColors, string> _systemColors = new()
        {
            { PrimaryColors.DaybreakBlue, "#1890ff" },
            { PrimaryColors.Cyan, "#13c2c2" },
            { PrimaryColors.DustRed, "#f5222d" },
            { PrimaryColors.Lime, "#bae637" },
            { PrimaryColors.SunriseYellow, "#fadb14" },
            { PrimaryColors.SunsetOrange, "#fa541c" },
            { PrimaryColors.CalendulaGold, "#faad14" },
        };
        /// <summary>
        /// 绑定系统命令
        /// </summary>
        private void BindSystemBindings()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sen, e) => { SystemCommands.MinimizeWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sen, e) => { SystemCommands.MaximizeWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sen, e) => { SystemCommands.CloseWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sen, e) => { SystemCommands.RestoreWindow(this); }));
        }

        public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register(
            "CaptionHeight", typeof(double), typeof(CustomWindow), new PropertyMetadata(default(double)));
        /// <summary>
        /// 标题栏高度
        /// </summary>
        public double CaptionHeight
        {
            get => (double)GetValue(CaptionHeightProperty);
            set => SetValue(CaptionHeightProperty, value);
        }

        public static readonly DependencyProperty SwitchThemeCommandProperty = DependencyProperty.Register(
            "SwitchThemeCommand", typeof(ICommand), typeof(CustomWindow), new PropertyMetadata(default(ICommand)));

        public ICommand SwitchThemeCommand
        {
            get => (ICommand)GetValue(SwitchThemeCommandProperty);
            set => SetValue(SwitchThemeCommandProperty, value);
        }

        public static readonly DependencyProperty SwitchColorCommandProperty = DependencyProperty.Register(
            "SwitchColorCommand", typeof(ICommand), typeof(CustomWindow), new PropertyMetadata(default(ICommand)));

        public ICommand SwitchColorCommand
        {
            get => (ICommand)GetValue(SwitchColorCommandProperty);
            set => SetValue(SwitchColorCommandProperty, value);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild(SystemColorsName) is ItemsControl itemsControl)
            {
                SystemColors = itemsControl;
                SystemColors.ItemsSource = _systemColors;
            }
        }

        public static readonly DependencyProperty UserInfoProperty = DependencyProperty.Register(
            "UserInfo", typeof(UserInfoDto), typeof(CustomWindow), new PropertyMetadata(default(UserInfoDto)));

        public UserInfoDto UserInfo
        {
            get => (UserInfoDto)GetValue(UserInfoProperty);
            set => SetValue(UserInfoProperty, value);
        }
    }
}
