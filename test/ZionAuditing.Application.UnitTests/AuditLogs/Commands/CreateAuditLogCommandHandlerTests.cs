using NSubstitute;
using Shouldly;
using Xunit;
using ZionAuditing.Application.AuditLogs.Commands;
using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.Repositories;

namespace ZionAuditing.Application.UnitTests.AuditLogs.Commands;

public sealed class CreateAuditLogCommandHandlerTests
{
    private readonly IAuditLogRepository _repository;
    private readonly CreateAuditLogCommandHandler _handler;

    public CreateAuditLogCommandHandlerTests()
    {
        _repository = Substitute.For<IAuditLogRepository>();
        _handler = new CreateAuditLogCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnSuccessWithAuditLogId()
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
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBe(Guid.Empty);
        await _repository.Received(1).AddAsync(
            Arg.Is<AuditLog>(a =>
                a.EntityName.Value == command.EntityName &&
                a.EntityId == command.EntityId &&
                a.UserId.Value == command.UserId &&
                a.Action.Value == command.Action),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithInvalidEntityName_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "",
            EntityId: Guid.NewGuid(),
            UserId: "admin@test.com",
            Action: "Create",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.EntityName.Empty");
        await _repository.DidNotReceive().AddAsync(
            Arg.Any<AuditLog>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithInvalidUserId_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "User",
            EntityId: Guid.NewGuid(),
            UserId: "",
            Action: "Create",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.UserId.Empty");
        await _repository.DidNotReceive().AddAsync(
            Arg.Any<AuditLog>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithInvalidAction_ShouldReturnFailure()
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "User",
            EntityId: Guid.NewGuid(),
            UserId: "admin@test.com",
            Action: "",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Validation.AuditAction.Empty");
        await _repository.DidNotReceive().AddAsync(
            Arg.Any<AuditLog>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNullOptionalFields_ShouldSucceed()
    {
        // Arrange
        var command = new CreateAuditLogCommand(
            EntityName: "User",
            EntityId: Guid.NewGuid(),
            UserId: "admin@test.com",
            Action: "Delete",
            OldValues: null,
            NewValues: null,
            IpAddress: null,
            UserAgent: null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        await _repository.Received(1).AddAsync(
            Arg.Any<AuditLog>(),
            Arg.Any<CancellationToken>());
    }
}
