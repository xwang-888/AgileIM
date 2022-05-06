using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Agile.Client.Service.Api.Common;
using Newtonsoft.Json;
using RestSharp;

namespace Agile.Client.Service.Api
{
    public class LoginApi : IApiParameterBase
    {
        public string ApiPath => "Api/User/Login";
        public Method Method => Method.Post;
        public string ContentTypeStr => ContentType.Json;
        public bool IsToken => false;
        public string UserAccountOrMobile { get; set; }
        public string Password { get; set; }
    }
    public class RefreshTokenApi : IApiParameterBase
    {
        public string ApiPath => "Api/User/RefreshToken";
        public Method Method => Method.Post;
        public bool IsToken => false;
        public string ContentTypeStr => ContentType.UrlEncoded;
        public string RefreshToken { get; set; }

    }
    public class GetFriendListByUserIdApi : IApiParameterBase
    {
        public string ApiPath => "Api/Friend/GetFriendListByUserId";
        public Method Method => Method.Post;
        public bool IsToken => true;
        public string ContentTypeStr => ContentType.Json;
        public string UserId { get; set; }

    }
}
