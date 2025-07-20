
using Azure;
using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Results;
using BTM.Account.Domain.Users;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BTM.Account.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILoggingService _loggingService;

        public RedisCacheService(IDistributedCache cache, ILoggingService loggingService)
        {
            _cache = cache;
            _loggingService = loggingService;
        }

        public async Task SetCacheValueAsync<T>(string key, T value)
        {
            var jsonValue = JsonConvert.SerializeObject(value);  // Serialize object to JSON

            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Optional cache expiration settings

            await _cache.SetStringAsync(key, jsonValue, options);  // Store serialized object in Redis cache
        }

        public async Task<Result<T>> GetCacheValueAsync<T>(string key)
        {
            var jsonValue = await _cache.GetStringAsync(key);  // Retrieve JSON string from cache

            if (string.IsNullOrEmpty(jsonValue))
            {
                // If the value is null or empty, return the default value for T
                return default;
            }

            try
            {
                // Attempt to deserialize the string into an object of type T
                var deserializedValue = JsonConvert.DeserializeObject<T>(jsonValue);

                // Return the deserialized value (it could be null if the JSON content was valid but empty)
                return new Result<T>().SuccessResult(deserializedValue);
            }
            catch (JsonException ex)
            {
                _loggingService.LogError($"Failed to deserialize cache value for key '{key}': {ex.Message}", ex);
                return default;  // Return default in case of failure
            }
        }
    }
}
