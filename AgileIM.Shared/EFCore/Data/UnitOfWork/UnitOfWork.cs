using AgileIM.Shared.EFCore.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace AgileIM.Shared.EFCore.Data.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly TContext _dbContext;

        private bool disposed = false;

        private Dictionary<Type, object> Repositories = new();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 清空所有仓库，释放UnitOfWork资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (!disposing) return;
            // 清空仓库
            Repositories?.Clear();
            // 释放dbContext
            _dbContext.Dispose();
        }

        public IRepositoryBase<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!Repositories.ContainsKey(type))
            {
                Repositories[type] = new RepositoryBase<T>(_dbContext);
            }

            return (IRepositoryBase<T>)Repositories[type];
        }

        public TContext GetDbContext<TContext>() where TContext : DbContext
        {
            return _dbContext as TContext;
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess = true)
        {
            return _dbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true)
        {
            return await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

    }
}
