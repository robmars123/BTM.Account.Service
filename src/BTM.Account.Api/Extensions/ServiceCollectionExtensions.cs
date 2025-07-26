using BTM.Account.Application.Dependencies;
using BTM.Account.Infrastructure.Dependencies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.OpenApi.Models;

namespace BTM.Account.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["RedisSettings:ConnectionString"];
            options.InstanceName = "BTMAccount.Cache";
        });

        services.AddOpenApi();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please enter 'Bearer' followed by your token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
        });

        services.AddApplication(); // Ensure AddApplication is defined in the BTM.Account.Application.Dependencies namespace
        services.AddInfrastructure(configuration);

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5001";
              options.Audience = "AccountAPI";
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    ValidTypes = new[] { "at+jwt" }
                };
            });

        services.AddAuthorization();

        return services;
    }
}
