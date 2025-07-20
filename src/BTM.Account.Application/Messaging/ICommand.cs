using BTM.Account.Application.Results;
using BTM.Account.Domain.Abstractions;
using MediatR;

namespace BTM.Account.Application.Messaging
{
    public interface ICommand : IRequest<Result>, IBaseCommand
    {
    }

    public interface ICommand<TReponse> : IRequest<Result<TReponse>>, IBaseCommand
    {
    }

    public interface IBaseCommand
    {
    }
}
