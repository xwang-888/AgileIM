using AgileIM.Shared.Models.Users;

namespace AgileIM.Service.Service
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
