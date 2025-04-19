using BTM.Account.Application.Abstractions;
using BTM.Account.Application.DTOs;
using BTM.Account.Application.Results;
using BTM.Account.Application.Users.RegisterUser;
using BTM.Account.Domain.Users;
using BTM.Account.Shared.Common;
using Newtonsoft.Json;

namespace BTM.Account.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpRequestService _httpRequestService;
        private readonly ILoggingService _logger;
        private readonly ICacheService _cacheService;

        public UserService(IHttpRequestService httpRequestService, ILoggingService logger, ICacheService cacheService)
        {
            _httpRequestService = httpRequestService;
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task<Result<UserDTO>> GetUserAsync(string? userId, string? accessToken)
        {
            var cacheKey = $"User_{userId}";
            var cachedUser = await _cacheService.GetCacheValueAsync<User>(cacheKey);

            //if cache is not null, return the cached user
            if (cachedUser != null)
            {
                return new Result<UserDTO>().SuccessResult(new UserDTO
                {
                    Email = cachedUser.Data?.Email,
                    Username = cachedUser.Data?.Username
                });
            }

            UserDTO? result = new UserDTO();
            try
            {
                HttpResponseMessage response = await _httpRequestService.GetRequestAsync($"{GlobalConstants.ApiEndpoints.UsersEndpoint}/{userId}", null, accessToken ?? string.Empty);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await DeserializeResultUserDTO(response);

                    _logger.LogError($"Error occurred while getting user: {errorResponse?.ErrorMessages}");
                    return errorResponse != null
                        ? new Result<UserDTO>().FailureResult(errorResponse.ErrorMessages)
                        : new Result<UserDTO>().FailureResult(response.StatusCode.ToString());
                }

                if (response != null)
                {
                    var user = DeserializeResultObject<User>(response);
                    if (user != null)
                    {
                        result = new UserDTO
                        {
                            Email = user.Result.Email,
                            Username = user.Result.Username
                        };
                        //set cache, setting the cache to expire in 10 minutes
                        await _cacheService.SetCacheValueAsync(cacheKey, result);
                    }
                }


                result = await DeserializeResultObject<UserDTO>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

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

                if (errorResponse == null)
                {
                    return Result.FailureResult("An unexpected error occurred while processing the request.");
                }

                return errorResponse != null
                    ? Result.FailureResult(errorResponse.ErrorMessages)
                    : Result.FailureResult("An unexpected error occurred.");
            }
            var registerResult = await DeserializeResultObject<Result>(response);
            return registerResult ?? Result.FailureResult("No result returned from the server.");
        }

        private static async Task<T?> DeserializeResultObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
