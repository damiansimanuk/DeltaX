namespace ECommerce.Shared.Entities;

using System;

public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    SellerDto Seller,
    CategoryDto[] Categories,
    StockDto Stock,
    ProductDetailDto[] Details,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
