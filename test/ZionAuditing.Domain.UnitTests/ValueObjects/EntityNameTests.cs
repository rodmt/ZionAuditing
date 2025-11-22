using Shouldly;
using Xunit;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Domain.UnitTests.ValueObjects;

public sealed class EntityNameTests
{
    [Fact]
    public void Create_WithValidName_ShouldReturnSuccess()
    {
        // Arrange
        var name = "User";

        // Act
        var result = EntityName.Create(name);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ShouldReturnFailure(string invalidName)
    {
        // Arrange & Act
        var result = EntityName.Create(invalidName);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.EntityName.Empty");
        result.Error.Message.ShouldBe("Entity name cannot be empty");
    }

    [Fact]
    public void Create_WithNameExceeding255Characters_ShouldReturnFailure()
    {
        // Arrange
        var longName = new string('A', 256);

        // Act
        var result = EntityName.Create(longName);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.EntityName.TooLong");
        result.Error.Message.ShouldBe("Entity name cannot exceed 255 characters");
    }

    [Fact]
    public void Create_WithMaxLengthName_ShouldReturnSuccess()
    {
        // Arrange
        var maxLengthName = new string('A', 255);

        // Act
        var result = EntityName.Create(maxLengthName);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Value.ShouldBe(maxLengthName);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var name1 = EntityName.Create("User").Value;
        var name2 = EntityName.Create("User").Value;

        // Act & Assert
        name1.Equals(name2).ShouldBeTrue();
        (name1 == name2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var name1 = EntityName.Create("User").Value;
        var name2 = EntityName.Create("Product").Value;

        // Act & Assert
        name1.Equals(name2).ShouldBeFalse();
        (name1 != name2).ShouldBeTrue();
    }

    [Fact]
    public void GetHashCode_WithSameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        var name1 = EntityName.Create("User").Value;
        var name2 = EntityName.Create("User").Value;

        // Act
        var hash1 = name1.GetHashCode();
        var hash2 = name2.GetHashCode();

        // Assert
        hash1.ShouldBe(hash2);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var name = "TestEntity";
        var entityName = EntityName.Create(name).Value;

        // Act
        var result = entityName.ToString();

        // Assert
        result.ShouldBe(name);
    }
}
