using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace AgileIM.Shared.Common.Collections
{
    public static class EnumerablePagedListExtensions
    {
        /// <summary>
        /// 根据起始页以及页大小，将数据转换为分页集合
        /// </summary>
        /// <typeparam name="TSource">数据源的类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="pageIndex">当前页的索引</param>
        /// <param name="pageSize">每页的大小</param>
        /// <param name="indexFrom">索引的起始值</param>
        public static IPagedList<TSource> ToPagedList<TSource>(this IEnumerable<TSource> source, int pageIndex,
            int pageSize, int indexFrom = 0) => new PageList<TSource>(source, pageIndex, pageSize, indexFrom);

        /// <summary>
        /// 根据转换器，起始页和页大小把数据转换成页集合
        /// </summary>
        /// <typeparam name="TSource">数据源的类型</typeparam>
        /// <typeparam name="TResult">返回的类型</typeparam>
        /// <param name="source">要转换的源</param>
        /// <param name="converter">转换器转换源为要返回的类型</param>
        /// <param name="pageIndex">当前页的索引</param>
        /// <param name="pageSize">每页的大小</param>
        /// <param name="indexFrom">索引的起始值</param>
        public static IPagedList<TResult> ToPagedList<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex,
            int pageSize, int indexFrom = 0) => new PageList<TSource, TResult>(source, converter, pageIndex, pageSize, indexFrom);
    }

    public static class QueryablePagedListExtensions
    {
        /// <summary>
        /// 根据起始页以及页大小，将数据转换为分页集合
        /// </summary>
        /// <typeparam name="T">数据源的类型</typeparam>
        /// <param name="source">源</param>
        /// <param name="pageIndex">当前页的索引</param>
        /// <param name="pageSize">每页的大小</param>
        /// <param name="indexFrom">索引的起始值</param>
        /// <param name="cancellationToken">观察等待任务完成</param>
        /// <param name="continueOnCapturedContext">继续捕获上下文</param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default, bool continueOnCapturedContext = false)
        {
            if (indexFrom > pageIndex)
                throw new ArgumentException($"IndexFrom：{indexFrom}>PageIndex：{pageIndex},索引起始值必须小于或等于当前页索引值");

            var count = await source.CountAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext);
            var items = await source.Skip((pageIndex + indexFrom) * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext);

            var pageList = new PageList<T>()
            {
                IndexFrom = indexFrom,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = count,
                Items = items,
                PageCount = (int)Math.Ceiling((double)count / pageSize),
            };

            return pageList;
        }
    }
}
