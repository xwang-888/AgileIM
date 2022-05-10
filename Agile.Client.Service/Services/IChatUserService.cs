using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Client.Service.Services
{
    public interface IChatUserService
    {
        /// <summary>
        /// 获取正在聊天的好友列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<ChatUser>?> GetChatUsersByUserId(string userId);
        /// <summary>
        /// 插入聊天列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<ChatUser?> InsertAsync(string userId, string friendId);
        /// <summary>
        /// 删除聊天列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string userId, string friendId);
    }
}
