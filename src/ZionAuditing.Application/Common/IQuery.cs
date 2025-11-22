using MediatR;
using ZionAuditing.Domain.Common;

namespace ZionAuditing.Application.Common;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
