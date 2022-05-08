using AgileIM.Service.Services.BaseService.Impl;
using AgileIM.Service.Services.UserService;
using AgileIM.Shared.EFCore;
using AgileIM.Shared.EFCore.Data.UnitOfWork;
using AgileIM.Shared.Models.Friend.Entity;
using AgileIM.Shared.Models.Users.Entity;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Service.Services.FriendService.Impl
{
    public class FriendService : BaseCrudService<Friend>, IFriendService
    {

        public FriendService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork _unitOfWork;

        public async Task<IEnumerable<Friend>?> GetFriendListByUserIdAsync(string uId)
        {
            var rep = _unitOfWork.GetRepository<Friend>();
            return await rep.GetAll()
                            .Include(a => a.FriendUser)
                            .Where(a => a.UserId.Equals(uId))
                            .ToListAsync();
        }

        public async Task<bool> DeleteFriendAsync(string uId, string friendId)
        {
            var rep = _unitOfWork.GetRepository<Friend>();
            var model = await rep.FirstOrDefaultAsync(a => a.UserId.Equals(uId) && a.FriendUser.Equals(friendId));

            return model is not null && await base.DeleteAsync(model);
        }

        public async Task<Friend?> UpdateUserNote(string uId, string friendId, string userNote)
        {
            try
            {
                var rep = _unitOfWork.GetRepository<Friend>();
                var model = await rep.FirstOrDefaultAsync(a => a.UserId.Equals(uId) && a.FriendId.Equals(friendId));
                if (model is null) return null;

                model.UserNote = userNote;
                return await base.UpdateAsync(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public Task<bool> ExistFriend(string uId, string friendId)
            => _unitOfWork.GetRepository<Friend>().AnyAsync(a => a.UserId.Equals(uId) && a.FriendId.Equals(friendId));
    }
}
