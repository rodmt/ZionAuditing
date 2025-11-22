using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using Shouldly;
using Xunit;
using ZionAuditing.Application.Behaviors;
using ZionAuditing.Application.Common;
using ZionAuditing.Domain.Common;

namespace ZionAuditing.Application.UnitTests.Behaviors;

public sealed class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WithNoValidators_ShouldCallNext()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestCommand>>();
        var behavior = new ValidationBehavior<TestCommand, Result>(validators);
        var request = new TestCommand();
        var expectedResult = Result.Success();
        var next = Substitute.For<RequestHandlerDelegate<Result>>();
        next().Returns(expectedResult);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.ShouldBe(expectedResult);
        await next.Received(1)();
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCallNext()
    {
        // Arrange
        var validator = Substitute.For<IValidator<TestCommand>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<TestCommand>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var validators = new[] { validator };
        var behavior = new ValidationBehavior<TestCommand, Result>(validators);
        var request = new TestCommand();
        var expectedResult = Result.Success();
        var next = Substitute.For<RequestHandlerDelegate<Result>>();
        next().Returns(expectedResult);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.ShouldBe(expectedResult);
        await next.Received(1)();
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ShouldReturnFailureWithoutCallingNext()
    {
        // Arrange
        var validationFailure = new ValidationFailure("Property", "Error message");
        var validator = Substitute.For<IValidator<TestCommand>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<TestCommand>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { validationFailure }));

        var validators = new[] { validator };
        var behavior = new ValidationBehavior<TestCommand, Result>(validators);
        var request = new TestCommand();
        var next = Substitute.For<RequestHandlerDelegate<Result>>();

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.Validation.Failed");
        result.Error.Message.ShouldContain("Error message");
        await next.DidNotReceive()();
    }

    [Fact]
    public async Task Handle_WithMultipleValidationErrors_ShouldCombineErrorMessages()
    {
        // Arrange
        var validationFailures = new[]
        {
            new ValidationFailure("Property1", "Error 1"),
            new ValidationFailure("Property2", "Error 2")
        };
        var validator = Substitute.For<IValidator<TestCommand>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<TestCommand>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));

        var validators = new[] { validator };
        var behavior = new ValidationBehavior<TestCommand, Result>(validators);
        var request = new TestCommand();
        var next = Substitute.For<RequestHandlerDelegate<Result>>();

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Message.ShouldContain("Error 1");
        result.Error.Message.ShouldContain("Error 2");
        result.Error.Message.ShouldContain(";");
    }

    private sealed record TestCommand : ICommand;
}
