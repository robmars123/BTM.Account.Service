using BTM.Account.ApiClient.Abstractions;
using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Results;
using BTM.Account.Shared.Common;

namespace BTM.Account.ApiClient.Products.Clients;
public class ProductApiClient : IProductApiClient
{
  private readonly ITokenService _tokenService;
  private readonly IHttpRequestService _httpRequestService;

  public ProductApiClient(ITokenService tokenService, IHttpRequestService httpRequestService)
  {
    _tokenService = tokenService;
    _httpRequestService = httpRequestService;
  }
  public async Task<Result> GetAsync()
  {
    string accessToken = await _tokenService.GetAccessTokenAsync();
    string context = GlobalConstants.ApiConstants.ProductsAPI;
    string endpoint = GlobalConstants.ApiEndpoints.ProductsEndpoint;

    HttpResponseMessage response = await _httpRequestService.GetRequestAsync(context, endpoint, null, accessToken);

    if (!response.IsSuccessStatusCode) return Result.FailureResult("An error occurred.");

    var content = await response.Content.ReadAsStringAsync();

    return Result.SuccessResult(content);
  }
}
