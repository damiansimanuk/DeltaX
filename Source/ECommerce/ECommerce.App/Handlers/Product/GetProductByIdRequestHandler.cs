namespace ECommerce.App.Handlers.Product;

using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Product;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts.Product;
using ECommerce.Shared.Entities.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetProductByIdRequestHandler(
    ECommerceDbContext dbContext,
    MapperService mapper
    ) : IRequestHandler<GetProductByIdRequest, ProductSingleDto?>
{
    public async Task<ProductSingleDto?> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Set<Product>()
            .Include(e => e.Categories)
            .Include(e => e.Seller)
            .Include(e => e.Details)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        return product == null ? null : mapper.ToDto(product);
    }
}