using BTM.Account.Domain.Abstractions;

namespace BTM.Account.Domain.Users
{
    public interface IUserRepository
    {
        Task<Result> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Result> AddAsync(User newUser, CancellationToken cancellationToken);
    }
}
