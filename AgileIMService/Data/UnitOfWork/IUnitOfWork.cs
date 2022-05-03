using AgileIM.Service.Data.Repository;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Service.Data.UnitOfWork
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
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        TContext GetDbContext<TContext>() where TContext : DbContext;
        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库中
        /// </summary>
        /// <param name="ensureAutoHistory">用于支持自动记录数据更改历史记录
        /// 将记录所有数据更改历史在一个命名，此表将记录数据，历史记录。
        /// </param>
        /// <returns></returns>
        int SaveChanges(bool ensureAutoHistory = false);
        /// <summary>
        /// 将在此上下文中所做的所有更改保存到基础数据库中（异步）
        /// </summary>
        /// <param name="ensureAutoHistory">用于支持自动记录数据更改历史记录
        /// 将记录所有数据更改历史在一个命名，此表将记录数据，历史记录。
        /// </param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);
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
