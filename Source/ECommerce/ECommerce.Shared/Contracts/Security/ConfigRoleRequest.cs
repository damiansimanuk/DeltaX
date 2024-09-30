namespace ECommerce.Shared.Contracts.Security;

using DeltaX.ResultFluent;
using ECommerce.Shared.Entities.Security;
using MediatR;

public record ConfigRoleRequest(
    string? RoleId,
    string Name,
    string[] Resources,
    string[] Actions
    ) : IRequest<Result<RoleDto>>;
