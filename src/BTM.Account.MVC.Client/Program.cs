using BTM.Account.Infrastructure.Dependencies;
using BTM.Account.MVC.UI.Dependencies;

namespace BTM.Account.MVC.Client;

public static class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    // Register all services in a clean, chained format
    builder.Services
        .RegisterServices()
        .AddAuthenticationService(builder.Configuration)
        .AddApiHttpClients(builder.Configuration)
        .AddAuthorizationPolicies()
        .AddRedisCache(builder.Configuration);

    var app = builder.Build();

    app.ConfigureRequestPipeline();

    app.MapDefaultEndpoints();
    app.MapStaticAssets(); // Can't chain beyond this

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Index}/{id?}")
       .WithStaticAssets(); // this is allowed on MapControllerRoute

    app.Run();
  }
}
