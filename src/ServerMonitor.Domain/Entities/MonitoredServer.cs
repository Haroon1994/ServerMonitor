namespace ServerMonitor.Domain.Entities;

using ServerMonitor.Domain.Enums;

public class MonitoredServer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int CheckIntervalSeconds { get; set; } = 60;
    public ServerStatus Status { get; set; } = ServerStatus.Unknown;
    public int? LastResponseTimeMs { get; set; }
    public DateTime? LastCheckedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<HealthCheck> HealthChecks { get; set; } = new List<HealthCheck>();
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
