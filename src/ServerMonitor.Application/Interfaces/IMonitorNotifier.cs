namespace ServerMonitor.Application.Interfaces;

using ServerMonitor.Application.DTOs;

public interface IMonitorNotifier
{
    Task SendHealthCheckUpdate(HealthCheckDto check);
    Task SendServerStatusChanged(ServerDto server);
    Task SendIncidentCreated(IncidentDto incident);
    Task SendIncidentResolved(IncidentDto incident);
}
