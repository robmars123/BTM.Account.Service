using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Abstractions.UserIdentityManager;
using BTM.Account.Application.Messaging;
using BTM.Account.Application.Results;
using BTM.Account.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BTM.Account.Application.Users.RegisterUser
{
  public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
  {
    private readonly IUserIdentityManager _userIdentityManager;
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserIdentityManager userIdentityManager, IUserRepository userRepository)
    {
      _userIdentityManager = userIdentityManager;
      _userRepository = userRepository;
    }
    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      try
      {
        bool exist = await UserExistsAsync(request, cancellationToken);

        if (exist) return Result.FailureResult("User already exists.");

        User? user = User.Create(Guid.NewGuid(), request.Email, request.Username, request.Password);

        await _userIdentityManager.CreateUserAsync(request.Email, request.Username, request.Password);

        return Result.SuccessResult();
      }
      catch (Exception ex)
      {
        return Result.FailureResult("No user has been created.");
      }
    }

    private async Task<bool> UserExistsAsync(RegisterUserCommand request, CancellationToken cancellationToken)
    {
      var existingUser = await _userRepository.UserExistsAsync(request.Email, cancellationToken);

      return existingUser != null;
    }
  }
}
