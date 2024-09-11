namespace DeltaX.Core.Abstractions.Event;

public abstract record DomainEventBase : EventBase, IDomainEvent
{
    public bool IsIntegration { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
