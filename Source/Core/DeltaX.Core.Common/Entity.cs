namespace DeltaX.Core.Common;
using DeltaX.Core.Abstractions;
using DeltaX.Core.Abstractions.Event;
using System.Collections.Concurrent;
using System.Collections.Generic;

public abstract class Entity<TKey> : IEntity<TKey>, IEquatable<Entity<TKey>>
{
    int? requestedHashCode;
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


    public bool IsTransient()
    {
        return Id?.Equals(default) == true;
    }

    public override int GetHashCode()
    {
        requestedHashCode ??= IsTransient()
                ? base.GetHashCode()
                : Id!.GetHashCode() ^ 31;

        return requestedHashCode.Value;
    }

    public bool Equals(Entity<TKey>? other)
    {
        if (other == null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, other))
        {
            return true;
        }

        if (other.IsTransient() || IsTransient())
        {
            return false;
        }
        else
        {
            return Id!.Equals(other.Id);
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Entity<TKey>);
    }

    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
    {
        return Object.Equals(left, null) ? Object.Equals(right, null) : left.Equals(right);
    }

    public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
    {
        return !(left == right);
    }
}
