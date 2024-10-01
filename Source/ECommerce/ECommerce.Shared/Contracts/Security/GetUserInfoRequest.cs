namespace ECommerce.Shared.Contracts.Security;
using DeltaX.ResultFluent;
using ECommerce.Shared.Entities.Security;
using MediatR;

public record GetUserInfoRequest(
    string UserId
    ) : IRequest<Result<UserInfoDto>>;