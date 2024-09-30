namespace ECommerce.Shared.Contracts.Product;
using DeltaX.ResultFluent;
using ECommerce.Shared.Entities.Product;
using MediatR;

public record ConfigProductRequest(
    int SellerId,
    string Name,
    string Description,
    string[] Categories,
    ConfigProductDetailDto[] Details
    ) : IRequest<Result<ProductDto>>;

public record ConfigProductDetailDto(
    string ImageUrl,
    string Description);
