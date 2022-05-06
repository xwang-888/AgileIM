using AgileIM.Shared.Models.Users.Entity;

namespace AgileIM.Service.Services.UserService
{
    public interface IFriendService : IBaseCrudService<Friend>
    {
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        Task<IEnumerable<User>?> GetFriendListByUserIdAsync(string uId);
        Task<bool> DeleteFriendAsync(string uId, string friendId);
    }
}
