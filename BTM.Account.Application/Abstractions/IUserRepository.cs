using BTM.Account.Domain.Abstractions;
using BTM.Account.Domain.Users;

namespace BTM.Account.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Result> AddAsync(User newUser, CancellationToken cancellationToken);
        Task<bool> ValidateCredentials(string userName, string password);
    }
}
