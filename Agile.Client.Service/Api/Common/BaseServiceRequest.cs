using Agile.Client.Service.RestSharp;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileIM.Shared.Models.ApiResult;

namespace Agile.Client.Service.Api.Common
{
    public static class BaseServiceRequest
    {
        public static string RequestUrl { get; } = ApiRequest.UrlPrefix;
        /// <summary>
        /// restSharp实例
        /// </summary>
        public static RestSharpCertificateMethod RestSharp = new();



        private static string DicToText(Dictionary<string, object>? dic)
        {
            var str = "";
            if (dic != null)
            {
                foreach (var key in dic.Keys)
                {
                    if (str != "")
                    {
                        str += "&";
                    }
                    str += key + "=" + dic[key];
                }
            }


            return str;
        }


        /// <summary>
        /// T请求
        /// </summary>
        /// <param name="apiParameter">请求参数</param>
        /// <returns></returns>
        public static async Task<TResponse> GetRequest<TResponse>(this IApiParameterBase apiParameter) where TResponse : Response, new()
        {
            var url = RequestUrl + apiParameter.ApiPath;

            var param = JsonConvert.SerializeObject(apiParameter);
            if (apiParameter.ContentTypeStr is ContentType.UrlEncoded)
            {
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(param);
                param = DicToText(dic);
            }
            var result = await RestSharp.RequestBehavior<TResponse>(url, apiParameter.Method, param, apiParameter.IsToken, apiParameter.ContentTypeStr);
            return result;
        }
    }
}
