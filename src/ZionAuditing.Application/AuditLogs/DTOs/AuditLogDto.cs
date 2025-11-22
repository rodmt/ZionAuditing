namespace ZionAuditing.Application.AuditLogs.DTOs;

public sealed record AuditLogDto(
    Guid Id,
    string EntityName,
    Guid EntityId,
    string UserId,
    string Action,
    string? OldValues,
    string? NewValues,
    DateTime Timestamp,
    string? IpAddress,
    string? UserAgent);
