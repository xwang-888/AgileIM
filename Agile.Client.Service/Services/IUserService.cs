using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileIM.Shared.Models.Users.Dto;

namespace Agile.Client.Service.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="accountOrPhone">账号或者手机号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<LoginUserDto?> Login(string accountOrPhone, string password);
        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<RefreshTokenDto?> RefreshToken(string refreshToken);
    }
}
