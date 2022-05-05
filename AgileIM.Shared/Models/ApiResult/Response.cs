using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileIM.Shared.Models.ApiResult
{
    /// <summary>
    /// 返回模型
    /// </summary>
    public class Response
    {

        public Response() { }

        public Response(int code, string message, object? data = null)
        {
            Message = message;
            Code = code;
            Data = data;
        }
        /// <summary>
        /// 消息
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object? Data { get; set; }
    }
    public class Response<T> : Response
    {

        public Response() { }
        public Response(int code, string message, T data) : base(code, message, data)
        {
            Data = data;
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        public new T? Data { get; set; }
    }
}
