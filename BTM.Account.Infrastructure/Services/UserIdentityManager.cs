using BTM.Account.Application.Abstractions.UserIdentityManager;
using BTM.Account.Application.Results;
using BTM.Account.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace BTM.Account.Infrastructure.Services
{
  public class UserIdentityManager : IUserIdentityManager
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public UserIdentityManager(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<Result> CreateUserAsync(string email, string username, string password)
    {
      var user = new ApplicationUser
      {
        Email = email,
        UserName = username
      };

      var identityResult = await _userManager.CreateAsync(user, password);

      if (!identityResult.Succeeded)
      {
        return Result.FailureResult(identityResult.Errors.Select(e => e.Description).ToList());
      }

      return Result.SuccessResult();
    }
  }
}
