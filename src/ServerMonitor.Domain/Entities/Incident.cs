namespace ServerMonitor.Domain.Entities;

public class Incident
{
    public Guid Id { get; set; }

    public Guid ServerId { get; set; }
    public MonitoredServer Server { get; set; } = null!;

    public string ServerName { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
