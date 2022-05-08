using Agile.Client.Service.Api;
using Agile.Client.Service.Api.Common;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Friend.Entity;
using AgileIM.Shared.Models.Users.Dto;
using AgileIM.Shared.Models.Users.Entity;

using AutoMapper;

namespace Agile.Client.Service.Services.Impl
{
    public class FriendService : IFriendService
    {
        public FriendService(IMapper mapper)
        {
            _mapper = mapper;
        }

        private readonly IMapper _mapper;

        public async Task<Response<IEnumerable<UserInfoDto>?>> GetFriendListByUserId(string userId)
        {
            var api = new GetFriendListByUserIdApi(userId);
            var response = new Response<IEnumerable<UserInfoDto>?>();
            var result = await api.GetRequest<Response<IEnumerable<Friend>>>();
            if (result is null) return null;

            response.Message = result.Message;
            response.Code = result.Code;
            if (result?.Data is not null)
            {
                response.Data = result.Data.Select(a =>
                {
                    var user = _mapper.Map<User, UserInfoDto>(a.FriendUser);
                    user.UserNote = a.UserNote;
                    return user;
                });
            }

            return response;
        }

        public async Task<Response<string?>> UpdateUserNote(string userId, string friendId, string userNote)
        {
            var api = new UpdateUserNoteApi()
            {
                UserId = userId,
                FriendId = friendId,
                UserNote = userNote
            };
            return await api.GetRequest<Response<string?>>();
        }

        public async Task<Response<Friend?>> AddFriend(string userId, string friendId)
        {
            var api = new AddFriendApi()
            {
                UserId = userId,
                FriendId = friendId
            };
            return await api.GetRequest<Response<Friend?>>();
        }
    }
}
