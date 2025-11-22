using NSubstitute;
using Shouldly;
using Xunit;
using ZionAuditing.Application.AuditLogs.Queries;
using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.Repositories;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Application.UnitTests.AuditLogs.Queries;

public sealed class GetAuditLogByIdQueryHandlerTests
{
    private readonly IAuditLogRepository _repository;
    private readonly GetAuditLogByIdQueryHandler _handler;

    public GetAuditLogByIdQueryHandlerTests()
    {
        _repository = Substitute.For<IAuditLogRepository>();
        _handler = new GetAuditLogByIdQueryHandler(_repository);
    }

    [Fact]
    public async Task Handle_WithExistingAuditLog_ShouldReturnSuccessWithDto()
    {
        // Arrange
        var auditLogId = Guid.NewGuid();
        var entityId = Guid.NewGuid();
        var auditLog = AuditLog.Create(
            EntityName.Create("User").Value,
            entityId,
            UserId.Create("admin@test.com").Value,
            AuditAction.Update,
            "{\"old\":\"value\"}",
            "{\"new\":\"value\"}",
            "192.168.1.1",
            "Mozilla/5.0").Value;

        _repository.GetByIdAsync(auditLogId, Arg.Any<CancellationToken>())
            .Returns(auditLog);

        var query = new GetAuditLogByIdQuery(auditLogId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.EntityName.ShouldBe("User");
        result.Value.EntityId.ShouldBe(entityId);
        result.Value.UserId.ShouldBe("admin@test.com");
        result.Value.Action.ShouldBe("Update");
        result.Value.OldValues.ShouldBe("{\"old\":\"value\"}");
        result.Value.NewValues.ShouldBe("{\"new\":\"value\"}");
        result.Value.IpAddress.ShouldBe("192.168.1.1");
        result.Value.UserAgent.ShouldBe("Mozilla/5.0");
    }

    [Fact]
    public async Task Handle_WithNonExistingAuditLog_ShouldReturnFailure()
    {
        // Arrange
        var auditLogId = Guid.NewGuid();
        _repository.GetByIdAsync(auditLogId, Arg.Any<CancellationToken>())
            .Returns((AuditLog?)null);

        var query = new GetAuditLogByIdQuery(auditLogId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("AuditLog.NotFound");
        result.Error.Message.ShouldContain(auditLogId.ToString());
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithCorrectId()
    {
        // Arrange
        var auditLogId = Guid.NewGuid();
        var query = new GetAuditLogByIdQuery(auditLogId);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _repository.Received(1).GetByIdAsync(
            auditLogId,
            Arg.Any<CancellationToken>());
    }
}
