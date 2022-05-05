using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using AgileIM.Client.Common;

using Microsoft.Toolkit.Mvvm.Input;

namespace AgileIM.Client.Controls
{
    public class DialogHost : ContentControl
    {
        /// <summary>
        /// 消息容器的暂存字典，通过Token查找到要发送消息的容器
        /// </summary>
        private static readonly Dictionary<string, Panel> PanelDic = new();
        /// <summary>
        /// 容器
        /// </summary>
        private static Panel Panel { get; set; }
        /// <summary>
        /// 记录当前容器所在的窗口
        /// </summary>
        private static Window _parenWindow;

        public static readonly DependencyProperty ControllerTokenProperty = DependencyProperty.RegisterAttached("ControllerToken", typeof(string), typeof(DialogHost), new FrameworkPropertyMetadata(default(string), OnTokenChanged));

        public static string GetControllerToken(DependencyObject element) =>
            (string)element.GetValue(ControllerTokenProperty);
        /// <summary>
        /// 设置唯一标识
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetControllerToken(DependencyObject element, string value) => element.SetValue(ControllerTokenProperty, value);


        private static void OnTokenChanged(DependencyObject sen, DependencyPropertyChangedEventArgs args)
        {
            if (sen is not System.Windows.Controls.Panel panel) return;
            if (args.NewValue is null)
                UnRegister(panel);
            else
                Register(panel, args.NewValue.ToString());
        }

        private static void Register(Panel panel, string token)
        {
            PanelDic.TryAdd(token, panel);
        }

        private static void UnRegister(Panel panel)
        {
            var firstPanel = PanelDic.FirstOrDefault(a => ReferenceEquals(panel, a.Value));
            if (!string.IsNullOrEmpty(firstPanel.Key))
            {
                PanelDic.Remove(firstPanel.Key);
                panel.ContextMenu = null;
            }
        }

        private static Window GetActiveWindow() =>
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

        private static Panel CreateDefaultPanel()
        {
            // 查找当前激活的窗口
            var element = GetActiveWindow();
            _parenWindow = element;
            // 查找当前窗口的装饰器
            var decorator = VisualHelper.GetChild<AdornerDecorator>(element);
            var layer = decorator?.AdornerLayer;
            if (layer is null) return null;
            // 创建容器
            var grid = new Grid() { Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#80000000")) };
            //var scrollViewer = new ScrollViewer()
            //{
            //    HorizontalAlignment = HorizontalAlignment.Stretch,
            //    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
            //    Padding = new Thickness(0),
            //    Content = grid
            //};
            var container = new AdornerContainer(layer)
            {
                Child = grid,
                IsHitTestVisible = true
            };
            layer.Add(container);

            return grid;
        }

        public static void Show(ContentControl contentControl, string token = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (PanelDic.TryGetValue(token, out var panel))
                    {
                        panel?.Children.Add(contentControl);
                    }
                    else
                        PanelAddControl(contentControl);
                }
                else
                    PanelAddControl(contentControl);

                var showStoryboard = new Storyboard();
                var animation = new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 300)));
                Storyboard.SetTarget(animation, contentControl);
                Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));
                showStoryboard.Children.Add(animation);
                showStoryboard.Begin();
            });
        }

        public static void Close(ContentControl contentControl, string token)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var showStoryboard = new Storyboard();
                var animation = new DoubleAnimation(1, 0, new Duration(new TimeSpan(0, 0, 0, 0, 300)));
                Storyboard.SetTarget(animation, contentControl);
                Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));
                showStoryboard.Children.Add(animation);
                showStoryboard.Completed += (sen, e) =>
                {
                    if (token is not null && PanelDic.TryGetValue(token, out var panel))
                    {
                        panel?.Children.Remove(contentControl);
                    }
                    else
                        PanelRemoveControl(contentControl);
                };

                showStoryboard.Begin();
            });
        }


        private static void PanelAddControl(ContentControl contentControl)
        {
            if (_parenWindow is null)
                Panel = CreateDefaultPanel();
            else
            {
                var element = GetActiveWindow();
                if (!ReferenceEquals(_parenWindow, element))
                    Panel = CreateDefaultPanel();
            }
            Panel.Visibility = Visibility.Visible;
            Panel?.Children.Add(contentControl);
        }

        private static void PanelRemoveControl(ContentControl contentControl)
        {
            Panel.Visibility = Visibility.Collapsed;
            Panel.Children.Remove(contentControl);
        }
    }

    public class Dialog : ContentControl
    {
        public Dialog()
        {
            CloseCommand = new RelayCommand(() =>
            {
                DialogHostHelper.CloseDialog(this);
            });
        }


        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(
            "CloseCommand", typeof(ICommand), typeof(Dialog), new PropertyMetadata(default(ICommand)));


        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }
    }

    public class DialogHostHelper
    {
        public static void ShowDialog(ContentControl contentControl, string token = null, bool isDefaultDialog = true)
        {
            var con = contentControl;
            if (isDefaultDialog)
                con = new Dialog()
                { Content = contentControl, Width = contentControl.Width, Height = contentControl.Height };

            DialogHost.Show(con, token);
        }

        public static void CloseDialog(ContentControl contentControl, string token = null)
        {

            DialogHost.Close(contentControl, token);
        }
    }

}
