namespace ECommerce.App.Handlers;

using DeltaX.Core.Common;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts;
using ECommerce.Shared.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetProductRequestHandler(
    ECommerceDbContext dbContext,
    MapperService mapper
    ) : IRequestHandler<GetProductRequest, Pagination<ProductDto>>
{
    public async Task<Pagination<ProductDto>> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Product>().Include(e => e.Categories).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.filterText))
        {
            query = query.Where(p => p.Name.Contains(request.filterText)
                || p.Categories.Any(c => c.Name.Contains(request.filterText)));
        }

        var count = await query.CountAsync();
        var result = await query
            .Skip(request.RowsOffset)
            .Take(request.RowsPerPage)
            .ToListAsync();

        var res = new Pagination<Product>(
            result,
            count,
            request.RowsPerPage,
            request.RowsOffset,
            request.Page
        );

        return mapper.ToDto(res);
    }
}
