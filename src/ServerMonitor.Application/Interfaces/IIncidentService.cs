namespace ServerMonitor.Application.Interfaces;

using ServerMonitor.Application.DTOs;

public interface IIncidentService
{
    Task<List<IncidentDto>> GetAllAsync(int limit = 20);
    Task<List<IncidentDto>> GetByServerIdAsync(Guid serverId);
}
