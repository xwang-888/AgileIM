using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileIM.Shared.Common.Collections
{
    public class PageList<T> : IPagedList<T>
    {
        /// <summary>
        /// 初始化<see cref="PageList{T}"/>类型的默认分页实例
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">当前页的索引</param>
        /// <param name="pageSize">当前页的大小</param>
        /// <param name="indexFrom">索引起始值</param>
        public PageList(IEnumerable<T>? source, int pageIndex, int pageSize, int indexFrom)
        {
            if (indexFrom > pageIndex)
                throw new ArgumentException($"IndexFrom：{indexFrom}>PageIndex：{pageIndex},索引起始值必须小于或等于当前页索引值");

            if (source is null)
                throw new ArgumentNullException(nameof(source));


            IndexFrom = indexFrom;
            PageIndex = pageIndex;
            PageSize = pageSize;

            if (source is IQueryable<T> queryable)
            {
                TotalCount = queryable.Count();
                PageCount = (int)Math.Ceiling((double)TotalCount / PageSize);
                Items = queryable.Skip((PageIndex - IndexFrom) - PageSize).Take(PageSize).ToList();
            }
            else
            {
                TotalCount = source.Count();
                PageCount = (int)Math.Ceiling((double)TotalCount / PageSize);
                Items = source.Skip((PageIndex - IndexFrom) - PageSize).Take(PageSize).ToList();
            }
        }



        public PageList() => Items = Array.Empty<T>();

        public int IndexFrom { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public IList<T> Items { get; set; }
        public bool HasPreviousPage => PageIndex - IndexFrom > 0;
        public bool HasNextPage => PageIndex - IndexFrom + 1 < PageCount;

    }

    public class PageList<TSource, TResult> : IPagedList<TResult>
    {
        /// <summary>
        /// 初始化<see cref="PageList{TResult}"/>类型分页实例
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="converter">转化器</param>
        /// <param name="pageIndex">当前的索引</param>
        /// <param name="pageSize">每页的数量</param>
        /// <param name="indexFrom">索引起始值</param>
        public PageList(IEnumerable<TSource>? source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex, int pageSize, int indexFrom)
        {
            if (indexFrom > pageIndex)
                throw new ArgumentException($"IndexFrom：{indexFrom}>PageIndex：{pageIndex},索引起始值必须小于或等于当前页索引值");

            if (source is null)
                throw new ArgumentNullException(nameof(source));


            IndexFrom = indexFrom;
            PageIndex = pageIndex;
            PageSize = pageSize;

            if (source is IQueryable<TSource> queryable)
            {
                TotalCount = queryable.Count();
                PageCount = (int)Math.Ceiling((double)TotalCount / PageSize);
                var items = queryable.Skip((PageIndex - IndexFrom) - PageSize).Take(PageSize).ToList();
                Items = new List<TResult>(converter(items));
            }
            else
            {
                TotalCount = source.Count();
                PageCount = (int)Math.Ceiling((double)TotalCount / PageSize);
                var items = source.Skip((PageIndex - IndexFrom) - PageSize).Take(PageSize).ToList();
                Items = new List<TResult>(converter(items));
            }
        }



        public PageList() => Items = Array.Empty<TResult>();

        public int IndexFrom { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public IList<TResult> Items { get; set; }
        public bool HasPreviousPage => PageIndex - IndexFrom > 0;
        public bool HasNextPage => PageIndex - IndexFrom + 1 < PageCount;

    }
}
