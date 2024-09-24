namespace ECommerce.Shared.Contracts;
using DeltaX.Core.Common;
using ECommerce.Shared.Entities;
using MediatR;

public record GetSellerListRequest(
    string? userId,
    int RowsPerPage,
    int? RowsOffset,
    int? Page
    ) : IRequest<Pagination<SellerDto>>;
