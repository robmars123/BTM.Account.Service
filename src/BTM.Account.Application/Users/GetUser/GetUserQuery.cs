using BTM.Account.Application.Messaging;
using BTM.Account.Application.Results;
using MediatR;

namespace BTM.Account.Application.Users.GetUser
{
    public sealed record GetUserQuery(Guid Id) : IQuery<GetUserResponse>;
}
