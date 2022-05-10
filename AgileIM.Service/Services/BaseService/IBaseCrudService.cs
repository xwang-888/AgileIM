using AgileIM.Shared.Models;

namespace AgileIM.Service.Services.BaseService
{
    public interface IBaseCrudService<T> where T : BaseEntity
    {
        Task<T?> InsertAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
