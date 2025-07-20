using System.Net.Http;

namespace BTM.Account.Application.Abstractions
{
    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync();
    }
}
