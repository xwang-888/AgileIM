using AgileIM.Client.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace AgileIM.Client.Controls
{
    /// <summary>
    /// 全局消息通知
    /// </summary>
    public class MessageTip : Control
    {
        #region MessageTip Control
        private static readonly int DefaultSeconds = 3;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(MessageTip), new PropertyMetadata(default(string)));
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty MessageTipTypeProperty = DependencyProperty.Register(
            "MessageType", typeof(MessageType), typeof(MessageTip), new PropertyMetadata(default(MessageType)));
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType
        {
            get => (MessageType)GetValue(MessageTipTypeProperty);
            set => SetValue(MessageTipTypeProperty, value);
        }

        public static readonly DependencyProperty SecondsProperty = DependencyProperty.Register(
            "Seconds", typeof(int), typeof(MessageTip), new PropertyMetadata(DefaultSeconds));
        /// <summary>
        /// 持续时间(ms)
        /// </summary>
        public int Seconds
        {
            get => (int)GetValue(SecondsProperty);
            set => SetValue(SecondsProperty, value);
        }

        private static readonly ResourceDictionary ResourceDictionary = new() { Source = new Uri("pack://application:,,,/AgileIM.Client;component/Themes/ControlThemes/AgileIM.Client.Themes.Message.xaml", UriKind.Absolute) };
        public async void Close()
        {
            await Task.Delay(new TimeSpan(0, 0, 0, Seconds));

            try
            {
                var storyboard = (Storyboard)ResourceDictionary["MsgClose"];
                storyboard?.Begin(this);
                await Task.Delay(300);
            }
            catch (Exception e)
            {

            }
            finally
            {
                MessagePanel?.Children.Remove(this);
            }
        }


        #endregion

        #region MessageTip Controller
        /// <summary>
        /// 消息容器的暂存字典，通过Token查找到要发送消息的容器
        /// </summary>

        private static readonly Dictionary<string, Panel> PanelDic = new();
        /// <summary>
        /// 消息容器
        /// </summary>
        private static Panel MessagePanel { get; set; }
        /// <summary>
        /// 记录当前消息容器所在的窗口
        /// </summary>
        private static Window _parentWindow;

        public static readonly DependencyProperty ControllerTokenProperty = DependencyProperty.RegisterAttached(
          "ControllerToken", typeof(string), typeof(MessageTip), new PropertyMetadata(default(string), OnTokenChanged));

        public static string GetControllerToken(DependencyObject element) => (string)element.GetValue(ControllerTokenProperty);
        /// <summary>
        /// 设置唯一标识
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetControllerToken(DependencyObject element, string value) => element.SetValue(ControllerTokenProperty, value);
        /// <summary>
        /// Token property value changed.
        /// </summary>
        /// <param name="sen"></param>
        /// <param name="args"></param>
        private static void OnTokenChanged(DependencyObject sen, DependencyPropertyChangedEventArgs args)
        {
            if (sen is not Panel panel) return;

            if (args.NewValue == null)
                UnRegister(panel);
            else
                Register(panel, args.NewValue.ToString());
        }
        /// <summary>
        /// 注销容器
        /// </summary>
        /// <param name="panel"></param>
        private static void UnRegister(Panel panel)
        {
            var firstDic = PanelDic.FirstOrDefault(a => ReferenceEquals(panel, a.Value));
            if (!string.IsNullOrEmpty(firstDic.Key))
            {
                PanelDic.Remove(firstDic.Key);
                panel.ContextMenu = null;
            }
        }
        /// <summary>
        /// 注册容器
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="token"></param>
        private static void Register(Panel panel, string token)
        {
            if (!PanelDic.ContainsKey(token))
                PanelDic.Add(token, panel);
        }
        /// <summary>
        /// 未指定消息容器，创建全局默认消息容器
        /// </summary>
        /// <returns></returns>
        private static Panel CreateDefaultPanel()
        {
            // 查找当前激活的窗口
            var element = GetActiveWindow();
            _parentWindow = element;
            // 查找当前窗口装饰器的容器
            var decorator = VisualHelper.GetChild<AdornerDecorator>(element);
            var layer = decorator?.AdornerLayer;
            if (layer == null) return null;

            // 创建消息容器
            var panel = new StackPanel()
            {
                VerticalAlignment = VerticalAlignment.Top,
            };
            var scrollViewer = new ScrollViewer()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                IsInertiaEnabled = true,
                IsPenetrating = true,
                Padding = new Thickness(20),
                Content = panel
            };

            var container = new AdornerContainer(layer)
            {
                Child = scrollViewer
            };
            layer.Add(container);

            return panel;
        }
        private static Window GetActiveWindow() =>
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        /// <summary>
        /// 全局容器添加消息
        /// </summary>
        /// <param name="msgCtl"></param>
        private static void MessagePanelAddMessage(MessageTip msgCtl)
        {
            if (_parentWindow == null)
                MessagePanel = CreateDefaultPanel();
            else
            {
                var element = GetActiveWindow();
                if (!ReferenceEquals(_parentWindow, element))
                    MessagePanel = CreateDefaultPanel();
            }
            MessagePanel?.Children.Add(msgCtl);
            var scrollViewer = VisualHelper.GetParent<ScrollViewer>(MessagePanel);
            scrollViewer?.ScrollToEnd();
        }
        private static void Show(MessageInfo msgInfo)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                var msgCtl = new MessageTip()
                {
                    Text = msgInfo.Content,
                    MessageType = msgInfo.MessageType,
                    Seconds = msgInfo.Duration,
                };
                if (!string.IsNullOrEmpty(msgInfo.Token))
                {
                    if (PanelDic.TryGetValue(msgInfo.Token, out var panel))
                    {
                        panel?.Children.Add(msgCtl);
                        var scrollViewer = VisualHelper.GetParent<ScrollViewer>(panel);
                        scrollViewer?.ScrollToEnd();
                    }
                    else
                        MessagePanelAddMessage(msgCtl);
                }
                else
                    MessagePanelAddMessage(msgCtl);

                msgCtl.Close();
            });
        }
        /// <summary>
        /// 普通消息提示
        /// </summary>
        /// <param name="content">文字</param>
        /// <param name="duration">持续时间,默认为3ms</param>
        /// <param name="token">容器的唯一标识,默认值为null自动跟踪</param>
        public static void Info(string content, int duration = 3, string token = null) =>
            MessageTip.Show(new MessageInfo() { Content = content, Duration = duration, MessageType = MessageType.Info, Token = token });

        /// <summary>
        /// 成功消息提示
        /// </summary>
        /// <param name="content">文字</param>
        /// <param name="duration">持续时间,默认为3ms</param>
        /// <param name="token">容器的唯一标识,默认值为null自动跟踪</param>
        public static void Success(string content, int duration = 3, string token = null) =>
            MessageTip.Show(new MessageInfo() { Content = content, Duration = duration, MessageType = MessageType.Success, Token = token });

        /// <summary>
        /// 警告消息提示
        /// </summary>
        /// <param name="content">文字</param>
        /// <param name="duration">持续时间,默认为3ms</param>
        /// <param name="token">容器的唯一标识,默认值为null自动跟踪</param>
        public static void Warning(string content, int duration = 3, string token = null) =>
            Show(new MessageInfo() { Content = content, Duration = duration, MessageType = MessageType.Warning, Token = token });

        /// <summary>
        /// 错误消息提示
        /// </summary>
        /// <param name="content">文字</param>
        /// <param name="duration">持续时间,默认为3ms</param>
        /// <param name="token">容器的唯一标识,默认值为null自动跟踪</param>
        public static void Error(string content, int duration = 3, string token = null) =>
            Show(new MessageInfo() { Content = content, Duration = duration, MessageType = MessageType.Error, Token = token });


        #endregion
    }

    public class MessageInfo
    {
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
        public int Duration { get; set; }
        public string Token { get; set; }
    }


    public enum MessageType
    {
        Info,
        Success,
        Warning,
        Error
    }
}
