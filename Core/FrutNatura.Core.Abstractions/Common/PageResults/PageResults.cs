namespace FrutNatura.Core.Abstractions.Common.PageResults;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int Page { get; }
    public int PageSize { get; }

    public PagedResult(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page < 1 ? 1 : page;
        PageSize = pageSize <= 0 ? 20 : pageSize;
    }
}
