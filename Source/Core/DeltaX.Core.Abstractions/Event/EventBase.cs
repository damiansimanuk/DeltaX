namespace DeltaX.Core.Abstractions.Event;

public abstract record EventBase : IEvent
{
    private static string guid = Guid.NewGuid().ToString("D");

    private static int count = 1;
    public string EventName { get; set; }
    public string EventId { get; }

    protected EventBase()
    {
        EventId ??= $"{guid}-{++count}";
        EventName ??= GetType().Name;
    }
}