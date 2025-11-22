using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Xunit;
using ZionAuditing.Api.IntegrationTests.Fixtures;
using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.Repositories;

namespace ZionAuditing.Api.IntegrationTests.Endpoints;

public sealed class CreateAuditLogEndpointTests : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;

    public CreateAuditLogEndpointTests(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAuditLog_WithValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var repository = _factory.Services.GetRequiredService<IAuditLogRepository>();
        repository.AddAsync(Arg.Any<AuditLog>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var request = new
        {
            entityName = "User",
            entityId = Guid.NewGuid(),
            userId = "admin@test.com",
            action = "Create",
            oldValues = (string?)null,
            newValues = "{\"name\":\"John Doe\"}",
            ipAddress = "192.168.1.1",
            userAgent = "Mozilla/5.0"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/audit-logs", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
    }

    [Fact]
    public async Task CreateAuditLog_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new
        {
            entityName = "",
            entityId = Guid.NewGuid(),
            userId = "admin@test.com",
            action = "Create",
            oldValues = (string?)null,
            newValues = (string?)null,
            ipAddress = (string?)null,
            userAgent = (string?)null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/audit-logs", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
