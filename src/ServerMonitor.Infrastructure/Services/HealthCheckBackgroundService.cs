namespace ServerMonitor.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerMonitor.Application.Interfaces;
using ServerMonitor.Infrastructure.Data;

public class HealthCheckBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<HealthCheckBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Health check background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunChecksAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error running health checks");
            }

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }

    private async Task RunChecksAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var healthCheckService = scope.ServiceProvider.GetRequiredService<IHealthCheckService>();

        var now = DateTime.UtcNow;
        var servers = db.Servers
            .Where(s => s.IsActive)
            .Where(s => s.LastCheckedAt == null
                || s.LastCheckedAt.Value.AddSeconds(s.CheckIntervalSeconds) <= now)
            .ToList();

        foreach (var server in servers)
        {
            if (stoppingToken.IsCancellationRequested) break;

            try
            {
                var result = await healthCheckService.RecordCheckAsync(server.Id);
                logger.LogInformation("Checked {Name}: {Status} ({ResponseTime}ms)",
                    server.Name, result.Status, result.ResponseTimeMs);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to check server {Name}", server.Name);
            }
        }
    }
}
