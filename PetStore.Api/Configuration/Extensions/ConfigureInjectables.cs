using Microsoft.Extensions.DependencyInjection;
using PetStore.Api.Orchestrators;
using PetStore.Api.Orchestrators.Implementations;

namespace PetStore.Api.Configuration.Extensions
{
    public static class ConfigureInjectables
    {
        public static IServiceCollection AddScopedInjectables(this IServiceCollection services)
        {
            services.AddScoped<IOrderOrchestrator, OrderOrchestrator>();

            return services;
        }
    }
}
