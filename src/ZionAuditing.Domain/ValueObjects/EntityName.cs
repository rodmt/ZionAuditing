using ZionAuditing.Domain.Common;

namespace ZionAuditing.Domain.ValueObjects;

public sealed class EntityName : ValueObject
{
    public string Value { get; }

    private EntityName(string value)
    {
        Value = value;
    }

    public static Result<EntityName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<EntityName>(
                Error.Validation("EntityName.Empty", "Entity name cannot be empty"));
        }

        if (value.Length > 255)
        {
            return Result.Failure<EntityName>(
                Error.Validation("EntityName.TooLong", "Entity name cannot exceed 255 characters"));
        }

        return Result.Success(new EntityName(value));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
