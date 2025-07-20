using BTM.Account.Application.Results;
using BTM.Account.Application.Users.RegisterUser;
using BTM.Account.Domain.Users;

namespace BTM.Account.Application.Abstractions
{
  public interface IUserRepository
  {
    void CreateUser(User model, string password, CancellationToken cancellationToken);
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    void UpdateUser(User model, CancellationToken cancellationToken);
    Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken);
  }
}
