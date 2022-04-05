using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;

namespace AgileIM.Client.Controls
{
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
        }
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
    }
}
