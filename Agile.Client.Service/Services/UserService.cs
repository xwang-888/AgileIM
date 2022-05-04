using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Agile.Client.Service.Api;
using Agile.Client.Service.Api.Common;
using Agile.Client.Service.RestSharp;

using AgileIM.Shared.Models.Users.Dto;

namespace Agile.Client.Service.Services
{
    public class UserService : IUserService
    {
        public async Task<LoginUserDto?> Login(string accountOrPhone, string password)
        {
            var loginApi = new LoginApi
            {
                AccountOrPhone = accountOrPhone,
                Password = password
            };

            var login = await loginApi.GetRequest<LoginUserDto>();
            ApiConfiguration.SetTokenValue(login.AccessToken, login.RefreshToken, login.TokenExpireTime);
            return login;
        }

        public async Task<RefreshTokenDto?> RefreshToken(string refreshToken)
        {
            var api = new RefreshTokenApi { RefreshToken = refreshToken };

            return await api.GetRequest<RefreshTokenDto>();
        }
    }


}
