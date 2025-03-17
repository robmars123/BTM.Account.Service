using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Messaging;
using BTM.Account.Domain.Abstractions;
using BTM.Account.Domain.Users;

namespace BTM.Account.Application.Users.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;

        public RegisterUserCommandHandler(IUserRepository userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User? user = _userFactory.CreateUser(request.Email, request.FirstName, request.LastName, request.Password);

                Result result = await _userRepository.AddAsync(user, cancellationToken);

                return result.IsFailure ? Result.Failure<Guid>(result.Error) : Result.Success(user.Id);
            }
            catch (Exception ex)
            {
                return (Result<Guid>)Result.Failure(new Error("UnexpectedError", ex.Message));
            }
        }
    }
}
