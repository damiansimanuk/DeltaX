namespace ECommerce.App.Controllers;

using DeltaX.Core.Common;
using DeltaX.ResultFluent;
using ECommerce.Shared.Contracts;
using ECommerce.Shared.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpGet("productList")]
    public async Task<Pagination<ProductDto>> GetProduct([FromQuery] GetProductRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost("product")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public Task<ActionResult<ProductDto>> ConfigProduct(ConfigProductRequest request)
    {
        return mediator.RequestAsync(request);
    }

    [HttpGet("sellerList")]
    public async Task<Pagination<SellerDto>> GetProduct([FromQuery] GetSellerRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost("seller")]
    public async Task<Result<SellerDto>> ConfigSeller(ConfigSellerRequest request)
    {
        return await mediator.InvokeAsync(request);
    }


}
