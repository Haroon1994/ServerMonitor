namespace ServerMonitor.Application.Interfaces;

using ServerMonitor.Application.DTOs;

public interface IHealthCheckService
{
    Task<List<HealthCheckDto>> GetByServerIdAsync(Guid serverId, int limit = 50);
    Task<HealthCheckDto> RecordCheckAsync(Guid serverId);
}
