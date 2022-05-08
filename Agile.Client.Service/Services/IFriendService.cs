using AgileIM.Client.Models;
using AgileIM.Shared.Models.ApiResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileIM.Shared.Models.Friend.Entity;
using AgileIM.Shared.Models.Friend.Request;

namespace Agile.Client.Service.Services
{
    public interface IFriendService
    {
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Response<IEnumerable<UserInfoDto>?>> GetFriendListByUserId(string userId);
        /// <summary>
        /// 修改好友备注
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <param name="userNote"></param>
        /// <returns></returns>
        Task<Response<string?>> UpdateUserNote(string userId, string friendId, string userNote);
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<Response<Friend?>> AddFriend(string userId, string friendId);
    }
}
