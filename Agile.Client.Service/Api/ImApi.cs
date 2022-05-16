using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Agile.Client.Service.Api.Common;

using RestSharp;

namespace Agile.Client.Service.Api
{
    internal class SendMessageApi : IApiParameterBase
    {
        public string ApiPath => "api/im/SendMessage";
        public Method Method { get; } = Method.Post;
        public string ContentTypeStr => ContentType.Json;
        public bool IsToken => false;

        public string FromId { get; set; }
        public string TargetId { get; set; }
        public int MsgType { get; set; }
        public string? Content { get; set; }
    }
}
