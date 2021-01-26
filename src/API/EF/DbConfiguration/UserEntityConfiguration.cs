// unset

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace API.EF.DbConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();
            
            builder.Property(p => p.ExternalId).HasMaxLength(255);
            builder.Property(p => p.Email).HasMaxLength(255);

            builder.HasData(GetSeedData());
        }
        
        private IEnumerable<User> GetSeedData()
        {
            yield return new User(new Guid("16E41B39-A078-4496-83B9-EF6FA7074E4C"), "88421113", "bobsmith@email.com");
            yield return new User(new Guid("BEBC650B-A3E4-4B76-A2F2-019413D0542F"), "818727", "alicesmith@email.com");
        }
    }
}