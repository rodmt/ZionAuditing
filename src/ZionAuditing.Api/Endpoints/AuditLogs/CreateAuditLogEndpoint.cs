using FastEndpoints;
using MediatR;
using ZionAuditing.Application.AuditLogs.Commands;

namespace ZionAuditing.Api.Endpoints.AuditLogs;

public sealed class CreateAuditLogEndpoint : Endpoint<CreateAuditLogRequest, CreateAuditLogResponse>
{
    private readonly ISender _sender;

    public CreateAuditLogEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("/audit-logs");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create a new audit log entry";
            s.Description = "Creates a new audit log entry for tracking entity changes";
            s.Response<CreateAuditLogResponse>(201, "Audit log created successfully");
            s.Response(400, "Invalid request");
        });
    }

    public override async Task HandleAsync(CreateAuditLogRequest req, CancellationToken ct)
    {
        var command = new CreateAuditLogCommand(
            req.EntityName,
            req.EntityId,
            req.UserId,
            req.Action,
            req.OldValues,
            req.NewValues,
            req.IpAddress,
            req.UserAgent);

        var result = await _sender.Send(command, ct);

        if (result.IsFailure)
        {
            await SendErrorsAsync(400, ct);
            return;
        }

        await SendCreatedAtAsync<GetAuditLogByIdEndpoint>(
            new { id = result.Value },
            new CreateAuditLogResponse(result.Value),
            cancellation: ct);
    }
}

public sealed record CreateAuditLogRequest(
    string EntityName,
    Guid EntityId,
    string UserId,
    string Action,
    string? OldValues,
    string? NewValues,
    string? IpAddress,
    string? UserAgent);

public sealed record CreateAuditLogResponse(Guid Id);
