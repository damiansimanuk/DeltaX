namespace DeltaX.Core.Client;
using DeltaX.Core.Abstractions.Event;
using DeltaX.Core.Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ClientViewModelBase : INotifyPropertyChanged
{
    EventBus eventBus = new();
    public event PropertyChangedEventHandler? PropertyChanged;

    public IDisposable Subscribe<TE>(Action<TE> onNextMessage) where TE : IEvent
    {
        return eventBus.Subscribe(onNextMessage);
    }

    public void SendMessage(params IEvent[] events)
    {
        eventBus.SendMessage(events);
    }

    protected virtual bool SetProp<T>(ref T field, T value, [CallerMemberName] string propertyName = null!)
    {
        return SetProperty<T, IEvent>(ref field, value, null, propertyName);
    }

    protected virtual bool SetProp<T, TE>(ref T field, T value, Func<TE>? eventFunction, [CallerMemberName] string propertyName = null!)
        where TE : IEvent
    {
        return SetProperty(ref field, value, eventFunction, propertyName);
    }

    protected virtual bool SetProperty<T, TE>(ref T field, T value, Func<TE>? eventFunction, string propertyName)
        where TE : IEvent
    {
        if (field == null && value != null || field != null && !field.Equals(value))
        {
            field = value;
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

                var ev = eventFunction != null ? eventFunction.Invoke() : default;
                if (ev != null)
                {
                    eventBus.SendMessage(ev);
                }
            }
            catch { }
            return true;
        }

        return false;
    }
}
