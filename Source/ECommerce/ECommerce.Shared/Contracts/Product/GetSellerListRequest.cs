namespace ECommerce.Shared.Contracts.Product;
using DeltaX.Core.Common;
using ECommerce.Shared.Entities.Product;
using MediatR;

public record GetSellerListRequest(
    string? userId,
    int RowsPerPage,
    int? RowsOffset,
    int? Page
    ) : IRequest<Pagination<SellerDto>>;
