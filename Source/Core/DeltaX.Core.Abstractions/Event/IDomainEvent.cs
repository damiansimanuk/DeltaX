namespace DeltaX.Core.Abstractions.Event;

public interface IDomainEvent : IEvent
{
    public bool IsIntegration { get; }
    public DateTimeOffset CreatedAt { get; set; }
}
