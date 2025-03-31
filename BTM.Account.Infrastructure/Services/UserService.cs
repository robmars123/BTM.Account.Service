using BTM.Account.Application.Abstractions;
using BTM.Account.Application.DTOs;
using BTM.Account.Application.Results;
using BTM.Account.Application.Users.RegisterUser;
using Newtonsoft.Json;

namespace BTM.Account.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpRequestService _httpRequestService;

        public UserService(IHttpRequestService httpRequestService)
        {
            _httpRequestService = httpRequestService;
        }
        public async Task<Result<UserDTO>> GetUserAsync(string? userId, string? accessToken)
        {
            var response = await _httpRequestService.GetRequestAsync($"api/users/{userId}", null, accessToken ?? string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await DeserializeResultUserDTO(response);
                return errorResponse != null
                    ? new Result<UserDTO>().FailureResult(errorResponse.ErrorMessages)
                    : new Result<UserDTO>();
            }

            UserDTO? result = await DeserializeResultObject<UserDTO>(response);

            return new Result<UserDTO>().SuccessResult(result ?? new UserDTO());
        }

        private static async Task<Result<UserDTO>?> DeserializeResultUserDTO(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<Result<UserDTO>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Result> RegisterUser(string endpoint, RegisterUserCommand user, string? accessToken)
        {
            var response = await _httpRequestService.SendPostRequestAsync(endpoint, user, accessToken ?? string.Empty);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await DeserializeResultObject<Result>(response);
                return errorResponse != null
                    ? Result.FailureResult(errorResponse.ErrorMessages)
                    : new Result();
            }
            var registerResult = await DeserializeResultObject<Result>(response);
            return registerResult;
        }

        private static async Task<T?> DeserializeResultObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
