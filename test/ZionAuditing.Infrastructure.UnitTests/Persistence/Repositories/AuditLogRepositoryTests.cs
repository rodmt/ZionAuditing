using Shouldly;
using Xunit;
using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Infrastructure.UnitTests.Persistence.Repositories;

public sealed class AuditLogRepositoryTests
{
    // Note: These are examples. In a real scenario, you would use:
    // - Testcontainers for SQL Server integration tests
    // - Or in-memory database for lightweight testing
    // - Or Respawn for database cleanup between tests

    [Fact]
    public void AuditLog_Mapping_ShouldPreserveAllProperties()
    {
        // Arrange
        var entityName = EntityName.Create("User").Value;
        var entityId = Guid.NewGuid();
        var userId = UserId.Create("admin@test.com").Value;
        var action = AuditAction.Create;
        var oldValues = "{\"name\":\"Old\"}";
        var newValues = "{\"name\":\"New\"}";
        var ipAddress = "192.168.1.1";
        var userAgent = "Mozilla/5.0";

        // Act
        var auditLog = AuditLog.Create(
            entityName,
            entityId,
            userId,
            action,
            oldValues,
            newValues,
            ipAddress,
            userAgent).Value;

        // Assert
        auditLog.EntityName.Value.ShouldBe("User");
        auditLog.EntityId.ShouldBe(entityId);
        auditLog.UserId.Value.ShouldBe("admin@test.com");
        auditLog.Action.Value.ShouldBe("Create");
        auditLog.OldValues.ShouldBe(oldValues);
        auditLog.NewValues.ShouldBe(newValues);
        auditLog.IpAddress.ShouldBe(ipAddress);
        auditLog.UserAgent.ShouldBe(userAgent);
    }
}
