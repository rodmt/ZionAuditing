using Shouldly;
using Xunit;
using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.UnitTests.Builders;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Domain.UnitTests.Entities;

public sealed class AuditLogTests
{
    [Fact]
    public void Create_WithValidParameters_ShouldReturnSuccessResult()
    {
        // Arrange
        var entityName = EntityName.Create("User").Value;
        var entityId = Guid.NewGuid();
        var userId = UserId.Create("admin@test.com").Value;
        var action = AuditAction.Update;
        var oldValues = "{\"name\":\"John\"}";
        var newValues = "{\"name\":\"Jane\"}";
        var ipAddress = "192.168.1.1";
        var userAgent = "Mozilla/5.0";

        // Act
        var result = AuditLog.Create(
            entityName,
            entityId,
            userId,
            action,
            oldValues,
            newValues,
            ipAddress,
            userAgent);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.EntityName.ShouldBe(entityName);
        result.Value.EntityId.ShouldBe(entityId);
        result.Value.UserId.ShouldBe(userId);
        result.Value.Action.ShouldBe(action);
        result.Value.OldValues.ShouldBe(oldValues);
        result.Value.NewValues.ShouldBe(newValues);
        result.Value.IpAddress.ShouldBe(ipAddress);
        result.Value.UserAgent.ShouldBe(userAgent);
    }

    [Fact]
    public void Create_ShouldSetTimestampToUtcNow()
    {
        // Arrange
        var before = DateTime.UtcNow;
        var builder = new AuditLogBuilder();

        // Act
        var auditLog = builder.Build();
        var after = DateTime.UtcNow;

        // Assert
        auditLog.Timestamp.ShouldBeGreaterThanOrEqualTo(before);
        auditLog.Timestamp.ShouldBeLessThanOrEqualTo(after);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueId()
    {
        // Arrange
        var builder = new AuditLogBuilder();

        // Act
        var auditLog1 = builder.Build();
        var auditLog2 = builder.Build();

        // Assert
        auditLog1.Id.ShouldNotBe(Guid.Empty);
        auditLog2.Id.ShouldNotBe(Guid.Empty);
        auditLog1.Id.ShouldNotBe(auditLog2.Id);
    }

    [Fact]
    public void Create_ShouldAddDomainEvent()
    {
        // Arrange
        var builder = new AuditLogBuilder();

        // Act
        var auditLog = builder.Build();

        // Assert
        auditLog.DomainEvents.ShouldNotBeEmpty();
        auditLog.DomainEvents.Count.ShouldBe(1);
    }

    [Fact]
    public void AddEntry_WithValidParameters_ShouldReturnSuccess()
    {
        // Arrange
        var auditLog = new AuditLogBuilder().Build();
        var propertyName = "Email";
        var oldValue = "old@test.com";
        var newValue = "new@test.com";

        // Act
        var result = auditLog.AddEntry(propertyName, oldValue, newValue);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        auditLog.Entries.Count.ShouldBe(1);
        auditLog.Entries[0].PropertyName.ShouldBe(propertyName);
        auditLog.Entries[0].OldValue.ShouldBe(oldValue);
        auditLog.Entries[0].NewValue.ShouldBe(newValue);
    }

    [Fact]
    public void AddEntry_WithEmptyPropertyName_ShouldReturnFailure()
    {
        // Arrange
        var auditLog = new AuditLogBuilder().Build();

        // Act
        var result = auditLog.AddEntry(string.Empty, "old", "new");

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.AuditEntry.PropertyName");
    }

    [Fact]
    public void Equals_WithSameId_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entityName = EntityName.Create("User").Value;
        var userId = UserId.Create("user@test.com").Value;

        var auditLog1 = AuditLog.Create(
            entityName, id, userId, AuditAction.Create,
            null, null, null, null).Value;

        var auditLog2 = AuditLog.Create(
            entityName, id, userId, AuditAction.Update,
            null, null, null, null).Value;

        // Act & Assert
        auditLog1.Equals(auditLog2).ShouldBeTrue();
        (auditLog1 == auditLog2).ShouldBeTrue();
    }

    [Fact]
    public void Equals_WithDifferentId_ShouldReturnFalse()
    {
        // Arrange
        var builder = new AuditLogBuilder();
        var auditLog1 = builder.Build();
        var auditLog2 = builder.Build();

        // Act & Assert
        auditLog1.Equals(auditLog2).ShouldBeFalse();
        (auditLog1 != auditLog2).ShouldBeTrue();
    }
}
