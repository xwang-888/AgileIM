namespace Agile.Client.Service.Api.Common
{
    public class ApiRequest
    {
        private static string _addressPort;
        /// <summary>
        /// 请求地址
        /// </summary>
        public static string AddressPort
        {
            get => _addressPort;
            set
            {
                _addressPort = value;
                UrlPrefix = $"http://{value}/";
                WsPrefix = $"ws://{value}/ws";
            }
        }
        /// <summary>
        ///请求地址前缀 域名+端口
        /// </summary>
        public static string UrlPrefix { get; set; }
        /// <summary>
        ///请求地址前缀 域名+端口
        /// </summary>
        public static string WsPrefix { get; set; }
    }
}
