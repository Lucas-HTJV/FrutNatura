using System;
using System.Collections.Generic;

namespace FrutNatura.Form.Models
{
    public sealed class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
        public int Total { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)Total / Math.Max(1, PageSize));

        public PagedResult() { }

        public PagedResult(IReadOnlyList<T> items, int total, int page, int pageSize)
        {
            Items = items ?? Array.Empty<T>();
            Total = total;
            Page = page;
            PageSize = pageSize;
        }
    }
}
