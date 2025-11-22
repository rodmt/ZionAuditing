using FastEndpoints;
using MediatR;
using ZionAuditing.Application.AuditLogs.DTOs;
using ZionAuditing.Application.AuditLogs.Queries;

namespace ZionAuditing.Api.Endpoints.AuditLogs;

public sealed class GetAuditLogByIdEndpoint : Endpoint<GetAuditLogByIdRequest, AuditLogDto>
{
    private readonly ISender _sender;

    public GetAuditLogByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/audit-logs/{id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get audit log by ID";
            s.Description = "Retrieves an audit log entry by its unique identifier";
            s.Response<AuditLogDto>(200, "Audit log found");
            s.Response(404, "Audit log not found");
        });
    }

    public override async Task HandleAsync(GetAuditLogByIdRequest req, CancellationToken ct)
    {
        var query = new GetAuditLogByIdQuery(req.Id);
        var result = await _sender.Send(query, ct);

        if (result.IsFailure)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(result.Value, ct);
    }
}

public sealed record GetAuditLogByIdRequest
{
    public Guid Id { get; init; }
}
