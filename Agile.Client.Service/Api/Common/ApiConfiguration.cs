namespace Agile.Client.Service.Api.Common
{
    /// <summary>
    /// API设置类
    /// </summary>
    public class ApiConfiguration
    {
        /// <summary>
        /// 设置TokenValue
        /// </summary>
        /// <param name="token"></param>
        /// <param name="refreshToken"></param>
        /// <param name="tokenExpireTime"></param>
        public static void SetTokenValue(string token, string refreshToken, DateTime tokenExpireTime)
        {
            TokenValue = "Bearer " + token;
            RefreshToken = refreshToken;
            TokenExpireTime = tokenExpireTime;
        }

        public const string TokenKey = "Authorization";

        public static string TokenValue = "Bearer ";

        public static string? RefreshToken;

        public static DateTime TokenExpireTime;

    }
}
