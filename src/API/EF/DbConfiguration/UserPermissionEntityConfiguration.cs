// unset

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

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

            // BUG - cannot call it because it explodes trying to access the nav prop Permission. Check why..
            // builder.HasData(GetSeedData().ToList());
        }

        private IEnumerable<UserPermission> GetSeedData()
        {
            // bob
            yield return new UserPermission(
                new Guid("426E8F4D-6E47-4A81-A1D4-E1B5DE0366FD"),
                new Guid("16E41B39-A078-4496-83B9-EF6FA7074E4C"),
                new Guid("007B1D74-7E69-466A-8681-61121E5422C1"));

            // alice
            yield return new UserPermission(
                new Guid("4B57B9F8-DF55-44AC-8FBB-E63DCB9474D1"), 
                new Guid("BEBC650B-A3E4-4B76-A2F2-019413D0542F"),
                new Guid("007B1D74-7E69-466A-8681-61121E5422C1"));
        }
    }
}