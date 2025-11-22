using ZionAuditing.Domain.Common;
using ZionAuditing.Domain.Events;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Domain.Entities;

public sealed class AuditLog : AggregateRoot
{
    private readonly List<AuditEntry> _entries = [];

    public EntityName EntityName { get; private set; }
    public Guid EntityId { get; private set; }
    public UserId UserId { get; private set; }
    public AuditAction Action { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public IReadOnlyList<AuditEntry> Entries => _entries.AsReadOnly();

    private AuditLog(
        EntityName entityName,
        Guid entityId,
        UserId userId,
        AuditAction action,
        string? oldValues,
        string? newValues,
        string? ipAddress,
        string? userAgent)
    {
        EntityName = entityName;
        EntityId = entityId;
        UserId = userId;
        Action = action;
        OldValues = oldValues;
        NewValues = newValues;
        Timestamp = DateTime.UtcNow;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public static Result<AuditLog> Create(
        EntityName entityName,
        Guid entityId,
        UserId userId,
        AuditAction action,
        string? oldValues,
        string? newValues,
        string? ipAddress,
        string? userAgent)
    {
        var auditLog = new AuditLog(
            entityName,
            entityId,
            userId,
            action,
            oldValues,
            newValues,
            ipAddress,
            userAgent);

        auditLog.AddDomainEvent(new AuditLogCreatedEvent(auditLog.Id));

        return Result.Success(auditLog);
    }

    public Result AddEntry(string propertyName, string? oldValue, string? newValue)
    {
        var entry = AuditEntry.Create(propertyName, oldValue, newValue);
        if (entry.IsFailure)
        {
            return Result.Failure(entry.Error);
        }

        _entries.Add(entry.Value);
        return Result.Success();
    }
}
