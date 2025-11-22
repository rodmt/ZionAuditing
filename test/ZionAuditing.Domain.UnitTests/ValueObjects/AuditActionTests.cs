using Shouldly;
using Xunit;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Domain.UnitTests.ValueObjects;

public sealed class AuditActionTests
{
    [Theory]
    [InlineData("Create")]
    [InlineData("Update")]
    [InlineData("Delete")]
    [InlineData("Read")]
    [InlineData("Execute")]
    public void From_WithValidAction_ShouldReturnSuccess(string actionValue)
    {
        // Arrange & Act
        var result = AuditAction.From(actionValue);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(actionValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void From_WithInvalidAction_ShouldReturnFailure(string invalidAction)
    {
        // Arrange & Act
        var result = AuditAction.From(invalidAction);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.AuditAction.Empty");
        result.Error.Message.ShouldBe("Audit action cannot be empty");
    }

    [Fact]
    public void PredefinedActions_ShouldHaveCorrectValues()
    {
        // Arrange & Act & Assert
        AuditAction.Create.Value.ShouldBe("Create");
        AuditAction.Update.Value.ShouldBe("Update");
        AuditAction.Delete.Value.ShouldBe("Delete");
        AuditAction.Read.Value.ShouldBe("Read");
        AuditAction.Execute.Value.ShouldBe("Execute");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var action1 = AuditAction.From("Create").Value;
        var action2 = AuditAction.From("Create").Value;

        // Act & Assert
        action1.Equals(action2).ShouldBeTrue();
        (action1 == action2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var action1 = AuditAction.Create;
        var action2 = AuditAction.Update;

        // Act & Assert
        action1.Equals(action2).ShouldBeFalse();
        (action1 != action2).ShouldBeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var action = AuditAction.Create;

        // Act
        var result = action.ToString();

        // Assert
        result.ShouldBe("Create");
    }
}
