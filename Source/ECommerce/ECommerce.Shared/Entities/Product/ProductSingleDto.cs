namespace ECommerce.Shared.Entities.Product;

using System;

public record ProductSingleDto(
    int Id,
    string Name,
    string Description,
    SellerDto Seller,
    CategoryDto[] Categories,
    StockDto Stock,
    ProductDetailDto[] Details,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);