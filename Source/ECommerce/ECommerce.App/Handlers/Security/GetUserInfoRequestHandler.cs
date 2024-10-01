namespace ECommerce.App.Handlers.Security;

using DeltaX.ResultFluent;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Security;
using ECommerce.Shared.Contracts.Security;
using ECommerce.Shared.Entities.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class GetUserInfoRequestHandler(
    SecurityDbContext dbContext
    ) : IRequestHandler<GetUserInfoRequest, Result<UserInfoDto>>
{
    const string claimAction = "action";
    const string claimResource = "resource";

    public async Task<Result<UserInfoDto>> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
    {
        var e = await dbContext.Set<User>()
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .Where(u => u.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (e == null)
        {
            return Error.InvalidArgument("Email not found");
        }

        var result = new UserInfoDto(
            e.Id,
            e.UserName,
            e.FullName,
            e.Email!,
            e.PhoneNumber,
            e.Roles.Select(r => new RoleDto(
                r.Id,
                r.Name!,
                r.Claims.Where(c => c.ClaimType == claimResource).Select(c => c.ClaimValue).ToArray()!,
                r.Claims.Where(c => c.ClaimType == claimAction).Select(c => c.ClaimValue).ToArray()!
            )).ToArray()
        );

        return result;
    }
}
