namespace ECommerce.App.Handlers;

using DeltaX.Core.Common;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts;
using ECommerce.Shared.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetSellerRequestHandler(
    ECommerceDbContext dbContext,
    MapperService mapper
    ) : IRequestHandler<GetSellerRequest, Pagination<SellerDto>>
{
    public async Task<Pagination<SellerDto>> Handle(GetSellerRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Seller>().Include(e => e.Users).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.userId))
        {
            query = query.Where(p => p.Users.Any(c => c.Id == request.userId));
        }

        var count = await query.CountAsync();
        var result = await query
            .Skip(request.RowsOffset)
            .Take(request.RowsPerPage)
            .ToListAsync();

        var res = new Pagination<Seller>(
            result,
            count,
            request.RowsPerPage,
            request.RowsOffset,
            request.Page
        );

        return mapper.ToDto(res);
    }
}