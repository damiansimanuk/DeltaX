namespace ECommerce.Shared.Entities;

using System;

public record CategoryDto(
    int Id,
    string Name,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
