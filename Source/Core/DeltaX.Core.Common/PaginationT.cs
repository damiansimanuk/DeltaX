namespace DeltaX.Core.Common;

public record Pagination<TResult> : Pagination
{
    public int RowsCount { get; init; }
    public List<TResult> Items { get; init; } = null!;
    public int Pages { get; init; } = 1;
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }

    public Pagination() : base() { }

    public Pagination(int rowsPerPage = 10)
        : this(new(), 0, rowsPerPage, null, null) { }

    public Pagination(List<TResult> items, int rowsCount, Pagination pagination)
        : this(items, rowsCount, pagination.RowsPerPage, pagination.RowsOffset, pagination.Page) { }

    public Pagination(List<TResult> items, int rowsCount, int rowsPerPage, int? rowsOffset, int? page)
        : base(rowsPerPage, rowsOffset, page)
    {
        RowsCount = rowsCount;
        Items = items ?? new();
        Pages = 1 + ((rowsCount == 0 ? 0 : rowsCount - 1) / rowsPerPage);
        HasNextPage = Page < Pages;
        HasPreviousPage = Page > 1;
    }
}
