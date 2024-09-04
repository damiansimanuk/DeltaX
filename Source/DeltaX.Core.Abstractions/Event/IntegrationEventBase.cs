namespace DeltaX.Core.Abstractions.Event;

public abstract record IntegrationEventBase : EventBase
{
    public IntegrationEventBase()
    {
        IsIntegration = true;
    }
}
