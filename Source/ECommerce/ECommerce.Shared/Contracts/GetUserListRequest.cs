namespace ECommerce.Shared.Contracts;
using DeltaX.Core.Common;
using ECommerce.Shared.Entities;
using MediatR;

public record GetUserListRequest(
    int RowsPerPage,
    int? RowsOffset,
    int? Page
    ) : IRequest<Pagination<UserSimpleDto>>;