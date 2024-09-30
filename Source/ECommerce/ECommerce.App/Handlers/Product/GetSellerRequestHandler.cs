namespace ECommerce.App.Handlers.Product;

using DeltaX.Core.Common;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Product;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts.Product;
using ECommerce.Shared.Entities.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetSellerRequestHandler(
    ECommerceDbContext dbContext,
    MapperService mapper
    ) : IRequestHandler<GetSellerListRequest, Pagination<SellerDto>>
{
    public async Task<Pagination<SellerDto>> Handle(GetSellerListRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Seller>().Include(e => e.Users).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.userId))
        {
            query = query.Where(p => p.Users.Any(c => c.Id == request.userId));
        }

        var pagination = new Pagination(request.RowsPerPage, request.RowsOffset, request.Page);
        var count = await query.CountAsync();
        var result = await query
            .Skip(pagination.RowsOffset)
            .Take(pagination.RowsPerPage)
            .ToListAsync();
        return mapper.ToDto(pagination.Load(result, count));
    }
}