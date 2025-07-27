using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace BTM.Account.Infrastructure.Dependencies;
public static class HttpClientExtensions
{
  public static IServiceCollection AddApiHttpClients(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddHttpClient("AccountAPI", client =>
    {
      client.BaseAddress = new Uri(configuration["AccountAPI"]);
      client.DefaultRequestHeaders.Clear();
      client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    }).AddUserAccessTokenHandler();

    services.AddHttpClient("IDPClient", client =>
    {
      client.BaseAddress = new Uri(configuration["Authentication:Authority"]);
    });

    return services;
  }
}

