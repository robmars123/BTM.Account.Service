using BTM.Account.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BTM.Account.Application.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            RegisterDependencies(services);
            RegisterFactories(services);

            return services;
        }

        private static void RegisterFactories(IServiceCollection services)
        {
           // services.AddScoped<IUserFactory, UserFactory>();
        }

        private static void RegisterDependencies(IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
        }
    }
}
