namespace DeltaX.Core.Hosting.Remote;
using DeltaX.Core.Abstractions.Event;
using DeltaX.Core.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

public class EventBusToMediatorHub(IHubContext<MediatorHubServer> hubContext, EventBus events) : IHostedService
{
    private IDisposable? disposable;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        disposable = events.SubscribeAll(OnNextMessage);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        disposable?.Dispose();
        return Task.CompletedTask;
    }

    private void OnNextMessage(IEvent e)
    {
        try
        {
            hubContext.Clients.Group(e.EventName).SendAsync("OnNextMessage", RequestSerializer.Wrap(e));
        }
        catch { }
    }
}
