using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BTM.Account.Infrastructure.Dependencies;
public static class RedisCacheExtensions
{
  public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration = configuration["Redis:ConnectionString"];
    });

    return services;
  }
}

