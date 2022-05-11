using AgileIM.Shared.EFCore.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace AgileIM.Shared.EFCore.Data.UnitOfWork
{
    /// <summary>
    /// 为工作单元提供接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 获取 <typeparamref name="T"/>Repository存储库
        /// </summary>
        /// <typeparamref name="T" >实体的类型</typeparamref>
        /// <returns></returns>
        IRepositoryBase<T> GetRepository<T>() where T : class;
        /// <summary>
        /// 获取上下文对象
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        TContext GetDbContext<TContext>() where TContext : DbContext;
        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库中
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">
        ///     Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after the changes have
        ///     been sent successfully to the database.
        /// </param>
        /// <returns></returns>
        int SaveChanges(bool acceptAllChangesOnSuccess = true);
        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库中（异步）
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">
        ///     Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after the changes have
        ///     been sent successfully to the database.
        /// </param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true);
        /// <summary>
        /// 执行原生SQL命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);
        /// <summary>
        /// 执行原生SQL命令（异步）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

    }
}
