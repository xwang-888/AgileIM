using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using AgileIM.Shared.Common.Collections;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AgileIM.Service.Data.Repository
{
    public class RepositoryBase<T> where T : class
    {

        public RepositoryBase(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }

        protected readonly DbContext DbContext;
        protected readonly DbSet<T> DbSet;

        public virtual Task<IPagedList<T>> GetPagedList(Expression<Func<T, bool>>? predicate = null, List<string>? includePaths = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int pageIndex = 0, int pageSize = 20, int indexFrom = 0, bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> queryable = DbSet;
            if (disableTracking)
                /*返回一个新查询，返回的实体将不会缓存在DbContext或ObjectContext中。
                此方法通过调用底层查询对象的AsNoTracking方法来工作。
                如果底层查询对象没有AsNoTracking方法，那么这个方法就不会有任何影响。*/
                queryable = queryable.AsNoTracking();

            if (includePaths is not null)
                foreach (var includePath in includePaths)
                    queryable.Include(includePath);

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            return orderBy is not null ?
                orderBy(queryable).ToPagedListAsync(pageIndex, pageSize, indexFrom, cancellationToken) :
                queryable.ToPagedListAsync(pageIndex, pageSize, indexFrom, cancellationToken);
        }

        public virtual ValueTask<EntityEntry<T>> InsertAsync(T entity)
        {
            return DbSet.AddAsync(entity);
        }

        public virtual Task InsertAsync(params T[] entities)
        {
            return DbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Update(params T[] entities)
        {
            DbSet.UpdateRange(entities);
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
        public virtual void Delete(params T[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            DbSet.RemoveRange(DbContext.Set<T>().Where(predicate));
        }

        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>>? selector = null)
        {
            return selector is null ? DbSet.AnyAsync() : DbSet.AnyAsync(selector);
        }

        public virtual Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate is null ? DbSet.CountAsync() : DbSet.CountAsync(predicate);
        }

        public virtual Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate is null ? DbSet.FirstOrDefaultAsync() : DbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual IQueryable<T> GetAll() => DbSet.AsQueryable();

        public virtual Task<List<T>> SelectAsync(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
