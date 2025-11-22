using ZionAuditing.Application.Common;
using ZionAuditing.Domain.Common;
using ZionAuditing.Domain.Entities;
using ZionAuditing.Domain.Repositories;
using ZionAuditing.Domain.ValueObjects;

namespace ZionAuditing.Application.AuditLogs.Commands;

public sealed class CreateAuditLogCommandHandler : ICommandHandler<CreateAuditLogCommand, Guid>
{
    private readonly IAuditLogRepository _repository;

    public CreateAuditLogCommandHandler(IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
    {
        var entityNameResult = EntityName.Create(request.EntityName);
        if (entityNameResult.IsFailure)
        {
            return Result.Failure<Guid>(entityNameResult.Error);
        }

        var userIdResult = UserId.Create(request.UserId);
        if (userIdResult.IsFailure)
        {
            return Result.Failure<Guid>(userIdResult.Error);
        }

        var actionResult = AuditAction.From(request.Action);
        if (actionResult.IsFailure)
        {
            return Result.Failure<Guid>(actionResult.Error);
        }

        var auditLogResult = AuditLog.Create(
            entityNameResult.Value,
            request.EntityId,
            userIdResult.Value,
            actionResult.Value,
            request.OldValues,
            request.NewValues,
            request.IpAddress,
            request.UserAgent);

        if (auditLogResult.IsFailure)
        {
            return Result.Failure<Guid>(auditLogResult.Error);
        }

        await _repository.AddAsync(auditLogResult.Value, cancellationToken);

        return Result.Success(auditLogResult.Value.Id);
    }
}
