namespace Spent.Commons.Dtos;

public class PagedResult<T>
{
    public PagedResult(IAsyncEnumerable<T> items, long totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

    public PagedResult() { }

    public IAsyncEnumerable<T>? Items { get; set; }

    public long TotalCount { get; set; }
}
