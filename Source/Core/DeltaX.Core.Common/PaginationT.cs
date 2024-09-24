namespace DeltaX.Core.Common;

public record Pagination<TResult>
{
    public int RowsPerPage { get; init; } = 10;
    public int RowsOffset { get; init; } = 0;
    public int Page { get; init; } = 1;
    public int RowsCount { get; init; }
    public List<TResult> Items { get; init; } = [];
    public int Pages { get; init; } = 1;
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }

    public Pagination() { }

    public Pagination(List<TResult> items, int rowsCount, Pagination pagination)
        : this(items, rowsCount, pagination.RowsPerPage, pagination.RowsOffset, pagination.Page) { }

    public Pagination(List<TResult> items, int rowsCount, int rowsPerPage, int rowsOffset, int page)
    {
        RowsPerPage = rowsPerPage;
        RowsOffset = rowsOffset;
        Page = page;
        RowsCount = rowsCount;
        Items = items ?? new();
        Pages = 1 + ((rowsCount == 0 ? 0 : rowsCount - 1) / rowsPerPage);
        HasNextPage = Page < Pages;
        HasPreviousPage = Page > 1;
    }
}
