using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCRM.Domain.Leads;

namespace TinyCRM.Infrastructure.EntityTypeConfiguration
{
    public class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Description).HasMaxLength(1024);
            builder.Property(p => p.AccountId).IsRequired();
            builder.Property(p => p.StatusLead).IsRequired();
            builder.Property(p => p.SourceLead).IsRequired();
            builder.Property(p => p.DateQuanlified).IsRequired();
            builder.Property(p => p.EstimatedRevenue).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasOne(a => a.Account)
                .WithMany(l => l.Leads)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
