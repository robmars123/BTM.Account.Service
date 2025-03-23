using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Messaging;
using BTM.Account.Domain.Abstractions;
using BTM.Account.Domain.Users;

namespace BTM.Account.Application.Users.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User? user = User.Create(Guid.NewGuid(), request.Email, request.Username, request.Password);

                var response = await _userRepository.CreateUserAsync(user, user.Password, cancellationToken);

                if (!response.IsSuccess) return Result.FailureResult(response.ErrorMessages);

                return Result.SuccessResult();
            }
            catch (Exception ex)
            {
                return Result.FailureResult("No user found");
            }
        }
    }
}
