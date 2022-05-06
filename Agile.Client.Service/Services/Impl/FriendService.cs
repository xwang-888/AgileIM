using Agile.Client.Service.Api;
using Agile.Client.Service.Api.Common;
using AgileIM.Client.Models;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Friend.Entity;
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
            var api = new GetFriendListByUserIdApi { UserId = userId };
            var response = new Response<IEnumerable<UserInfoDto>?>();
            var result = await api.GetRequest<Response<IEnumerable<Friend>?>>();
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
    }
}
