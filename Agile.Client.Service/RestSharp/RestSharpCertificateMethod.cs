using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Client.Service.Api;
using AgileIM.Shared.Models.ApiResult;
using Newtonsoft.Json;

namespace Agile.Client.Service.RestSharp
{
    public class RestSharpCertificateMethod
    {
        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="method">请求类型</param>
        /// <param name="pms">参数</param>
        /// <param name="isToken">是否Token</param>
        /// <param name="isJson">是否Json</param>
        /// <returns></returns>
        public async Task<TResponse?> RequestBehavior<TResponse>(string url, Method method, string pms,
            bool isToken = true, bool isJson = true) where TResponse : class
        {
            var client = new RestClient(url);
            var request = new RestRequest(url, method);
            if (isToken)
            {
                client.AddDefaultHeader(ApiConfiguration.TokenKey, ApiConfiguration.TokenValue);
            }
            switch (method)
            {
                case Method.Get:
                    request.AddHeader("Content-Type", "application/json");
                    break;
                case Method.Post:
                    if (isJson)
                    {
                        request.AddHeader("Content-Type", "application/json");
                        request.AddJsonBody(pms);
                    }
                    else
                    {
                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                        request.AddParameter("application/x-www-form-urlencoded",
                            pms, ParameterType.RequestBody);
                    }
                    break;
                case Method.Put:
                    request.AddHeader("Content-Type", "application/json");
                    break;
                case Method.Delete:
                    request.AddHeader("Content-Type", "application/json");
                    break;
                default:
                    request.AddHeader("Content-Type", "application/json");
                    break;
            }
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<TResponse>(response.Content);
            return new Result()
            {
                Code = (int)response.StatusCode,
                Msg = response.StatusDescription
            } as TResponse;
        }
    }
}
