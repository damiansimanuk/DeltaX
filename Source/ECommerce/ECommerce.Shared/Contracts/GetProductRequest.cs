namespace ECommerce.Shared.Contracts;
using DeltaX.Core.Common;
using ECommerce.Shared.Entities;
using MediatR;

public record GetProductRequest(
    string? filterText,
    int RowsPerPage,
    int RowsOffset,
    int Page) : IRequest<Pagination<ProductDto>>;


public record GetSellerRequest(
    string? userId,
    int RowsPerPage,
    int RowsOffset,
    int Page) : IRequest<Pagination<SellerDto>>;