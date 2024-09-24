namespace ECommerce.Shared.Contracts;
using DeltaX.ResultFluent;
using ECommerce.Shared.Entities;
using MediatR;

public record ConfigSellerRequest(
    string Name,
    string Email,
    string PhoneNumber
    ) : IRequest<Result<SellerDto>>;
