using Shouldly;
using Xunit;
using ZionAuditing.Application.AuditLogs.Commands;

namespace ZionAuditing.Application.UnitTests.AuditLogs.Commands;

public sealed class CreateAuditLogCommandValidatorTests
{
    private readonly CreateAuditLogCommandValidator _validator;

    public CreateAuditLogCommandValidatorTests()
    {
        _validator = new CreateAuditLogCommandValidator();
    }

    [Fact]
    public async Task Validate_WithValidCommand_ShouldPass()
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "User",
            EntityId: Guid.NewGuid(),
            UserId: "admin@test.com",
            Action: "Create",
            OldValues: null,
            NewValues: "{\"name\":\"John\"}",
            IpAddress: "192.168.1.1",
            UserAgent: "Mozilla/5.0");

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Validate_WithInvalidEntityName_ShouldFail(string invalidEntityName)
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: invalidEntityName,
            EntityId: Guid.NewGuid(),
            UserId: "admin@test.com",
            Action: "Create",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "EntityName");
    }

    [Fact]
    public async Task Validate_WithEntityNameExceeding255Characters_ShouldFail()
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: new string('A', 256),
            EntityId: Guid.NewGuid(),
            UserId: "admin@test.com",
            Action: "Create",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "EntityName");
    }

    [Fact]
    public async Task Validate_WithEmptyEntityId_ShouldFail()
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "User",
            EntityId: Guid.Empty,
            UserId: "admin@test.com",
            Action: "Create",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "EntityId");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Validate_WithInvalidUserId_ShouldFail(string invalidUserId)
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "User",
            EntityId: Guid.NewGuid(),
            UserId: invalidUserId,
            Action: "Create",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "UserId");
    }

    [Fact]
    public async Task Validate_WithUserIdExceeding450Characters_ShouldFail()
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "User",
            EntityId: Guid.NewGuid(),
            UserId: new string('A', 451),
            Action: "Create",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "UserId");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Validate_WithInvalidAction_ShouldFail(string invalidAction)
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "User",
            EntityId: Guid.NewGuid(),
            UserId: "admin@test.com",
            Action: invalidAction,
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Action");
    }
}
