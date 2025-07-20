using BTM.Account.Application.Results;
using BTM.Account.Domain.Abstractions;
using MediatR;

namespace BTM.Account.Application.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
