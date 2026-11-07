namespace ServerMonitor.Application.DTOs;

public record DashboardDto(
    int TotalServers,
    int ServersUp,
    int ServersDown,
    double UptimePercentage,
    List<IncidentDto> RecentIncidents,
    List<ServerDto> Servers);
