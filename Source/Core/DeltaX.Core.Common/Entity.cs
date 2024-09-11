namespace DeltaX.Core.Common;
using DeltaX.Core.Abstractions;
using DeltaX.Core.Abstractions.Event;
using System.Collections.Concurrent;
using System.Collections.Generic;

public abstract class Entity<TKey> : IEntity<TKey>
{
    ConcurrentQueue<Func<IDomainEvent>> domainEvents = new();

    public TKey Id { get; protected set; } = default!;
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return domainEvents.Select(e => e.Invoke()).ToList();
    }

    public void AddDomainEvent(Func<IDomainEvent> eventItem)
    {
        domainEvents.Enqueue(eventItem);
    }

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }
}
