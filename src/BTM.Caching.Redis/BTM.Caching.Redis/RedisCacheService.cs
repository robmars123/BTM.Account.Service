using BTM.Caching.Redis.Abstractions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BTM.Caching.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var serializedValue = JsonConvert.SerializeObject(value);

            await db.StringSetAsync(key, serializedValue, expiration);
        }

        public async Task RemoveCacheAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
