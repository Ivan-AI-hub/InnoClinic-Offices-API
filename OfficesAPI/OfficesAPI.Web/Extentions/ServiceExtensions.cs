using Microsoft.OpenApi.Models;
using OfficesAPI.Application;
using OfficesAPI.Application.Abstraction;
using OfficesAPI.Domain.Interfaces;
using OfficesAPI.Persistence.Repositories;
using OfficesAPI.Web.Settings;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace OfficesAPI.Web.Extentions
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IOfficeService, OfficeService>();
        }
        public static void ConfigureLogger(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment, string elasticUriSection)
        {
            services.AddSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration[elasticUriSection]))
                    {
                        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 2,
                        NumberOfReplicas = 1
                    })
                    .Enrich.WithProperty("Environment", environment.EnvironmentName)
                    .ReadFrom.Configuration(configuration);
            });
        }
        public static void ConfigureCaching(this IServiceCollection services, RedisSettings settings)
        {
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = settings.Configuration;
                options.InstanceName = settings.InstanceName;
            });
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                           Name = "Bearer",
                        },
                        new List<string>()
                    }
                });

            });
        }
    }
}
