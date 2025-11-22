using Shouldly;
using Xunit;
using ZionAuditing.Domain.Common;

namespace ZionAuditing.Domain.UnitTests.Common;

public sealed class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        // Arrange
        var error = Error.Validation("Test.Error", "Test error message");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void GenericSuccess_ShouldCreateSuccessResultWithValue()
    {
        // Arrange
        var value = 42;

        // Act
        var result = Result.Success(value);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
        result.Value.ShouldBe(value);
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void GenericFailure_ShouldCreateFailureResult()
    {
        // Arrange
        var error = Error.Validation("Test.Error", "Test error message");

        // Act
        var result = Result.Failure<int>(error);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void Value_OnFailureResult_ShouldThrowException()
    {
        // Arrange
        var result = Result.Failure<int>(Error.Validation("Test", "Test"));

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => result.Value)
            .Message.ShouldBe("Cannot access value of a failure result");
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldCreateSuccessResult()
    {
        // Arrange
        var value = "test";

        // Act
        Result<string> result = value;

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void Constructor_WithSuccessAndError_ShouldThrowException()
    {
        // Arrange
        var error = Error.Validation("Test", "Test");

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => new Result(true, error))
            .Message.ShouldBe("Success result cannot have an error");
    }

    [Fact]
    public void Constructor_WithFailureAndNoError_ShouldThrowException()
    {
        // Arrange & Act & Assert
        Should.Throw<InvalidOperationException>(() => new Result(false, Error.None))
            .Message.ShouldBe("Failure result must have an error");
    }
}
