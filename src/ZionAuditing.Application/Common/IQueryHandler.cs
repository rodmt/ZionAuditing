using MediatR;
using ZionAuditing.Domain.Common;

namespace ZionAuditing.Application.Common;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
