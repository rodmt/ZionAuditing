using ZionAuditing.Domain.Common;

namespace ZionAuditing.Domain.Entities;

public sealed class AuditEntry : Entity
{
    public string PropertyName { get; private set; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }

    private AuditEntry(string propertyName, string? oldValue, string? newValue)
    {
        PropertyName = propertyName;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public static Result<AuditEntry> Create(string propertyName, string? oldValue, string? newValue)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return Result.Failure<AuditEntry>(
                Error.Validation("AuditEntry.PropertyName", "Property name cannot be empty"));
        }

        return Result.Success(new AuditEntry(propertyName, oldValue, newValue));
    }
}
