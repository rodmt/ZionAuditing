using ZionAuditing.Domain.Common;

namespace ZionAuditing.Domain.Events;

public sealed record AuditLogCreatedEvent(Guid AuditLogId) : DomainEvent;
