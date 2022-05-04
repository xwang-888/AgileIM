using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Client.Service.Api
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
        public static void SetTokenValue(string token)
        {
            TokenValue = "Bearer " + token;
        }

        public const string TokenKey = "Authorization";

        public static string TokenValue = "Bearer ";

        public const string UrlEncoded = "application/x-www-form-urlencoded";

        public const string UrlJson = "application/json";
    }
}
