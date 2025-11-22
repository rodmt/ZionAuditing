using ZionAuditing.Domain.Common;

namespace ZionAuditing.Domain.ValueObjects;

public sealed class AuditAction : ValueObject
{
    public string Value { get; }

    private AuditAction(string value)
    {
        Value = value;
    }

    public static readonly AuditAction Create = new("Create");
    public static readonly AuditAction Update = new("Update");
    public static readonly AuditAction Delete = new("Delete");
    public static readonly AuditAction Read = new("Read");
    public static readonly AuditAction Execute = new("Execute");

    public static Result<AuditAction> From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<AuditAction>(
                Error.Validation("AuditAction.Empty", "Audit action cannot be empty"));
        }

        return Result.Success(new AuditAction(value));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
