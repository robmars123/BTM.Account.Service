using BTM.Account.Application.Results;

namespace BTM.Account.ApiClient.Abstractions;
public interface IProductApiClient
{
  Task<Result> GetAsync();
}
