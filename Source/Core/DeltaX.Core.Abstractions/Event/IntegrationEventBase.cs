namespace DeltaX.Core.Abstractions.Event;

public abstract record IntegrationEventBase : DomainEventBase
{
    public IntegrationEventBase()
    {
        IsIntegration = true;
    }
}
