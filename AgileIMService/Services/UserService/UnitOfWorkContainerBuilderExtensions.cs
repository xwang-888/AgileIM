using AgileIM.Service.Data.Repository;
using AgileIM.Service.Data.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Service.Services.UserService
{
    public static class UnitOfWorkContainerBuilderExtensions
    {

        /// <summary>
        /// ContainerBuilder扩展方法 ，注册工作单元
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterUnitOfWork<TContext>(this IServiceCollection serviceCollection) where TContext : DbContext
        {
            return serviceCollection.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        }

        /// <summary>
        /// ContainerBuilder扩展方法 注册存储库
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection RegisterRepository<T, TRepository>(this IServiceCollection serviceCollection)
            where T : class
            where TRepository : class, IRepositoryBase<T>
        {
            return serviceCollection.AddScoped<IRepositoryBase<T>, TRepository>();
        }
    }
}
