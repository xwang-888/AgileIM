using Newtonsoft.Json;

namespace AgileIM.Shared.Models.Users.Dto
{
    public class LoginUserDto
    {
        /// <summary>
        /// token
        /// </summary>
        public string? AccessToken { get; set; }
        /// <summary>
        /// 刷新Token
        /// </summary>
        public string? RefreshToken { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户头像相对路径
        /// </summary>
        public string UserLogo { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户昵称别名
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string? Signature { get; set; }
        /// <summary>
        /// token过期时间
        /// </summary>
        public DateTime TokenExpireTime { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }
    }
}
