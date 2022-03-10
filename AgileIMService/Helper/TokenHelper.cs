using System.IdentityModel.Tokens.Jwt;

using static AgileIM.Service.Helper.TokenHelper;

namespace AgileIM.Service.Helper
{
    public class TokenHelper
    {
        /// <summary>
        /// token信息
        /// </summary>
        public class TokenInfo
        {
            /// <summary>
            /// 人员ID
            /// </summary>
            public string? UserId { get; set; }
            /// <summary>
            /// 入参的token
            /// </summary>
            public string? Token { get; set; }
            /// <summary>
            /// 唯一ID 用来标识ws 客户端ID
            /// </summary>
            public string? ClientId { get; set; }
        }

        public static bool GetAllClaim(HttpContext? content, out TokenInfo tokenInfo)
        {
            tokenInfo = new TokenInfo();
            var token = content != null ? content.Request.Query["Authorization"].ToString() : "";
            if (string.IsNullOrEmpty(token)) return false;
            tokenInfo = GetTokenInfo(token);
            return true;
        }

        public static TokenInfo GetTokenInfo(string token)
        {
            var tokenInfo = new TokenInfo();
            if (token.StartsWith("Bearer "))
            {
                token = token.Replace("Bearer ", "");
            }
            var tokenHandler = new JwtSecurityToken(token);
            tokenInfo.UserId = tokenHandler.Claims.FirstOrDefault(m => m.Type == "userId")?.Value;
            tokenInfo.ClientId = tokenHandler.Claims.FirstOrDefault(m => m.Type == "clientId")?.Value;
            tokenInfo.Token = token;
            return tokenInfo;
        }
    }
}
