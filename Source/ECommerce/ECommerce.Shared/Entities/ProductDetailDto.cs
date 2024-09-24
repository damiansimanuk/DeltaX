namespace ECommerce.Shared.Entities;

using System;

public record ProductDetailDto(
    string ImageUrl,
    string Description,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
