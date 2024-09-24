namespace ECommerce.App.Handlers;

using DeltaX.Core.Hosting.Auth;
using DeltaX.ResultFluent;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts;
using ECommerce.Shared.Entities;
using ECommerce.Shared.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class ConfigProductRequestHandler(
    ECommerceDbContext dbContext,
    AuthorizationService authorization,
    MapperService mapper
    ) : IRequestHandler<ConfigProductRequest, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(ConfigProductRequest request, CancellationToken cancellationToken)
    {
        var eb = ErrorBuilder.Create()
            .Add(authorization.ValidateAccess(nameof(ConfigProductRequest)))
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

        var existentCategories = dbContext.Set<Category>()
            .Where(e => request.Categories.Contains(e.Name))
            .ToList();
        var newsCategories = request.Categories
            .Where(e => !existentCategories.Any(c => c.Name == e))
            .Select(e => new Category { Name = e })
            .ToList();

        var details = request.Details
            .Select(d => new ProductDetail
            {
                ImageUrl = d.ImageUrl,
                Description = d.Description,
            })
            .ToList();

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Seller = seller,
            Categories = [.. existentCategories, .. newsCategories],
            Details = details,
            //UserId = authorization.CurrentUser.Identifier,
        };

        dbContext.AddDomainEvent(() => new ProductCreated(product.Id, product.Name, product.Description));
        dbContext.Add(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.ToDto(product);
    }
}
