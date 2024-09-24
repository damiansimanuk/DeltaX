namespace ECommerce.Shared.Entities;

using System;

public record SellerDto(
    int Id,
    string Name,
    string Email,
    string PhoneNumber,
    UserSimpleDto[] Users,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
