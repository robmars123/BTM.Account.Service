using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Results;
using BTM.Account.Domain.Users;
using BTM.Account.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BTM.Account.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly ILoggingService _loggingService;
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ILoggingService loggingService, ApplicationDbContext dbContext)
    {
      _loggingService = loggingService;
      _dbContext = dbContext;
    }

    public async Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
      if (!Guid.TryParse(userId, out var userGuid))
      {
        return null;
      }

      var applicationUser = await _dbContext.ApplicationUsers
          .FirstOrDefaultAsync(u => u.Id == userGuid, cancellationToken);

      if (applicationUser == null) return null;

      return User.Create(applicationUser.Id, applicationUser.Email, applicationUser.UserName, applicationUser.PasswordHash!);
    }

    public void CreateUser(User user, string password, CancellationToken cancellationToken)
    {
      try
      {
        ApplicationUser applicationUser = new ApplicationUser
        {
          Email = user.Email,
          UserName = user.Username
        };
        
        _dbContext.ApplicationUsers.Add(applicationUser);
      }
      catch (Exception ex)
      {
        _loggingService.LogError(ex.Message, ex);
      }
    }

    public void UpdateUser(User user, CancellationToken cancellationToken)
    {
      ApplicationUser applicationUser = new ApplicationUser
      {
        Id = user.Id,
        Email = user.Email,
        UserName = user.Username
      };

      _dbContext.ApplicationUsers.Update(applicationUser);
    }

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
      var applicationUser = await _dbContext.ApplicationUsers
          .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

      if(applicationUser == null)
      {
        return null;
      }
      return User.Create(applicationUser.Id, applicationUser.Email, applicationUser.UserName, applicationUser.PasswordHash!);
    }

    public async Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken)
    {
      var user = await _dbContext.ApplicationUsers
          .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
      return user != null;
    }
  }

}
