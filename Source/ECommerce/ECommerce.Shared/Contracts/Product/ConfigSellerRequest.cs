﻿namespace ECommerce.Shared.Contracts.Product;
using DeltaX.ResultFluent;
using ECommerce.Shared.Entities.Product;
using MediatR;

public record ConfigSellerRequest(
    int? SellerId,
    string Name,
    string Email,
    string PhoneNumber
    ) : IRequest<Result<SellerDto>>;
