using BTM.Account.Application.Results;
using BTM.Account.Domain.Users;

namespace BTM.Account.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<Result> CreateUserAsync(User model, string password, CancellationToken cancellationToken);
        Task<Result<User>> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Result<User>> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
        Task<Result> UpdateUserAsync(User model, CancellationToken cancellationToken);
    }
}
