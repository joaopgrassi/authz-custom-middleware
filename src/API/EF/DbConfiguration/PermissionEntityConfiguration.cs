// unset

using AuthUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace API.EF.DbConfiguration
{
    public class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.Property(p => p.Name).HasMaxLength(255);

            builder.HasData(GetSeedData());
        }

        private IEnumerable<Permission> GetSeedData()
        {
            yield return new Permission(new Guid("007B1D74-7E69-466A-8681-61121E5422C1"), Permissions.Login);
            yield return new Permission(new Guid("C8B858D6-404D-4127-B376-126ADB852B83"), Permissions.Create);
            yield return new Permission(new Guid("7762BE20-68CB-44EB-8F70-A58B0B3C91CC"), Permissions.Update);
            yield return new Permission(new Guid("F9E09403-A61E-4C45-8F06-7CA058F3317E"), Permissions.Delete);
        }
    }
}