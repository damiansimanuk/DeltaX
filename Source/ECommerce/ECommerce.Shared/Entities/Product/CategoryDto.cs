namespace ECommerce.Shared.Entities.Product;

using System;

public record CategoryDto(
    int Id,
    string Name,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
