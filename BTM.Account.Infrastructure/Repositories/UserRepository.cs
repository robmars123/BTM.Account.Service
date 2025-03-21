using BTM.Account.Application.Abstractions;
using BTM.Account.Domain.Abstractions;
using BTM.Account.Domain.Users;
using BTM.Account.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace BTM.Account.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordService _passwordService;

        public UserRepository(ApplicationDbContext dbContext, IPasswordService passwordService)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
        }

        public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Result.Failure<User>(Error.NullValue);
            }

            var user = await _dbContext.Users
                .Include(u => u.Claims) // Include the claims
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            return (user != null
                    ? Result.Success(user)
                     : Result.Failure<User>(Error.UserNotFound));
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
            return Result.Success();
        }

        public async Task<bool> ValidateCredentials(string userName, string password)
        {
            var user = await _dbContext.Users
                .Include(u => u.Claims) // Include the claims
                .FirstOrDefaultAsync(u => u.Email.ToLower() == userName.ToLower());
            if (user != null)
            {
                return _passwordService.VerifyPassword(password, user.Password);
            }
            return false;
        }

    }
}
