namespace DeltaX.Core.Abstractions.Event;

public interface IEvent
{
    public string EventId { get; }
    public string EventName { get; }
    public bool IsIntegration { get; }
    public DateTimeOffset CreatedAt { get; set; }
}
