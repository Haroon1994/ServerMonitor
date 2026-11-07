namespace ServerMonitor.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;
using ServerMonitor.Domain.Enums;
using ServerMonitor.Infrastructure.Data;

public class DashboardService(AppDbContext db) : IDashboardService
{
    public async Task<DashboardDto> GetDashboardAsync()
    {
        var servers = await db.Servers
            .OrderBy(s => s.Name)
            .ToListAsync();

        var total = servers.Count;
        var up = servers.Count(s => s.Status == ServerStatus.Up);
        var down = servers.Count(s => s.Status == ServerStatus.Down);

        var since = DateTime.UtcNow.AddHours(-24);
        var recentChecks = await db.HealthChecks
            .Where(h => h.CheckedAt >= since)
            .ToListAsync();

        var uptime = recentChecks.Count > 0
            ? recentChecks.Count(c => c.Status == ServerStatus.Up) * 100.0 / recentChecks.Count
            : 0;

        var recentIncidents = await db.Incidents
            .OrderByDescending(i => i.StartedAt)
            .Take(10)
            .Select(i => new IncidentDto(
                i.Id, i.ServerId, i.ServerName,
                i.StartedAt, i.ResolvedAt, i.ErrorMessage))
            .ToListAsync();

        var serverDtos = servers.Select(s => new ServerDto(
            s.Id, s.Name, s.Url, s.CheckIntervalSeconds,
            s.Status, s.LastResponseTimeMs, s.LastCheckedAt,
            s.CreatedAt, s.IsActive)).ToList();

        return new DashboardDto(total, up, down, Math.Round(uptime, 2), recentIncidents, serverDtos);
    }
}
