using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BTM.Account.Infrastructure.Dependencies;
public static class StackExchangeRedisCache
{
  public static void AddStackExchangeRedisCache(this WebApplicationBuilder builder)
  {
    builder.Services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration = builder.Configuration["RedisSettings:ConnectionString"]; // Connection string to Redis server
      options.InstanceName = "BTMAccount.Cache"; // Optional instance name for the cache
    });
  }
}
