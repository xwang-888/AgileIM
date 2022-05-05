using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Client.Service.Api;
using Agile.Client.Service.Api.Common;
using AgileIM.Shared.Models.ApiResult;
using AgileIM.Shared.Models.Users.Request;
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
        /// <param name="contentType">contentType</param>
        /// <returns></returns>
        public async Task<TResponse?> RequestBehavior<TResponse>(string url, Method method, string pms,
            bool isToken = true, string contentType = ContentType.Json) where TResponse : class
        {
            var client = new RestClient(url);
            var request = new RestRequest { Method = method };

            if (isToken)
            {
                client.AddDefaultHeader(ApiConfiguration.TokenKey, ApiConfiguration.TokenValue);
            }

            switch (contentType)
            {
                case ContentType.Json:
                    request.AddStringBody(pms, contentType);
                    break;
                case ContentType.UrlEncoded:
                    request.AddParameter("application/x-www-form-urlencoded",
                        pms, ParameterType.RequestBody);
                    break;
            }
            var response = await client.ExecuteAsync<TResponse>(request);

            if (response.StatusCode is System.Net.HttpStatusCode.OK)
                if (response.Data is not null)
                    return response.Data;

            return new Response()
            {
                Code = (int)response.StatusCode,
                Message = response.StatusDescription
            } as TResponse;
        }
    }
}
