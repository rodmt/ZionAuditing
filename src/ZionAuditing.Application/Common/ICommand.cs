using MediatR;
using ZionAuditing.Domain.Common;

namespace ZionAuditing.Application.Common;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
