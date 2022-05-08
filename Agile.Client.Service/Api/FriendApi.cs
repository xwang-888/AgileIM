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
        public GetFriendListByUserIdApi(string userId)
        {
            UserId = userId;
        }

        public string ApiPath => $"Api/Friend/GetFriendListByUserId?userId={UserId}";
        public Method Method => Method.Get;
        public bool IsToken => true;
        public string ContentTypeStr => ContentType.Json;
        private string UserId { get; }

    }
    public class UpdateUserNoteApi : IApiParameterBase
    {
        public string ApiPath => $"Api/Friend/UpdateUserNote";
        public Method Method => Method.Post;
        public bool IsToken => true;
        public string ContentTypeStr => ContentType.Json;
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string UserNote { get; set; }

    }
    public class AddFriendApi : IApiParameterBase
    {
        public string ApiPath => $"Api/Friend/Add";
        public Method Method => Method.Post;
        public bool IsToken => true;
        public string ContentTypeStr => ContentType.Json;
        public string UserId { get; set; }
        public string FriendId { get; set; }

    }
}
