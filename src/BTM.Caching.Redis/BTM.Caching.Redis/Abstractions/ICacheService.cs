namespace BTM.Caching.Redis.Abstractions
{
    public interface ICacheService
    {
        Task<T> GetCacheAsync<T>(string key);
        Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveCacheAsync(string key);
    }
}
