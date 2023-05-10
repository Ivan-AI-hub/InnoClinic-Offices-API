﻿using Microsoft.OpenApi.Models;
using OfficesAPI.DAL.Repositories;
using OfficesAPI.Domain.Interfaces;
using OfficesAPI.Services;
using OfficesAPI.Services.Abstraction;

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
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IOfficeService, OfficeService>();
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
