using System.Configuration;
using BTM.Account.ApiClient.Abstractions;
using BTM.Account.ApiClient.Products.Clients;
using BTM.Account.Application.Abstractions;
using BTM.Account.Infrastructure;
using BTM.Account.Infrastructure.Dependencies;
using BTM.Account.Infrastructure.Models;
using BTM.Account.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BTM.Account.MVC.UI.Dependencies
{
  public static class Dependencies
  {
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IHttpRequestService, HttpRequestService>();
      services.AddSingleton<ILoggingService, LoggingService>();
      services.AddSingleton<ICacheService, RedisCacheService>();

      services.AddTransient<IProductApiClient, ProductApiClient>();

      AddMvcControllers(services);
      return services;
    }

    private static void AddMvcControllers(IServiceCollection services)
    {
      // Add controllers with views and configure JSON options
      // This is necessary to ensure that the JSON serialization does not use camelCase naming policy
      // which is the default in .NET 6 and later.
      services.AddControllersWithViews()
              .AddJsonOptions(options =>
              {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use original property names
              });
    }
  }
}
