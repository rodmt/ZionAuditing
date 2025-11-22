using ZionAuditing.Domain.Common;

namespace ZionAuditing.Domain.ValueObjects;

public sealed class UserId : ValueObject
{
    public string Value { get; }

    private UserId(string value)
    {
        Value = value;
    }

    public static Result<UserId> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<UserId>(
                Error.Validation("UserId.Empty", "User ID cannot be empty"));
        }

        return Result.Success(new UserId(value));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
