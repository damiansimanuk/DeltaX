namespace ECommerce.Shared.Entities.Product;

using System;

public record StockDto(
    int Id,
    int QuantityAvailable,
    StockMovementDto[] Movements,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
