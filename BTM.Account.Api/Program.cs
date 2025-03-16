
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BTM.Account.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //option 1
        .AddJwtBearer(options =>
        {
            options.Authority = "https://localhost:5001"; // url of IDP
            options.Audience = "AccountAPI"; // this is the client id of the API
            options.TokenValidationParameters = new()
            {
                NameClaimType = "name",
                RoleClaimType = "role",
                ValidTypes = new[] { "at+jwt" }
            };
        });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
