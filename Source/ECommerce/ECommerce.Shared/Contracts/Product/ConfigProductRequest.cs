namespace ECommerce.Shared.Contracts.Product;
using DeltaX.ResultFluent;
using ECommerce.Shared.Entities.Product;
using MediatR;

public record ConfigProductRequest(
    int? ProductId,
    int SellerId,
    string Name,
    string Description,
    string[] Categories,
    ConfigProductDetailDto[] Details
    ) : IRequest<Result<ProductSingleDto>>;

public record ConfigProductDetailDto(
    int? Id,
    string ImageUrl,
    string Description);
