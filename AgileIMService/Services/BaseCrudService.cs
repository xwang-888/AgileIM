using AgileIM.Service.Data.UnitOfWork;
using AgileIM.Shared.Models;
using AgileIM.Shared.Models.Users.Entity;

using System.Linq.Expressions;

namespace AgileIM.Service.Services
{
    public class BaseCrudService<T> : IBaseCrudService<T> where T : BaseEntity
    {
        public BaseCrudService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork _unitOfWork;
        public virtual async Task<T?> InsertAsync(T entity)
        {
            var rep = _unitOfWork.GetRepository<T>();
            var model = await rep.InsertAsync(entity);

            return await _unitOfWork.SaveChangesAsync() > 0 ? model.Entity : null;
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            var rep = _unitOfWork.GetRepository<T>();
            rep.Update(entity);
            T? model = null;
            if (await _unitOfWork.SaveChangesAsync() > 0)
                model = await rep.FirstOrDefaultAsync(a => a.Id.Equals(entity.Id));

            return model;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            var rep = _unitOfWork.GetRepository<T>();
            rep.Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

    }
}
