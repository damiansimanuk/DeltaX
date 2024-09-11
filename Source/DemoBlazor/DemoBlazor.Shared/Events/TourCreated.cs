namespace DemoBlazor.Shared.Events;
using DeltaX.Core.Abstractions.Event;

public record TourCreated(
    int TourId,
    string TourName,
    string Description
    ) : IntegrationEventBase;