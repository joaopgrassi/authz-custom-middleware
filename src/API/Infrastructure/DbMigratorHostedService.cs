using System;
using System.Threading;
using System.Threading.Tasks;
using API.EF;
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
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
