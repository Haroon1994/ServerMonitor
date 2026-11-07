namespace ServerMonitor.Application.Interfaces;

using ServerMonitor.Application.DTOs;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}
