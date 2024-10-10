namespace ECommerce.App.Handlers.Product;

using DeltaX.Core.Abstractions;
using DeltaX.Core.Hosting.Auth;
using DeltaX.ResultFluent;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Product;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts.Product;
using ECommerce.Shared.Entities.Product;
using ECommerce.Shared.Events;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

public class ConfigProductRequestHandler(
    ECommerceDbContext dbContext,
    AuthorizationService authorization,
    MapperService mapper
    ) : IRequestHandler<ConfigProductRequest, Result<ProductSingleDto>>
{
    public async Task<Result<ProductSingleDto>> Handle(ConfigProductRequest request, CancellationToken cancellationToken)
    {
        var eb = ErrorBuilder.Create()
            .Add(authorization.ValidateAccessAction($"sellerId:{request.SellerId}", nameof(ConfigProductRequest)))
            .Add(string.IsNullOrWhiteSpace(request.Name), Error.InvalidArgument("The 'Name' field cannot be empty."))
            .Add(string.IsNullOrWhiteSpace(request.Description), Error.InvalidArgument("The 'Description' field cannot be empty."))
            .Add(request.SellerId <= 0, Error.InvalidArgument("The 'SellerId' field cannot be less than or equal to 0."));

        if (eb.HasError)
        {
            return eb.GetErrors();
        }

        var seller = dbContext.Set<Seller>().FirstOrDefault(e => e.Id == request.SellerId);
        if (seller == null)
        {
            return Error.InvalidArgument($"No se encontró el vendedor con SellerId = {request.SellerId}");
        }

        var product = dbContext.Set<Product>()
            .Include(e => e.Categories)
            .Include(e => e.Details)
            .FirstOrDefault(e => e.Id == request.ProductId);

        if (product != null && product.SellerId != request.SellerId)
        {
            return Error.InvalidArgument($"El producto con ProductId = {request.ProductId} no pertenece al vendedor con SellerId = {request.SellerId}");
        }

        var requestCategories = request.Categories?.Select(e => e.ToUpper()).ToList() ?? [];

        var existentCategories = dbContext.Set<Category>()
            .Where(e => requestCategories.Contains(e.NormalizedName))
            .ToList();

        Merge(existentCategories, product?.Categories ?? [], requestCategories, (e, c) => e.NormalizedName == c,
            out var toRelationCategory, out var toAddCategoryRaw, out var toDeleteCategory);

        Merge([], product?.Details ?? [], request.Details, (e, c) => e.Id == c.Id,
            out _, out var toAddDetailRaw, out var toDeleteDetail);

        var toAddCategory = toAddCategoryRaw
            .Select(e => new Category
            {
                Name = e,
                NormalizedName = e.ToUpper()
            })
            .ToList();

        var toAddDetail = toAddDetailRaw
            .Select(d => new ProductDetail
            {
                ImageUrl = d.ImageUrl,
                Description = d.Description,
            })
            .ToList();

        if (product == null)
        {
            product = new Product
            {
                Seller = seller,
                Name = request.Name,
                Description = request.Description,
                Categories = [.. toRelationCategory, .. toAddCategory],
                Details = [.. toAddDetail],
            };
            dbContext.Add(product);
        }
        else
        {
            product.Description = request.Description;
            product.Details.RemoveAll(e => toDeleteDetail.Any(d => d.Id == e.Id));
            product.Details.AddRange(toAddDetail);
            product.Categories.RemoveAll(c => toDeleteCategory.Any(dc => dc.Id == c.Id));
            product.Categories.AddRange(toRelationCategory);
            product.Categories.AddRange(toAddCategory);

            dbContext.Update(product);
        }

        dbContext.AddDomainEvent(() => new ProductCreated(product.Id, product.Name, product.Description));

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.ToDto(product);
    }

    void Merge<T, TC>(
        IEnumerable<T> existent,
        IEnumerable<T> configured,
        IEnumerable<TC> expected,
        Func<T, TC, bool> comparer,
        out List<T> toRelation,
        out List<TC> toAddRaw,
        out List<T> toDelete)
    {
        toRelation = existent
            .Where(e => expected.Any(c => comparer(e, c)) && !configured.Any(c => c!.Equals(e)))
            .ToList();

        toAddRaw = expected
            .Where(es => !existent.Any(et => comparer(et, es)) && !configured.Any(et => comparer(et, es)))
            .ToList();

        toDelete = configured
            .Where(e => !expected.Any(c => comparer(e, c)))
            .ToList();
    }
}
