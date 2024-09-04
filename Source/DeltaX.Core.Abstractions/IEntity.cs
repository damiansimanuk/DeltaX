namespace DeltaX.Core.Abstractions;
using DeltaX.Core.Abstractions.Event;

public interface IEntity
{
    DateTimeOffset? UpdatedAt { get; set; }
    DateTimeOffset? CreatedAt { get; set; }
    IReadOnlyCollection<IEvent> GetDomainEvents();
    void ClearDomainEvents();
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}
