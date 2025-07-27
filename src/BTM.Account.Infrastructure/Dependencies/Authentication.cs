using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BTM.Account.Infrastructure.Dependencies;
public static class Authentication
{
  public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
  {
    // Add OpenID Connect authentication
    ConfigureAuthentication(services, configuration);
    // Add OpenID Connect access token management service
    AddOpenIdConnectAccessTokenManagementService(services);

    return services;
  }

  public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddAuthentication(options =>
    {
      options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
      options.AccessDeniedPath = "/Account/AccessDenied";
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
      ConfigureClientOpenIdConnect(options, configuration);
    });
  }

  private static void ConfigureClientOpenIdConnect(OpenIdConnectOptions options, IConfiguration configuration)
  {
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    // Ideally pull from appsettings.json or environment variables
    options.Authority = configuration["Authentication:Authority"];
    options.ClientId = configuration["Authentication:ClientId"];
    options.ClientSecret = configuration["Authentication:ClientSecret"];
    options.ResponseType = "code";

    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClaimActions.Remove("aud");
    options.ClaimActions.DeleteClaim("sid");
    options.ClaimActions.DeleteClaim("idp");

    options.Scope.Add("AccountAPI.fullaccess");
    options.Scope.Add("offline_access");

    options.TokenValidationParameters = new()
    {
      NameClaimType = "name",
      RoleClaimType = "role"
    };

    options.SaveTokens = true;
  }

  private static void AddOpenIdConnectAccessTokenManagementService(IServiceCollection services)
  {
    // Adds support for OpenID Connect access token management
    services.AddOpenIdConnectAccessTokenManagement();
  }
}
