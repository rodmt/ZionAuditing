using ZionAuditing.Application.AuditLogs.DTOs;
using ZionAuditing.Application.Common;

namespace ZionAuditing.Application.AuditLogs.Queries;

public sealed record GetAuditLogByIdQuery(Guid Id) : IQuery<AuditLogDto>;
