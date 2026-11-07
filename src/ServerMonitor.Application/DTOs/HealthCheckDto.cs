namespace ServerMonitor.Application.DTOs;

using ServerMonitor.Domain.Enums;

public record HealthCheckDto(
    Guid Id,
    Guid ServerId,
    ServerStatus Status,
    int? ResponseTimeMs,
    string? ErrorMessage,
    DateTime CheckedAt);
