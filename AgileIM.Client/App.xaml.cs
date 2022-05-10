using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Agile.Client.Service.Services;
using Agile.Client.Service.Services.Impl;

using AgileIM.Client.Common;
using AgileIM.Client.ViewModels;
using AgileIM.Shared.Common.AutoMapper;
using AgileIM.Shared.EFCore.Data.Repository;
using AgileIM.Shared.EFCore.Data.Repository.Client;
using AgileIM.Shared.EFCore.Data.UnitOfWork;
using AgileIM.Shared.EFCore.DbContexts;
using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;

using Autofac;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
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
            #region 判断有无数据库
            var dbContext = ServiceProvider.Get<AgileImClientDbContext>();
            using (dbContext)
            {
                dbContext.Database.EnsureCreated();
            }
            #endregion
            base.OnStartup(e);
        }

        private IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();

            #region DbContext
            builder.Register(c =>
            {
                var dbPath = "Data Source=ChatMsg.db";
                var optionsBuilder = new DbContextOptionsBuilder<AgileImClientDbContext>();
                optionsBuilder.UseSqlite(dbPath);
                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterType<AgileImClientDbContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork<AgileImClientDbContext>>().As<IUnitOfWork>();
            builder.RegisterType<ChatUserRepository>().As<IRepositoryBase<ChatUser>>();
            builder.RegisterType<MessagesRepository>().As<IRepositoryBase<Shared.Models.ClientModels.Message.Entity.Messages>>();

            #endregion

            #region Service
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<FriendService>().As<IFriendService>();
            builder.RegisterType<ChatUserService>().As<IChatUserService>();
            builder.RegisterType<MessagesService>().As<IMessagesService>();

            #endregion

            #region ViewModel

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<LoginViewModel>();
            builder.RegisterType<MailListViewModel>().SingleInstance();
            builder.RegisterType<ChatViewModel>();
            builder.RegisterType<CreateChatViewModel>();
            builder.RegisterType<AddNewFriendViewModel>();

            #endregion

            #region AutoMapper
            var config = new MapperConfiguration(cfg =>
              {
                  cfg.AddProfile<AgileProfile>();
              });

            builder.Register(c => config).AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>();
            #endregion

            return builder.Build();
        }
    }
}
