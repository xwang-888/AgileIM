namespace Agile.Client.Service.Api.Common
{
    public class ApiRequest
    {
        private static string _url;

        /// <summary>
        ///请求地址前缀 域名+端口
        /// </summary>
        public static string UrlPrefix => _url ?? "http://127.0.0.1:9659/";
    }
}
