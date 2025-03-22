using BTM.Account.Application.Messaging;

namespace BTM.Account.Application.Users.RegisterUser
{
    public sealed record RegisterUserCommand(
            string Email,
            string Username,
            string Password) : ICommand;
}
