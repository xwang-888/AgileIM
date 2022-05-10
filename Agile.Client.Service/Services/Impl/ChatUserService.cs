using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.EFCore.Data.UnitOfWork;
using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;
using AgileIM.Shared.Models.ClientModels.Message.Entity;
using AgileIM.Shared.Models.Friend.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Agile.Client.Service.Services.Impl
{
    public class ChatUserService : IChatUserService
    {
        public ChatUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork _unitOfWork;

        public async Task<IEnumerable<ChatUser>?> GetChatUsersByUserId(string userId)
        {
            try
            {
                var rep = _unitOfWork.GetRepository<ChatUser>();
                return await rep.GetAll().Where(a => a.UserId.Equals(userId)).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<ChatUser?> InsertAsync(string userId, string friendId)
        {
            var rep = _unitOfWork.GetRepository<ChatUser>();

            var model = await rep.InsertAsync(new ChatUser { FriendId = friendId, UserId = userId });

            return await _unitOfWork.SaveChangesAsync() > 0 ? model.Entity : null;
        }
        public async Task<bool> DeleteAsync(string userId, string friendId)
        {
            var rep = _unitOfWork.GetRepository<ChatUser>();
            var model = await rep.FirstOrDefaultAsync(a => a.UserId.Equals(userId) && a.FriendId.Equals(friendId));
            if (model is null) return false;
            rep.Delete(model);
            var isDel = await _unitOfWork.SaveChangesAsync() > 0;
            if (isDel)
            {
                // 清空消息
                var repMsg = _unitOfWork.GetRepository<Messages>();
                repMsg.Delete(a =>
                   (a.FromId.Equals(userId) && a.TargetId.Equals(friendId)) ||
                   (a.FromId.Equals(friendId) && a.TargetId.Equals(userId)));

                return await _unitOfWork.SaveChangesAsync() > 0;
            }

            return false;
        }

    }
}
