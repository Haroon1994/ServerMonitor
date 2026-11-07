namespace ServerMonitor.Application.DTOs;

public record IncidentDto(
    Guid Id,
    Guid ServerId,
    string ServerName,
    DateTime StartedAt,
    DateTime? ResolvedAt,
    string? ErrorMessage);
