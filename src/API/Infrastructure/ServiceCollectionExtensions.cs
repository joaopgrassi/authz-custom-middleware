using System;
using System.IO;
using API.Configuration;
using AuthUtils.PolicyProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
                var filePath = Path.Combine(AppContext.BaseDirectory, "API.xml");
                options.IncludeXmlComments(filePath);
            });

            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = appSettings.IdentityServer.BaseUrl;
                    options.Audience = appSettings.IdentityServer.Audience;
                    options.RequireHttpsMetadata = appSettings.IdentityServer.RequireHttps;
                    
                    // need this to avoid the legacy MS claim types. Sigh*
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.TokenValidationParameters.NameClaimType = "name";
                });
            
            services.AddAuthorization(options =>
            {
                // require all users to be authenticated by default
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                
                // Register our age limit policy
                options.AddPolicy("Over18YearsOld", policy => policy.RequireAssertion(context =>
                    context.User.HasClaim(c =>
                        (c.Type == "DateOfBirth" && DateTime.Now.Year - DateTime.Parse(c.Value).Year >= 18)
                    )));
            });
            
            // Register our custom Authorization handler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            
            // Overrides the DefaultAuthorizationPolicyProvider with our own
            // https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authorization/Core/src/DefaultAuthorizationPolicyProvider.cs
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
            
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
