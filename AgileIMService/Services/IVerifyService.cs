using AgileIM.Shared.Models.Users;
using AgileIM.Shared.Models.Users.Entity;

namespace AgileIM.Service.Services
{
    public interface IVerifyService
    {
        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <returns></returns>
        public Task<User?> VerifyUser(string account,string password);
    }
}
