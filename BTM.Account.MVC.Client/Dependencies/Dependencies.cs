using BTM.Account.Application.Abstractions;
using BTM.Account.Infrastructure.Services;

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
        }
    }
}