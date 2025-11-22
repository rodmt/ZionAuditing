using Shouldly;
using Xunit;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Domain.UnitTests.ValueObjects;

public sealed class UserIdTests
{
    [Fact]
    public void Create_WithValidUserId_ShouldReturnSuccess()
    {
        // Arrange
        var userId = "user@test.com";

        // Act
        var result = UserId.Create(userId);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(userId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidUserId_ShouldReturnFailure(string invalidUserId)
    {
        // Arrange & Act
        var result = UserId.Create(invalidUserId);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.UserId.Empty");
        result.Error.Message.ShouldBe("User ID cannot be empty");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var userId1 = UserId.Create("admin@test.com").Value;
        var userId2 = UserId.Create("admin@test.com").Value;

        // Act & Assert
        userId1.Equals(userId2).ShouldBeTrue();
        (userId1 == userId2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var userId1 = UserId.Create("admin@test.com").Value;
        var userId2 = UserId.Create("user@test.com").Value;

        // Act & Assert
        userId1.Equals(userId2).ShouldBeFalse();
        (userId1 != userId2).ShouldBeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var userIdValue = "test@example.com";
        var userId = UserId.Create(userIdValue).Value;

        // Act
        var result = userId.ToString();

        // Assert
        result.ShouldBe(userIdValue);
    }
}
