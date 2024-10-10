namespace ECommerce.Shared.Entities.Product;

using System;

public record ProductDetailDto(
    int Id,
    string ImageUrl,
    string Description,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
