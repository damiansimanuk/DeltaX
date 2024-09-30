namespace ECommerce.Shared.Contracts.Security;

using DeltaX.Core.Common;
using ECommerce.Shared.Entities.Security;
using MediatR;

public record GetRoleListRequest(
    int RowsPerPage = 10,
    int? RowsOffset = null,
    int? Page = null
    ) : IRequest<Pagination<RoleDto>>;