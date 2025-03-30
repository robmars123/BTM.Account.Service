using BTM.Account.Domain.Abstractions;
using MediatR;

namespace BTM.Account.Application.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}
