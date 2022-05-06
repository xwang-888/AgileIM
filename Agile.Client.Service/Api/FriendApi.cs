using Agile.Client.Service.Api.Common;
using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Client.Service.Api
{
    public class GetFriendListByUserIdApi : IApiParameterBase
    {
        public string ApiPath => "Api/Friend/GetFriendListByUserId";
        public Method Method => Method.Post;
        public bool IsToken => true;
        public string ContentTypeStr => ContentType.Json;
        public string UserId { get; set; }

    }
}
