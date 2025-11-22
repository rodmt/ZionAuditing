using Dapper;
using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.Repositories;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Infrastructure.Persistence.Repositories;

public sealed class AuditLogRepository : IAuditLogRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AuditLogRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<AuditLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
            SELECT
                Id,
                EntityName,
                EntityId,
                UserId,
                Action,
                OldValues,
                NewValues,
                Timestamp,
                IpAddress,
                UserAgent
            FROM AuditLogs
            WHERE Id = @Id
            """;

        var result = await connection.QueryFirstOrDefaultAsync<AuditLogDto>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        return result is not null ? MapToEntity(result) : null;
    }

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(Guid entityId, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
            SELECT
                Id,
                EntityName,
                EntityId,
                UserId,
                Action,
                OldValues,
                NewValues,
                Timestamp,
                IpAddress,
                UserAgent
            FROM AuditLogs
            WHERE EntityId = @EntityId
            ORDER BY Timestamp DESC
            """;

        var results = await connection.QueryAsync<AuditLogDto>(
            new CommandDefinition(sql, new { EntityId = entityId }, cancellationToken: cancellationToken));

        return results.Select(MapToEntity);
    }

    public async Task<IEnumerable<AuditLog>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
            SELECT
                Id,
                EntityName,
                EntityId,
                UserId,
                Action,
                OldValues,
                NewValues,
                Timestamp,
                IpAddress,
                UserAgent
            FROM AuditLogs
            WHERE UserId = @UserId
            ORDER BY Timestamp DESC
            """;

        var results = await connection.QueryAsync<AuditLogDto>(
            new CommandDefinition(sql, new { UserId = userId }, cancellationToken: cancellationToken));

        return results.Select(MapToEntity);
    }

    public async Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
            INSERT INTO AuditLogs (
                Id,
                EntityName,
                EntityId,
                UserId,
                Action,
                OldValues,
                NewValues,
                Timestamp,
                IpAddress,
                UserAgent
            ) VALUES (
                @Id,
                @EntityName,
                @EntityId,
                @UserId,
                @Action,
                @OldValues,
                @NewValues,
                @Timestamp,
                @IpAddress,
                @UserAgent
            )
            """;

        await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    auditLog.Id,
                    EntityName = auditLog.EntityName.Value,
                    auditLog.EntityId,
                    UserId = auditLog.UserId.Value,
                    Action = auditLog.Action.Value,
                    auditLog.OldValues,
                    auditLog.NewValues,
                    auditLog.Timestamp,
                    auditLog.IpAddress,
                    auditLog.UserAgent
                },
                cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<AuditLog>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
            SELECT
                Id,
                EntityName,
                EntityId,
                UserId,
                Action,
                OldValues,
                NewValues,
                Timestamp,
                IpAddress,
                UserAgent
            FROM AuditLogs
            ORDER BY Timestamp DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY
            """;

        var results = await connection.QueryAsync<AuditLogDto>(
            new CommandDefinition(
                sql,
                new { Offset = (page - 1) * pageSize, PageSize = pageSize },
                cancellationToken: cancellationToken));

        return results.Select(MapToEntity);
    }

    private static AuditLog MapToEntity(AuditLogDto dto)
    {
        var entityName = EntityName.Create(dto.EntityName).Value;
        var userId = UserId.Create(dto.UserId).Value;
        var action = AuditAction.From(dto.Action).Value;

        var auditLog = AuditLog.Create(
            entityName,
            dto.EntityId,
            userId,
            action,
            dto.OldValues,
            dto.NewValues,
            dto.IpAddress,
            dto.UserAgent).Value;

        return auditLog;
    }

    private sealed record AuditLogDto(
        Guid Id,
        string EntityName,
        Guid EntityId,
        string UserId,
        string Action,
        string? OldValues,
        string? NewValues,
        DateTime Timestamp,
        string? IpAddress,
        string? UserAgent);
}
