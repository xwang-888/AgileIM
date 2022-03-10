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
    public class Result
    {

        public Result(string code, string msg, object? data = null)
        {
            Msg = msg;
            Code = code;
            Data = data;
        }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object? Data { get; set; }
    }
    public class Result<T> : Result
    {

        public Result(string code, string msg, T data) : base(code, msg, data)
        {
            Data = data;
        }
        /// <summary>
        /// 返回数据
        /// </summary>
        public new T Data { get; set; }
    }
}
