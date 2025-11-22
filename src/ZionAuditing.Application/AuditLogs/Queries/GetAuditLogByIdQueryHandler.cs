using ZionAuditing.Application.AuditLogs.DTOs;
using ZionAuditing.Application.Common;
using ZionAuditing.Domain.Common;
using ZionAuditing.Domain.Repositories;

namespace ZionAuditing.Application.AuditLogs.Queries;

public sealed class GetAuditLogByIdQueryHandler : IQueryHandler<GetAuditLogByIdQuery, AuditLogDto>
{
    private readonly IAuditLogRepository _repository;

    public GetAuditLogByIdQueryHandler(IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AuditLogDto>> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
    {
        var auditLog = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (auditLog is null)
        {
            return Result.Failure<AuditLogDto>(
                Error.NotFound("AuditLog", request.Id));
        }

        var dto = new AuditLogDto(
            auditLog.Id,
            auditLog.EntityName.Value,
            auditLog.EntityId,
            auditLog.UserId.Value,
            auditLog.Action.Value,
            auditLog.OldValues,
            auditLog.NewValues,
            auditLog.Timestamp,
            auditLog.IpAddress,
            auditLog.UserAgent);

        return Result.Success(dto);
    }
}
