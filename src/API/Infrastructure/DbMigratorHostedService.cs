using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.EF;
using AuthUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Infrastructure
{
    /// <summary>
    /// A hosted service that will migrate the database automatically.
    /// This is only for demo/dev purpose..
    /// </summary>
    public class DbMigratorHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DbMigratorHostedService> _logger;
        
        public DbMigratorHostedService(IServiceProvider services, 
            ILogger<DbMigratorHostedService> logger)
        {
            _serviceProvider = services;
            _logger = logger;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AuthzContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);

            await SeedDb(dbContext, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async ValueTask SeedDb(AuthzContext dbContext, CancellationToken cancellationToken)
        {
            if (!await dbContext.Permissions.AnyAsync(cancellationToken))
            {
                // if no permissions are present, we create everything from scratch
                // this is so we can all be in the same page during the blog post;
                
                var permissions = new[]
                {
                    new Permission(Guid.NewGuid(), Permissions.Create),
                    new Permission(Guid.NewGuid(), Permissions.Read),
                    new Permission(Guid.NewGuid(), Permissions.Update),
                    new Permission(Guid.NewGuid(), Permissions.Delete)
                };
                dbContext.Permissions.AddRange(permissions);

                var users = new[]
                {
                    new User(Guid.NewGuid(), "818727", "alicesmith@email.com"),
                    new User(Guid.NewGuid(), "88421113", "bobsmith@email.com")
                };
                dbContext.Users.AddRange(users);

                var alicePermissions = new[]
                {
                    // Alice has CRUD permissions
                    new UserPermission(Guid.NewGuid(), users[0].Id, permissions[0].Id),
                    new UserPermission(Guid.NewGuid(), users[0].Id, permissions[1].Id),
                    new UserPermission(Guid.NewGuid(), users[0].Id, permissions[2].Id),
                    new UserPermission(Guid.NewGuid(), users[0].Id, permissions[3].Id)
                };
                
                var bobPermissions = new[]
                {
                    // Bob has only Read permission
                    new UserPermission(Guid.NewGuid(), users[1].Id, permissions[1].Id),
                };
                dbContext.UserPermissions.AddRange(alicePermissions.Concat(bobPermissions));

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
