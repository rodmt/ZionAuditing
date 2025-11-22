using Shouldly;
using Xunit;
using ZionAuditing.Domain.Entities;

namespace ZionAuditing.Domain.UnitTests.Entities;

public sealed class AuditEntryTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldReturnSuccessResult()
    {
        // Arrange
        var propertyName = "FirstName";
        var oldValue = "John";
        var newValue = "Jane";

        // Act
        var result = AuditEntry.Create(propertyName, oldValue, newValue);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.PropertyName.ShouldBe(propertyName);
        result.Value.OldValue.ShouldBe(oldValue);
        result.Value.NewValue.ShouldBe(newValue);
    }

    [Fact]
    public void Create_WithEmptyPropertyName_ShouldReturnFailure()
    {
        // Arrange
        var propertyName = string.Empty;

        // Act
        var result = AuditEntry.Create(propertyName, "old", "new");

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.AuditEntry.PropertyName");
        result.Error.Message.ShouldBe("Property name cannot be empty");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidPropertyName_ShouldReturnFailure(string invalidPropertyName)
    {
        // Arrange & Act
        var result = AuditEntry.Create(invalidPropertyName, "old", "new");

        // Assert
        result.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public void Create_WithNullValues_ShouldSucceed()
    {
        // Arrange
        var propertyName = "OptionalField";

        // Act
        var result = AuditEntry.Create(propertyName, null, null);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.OldValue.ShouldBeNull();
        result.Value.NewValue.ShouldBeNull();
    }

    [Fact]
    public void Create_ShouldGenerateUniqueId()
    {
        // Arrange
        var propertyName = "TestProperty";

        // Act
        var entry1 = AuditEntry.Create(propertyName, "old1", "new1").Value;
        var entry2 = AuditEntry.Create(propertyName, "old2", "new2").Value;

        // Assert
        entry1.Id.ShouldNotBe(Guid.Empty);
        entry2.Id.ShouldNotBe(Guid.Empty);
        entry1.Id.ShouldNotBe(entry2.Id);
    }
}
