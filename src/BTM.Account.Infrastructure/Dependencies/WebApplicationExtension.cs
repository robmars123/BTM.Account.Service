using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BTM.Account.Infrastructure.Dependencies;
public static class WebApplicationExtensions
{
  public static WebApplication? ConfigureRequestPipeline(this WebApplication app)
  {
    if (!app.Environment.IsDevelopment())
    {
      app.UseExceptionHandler("/Account/Error");
      app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    return app;
  }
}

