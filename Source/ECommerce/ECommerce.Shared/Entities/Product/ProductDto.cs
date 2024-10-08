﻿namespace ECommerce.Shared.Entities.Product;

using System;

public record ProductDto(
    int Id,
    string Name,
    string Description,
    SellerDto Seller,
    CategoryDto[] Categories,
    StockDto Stock,
    bool Active,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
