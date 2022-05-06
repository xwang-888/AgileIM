using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileIM.Client.Models;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users.Dto;

namespace Agile.Client.Service.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userAccountOrMobile">账号或者手机号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<Response<LoginUserDto>?> Login(string userAccountOrMobile, string password);
        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<Response<RefreshTokenDto>?> RefreshToken(string refreshToken);

    }
}
