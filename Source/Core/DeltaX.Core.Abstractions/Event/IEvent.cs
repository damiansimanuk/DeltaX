namespace DeltaX.Core.Abstractions.Event;

public interface IEvent
{
    public string EventId { get; }
    public string EventName { get; }
}
