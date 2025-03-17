using BTM.Account.Domain.Abstractions;
using BTM.Account.Domain.Users;
using BTM.Account.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace BTM.Account.Infrastructure.Repositories
{
    internal sealed class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Result.Failure(Error.NullValue);
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            return (user != null
                    ? Result.Success(user.Id)
                     : Result.Failure(Error.UserNotFound));
        }

        public async Task<Result> AddAsync(User newUser, CancellationToken cancellationToken)
        {
            if (newUser == null)
                return new Result(false, Error.NullValue); 

            var existingUserResult = await GetByEmailAsync(newUser.Email, cancellationToken);
            if (existingUserResult.IsSuccess)
                return Result.Failure(Error.UserAlreadyExists);

            // If no existing user is found, add the new user to the database
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return the success result with the new user's ID
            return Result.Success(newUser.Id);
        }

    }
}
