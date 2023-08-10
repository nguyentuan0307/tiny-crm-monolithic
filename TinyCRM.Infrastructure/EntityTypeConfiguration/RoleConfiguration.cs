using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyCRM.Infrastructure.Identity.Role;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasMany<IdentityRoleClaim<string>>(r => r.Claims)
            .WithOne()
            .HasForeignKey(rc => rc.RoleId);
    }
}