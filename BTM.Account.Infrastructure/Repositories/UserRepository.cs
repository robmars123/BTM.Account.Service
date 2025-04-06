using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Results;
using BTM.Account.Domain.Abstractions;
using BTM.Account.Domain.Users;
using BTM.Account.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace BTM.Account.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggingService _loggingService;

        public UserRepository(UserManager<ApplicationUser> userManager, ILoggingService loggingService)
        {
            _userManager = userManager;
            _loggingService = loggingService;
        }

        public async Task<Result<User>> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
        {
            ApplicationUser? applicationUser = await _userManager.FindByIdAsync(userId);

            if (applicationUser == null || applicationUser.Email == null || applicationUser.UserName == null)
            {
                return new Result<User>().FailureResult("User not found");
            }

            User user = User.Create(applicationUser.Id, applicationUser.Email, applicationUser.UserName, applicationUser.PasswordHash!);

            return new Result<User>().SuccessResult(user);
        }

        public async Task<Result> CreateUserAsync(User user, string password, CancellationToken cancellationToken)
        {
            try
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    Email = user.Email,
                    UserName = user.Username
                };
                IdentityResult result = await _userManager.CreateAsync(applicationUser, password);

                if (!result.Succeeded)
                {
                    return Result.FailureResult(result.Errors.Select(e => e.Description).ToList());
                }

                return Result.SuccessResult();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex.Message, ex);
                return Result.FailureResult(ex.Message);
            }
        }

        public async Task<Result> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.Username
            };
            IdentityResult result = await _userManager.UpdateAsync(applicationUser);
            return Result.SuccessResult(result.ToString());
        }

        public async Task<Result<User>> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            IdentityUser<Guid> applicationUser = await _userManager.FindByEmailAsync(email);

            if (applicationUser == null || applicationUser.Email == null || applicationUser.UserName == null)
            {
                return new Result<User>().FailureResult("User not found");
            }

            User user = User.Create(applicationUser.Id, applicationUser.Email, applicationUser.UserName, applicationUser.PasswordHash!);

            return new Result<User>().SuccessResult(user);
        }
    }

}
