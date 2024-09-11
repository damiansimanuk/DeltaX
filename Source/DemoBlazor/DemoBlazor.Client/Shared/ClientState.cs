namespace DemoBlazor.Client.Shared;

using DeltaX.Core.Abstractions.Event;
using DeltaX.Core.Client;

public class ClientState : ClientViewModelBase
{
    private bool showSidebar = true;

    public bool ShowSidebar
    {
        get => showSidebar;
        set => SetProp(ref showSidebar, value, () => new SidebarVisibilityChanged(showSidebar));
    }

    public ClientState()
    {
        Subscribe<SidebarVisibilityChanged>(e => ShowSidebar = e.Visible);
    }
}

public record SidebarVisibilityChanged(bool Visible) : EventBase;
