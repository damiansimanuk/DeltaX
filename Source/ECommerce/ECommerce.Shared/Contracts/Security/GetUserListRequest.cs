namespace ECommerce.Shared.Contracts.Security;
using DeltaX.Core.Common;
using ECommerce.Shared.Entities.Security;
using MediatR;

public record GetUserListRequest(
    int RowsPerPage = 10,
    int? RowsOffset = null,
    int? Page = null
    ) : IRequest<Pagination<UserDto>>;
