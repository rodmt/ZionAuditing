namespace ZionAuditing.Domain.Common;

public sealed record Error
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

    public string Code { get; }
    public string Message { get; }

    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error NotFound(string entity, Guid id) =>
        new($"{entity}.NotFound", $"{entity} with ID '{id}' was not found");

    public static Error Validation(string code, string message) =>
        new($"Validation.{code}", message);

    public static Error Conflict(string code, string message) =>
        new($"Conflict.{code}", message);

    public static Error Failure(string code, string message) =>
        new($"Failure.{code}", message);
}
