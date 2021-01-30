using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.EF.DbConfiguration
{
    public class UserPermissionEntityConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Permissions)
                .HasForeignKey(pt => pt.UserId)
                .Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            builder.HasOne(p => p.Permission)
                .WithMany()
                .HasForeignKey(pt => pt.PermissionId)
                .Metadata.DependentToPrincipal.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}