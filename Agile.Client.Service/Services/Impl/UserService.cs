﻿using Agile.Client.Service.Api;
using Agile.Client.Service.Api.Common;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users.Dto;
using AgileIM.Shared.Models.Users.Entity;

using AutoMapper;

namespace Agile.Client.Service.Services.Impl
{
    public class UserService : IUserService
    {
        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }

        private readonly IMapper _mapper;
        public async Task<Response<LoginUserDto>?> Login(string userAccountOrMobile, string password)
        {
            var loginApi = new LoginApi
            {
                UserAccountOrMobile = userAccountOrMobile,
                Password = password
            };

            var result = await loginApi.GetRequest<Response<LoginUserDto>>();
            if (result.Data is not null)
                ApiConfiguration.SetTokenValue(result.Data.AccessToken, result.Data.RefreshToken, result.Data.TokenExpireTime);

            return result;
        }

        public async Task<Response<RefreshTokenDto>?> RefreshToken(string refreshToken)
        {
            var api = new RefreshTokenApi { RefreshToken = refreshToken };

            return await api.GetRequest<Response<RefreshTokenDto>>();
        }

        public async Task<Response<IEnumerable<UserInfoDto>?>> QueryFriends(string userAccountOrMobile)
        {
            var api = new QueryFriendsApi(userAccountOrMobile);
            var response = new Response<IEnumerable<UserInfoDto>?>();
            var result = await api.GetRequest<Response<IEnumerable<User>?>>();
            if (result?.Data is not null)
            {
                response.Message = result.Message;
                response.Code = result.Code;
                response.Data = _mapper.Map<IEnumerable<User>, IEnumerable<UserInfoDto>>(result.Data);
            }

            return response;
        }
    }

}
