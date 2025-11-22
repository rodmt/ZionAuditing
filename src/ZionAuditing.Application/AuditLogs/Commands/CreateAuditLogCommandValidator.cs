using FluentValidation;

namespace ZionAuditing.Application.AuditLogs.Commands;

public sealed class CreateAuditLogCommandValidator : AbstractValidator<CreateAuditLogCommand>
{
    public CreateAuditLogCommandValidator()
    {
        RuleFor(x => x.EntityName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.EntityId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty()
            .MaximumLength(450);

        RuleFor(x => x.Action)
            .NotEmpty()
            .MaximumLength(50);
    }
}
