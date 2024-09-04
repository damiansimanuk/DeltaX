namespace DeltaX.Core.Common;
public record Pagination
{
    public int RowsPerPage { get; init; } = 10;
    public int RowsOffset { get; init; }
    public int Page { get; init; } = 1;

    public Pagination() { }

    public Pagination(int rowsPerPage, int? rowsOffset = null, int? page = null)
    {
        RowsPerPage = rowsPerPage;
        if (rowsOffset.HasValue)
        {
            RowsOffset = rowsOffset.Value;
            Page = page ?? ((rowsOffset.Value / RowsPerPage) + 1);
        }
        else if (page.HasValue)
        {
            Page = page.Value;
            RowsOffset = rowsOffset ?? ((page.Value - 1) * RowsPerPage);
        }
    }

    public Pagination<TResult> Load<TResult>(List<TResult> items, int rowsCount)
    {
        return new Pagination<TResult>(items, rowsCount, this);
    }
}
