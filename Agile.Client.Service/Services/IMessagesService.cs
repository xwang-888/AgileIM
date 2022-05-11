using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.Models.ClientModels.Message.Entity;
using AgileIM.Shared.Models.Users.Dto;

namespace Agile.Client.Service.Services
{
    public interface IMessagesService
    {
        Task<IEnumerable<Messages>> GetMessagesByUserId(string userId);
        Task<IEnumerable<UserInfoDto>?> GetChatUsersMessages(string userId, IEnumerable<UserInfoDto>? userInfoList);
        Task<MessageDto?> SendMessage(Messages message);
    }
}
