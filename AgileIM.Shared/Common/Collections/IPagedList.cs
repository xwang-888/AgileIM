using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileIM.Shared.Common.Collections
{
    /// <summary>
    /// 为<typeparamref name="T"/>分页列表提供接口
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public interface IPagedList<T>
    {
        /// <summary>
        /// 索引起始值
        /// </summary>
        int IndexFrom { get; }
        /// <summary>
        /// 当前页索引
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 页面数量大小
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 列表数据总数量
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// 页面总页数
        /// </summary>
        int PageCount { get; }
        /// <summary>
        /// 当前页的列表数据
        /// </summary>
        IList<T> Items { get; }
        /// <summary>
        /// 有上一页
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 有下一页
        /// </summary>
        bool HasNextPage { get; }

    }
}
