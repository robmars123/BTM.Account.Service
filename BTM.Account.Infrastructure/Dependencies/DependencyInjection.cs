using BTM.Account.Application.Abstractions;
using BTM.Account.Application.Factories.HttpRequest;
using BTM.Account.Infrastructure.Models;
using BTM.Account.Infrastructure.Repositories;
using BTM.Account.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("BTM.Account.Infrastructure"));
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
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
