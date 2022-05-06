using AgileIM.Client.Models;
using AgileIM.Shared.Models.ApiResult;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
