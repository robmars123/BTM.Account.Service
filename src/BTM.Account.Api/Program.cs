using BTM.Account.Api.Extensions;

namespace BTM.Account.Api;

public static class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    builder.AddServiceDefaults();

    builder.Services.AddCustomServices(builder.Configuration);

    var app = builder.Build();

    app.UseCustomMiddlewares();

    app.Run();
  }
}
