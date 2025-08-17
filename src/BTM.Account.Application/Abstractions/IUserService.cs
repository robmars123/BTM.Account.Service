using BTM.Account.Application.DTOs;
using BTM.Account.Application.Results;
using BTM.Account.Application.Users.RegisterUser;

namespace BTM.Account.Application.Abstractions
{
    public interface IUserGateway
  {
        Task<Result<UserDTO>> GetUserAsync(string? userId, string? accessToken);
        Task<Result> RegisterUser(string endpoint, RegisterUserCommand user, string? accessToken);
    }
}
