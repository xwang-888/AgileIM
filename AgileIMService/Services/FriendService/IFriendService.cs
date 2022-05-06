using AgileIM.Service.Services.BaseService;
using AgileIM.Shared.Models.Friend.Entity;
using AgileIM.Shared.Models.Users.Entity;

namespace AgileIM.Service.Services.FriendService
{
    public interface IFriendService : IBaseCrudService<Friend>
    {
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        Task<IEnumerable<Friend>?> GetFriendListByUserIdAsync(string uId);
        Task<bool> DeleteFriendAsync(string uId, string friendId);
    }
}
