using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace PetStore.Api.Configuration.Extensions
{
    public static class ConfigureApi
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1.0", new Info
                {
                    Version = "1.0",
                    Title = "Pet Store API",
                    Description = "Pet Store API"
                });
            });
    }
}
