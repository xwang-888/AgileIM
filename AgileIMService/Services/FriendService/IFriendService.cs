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
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<bool> DeleteFriendAsync(string uId, string friendId);
        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="friendId"></param>
        /// <param name="userNote"></param>
        /// <returns></returns>
        Task<Friend?> UpdateUserNote(string uId, string friendId, string userNote);
        /// <summary>
        /// 判断好友是否存在
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<bool> ExistFriend(string uId, string friendId);
    }
}
