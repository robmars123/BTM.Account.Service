using BTM.Account.Application.Results;

namespace BTM.Account.Application.Abstractions
{
    public interface ICacheService
    {
        Task SetCacheValueAsync<T>(string key, T value);
        Task<Result<T>> GetCacheValueAsync<T>(string key);
    }
}
