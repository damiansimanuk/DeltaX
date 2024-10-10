namespace ECommerce.Shared.Contracts.Product;
using DeltaX.ResultFluent;
using ECommerce.Shared.Entities.Product;
using MediatR;

public record GetProductByIdRequest(
    int ProductId
    ) : IRequest<ProductSingleDto>;
