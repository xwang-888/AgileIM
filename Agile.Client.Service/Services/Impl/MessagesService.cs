using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgileIM.Shared.EFCore.Data.UnitOfWork;
using AgileIM.Shared.Models.ClientModels.ChatUser.Entity;
using AgileIM.Shared.Models.ClientModels.Message.Dto;
using AgileIM.Shared.Models.ClientModels.Message.Entity;
using AgileIM.Shared.Models.Users.Dto;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace Agile.Client.Service.Services.Impl
{
    public class MessagesService : IMessagesService
    {
        public MessagesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public async Task<IEnumerable<Messages>> GetMessagesByUserId(string userId)
        {
            var rep = _unitOfWork.GetRepository<Messages>();

            return await rep.GetAll().Where(a => a.FromId.Equals(userId)).ToListAsync();
        }
        public async Task<IEnumerable<UserInfoDto>?> GetChatUsersMessages(string userId, IEnumerable<UserInfoDto>? userInfoList)
        {
            if (userInfoList is null) return null;

            var rep = _unitOfWork.GetRepository<Messages>();
            var newUserInfoList = new List<UserInfoDto>(userInfoList.Count());
            foreach (var friend in userInfoList)
            {
                var messages = await rep.GetAll().Where(a => (a.FromId.Equals(userId) && a.TargetId.Equals(friend.Id)) || a.FromId.Equals(friend.Id) && a.TargetId.Equals(userId)).OrderBy(a => a.SendTime).ToListAsync();
                var msgDtoList = messages.Select(msg =>
                 {
                     var msgDto = _mapper.Map<Messages, MessageDto>(msg);
                     if (msg.FromId.Equals(userId))
                     {
                         msgDto.IsSelf = true;
                     }
                     return msgDto;
                 }).ToList();
                friend.Messages = new ObservableCollection<MessageDto>(msgDtoList);
                friend.LastMessage = msgDtoList.LastOrDefault();
                friend.UnreadMsgCount = msgDtoList.Count(a => !a.IsRead);
                newUserInfoList.Add(friend);
            }

            return newUserInfoList.OrderByDescending(a => a.LastMessage?.SendTime ?? a.LastLoginTime);
        }
        public async Task<MessageDto?> SendMessage(Messages message)
        {
            try
            {
                var rep = _unitOfWork.GetRepository<Messages>();
                var model = await rep.InsertAsync(message);
                var result = _mapper.Map<Messages, MessageDto>(model.Entity);
                return await _unitOfWork.SaveChangesAsync() > 0 ? result : null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<bool> UpdateMsgIsReadState(string fromId, string targetId)
        {
            var rep = _unitOfWork.GetRepository<Messages>();
            var result = await _unitOfWork.ExecuteSqlCommandAsync(
                 $"Update Messages SET IsRead=1 WHERE FromId='{fromId}' AND TargetId='{targetId}'");

            return result >= 0;
        }
    }
}
