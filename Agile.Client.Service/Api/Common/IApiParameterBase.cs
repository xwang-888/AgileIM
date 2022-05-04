using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;

namespace Agile.Client.Service.Api.Common
{
    public interface IApiParameterBase
    {
        [JsonIgnore]
        public string ApiPath { get; }
        [JsonIgnore]
        public Method Method { get; }
        [JsonIgnore]
        public string ContentTypeStr { get; }
        [JsonIgnore]
        public bool IsToken { get; }
    }
}
