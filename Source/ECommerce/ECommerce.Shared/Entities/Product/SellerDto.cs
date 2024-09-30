namespace ECommerce.Shared.Entities.Product;

using System;
using ECommerce.Shared.Entities.Security;

public record SellerDto(
    int Id,
    string Name,
    string Email,
    string PhoneNumber,
    UserSimpleDto[] Users,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
