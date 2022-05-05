using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using AgileIM.Client.Common;
using AgileIM.Client.Controls;
using AgileIM.Client.Messages;
using AgileIM.Client.ViewModels;

using Microsoft.Toolkit.Mvvm.Messaging;

namespace AgileIM.Client.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : CustomWindow
    {
        public LoginWindow()
        {
            InitializeComponent();


            DataContext = ServiceProvider.Get<LoginViewModel>();

            WeakReferenceMessenger.Default.Register<LoginMessage>(this, (obj, msg) =>
            {
                Close();
            });

        }

        protected override void OnClosed(EventArgs e)
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            base.OnClosed(e);
        }
    }
}
