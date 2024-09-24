namespace ECommerce.App.Controllers;

using DeltaX.ResultFluent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public static class MediatorExtension
{
    public static async Task<ActionResult<TResponse>> RequestAsync<TResponse>(this IMediator mediator, IRequest<Result<TResponse>> request)
    {
        var res = await Result.SuccessAsync(() => mediator.Send(request));

        return res.IsSuccess
           ? res.Value!
           : new BadRequestObjectResult(res.Errors);
    }

    public static async Task<ActionResult<TResponse>> RequestAsync<TResponse>(this IMediator mediator, IRequest<TResponse> request)
    {
        var res = await Result.SuccessAsync(() => mediator.Send(request));

        return res.IsSuccess
           ? res.Value!
           : new BadRequestObjectResult(res.Errors);
    }
 
    public static async Task<Result<TResponse>> InvokeAsync<TResponse>(this IMediator mediator, IRequest<Result<TResponse>> request)
    {
        return await Result.SuccessAsync(() => mediator.Send(request));
    }

    public static async Task<Result<TResponse>> InvokeAsync<TResponse>(this IMediator mediator, IRequest<TResponse> request)
    {
        return await Result.SuccessAsync(() => mediator.Send(request));
    }
}