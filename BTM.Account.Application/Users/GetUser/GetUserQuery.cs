using BTM.Account.Application.Messaging;

namespace BTM.Account.Application.Users.GetUser
{
    public sealed record GetUserQuery(Guid Id) : IQuery<GetUserResponse>;
}
