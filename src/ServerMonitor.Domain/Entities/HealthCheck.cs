namespace ServerMonitor.Domain.Entities;

using ServerMonitor.Domain.Enums;

public class HealthCheck
{
    public Guid Id { get; set; }

    public Guid ServerId { get; set; }
    public MonitoredServer Server { get; set; } = null!;

    public ServerStatus Status { get; set; }
    public int? ResponseTimeMs { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CheckedAt { get; set; }
}
