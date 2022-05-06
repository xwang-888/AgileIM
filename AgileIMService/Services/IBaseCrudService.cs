using AgileIM.Shared.Models;

using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AgileIM.Service.Services
{
    public interface IBaseCrudService<T> where T : BaseEntity
    {
        Task<T?> InsertAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
