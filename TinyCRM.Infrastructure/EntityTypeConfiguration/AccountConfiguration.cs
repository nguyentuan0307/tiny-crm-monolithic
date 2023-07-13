using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCRM.Domain.Accounts;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Phone).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Address).HasMaxLength(512);
            builder.Property(p => p.TotalSale).HasColumnType("decimal(18,2)");
        }
    }
}
