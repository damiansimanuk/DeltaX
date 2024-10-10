namespace ECommerce.Shared.Contracts.Security;

using DeltaX.ResultFluent;
using ECommerce.Shared.Entities.Security;
using MediatR;

public record ConfigUserRequest(
    string? UserId,
    string? FullName,
    string Email,
    string? PhoneNumber,
    string[] Roles
    ) : IRequest<Result<UserDto>>;