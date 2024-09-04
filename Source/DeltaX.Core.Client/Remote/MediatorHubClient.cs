namespace DeltaX.Core.Client.Remote;
using DeltaX.Core.Abstractions.Event;
using DeltaX.Core.Common;
using DeltaX.ResultFluent;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

public class MediatorHubClient
{
    private EventBus events = new();
    private HubConnection hub;
    private bool connected;

    public MediatorHubClient(Uri url)
    {
        hub = new HubConnectionBuilder().WithUrl(url).Build();
        events.SubscriptionAdded += OnSubscriptionAdded;
        events.SubscriptionRemoved += OnSubscriptionRemoved;
        hub.On<JsonElement>("OnNextMessage", OnNextMessage);
        hub.Reconnected += OnConnected;
        hub.Closed += OnClosedConnection;
    }

    public async Task StartAsync()
    {
        if (!connected)
        {
            await hub.StartAsync();
            await OnConnected(null!);
        }
    }

    public Task<Result<TResponse>> RequestAsync<TResponse>(IRequest<Result<TResponse>> comando)
    {
        return Result.SuccessAsync(async () =>
        {
            var jsonResponse = await hub.InvokeAsync<JsonElement>("Request", RequestSerializer.Wrap(comando));
            var response = RequestSerializer.Deserialize(jsonResponse);
            return response is Error err
                ? Result.Failure<TResponse>(err)
                : (Result<TResponse>)response;
        });
    }

    public Task<Result<TResponse>> RequestAsync<TResponse>(IRequest<TResponse> comando)
    {
        return Result.SuccessAsync(async () =>
        {
            var jsonResponse = await hub.InvokeAsync<JsonElement>("Request", RequestSerializer.Wrap(comando));
            var response = RequestSerializer.Deserialize(jsonResponse);
            return response is Error err
                ? Result.Failure<TResponse>(err)
                : Result.Success((TResponse)response);
        });
    }

    public IDisposable Subscribe<TEvent>(Action<TEvent> onNextMessage) where TEvent : IEvent
    {
        return events.Subscribe(onNextMessage);
    }

    private void OnNextMessage(JsonElement notification)
    {
        //Console.WriteLine($"MediatorHubClient OnNextMessage {notification}");
        var msg = RequestSerializer.Deserialize(notification) as IEvent;
        events.SendMessage(msg!);
    }

    private void OnSubscriptionAdded(object? sender, string eventName)
    {
        try
        {
            //Console.WriteLine($"MediatorHubClient OnSubscriptionAdded {eventName}");
            hub.InvokeAsync("Subscribe", eventName);
        }
        catch { }
    }

    private void OnSubscriptionRemoved(object? sender, string eventName)
    {
        try
        {
            //Console.WriteLine($"MediatorHubClient OnSubscriptionRemoved {eventName}");
            hub.InvokeAsync("Unsubscribe", eventName);
        }
        catch { }
    }

    private async Task OnConnected(string? arg)
    {
        Console.WriteLine($"MediatorHubClient OnConnected");
        connected = true;
        foreach (var eventName in events.GetEventNames())
        {
            try
            {
                //Console.WriteLine("OnConnected Subscribe {0}", eventName);
                await hub.InvokeAsync("Subscribe", eventName);
            }
            catch { }
        }
    }

    private Task OnClosedConnection(Exception? arg)
    {
        Console.WriteLine($"MediatorHubClient OnClosedConnection ");
        connected = false;
        return Task.CompletedTask;
    }
}
