namespace DeltaX.Core.Common;
using DeltaX.Core.Abstractions.Event;
using System.Collections.Concurrent;

public class EventBus
{
    private const string allMessages = "ALLMESSAGES";
    private ConcurrentDictionary<string, List<Action<IEvent>>> eventObservers = new();

    public event EventHandler<string> SubscriptionRemoved = default!;
    public event EventHandler<string> SubscriptionAdded = default!;

    class ActionOnDispose(Action onDispose, IDisposable? disposable = null) : IDisposable
    {
        public void Dispose()
        {
            onDispose?.Invoke();
            disposable?.Dispose();
        }
    }

    public IDisposable Subscribe<TE>(Action<TE> onNextMessage) where TE : IEvent
    {
        return Subscribe(typeof(TE).Name, (e) => onNextMessage((TE)e));
    }

    public IDisposable SubscribeAll(Action<IEvent> onNextMessage)
    {
        return Subscribe(allMessages, onNextMessage);
    }

    public IDisposable Subscribe(string eventName, Action<IEvent> observer)
    {
        lock (eventObservers)
        {
            Console.WriteLine("Try Subscribe " + eventName);
            if (!eventObservers.TryGetValue(eventName, out var observers))
            {
                observers = new List<Action<IEvent>>();
                eventObservers.TryAdd(eventName, observers);
            }

            observers.Add(observer);
            Console.WriteLine("observers count" + observers.Count());
            SubscriptionAdded?.Invoke(this, eventName);
        }

        return new ActionOnDispose(() => OnDisposeObserver(eventName, observer));
    }

    public int CountSubscriptions(string eventName)
    {
        return eventObservers.TryGetValue(eventName, out var observers) ? observers.Count : 0;
    }

    public ICollection<string> GetEventNames()
    {
        return eventObservers.Keys;
    }

    public void SendMessage(params IEvent[] events)
    {
        lock (eventObservers)
        {
            foreach (var domainEvent in events)
            {
                if (eventObservers.TryGetValue(allMessages, out var observersAll))
                {
                    foreach (var observer in observersAll)
                    {
                        observer?.Invoke(domainEvent);
                    }
                }

                if (eventObservers.TryGetValue(domainEvent.EventName, out var observers))
                {
                    foreach (var observer in observers)
                    {
                        observer?.Invoke(domainEvent);
                    }
                }
            }
        }
    }

    private void OnDisposeObserver(string eventName, Action<IEvent> observer)
    {
        if (eventObservers.TryGetValue(eventName, out var observers))
        {
            observers.Remove(observer);

            if (observers.Count == 0)
            {
                eventObservers.Remove(eventName, out _);
                SubscriptionRemoved?.Invoke(this, eventName);
            }
        }
    }
}
