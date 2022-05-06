using System.Net;
using AgileIM.Service.OAuth.Configs;
using AgileIM.Service.Services.BaseService.Impl;
using AgileIM.Shared.EFCore.Data.UnitOfWork;
using AgileIM.Shared.Models.Users.Dto;
using AgileIM.Shared.Models.Users.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AgileIM.Service.Services.UserService.Impl
{
    public class UserService : BaseCrudService<User>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IHttpClientFactory clientFactory, IConfiguration configuration) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public async Task<LoginUserDto?> Login(string accountOrPhone, string password)
        {
            try
            {
                var address = _configuration["ServerIpPort"];

                var url = $"{address}/connect/token";
                var client = _clientFactory.CreateClient();
                var listConfigs = new List<KeyValuePair<string, string>>()
                {
                    new ("grant_type","password"),
                    new ("username",accountOrPhone),
                    new ("password",password),
                    new ("client_id",OAuthConfig.SecretUserApi.ClientId),
                    new ("client_secret",OAuthConfig.SecretUserApi.Secret),
                };

                var result = await client.PostAsync(url, new FormUrlEncodedContent(listConfigs));

                if (result.StatusCode is not HttpStatusCode.OK) return null;

                var str = await result.Content.ReadAsStringAsync();
                var d = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
                var userObj = d?.Where(a => a.Key.Equals("user")).Select(a => a.Value).SingleOrDefault();
                if (userObj is null) return null;

                var loginUserJson = JsonConvert.SerializeObject(userObj);
                if (string.IsNullOrEmpty(loginUserJson)) return null;

                var user = JsonConvert.DeserializeObject<User>(loginUserJson);
                if (user is null) return null;


                var loginUserDto = new LoginUserDto()
                {
                    Account = user.Account,
                    Phone = user.Phone,
                    UserLogo = user.Image,
                    LastLoginTime = user.LastLoginTime,
                    Gender = user.Gender,
                    Address = user.Address,
                    Id = user.Id,
                    Nick = user.Nick,
                };

                if (d?.TryGetValue("access_token", out var token) is true)
                    loginUserDto.AccessToken = token.ToString();

                if (d?.TryGetValue("refresh_token", out var refreshToken) is true)
                    loginUserDto.RefreshToken = refreshToken.ToString();

                if (d?.TryGetValue("expires_in", out var expiresIn) is true)
                    loginUserDto.TokenExpireTime = DateTime.Now.AddSeconds(Convert.ToDouble(expiresIn.ToString()));


                return loginUserDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public async Task<bool> UpdateLastDateTime(string uid)
        {
            try
            {
                var userRep = _unitOfWork.GetRepository<User>();
                var user = await userRep.FirstOrDefaultAsync(a => a.Id.Equals(uid));
                if (user is null) return false;
                user.LastLoginTime = DateTime.Now;

                userRep.Update(user);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<RefreshTokenDto?> RefreshToken(string refreshToken)
        {
            try
            {
                var address = _configuration["ServerIpPort"];

                var url = $"{address}/connect/token";
                var client = _clientFactory.CreateClient();
                var listConfigs = new List<KeyValuePair<string, string>>()
                {
                    new("client_id", OAuthConfig.SecretUserApi.ClientId),
                    new("client_secret", OAuthConfig.SecretUserApi.Secret),
                    new("grant_type", "refresh_token"),
                    new("refresh_token", refreshToken),
                };
                var result = await client.PostAsync(url, new FormUrlEncodedContent(listConfigs));
                if (result.StatusCode != HttpStatusCode.OK) return null;

                var resultStr = await result.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(resultStr)) return null;

                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(resultStr);
                if (dic is null) return null;

                var tokenDto = new RefreshTokenDto();
                if (dic.TryGetValue("access_token", out var tokenObj))
                    tokenDto.Token = tokenObj.ToString();
                if (dic.TryGetValue("refresh_token", out var refreshTokenObj))
                    tokenDto.RefreshToken = refreshTokenObj.ToString();
                if (dic.TryGetValue("expires_in", out var expiresIn))
                    tokenDto.TokenExpireTime = DateTime.Now.AddSeconds(Convert.ToInt32(expiresIn));
                if (dic.TryGetValue("token_type", out var type))
                    tokenDto.Type = type.ToString();

                return tokenDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IEnumerable<User>?> QueryFriends(string userAccountOrMobile)
        {
            var userRep = _unitOfWork.GetRepository<User>();
            return await userRep.GetAll().Where(a => a.Account.Equals(userAccountOrMobile) || a.Phone.Equals(userAccountOrMobile)).ToListAsync();
        }

    }
}
