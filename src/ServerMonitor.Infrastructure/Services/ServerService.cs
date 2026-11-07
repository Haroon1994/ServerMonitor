namespace ServerMonitor.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;
using ServerMonitor.Domain.Entities;
using ServerMonitor.Infrastructure.Data;

public class ServerService(AppDbContext db) : IServerService
{
    public async Task<List<ServerDto>> GetAllAsync()
    {
        return await db.Servers
            .OrderBy(s => s.Name)
            .Select(s => ToDto(s))
            .ToListAsync();
    }

    public async Task<ServerDto?> GetByIdAsync(Guid id)
    {
        var server = await db.Servers.FindAsync(id);
        return server is null ? null : ToDto(server);
    }

    public async Task<ServerDto> CreateAsync(CreateServerDto dto)
    {
        var server = new MonitoredServer
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Url = dto.Url,
            CheckIntervalSeconds = dto.CheckIntervalSeconds,
            CreatedAt = DateTime.UtcNow
        };

        db.Servers.Add(server);
        await db.SaveChangesAsync();
        return ToDto(server);
    }

    public async Task<ServerDto?> UpdateAsync(Guid id, UpdateServerDto dto)
    {
        var server = await db.Servers.FindAsync(id);
        if (server is null) return null;

        if (dto.Name is not null) server.Name = dto.Name;
        if (dto.Url is not null) server.Url = dto.Url;
        if (dto.CheckIntervalSeconds.HasValue) server.CheckIntervalSeconds = dto.CheckIntervalSeconds.Value;
        if (dto.IsActive.HasValue) server.IsActive = dto.IsActive.Value;

        await db.SaveChangesAsync();
        return ToDto(server);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var server = await db.Servers.FindAsync(id);
        if (server is null) return false;

        db.Servers.Remove(server);
        await db.SaveChangesAsync();
        return true;
    }

    private static ServerDto ToDto(MonitoredServer s) => new(
        s.Id, s.Name, s.Url, s.CheckIntervalSeconds,
        s.Status, s.LastResponseTimeMs, s.LastCheckedAt,
        s.CreatedAt, s.IsActive);
}
