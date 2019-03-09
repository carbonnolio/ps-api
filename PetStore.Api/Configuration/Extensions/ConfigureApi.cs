using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetStore.Api.Services;
using PetStore.Api.Services.Implementations;
using PetStore.Api.Settings;
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

        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new NullReferenceException("Failed to configure app settings. Configuration cannot be null.");

            var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var dbFolder = configuration.GetSection("AppSettings:DbFolder");
            var dbFile = configuration.GetSection("AppSettings:DbFile");

            services.AddOptions();
            services.Configure<AppSettings>(options =>
            {
                options.InventoryApiUrl = configuration["AppSettings:InventoryApiUrl"];
                options.DbPath = $"{contentRoot}\\{dbFolder.Value}\\{dbFile.Value}";
            });

            return services;
        }

        public static IServiceCollection ConfigureLiteDb(this IServiceCollection services,
            IConfiguration configuration)
        {
            var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var dbFolder = configuration.GetSection("AppSettings:DbFolder");

            var dbDirPath = $"{contentRoot}\\{dbFolder.Value}";

            if (Directory.Exists(dbDirPath))
            {
                Directory.Delete(dbDirPath, true);
            }

            Directory.CreateDirectory(dbDirPath);

            services.AddTransient<ILiteDbService, LiteDbService>();

            return services;
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IInventoryClient, InventoryClient>();

            return services;
        }
    }
}
