using IdentityServer4.Models;
using IdentityServer4.Services;

namespace AgileIM.Service.OAuth
{
    public class ProfileService : IProfileService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //获取用户信息
                context.IssuedClaims = context.Subject.Claims.ToList(); ;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// 获取或设置一个值，该值指示主题是否处于活动状态，是否可以接收令牌。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}
