namespace ServerMonitor.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;
using ServerMonitor.Infrastructure.Data;

public class IncidentService(AppDbContext db) : IIncidentService
{
    public async Task<List<IncidentDto>> GetAllAsync(int limit = 20)
    {
        return await db.Incidents
            .OrderByDescending(i => i.StartedAt)
            .Take(limit)
            .Select(i => new IncidentDto(
                i.Id, i.ServerId, i.ServerName,
                i.StartedAt, i.ResolvedAt, i.ErrorMessage))
            .ToListAsync();
    }

    public async Task<List<IncidentDto>> GetByServerIdAsync(Guid serverId)
    {
        return await db.Incidents
            .Where(i => i.ServerId == serverId)
            .OrderByDescending(i => i.StartedAt)
            .Select(i => new IncidentDto(
                i.Id, i.ServerId, i.ServerName,
                i.StartedAt, i.ResolvedAt, i.ErrorMessage))
            .ToListAsync();
    }
}
