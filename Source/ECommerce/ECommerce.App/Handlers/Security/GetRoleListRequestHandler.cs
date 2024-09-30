namespace ECommerce.App.Handlers.Security;

using DeltaX.Core.Common;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Security;
using ECommerce.Shared.Contracts.Security;
using ECommerce.Shared.Entities.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class GetRoleListRequestHandler(
    SecurityDbContext dbContext
    ) : IRequestHandler<GetRoleListRequest, Pagination<RoleDto>>
{
    const string claimAction = "action";
    const string claimResource = "resource";

    public async Task<Pagination<RoleDto>> Handle(GetRoleListRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Role>().Include(r => r.Claims).AsQueryable();
        var pagination = new Pagination(request.RowsPerPage, request.RowsOffset, request.Page);
        var count = await query.CountAsync();
        var roles = await query
            .Skip(pagination.RowsOffset)
            .Take(pagination.RowsPerPage)
            .ToListAsync(cancellationToken);

        var result = roles.Select(r => new RoleDto(
            r.Id,
            r.Name!,
            r.Claims.Where(c => c.ClaimType == claimResource).Select(c => c.ClaimValue).ToArray()!,
            r.Claims.Where(c => c.ClaimType == claimAction).Select(c => c.ClaimValue).ToArray()!
        )).ToList();

        return pagination.Load(result, count);
    }
}
