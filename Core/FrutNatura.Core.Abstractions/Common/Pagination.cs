namespace FrutNatura.Core.Abstractions.Common;

public sealed class Pagination
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public Pagination Create(int page, int pageSize)
    {
        return new Pagination
        { 
            Page = page < 1 ? 1 : page,
            PageSize = pageSize <= 0 ? 20 : PageSize
        }; 
    } 
}
