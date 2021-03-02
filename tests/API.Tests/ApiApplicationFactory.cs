using System;
using System.Linq;
using System.Security.Claims;
using API.EF;
using API.Tests.MockAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace API.Tests
{
    public class ApiApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly string _connectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        
        // Default logged in user for all requests - can be overwritten in individual tests
        private readonly MockAuthUser _user = new MockAuthUser(
            new Claim("sub", Guid.NewGuid().ToString()),
            new Claim("email", "default-user@xyz.com"));

        public ApiApplicationFactory()
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services => 
                {
                    var originalDbContext = services.Single(
                        d => d.ServiceType == typeof(DbContextOptions<AuthzContext>));
                    services.Remove(originalDbContext);

                    services.AddTestAuthentication();
                    services.AddScoped(_ => _user);

                    // adds the Sqlite version of the dbcontext for tests
                    services.AddDbContext<AuthzContext>(options =>
                    {
                        options
                            .UseSqlite(_connection)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
                        
                        var fac = LoggerFactory.Create(b => _ = b.AddDebug());
                        options.UseLoggerFactory(fac);
                    });

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AuthzContext>();

                    // Ensure the db is created.
                    db.Database.MigrateAsync();
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddDebug();
                    builder.SetMinimumLevel(LogLevel.Information);
                });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection!.Close();
        }
    }
}