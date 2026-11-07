namespace ServerMonitor.API.Hubs;

using Microsoft.AspNetCore.SignalR;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;

public class SignalRMonitorNotifier(IHubContext<MonitorHub> hubContext) : IMonitorNotifier
{
    public Task SendHealthCheckUpdate(HealthCheckDto check) =>
        hubContext.Clients.All.SendAsync("HealthCheckUpdated", check);

    public Task SendServerStatusChanged(ServerDto server) =>
        hubContext.Clients.All.SendAsync("ServerStatusChanged", server);

    public Task SendIncidentCreated(IncidentDto incident) =>
        hubContext.Clients.All.SendAsync("IncidentCreated", incident);

    public Task SendIncidentResolved(IncidentDto incident) =>
        hubContext.Clients.All.SendAsync("IncidentResolved", incident);
}
