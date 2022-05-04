using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Client.Service.Api.Common
{
    public class ContentType
    {
        public const string Json = "application/json";
        public const string Xml = "application/xml";
        public const string Plain = "text/plain";
        public const string Binary = "application/octet-stream";
        public const string GZip = "application/x-gzip";
        public const string UrlEncoded = "application/x-www-form-urlencoded";
        public const string FromData = "multipart/form-data";
        public static readonly Dictionary<DataFormat, string> FromDataFormat = new Dictionary<DataFormat, string>()
        {
            {
                DataFormat.Xml,
                "application/xml"
            },
            {
                DataFormat.Json,
                "application/json"
            },
            {
                DataFormat.None,
                "text/plain"
            },
            {
                DataFormat.Binary,
                "application/octet-stream"
            }
        };
        public static readonly string[] JsonAccept = new string[4]
        {
            "application/json",
            "text/json",
            "text/x-json",
            "text/javascript"
        };
        public static readonly string[] XmlAccept = new string[2]
        {
            "application/xml",
            "text/xml"
        };
    }
}
