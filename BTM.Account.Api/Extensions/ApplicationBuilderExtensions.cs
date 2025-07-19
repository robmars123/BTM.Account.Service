using BTM.Account.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BTM.Account.Api.Extensions
{
  public static class ApplicationBuilderExtensions
  {
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
      using var scope = app.ApplicationServices.CreateScope();

      using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      dbContext.Database.Migrate();
    }

    public static WebApplication UseCustomMiddlewares(this WebApplication app)
    {
      app.MapDefaultEndpoints();

      if (app.Environment.IsDevelopment())
      {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
          c.RoutePrefix = string.Empty;
        });
      }

      app.UseHttpsRedirection();
      app.UseAuthentication();
      app.UseAuthorization();
      app.MapControllers();

      return app;
    }
  }
}
