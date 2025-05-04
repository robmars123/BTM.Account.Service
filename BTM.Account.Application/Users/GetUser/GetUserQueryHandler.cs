using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Messaging;
using BTM.Account.Application.Results;
using BTM.Account.Domain.Abstractions;
using BTM.Account.Domain.Users;

namespace BTM.Account.Application.Users.GetUser
{
    /// <summary>
    /// GetUserQuery - passed in object
    /// GetUserResponse - returned object
    /// </summary>
    public class GetUserQueryHandler : IQueryHandler<GetUserQuery, GetUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<GetUserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            User response = await _userRepository.GetUserByIdAsync(request.Id.ToString(), cancellationToken);

            if (response == null)
                return new Result<GetUserResponse>().FailureResult("No user found.");

            //Make a mapping
            var userResponse = new GetUserResponse
            {
                Id = response.Id,
                Email = response.Email,
                Username = response.Username
            };

            return new Result<GetUserResponse>().SuccessResult(userResponse);
        }
    }
}
