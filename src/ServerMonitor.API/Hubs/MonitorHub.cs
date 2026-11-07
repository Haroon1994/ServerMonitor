namespace ServerMonitor.API.Hubs;

using Microsoft.AspNetCore.SignalR;

public class MonitorHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
}
