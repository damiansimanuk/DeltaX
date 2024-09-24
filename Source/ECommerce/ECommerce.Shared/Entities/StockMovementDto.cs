namespace ECommerce.Shared.Entities;

using System;

public record StockMovementDto(
    int Id,
    int Quantity,
    StockMovementTypeEnum MovementType,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
