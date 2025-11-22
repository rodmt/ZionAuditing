using ZionAuditing.Application.Common;

namespace ZionAuditing.Application.AuditLogs.Commands;

public sealed record CreateAuditLogCommand(
    string EntityName,
    Guid EntityId,
    string UserId,
    string Action,
    string? OldValues,
    string? NewValues,
    string? IpAddress,
    string? UserAgent) : ICommand<Guid>;
