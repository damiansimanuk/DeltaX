namespace ECommerce.Shared.Events;
using DeltaX.Core.Abstractions.Event;

public record ProductCreated(
    int ProductId,
    string ProductName,
    string Description
    ) : IntegrationEventBase;