using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AgileIM.Service.Data.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        ValueTask<EntityEntry<T>> InsertAsync(T entity);
        Task InsertAsync(params T[] entities);
        void Update(T entity);
        void Delete(T entity);
        void Delete(params T[] entities);
        void Delete(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>>? selector = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        IQueryable<T> GetAll();
        Task<List<T>> SelectAsync(Expression<Func<T, bool>> whereLambda);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null);


    }
}
