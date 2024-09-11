namespace DeltaX.Core.Hosting.Database;

using DeltaX.Core.Abstractions;
using DeltaX.Core.Abstractions.Event;
using DeltaX.Core.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

public abstract class UnitOfWorkContext : DbContext
{
    private EventBus? eventBus;

    ConcurrentQueue<Func<IEvent>> domainEvents = new();

    public UnitOfWorkContext(DbContextOptions options, EventBus eventBus) : base(options)
    {
        this.eventBus = eventBus;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = EntitiesChanged();
        FillUpdatedAt();
        var result = await base.SaveChangesAsync(cancellationToken);
        PublishDomainEvents(entities);
        PublishLocalEvents();
        ChangeTracker.Clear();
        return result;
    }

    public void AddDomainEvent(Func<IEvent> eventItem)
    {
        domainEvents.Enqueue(eventItem);
    }

    protected virtual List<IEntity> EntitiesChanged()
    {
        return ChangeTracker
            .Entries<IEntity>()
            .Where(entry => entry.State == EntityState.Modified || entry.State == EntityState.Added || entry.State == EntityState.Deleted)
            .Select(e => e.Entity)
            .ToList();
    }

    protected virtual void PublishDomainEvents(List<IEntity> entities)
    {
        if (entities.Any())
        {
            var events = entities
                .Select(e => e.GetDomainEvents())
                .Where(e => e != null)
                .SelectMany(e => e).ToArray();

            entities.ForEach(e => e.ClearDomainEvents());

            SendMessage(events);
        }
    }

    protected virtual void PublishLocalEvents()
    {
        var events = domainEvents.Select(e => e.Invoke()).ToArray();
        if (events.Any())
        {
            SendMessage(events);
        }
    }

    protected virtual void SendMessage(params IEvent[] events)
    {
        eventBus?.SendMessage(events);
    }

    protected virtual void FillUpdatedAt()
    {
        ChangeTracker.Entries<IEntity>()
            .Where(entry => entry.State == EntityState.Modified || entry.State == EntityState.Added)
            .Select(entry => entry.Member(nameof(IEntity.UpdatedAt)))
            .Where(member => member != null)
            .ToList()
            .ForEach(member => member.CurrentValue = DateTimeOffset.UtcNow);
    }
}