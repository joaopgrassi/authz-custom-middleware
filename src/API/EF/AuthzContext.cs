// unset

using Microsoft.EntityFrameworkCore;

namespace API.EF
{
    public class AuthzContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<UserPermission> UserPermissions { get; set; } = null!;

        public AuthzContext(DbContextOptions<AuthzContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Applies all the configurations for entities.	See	the	Configuration folder
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}