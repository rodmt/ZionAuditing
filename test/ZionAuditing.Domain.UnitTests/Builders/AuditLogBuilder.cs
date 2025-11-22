using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Domain.UnitTests.Builders;

public sealed class AuditLogBuilder
{
    private EntityName _entityName = EntityName.Create("TestEntity").Value;
    private Guid _entityId = Guid.NewGuid();
    private UserId _userId = UserId.Create("test-user").Value;
    private AuditAction _action = AuditAction.Create;
    private string? _oldValues = null;
    private string? _newValues = null;
    private string? _ipAddress = "127.0.0.1";
    private string? _userAgent = "TestAgent/1.0";

    public AuditLogBuilder WithEntityName(string entityName)
    {
        _entityName = EntityName.Create(entityName).Value;
        return this;
    }

    public AuditLogBuilder WithEntityId(Guid entityId)
    {
        _entityId = entityId;
        return this;
    }

    public AuditLogBuilder WithUserId(string userId)
    {
        _userId = UserId.Create(userId).Value;
        return this;
    }

    public AuditLogBuilder WithAction(AuditAction action)
    {
        _action = action;
        return this;
    }

    public AuditLogBuilder WithOldValues(string oldValues)
    {
        _oldValues = oldValues;
        return this;
    }

    public AuditLogBuilder WithNewValues(string newValues)
    {
        _newValues = newValues;
        return this;
    }

    public AuditLogBuilder WithIpAddress(string ipAddress)
    {
        _ipAddress = ipAddress;
        return this;
    }

    public AuditLogBuilder WithUserAgent(string userAgent)
    {
        _userAgent = userAgent;
        return this;
    }

    public AuditLog Build()
    {
        return AuditLog.Create(
            _entityName,
            _entityId,
            _userId,
            _action,
            _oldValues,
            _newValues,
            _ipAddress,
            _userAgent).Value;
    }
}
