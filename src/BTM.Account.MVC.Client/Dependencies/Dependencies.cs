using System.Configuration;
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
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
      builder.Services.AddScoped<ITokenService, TokenService>();
      builder.Services.AddScoped<IUserService, UserService>();
      builder.Services.AddScoped<IHttpRequestService, HttpRequestService>();
      builder.Services.AddSingleton<ILoggingService, LoggingService>();
      builder.Services.AddSingleton<ICacheService, RedisCacheService>();

      //builder.Services.AddInfrastructure(builder.Configuration);

      //builder.Services.AddDbContext<ApplicationDbContext>(options =>
      //{
      //  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("BTM.Account.Infrastructure"));
      //});

    }
  }
}
