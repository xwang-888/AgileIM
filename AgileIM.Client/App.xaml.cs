using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Agile.Client.Service.Services;

using AgileIM.Client.Common;
using AgileIM.Client.ViewModels;

using Autofac;

using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace AgileIM.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceProvider.RegisterServiceLocator(ConfigureServices());
            base.OnStartup(e);
        }

        private IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UserService>().As<IUserService>();

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<LoginViewModel>();
            builder.RegisterType<MailListViewModel>();
            builder.RegisterType<ChatViewModel>();


            return builder.Build();
        }
    }
}
