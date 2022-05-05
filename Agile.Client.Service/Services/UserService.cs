using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Agile.Client.Service.Api;
using Agile.Client.Service.Api.Common;
using Agile.Client.Service.RestSharp;

using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users.Dto;

namespace Agile.Client.Service.Services
{
    public class UserService : IUserService
    {
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
    }


}
