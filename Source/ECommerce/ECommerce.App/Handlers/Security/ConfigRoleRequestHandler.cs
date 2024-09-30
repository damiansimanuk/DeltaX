namespace ECommerce.App.Handlers.Security;

using DeltaX.Core.Hosting.Auth;
using DeltaX.ResultFluent;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Security;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts.Security;
using ECommerce.Shared.Entities.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class ConfigRoleRequestHandler(
    SecurityDbContext dbContext,
    AuthorizationService authorization,
    MapperService mapper
) : IRequestHandler<ConfigRoleRequest, Result<RoleDto>>
{
    const string claimAction = "action";
    const string claimResource = "resource";

    public async Task<Result<RoleDto>> Handle(ConfigRoleRequest request, CancellationToken cancellationToken)
    {
        var eb = ErrorBuilder.Create()
            // .Add(authorization.ValidateAccess(nameof(ConfigRoleRequest)))
            .Add(string.IsNullOrWhiteSpace(request.Name), Error.InvalidArgument("The 'Name' field cannot be empty."))
            .Add(request.Resources == null, Error.InvalidArgument("The 'Resources' field cannot be null."))
            .Add(request.Actions == null, Error.InvalidArgument("The 'Actions' field cannot be null."));

        if (eb.HasError)
        {
            return eb.GetErrors();
        }

        var role = dbContext.Set<Role>().Include(r => r.Claims).FirstOrDefault(e => e.Id == request.RoleId);
        if (role != null)
        {
            ParseRoleClaims(role.Id, claimResource, request.Resources!, role.Claims, out var newResources, out var deleteResources);
            ParseRoleClaims(role.Id, claimAction, request.Actions!, role.Claims, out var newActions, out var deleteActions);

            role.Name = request.Name;
            role.NormalizedName = request.Name?.Normalize();

            dbContext.Update(role);
            dbContext.AddRange(newResources);
            dbContext.AddRange(newActions);
            dbContext.RemoveRange(deleteResources);
            dbContext.RemoveRange(deleteActions);
        }
        else
        {
            role = new Role()
            {
                Name = request.Name,
                NormalizedName = request.Name.Normalize(),
            };

            ParseRoleClaims(role.Id, claimResource, request.Resources!, new(), out var newResources, out var _);
            ParseRoleClaims(role.Id, claimAction, request.Actions!, new(), out var newActions, out var _);

            dbContext.Add(role);
            dbContext.AddRange(newResources);
            dbContext.AddRange(newActions);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var roleDto = new RoleDto(
            role.Id,
            request.Name!,
            request.Resources!,
            request.Actions!
        );

        return roleDto;
    }

    void ParseRoleClaims(
        string roleId,
        string claimType,
        string[] values,
        List<IdentityRoleClaim<string>> existentRoleClaims,
        out List<IdentityRoleClaim<string>> toAdd,
        out List<IdentityRoleClaim<string>> toDelete)
    {
        var existent = existentRoleClaims.Where(e => e.ClaimType == claimType)
            .Where(e => values.Contains(e.ClaimValue))
            .ToList();

        toAdd = values
            .Where(e => !existent.Any(c => c.ClaimValue == e))
            .Select(e => new IdentityRoleClaim<string> { RoleId = roleId, ClaimType = claimType, ClaimValue = e })
            .ToList();

        toDelete = existentRoleClaims.Where(e => e.ClaimType == claimType)
            .Where(e => !values.Contains(e.ClaimValue))
            .ToList();
    }
}
