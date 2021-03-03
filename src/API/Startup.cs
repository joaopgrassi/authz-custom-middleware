using API.Authorization;
using API.EF;
using API.Configuration;
using API.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    using System.IdentityModel.Tokens.Jwt;

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private AppSettings _appSettings = null!;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _appSettings = _configuration.ConfigureAndGet<AppSettings>(services, AppSettings.SectionName);

            services.AddControllers();
            services.AddDbContext<AuthzContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("AuthzConnection")));
            
            services.AddHostedService<DbMigratorHostedService>();

            services.AddScoped<IUserPermissionService, UserPermissionService>();

            services.AddSwagger(_appSettings);
            
            services.AddAuthentication(_appSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                options.OAuthClientId(_appSettings.Swagger.ClientId);
                options.OAuthAppName(_appSettings.Swagger.ClientId);
                options.OAuthUsePkce();
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            
            // order here matters - after UseAuthentication so we have the Identity populated in the HttpContext
            app.UseMiddleware<PermissionsMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
