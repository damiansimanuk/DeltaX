namespace ECommerce.App.Handlers.Security;

using DeltaX.Core.Common;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Security;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts.Security;
using ECommerce.Shared.Entities.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class GetUserListRequestHandler(
    SecurityDbContext dbContext,
    MapperService mapper
    ) : IRequestHandler<GetUserListRequest, Pagination<UserDto>>
{
    public async Task<Pagination<UserDto>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<User>().Include(u => u.Roles).AsQueryable();
        var pagination = new Pagination(request.RowsPerPage, request.RowsOffset, request.Page);
        var count = await query.CountAsync();
        var users = await query
            .Skip(pagination.RowsOffset)
            .Take(pagination.RowsPerPage)
            .ToListAsync(cancellationToken);

        var result = users.Select(e => new UserDto(
            e.Id,
            e.UserName,
            e.FullName,
            e.Email!,
            e.PhoneNumber,
            e.Roles.Select(u => u.Name).ToArray()!
        )).ToList();

        return pagination.Load(result, count);
    }
}
