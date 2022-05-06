using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users;
using AgileIM.Shared.Models.Users.Dto;
using AgileIM.Shared.Models.Users.Entity;

namespace AgileIM.Service.Services.UserService
{
    public interface IUserService : IBaseCrudService<User>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="accountOrPhone">账号或者手机号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<LoginUserDto?> Login(string accountOrPhone, string password);
        /// <summary>
        /// 登录成功修改最后登录时间
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<bool> UpdateLastDateTime(string uid);
        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<RefreshTokenDto?> RefreshToken(string refreshToken);
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="userAccountOrMobile"></param>
        /// <returns></returns>
        Task<IEnumerable<User>?> QueryFriends(string userAccountOrMobile);
    }
}
