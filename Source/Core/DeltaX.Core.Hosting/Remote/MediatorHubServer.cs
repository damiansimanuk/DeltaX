namespace DeltaX.Core.Hosting.Remote;
using DeltaX.Core.Common;
using DeltaX.ResultFluent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class MediatorHubServer(
    IMediator mediator,
    ILogger<MediatorHubServer> logger
    ) : Hub
{
    HashSet<string> subscriptions = new();
    List<IDisposable> disposables = new();

    public const string EndpointName = "/signalr-mediator";

    [Authorize]
    public async Task<object> Request(JsonElement request)
    {
        try
        {
            var content = RequestSerializer.Deserialize(request);
            var res = await mediator.Send(content);
            return RequestSerializer.Wrap(res!);
        }
        catch (Exception ex)
        {
            ex = ex.InnerException ?? ex;
            var code = ex.GetType().Name;
            var error = Error.Create(code, ex.Message, ex.StackTrace);
            return RequestSerializer.Wrap(error);
        }
    }

    public async Task Subscribe(string eventName)
    {
        logger.LogDebug("Subscribe {0}", eventName);
        await Groups.AddToGroupAsync(Context.ConnectionId, eventName);
    }

    public async Task Unsubscribe(string eventName)
    {
        logger.LogDebug("Unsubscribe {0}", eventName);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, eventName);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        logger.LogDebug("OnDisconnectedAsync");
        return base.OnDisconnectedAsync(exception);
    }

    public override Task OnConnectedAsync()
    {
        logger.LogDebug("OnConnectedAsync");
        return base.OnConnectedAsync();
    }
}
