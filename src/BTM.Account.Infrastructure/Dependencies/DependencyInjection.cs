using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Abstractions.UserIdentityManager;
using BTM.Account.Infrastructure.Models;
using BTM.Account.Infrastructure.Repositories;
using BTM.Account.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BTM.Account.Infrastructure.Dependencies
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
    {
      AddPersistence(services, configuration);
      RegisterServices(services);
      RegisterRepositories(services);
      RegisterLogging(services);
      RegisterCaching(services);

      AddJsonWebTokenHandler();
      return services;
    }


    private static void AddJsonWebTokenHandler()
    {
      JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }
    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"),
            sqlOptions =>
            {
              sqlOptions.MigrationsAssembly("BTM.Account.Infrastructure");
              sqlOptions.EnableRetryOnFailure();  // Enables transient failure retry
            });
      });


      services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

      services.Configure<IdentityOptions>(options =>
      {
        // Password complexity rules
        options.Password.RequireDigit = false; // Requires at least one digit
        options.Password.RequireLowercase = false; // Requires at least one lowercase letter
        options.Password.RequireUppercase = false; // Requires at least one uppercase letter
        options.Password.RequireNonAlphanumeric = false; // Requires at least one non-alphanumeric character (e.g., @, #, !)
        options.Password.RequiredLength = 5; // Minimum length for passwords
        options.Password.RequiredUniqueChars = 0; // Number of unique characters in the password
      });

    }

    private static void RegisterServices(IServiceCollection services)
    {
      services.AddScoped<IPasswordService, PasswordService>();
      services.AddScoped<IHttpRequestService, HttpRequestService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<ICacheService, RedisCacheService>();
      services.AddScoped<IUserIdentityManager, UserIdentityManager>();

    }

    private static void RegisterRepositories(IServiceCollection services)
    {
      services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void RegisterLogging(IServiceCollection services)
    {
      services.AddSingleton<ILoggingService, LoggingService>();
    }
    private static void RegisterCaching(IServiceCollection services)
    {
      //// Register Redis cache service in the DI container (Redis is now available throughout the application)
      //services.AddStackExchangeRedisCache(options =>
      //{
      //    options.Configuration = redisConnectionString; // Connection string to Redis server
      //    options.InstanceName = "cache";  // Cache name (optional, you can change it)
      //});
      //// Register Redis cache service
      //services.AddSingleton<ICacheService, RedisCacheService>();
    }
  }
}
