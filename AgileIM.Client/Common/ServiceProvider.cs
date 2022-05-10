using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

namespace AgileIM.Client.Common
{
    public class ServiceProvider
    {
        private static IContainer Instance { get; set; }
        /// <summary>
        /// 注册服务Locator
        /// </summary>
        /// <param name="container"></param>
        public static void RegisterServiceLocator(IContainer container)
        {
            Instance ??= container;
        }
        /// <summary>
        /// 通过组件名称获取提供服务的组件实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static T Get<T>(string typeName)
            => Instance.IsRegisteredWithName<T>(typeName) ?
                Instance.ResolveNamed<T>(typeName) :
                default(T);
        /// <summary>
        /// 获取提供服务的组件实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : class
        {
            if (Instance is null || !Instance.IsRegistered<T>()) return default(T);

            return Instance.Resolve<T>();
        }
    }
}
