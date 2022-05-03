using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileIM.Shared.Models.Users.Dto
{
    /// <summary>
    /// 刷新tokenDto
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// accessToken
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// refreshToken
        /// </summary>
        public string? RefreshToken { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime TokenExpireTime { get; set; }
        /// <summary>
        /// Token类型
        /// </summary>
        public string? Type { get; set; }
    }
}
