using System;
using API.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = appSettings.Swagger.Title, Version = "v1"});
                options.OperationFilter<SwaggerAuthorizeOperationFilter>();
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl =
                                    new Uri($"{appSettings.IdentityServer.BaseUrl}/connect/authorize"),
                                TokenUrl = new Uri($"{appSettings.IdentityServer.BaseUrl}/connect/token"),
                                Scopes = appSettings.Swagger.Scopes
                            }
                        }
                    });
            });

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = appSettings.IdentityServer.BaseUrl;
                    options.Audience = appSettings.IdentityServer.Audience;
                    options.RequireHttpsMetadata = appSettings.IdentityServer.RequireHttps;
                });
            return services;
        }
        
        public static T ConfigureAndGet<T>(
            this IConfiguration configuration, IServiceCollection services, string sectionName) where T: class
        {
            if (string.IsNullOrWhiteSpace(sectionName))
                throw new ArgumentException("Section name cannot be empty", nameof(sectionName));

            var section = configuration.GetSection(sectionName);
            services.Configure<T>(section);

            return section.Get<T>();
        }
    }
}