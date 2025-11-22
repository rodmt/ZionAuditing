using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;
using ZionAuditing.Api.IntegrationTests.Fixtures;
using ZionAuditing.Application.AuditLogs.DTOs;
using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.Repositories;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Api.IntegrationTests.Endpoints;

public sealed class GetAuditLogByIdEndpointTests : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;

    public GetAuditLogByIdEndpointTests(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAuditLogById_WithExistingId_ShouldReturnOk()
    {
        // Arrange
        var auditLogId = Guid.NewGuid();
        var entityId = Guid.NewGuid();
        var repository = _factory.Services.GetRequiredService<IAuditLogRepository>();

        var auditLog = AuditLog.Create(
            EntityName.Create("User").Value,
            entityId,
            UserId.Create("admin@test.com").Value,
            AuditAction.Create,
            null,
            "{\"name\":\"John\"}",
            "192.168.1.1",
            "Mozilla/5.0").Value;

        repository.GetByIdAsync(auditLogId, Arg.Any<CancellationToken>())
            .Returns(auditLog);

        // Act
        var response = await _client.GetAsync($"/api/audit-logs/{auditLogId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<AuditLogDto>();
        dto.ShouldNotBeNull();
        dto.EntityName.ShouldBe("User");
    }

    [Fact]
    public async Task GetAuditLogById_WithNonExistingId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        var repository = _factory.Services.GetRequiredService<IAuditLogRepository>();

        repository.GetByIdAsync(nonExistingId, Arg.Any<CancellationToken>())
            .Returns((AuditLog?)null);

        // Act
        var response = await _client.GetAsync($"/api/audit-logs/{nonExistingId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
