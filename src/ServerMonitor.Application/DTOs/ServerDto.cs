namespace ServerMonitor.Application.DTOs;

using ServerMonitor.Domain.Enums;

public record ServerDto(
    Guid Id,
    string Name,
    string Url,
    int CheckIntervalSeconds,
    ServerStatus Status,
    int? LastResponseTimeMs,
    DateTime? LastCheckedAt,
    DateTime CreatedAt,
    bool IsActive);

public record CreateServerDto(
    string Name,
    string Url,
    int CheckIntervalSeconds = 60);

public record UpdateServerDto(
    string? Name,
    string? Url,
    int? CheckIntervalSeconds,
    bool? IsActive);
