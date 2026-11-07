namespace ServerMonitor.Infrastructure.Services;

using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;
using ServerMonitor.Domain.Entities;
using ServerMonitor.Domain.Enums;
using ServerMonitor.Infrastructure.Data;

public class HealthCheckService(AppDbContext db, IHttpClientFactory httpClientFactory, IMonitorNotifier notifier) : IHealthCheckService
{
    public async Task<List<HealthCheckDto>> GetByServerIdAsync(Guid serverId, int limit = 50)
    {
        return await db.HealthChecks
            .Where(h => h.ServerId == serverId)
            .OrderByDescending(h => h.CheckedAt)
            .Take(limit)
            .Select(h => ToDto(h))
            .ToListAsync();
    }

    public async Task<HealthCheckDto> RecordCheckAsync(Guid serverId)
    {
        var server = await db.Servers.FindAsync(serverId)
            ?? throw new ArgumentException("Server not found");

        var client = httpClientFactory.CreateClient("HealthCheck");
        var sw = Stopwatch.StartNew();
        ServerStatus status;
        int? responseTimeMs = null;
        string? errorMessage = null;

        try
        {
            var response = await client.GetAsync(server.Url);
            sw.Stop();
            responseTimeMs = (int)sw.ElapsedMilliseconds;
            status = response.IsSuccessStatusCode ? ServerStatus.Up : ServerStatus.Down;
            if (!response.IsSuccessStatusCode)
                errorMessage = $"HTTP {(int)response.StatusCode}";
        }
        catch (Exception ex)
        {
            sw.Stop();
            status = ServerStatus.Down;
            errorMessage = ex.Message;
        }

        var check = new HealthCheck
        {
            Id = Guid.NewGuid(),
            ServerId = serverId,
            Status = status,
            ResponseTimeMs = responseTimeMs,
            ErrorMessage = errorMessage,
            CheckedAt = DateTime.UtcNow
        };

        db.HealthChecks.Add(check);

        var previousStatus = server.Status;
        server.Status = status;
        server.LastResponseTimeMs = responseTimeMs;
        server.LastCheckedAt = check.CheckedAt;

        Incident? newIncident = null;
        Incident? resolvedIncident = null;

        if (status == ServerStatus.Down && previousStatus != ServerStatus.Down)
        {
            newIncident = new Incident
            {
                Id = Guid.NewGuid(),
                ServerId = serverId,
                ServerName = server.Name,
                StartedAt = DateTime.UtcNow,
                ErrorMessage = errorMessage
            };
            db.Incidents.Add(newIncident);
        }
        else if (status == ServerStatus.Up && previousStatus == ServerStatus.Down)
        {
            resolvedIncident = await db.Incidents
                .Where(i => i.ServerId == serverId && i.ResolvedAt == null)
                .OrderByDescending(i => i.StartedAt)
                .FirstOrDefaultAsync();

            if (resolvedIncident is not null)
                resolvedIncident.ResolvedAt = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();

        var checkDto = ToDto(check);
        await notifier.SendHealthCheckUpdate(checkDto);

        if (previousStatus != status)
        {
            var serverDto = new ServerDto(
                server.Id, server.Name, server.Url, server.CheckIntervalSeconds,
                server.Status, server.LastResponseTimeMs, server.LastCheckedAt,
                server.CreatedAt, server.IsActive);
            await notifier.SendServerStatusChanged(serverDto);
        }

        if (newIncident is not null)
            await notifier.SendIncidentCreated(new IncidentDto(
                newIncident.Id, newIncident.ServerId, newIncident.ServerName,
                newIncident.StartedAt, newIncident.ResolvedAt, newIncident.ErrorMessage));

        if (resolvedIncident is not null)
            await notifier.SendIncidentResolved(new IncidentDto(
                resolvedIncident.Id, resolvedIncident.ServerId, resolvedIncident.ServerName,
                resolvedIncident.StartedAt, resolvedIncident.ResolvedAt, resolvedIncident.ErrorMessage));

        return checkDto;
    }

    private static HealthCheckDto ToDto(HealthCheck h) => new(
        h.Id, h.ServerId, h.Status,
        h.ResponseTimeMs, h.ErrorMessage, h.CheckedAt);
}
