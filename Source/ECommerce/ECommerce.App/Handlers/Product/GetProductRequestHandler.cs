namespace ECommerce.App.Handlers.Product;

using DeltaX.Core.Common;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Product;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts.Product;
using ECommerce.Shared.Entities.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetProductRequestHandler(
    ECommerceDbContext dbContext,
    MapperService mapper
    ) : IRequestHandler<GetProductListRequest, Pagination<ProductDto>>
{
    public async Task<Pagination<ProductDto>> Handle(GetProductListRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<Product>()
            .Include(e => e.Categories)
            .Include(e => e.Seller)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.FilterText))
        {
            query = query.Where(p => p.Name.Contains(request.FilterText)
                || p.Categories.Any(c => c.Name.Contains(request.FilterText)));
        }

        var pagination = new Pagination(request.RowsPerPage, request.RowsOffset, request.Page);
        var count = await query.CountAsync();
        var result = await query
            .Skip(pagination.RowsOffset)
            .Take(pagination.RowsPerPage)
            .ToListAsync(cancellationToken);
        return mapper.ToDto(pagination.Load(result, count));
    }
}
